USE [MalibanPosDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomerDeductionProcess]    Script Date: 3/6/2022 10:06:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_CustomerDeductionProcess] (@branchLocationId as int,@fromDate as Date,@toDate as Date,@category as int) 
as
begin
select em.FullName,sh.EmployeeCode as CustomerEpf,em.Number as CustomerNumber,sum(sh.TotalAmount-sh.CustomerReturnAmount) as DeductionTotal from SaleHeaders  as sh left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo
where sh.BranchLocationId=@branchLocationId and em.SchemeCode=@category and (sh.OrderDate >= @fromDate and sh.OrderDate <= @toDate) Group by sh.EmployeeCode,em.Number,em.FullName
update SaleHeaders set Paid=1 
from (
select sh.SaleHeaderId as SaleHeaderId,sh.Paid as SaleHeadrPaid,sh.EmployeeCode from SaleHeaders  as sh left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo
where sh.BranchLocationId=@branchLocationId and em.SchemeCode=@category and (sh.OrderDate >= @fromDate and sh.OrderDate <= @toDate) and sh.paid=0 Group by sh.SaleHeaderId,sh.Paid,sh.EmployeeCode
) B 
where SaleHeaders.SaleHeaderId=B.SaleHeaderId

update SaleDetails set Paid=1 
from (
select em.Number,sh.EmployeeCode,em.FullName,em.CreditLimit,sh.SaleHeaderId as SaleHeaderId,sd.Paid as SaleDetailPaid,sh.Paid as SaleHeadrPaid,PriceId,sd.ProductId,sd.BillDate,sd.CreateDate,sh.OrderTimeStamp,em.CreditBalance,em.PreviousCreditBalance from SaleDetails as sd left join SaleHeaders as sh on sd.SaleHeaderId=sh.SaleHeaderId left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo
where sh.BranchLocationId=@branchLocationId and em.SchemeCode=@category and (sd.CreateDate >= @fromDate and sd.CreateDate <= @toDate) and (sh.OrderDate >= @fromDate and sh.OrderDate <= @toDate) and sd.paid=0) B 
where SaleDetails.SaleHeaderId=B.SaleHeaderId

UPDATE
      [MI_Employer_Employees] 
SET
      [MI_Employer_Employees].[CreditBalance] = 0,
	  [MI_Employer_Employees].[PreviousCreditBalance] = 0

FROM
    (
select sh.EmployeeCode,em.Number from SaleHeaders  as sh left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo
where sh.BranchLocationId=@branchLocationId and em.SchemeCode=@category and (sh.OrderDate >= @fromDate and sh.OrderDate <= @toDate) and sh.paid=1 Group by sh.EmployeeCode,em.Number
    ) B
Where 
    [MI_Employer_Employees].Number  = B.Number


end

-- ================================
-- Create User-defined Table Type
-- ================================
USE [MalibanPosDB]
GO

-- Create the data type
CREATE TYPE MI_Employer_Employees AS TABLE 
(
   MI_Employer_EmployeeUid nvarchar(MAX) not null

)
GO

CREATE TYPE MI_Employer_Employees AS TABLE 
(
   MI_Employer_EmployeeUid nvarchar(MAX) not null,
   Number  nvarchar(MAX) not null,
   Deleted int not null,
   DateOfAppointment datetime2(7) not null,
   SchemeCode int not null,
   EPFNo nvarchar(20) not null,
   FullName nvarchar(200) not null,
   CreditLimit decimal(18, 2) not null,
   Scheme nvarchar(MAX) not null,
   CreditBalance decimal(18, 2) not null,
   PreviousCreditBalance decimal(18, 2) not null,
   DateResigned datetime2(7) not null
)

USE [MalibanPosDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStockDetailsByDateRange]    Script Date: 7/7/2022 12:02:49 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date:  7/6/2022 12:37:29 PM
-- Description:	<Description,,>
-- Modified date:  7/6/2022 12:37:29 PM
-- =============================================
Create PROCEDURE [dbo].[sp_GetStockDetailsByDateRange] (@branchLocationId as int)  
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select trd.Barcode as ItemCode 
      ,trd.ProductName as ProductName
	   ,trd.dSalePrice as UnitPrice
	   ,trd.dCostPrice as Cost
	   ,Sum(trd.dNetQuantity) as QTY
	   ,Sum(trd.dNetQuantity)*(trd.dCostPrice) CostValue
	   ,Sum(trd.dNetQuantity)*(trd.dSalePrice) SaleValue
	   ,trd.ProductExpireDate as ProductExpireDate
		from TransactionDetails  as trd 
where trd.BranchLocationId=@branchLocationId and trd.IsDeleted=0 and trd.dQuantity-trd.dIssueQuantity > 0 group by trd.ProductId,trd.PriceId,trd.ProductExpireDate,trd.ProductName,trd.Barcode,trd.dSalePrice,trd.dCostPrice 
end


USE [MalibanPosDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetSaleDetailsByDateRange]    Script Date: 7/7/2022 12:04:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date:  7/6/2022 12:37:29 PM
-- Description:	<Description,,>
-- Modified date:  7/6/2022 12:37:29 PM
-- =============================================
Create PROCEDURE [dbo].[sp_GetSaleDetailsByDateRange] (@branchLocationId as int,@paid as bit,@frmDate as Date,@toDate as Date)  
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select p.Barcode as ItemCode 
      ,p.ProductName as ProductName
	   ,pr.SalePrice as CashPrice
	   ,pr.ModifiedUnitPrice as Cost
	   ,Sum(sd.Quantity-sd.ReturnQuantity) as QTY
	   ,Sum(sd.Quantity-sd.ReturnQuantity)*(pr.SalePrice) SaleValue
	   ,Sum(sd.Quantity-sd.ReturnQuantity)*(pr.ModifiedUnitPrice) StockValue
	   ,sd.ProductExpireDate as ProductExpireDate
	   ,sd.SaleHeaderId as SaleHeaderId
	   ,sd.CreateDate as CreateDate
		from SaleDetails  as sd left join SaleHeaders as sh on sd.SaleHeaderId=sh.SaleHeaderId left join Products as p on sd.ProductId=p.ProductId left join Prices as pr on sd.PriceId=pr.PriceId
where sh.BranchLocationId=@branchLocationId and sh.paid=@paid and sd.Paid=@paid and (Convert(Date,sd.BillDate) >= @frmDate and Convert(Date,sd.BillDate) <= @toDate) group by sd.ProductId,sd.PriceId,sd.CreateDate,sd.ProductExpireDate,sd.SaleHeaderId,p.ProductName,p.Barcode,pr.SalePrice,pr.ModifiedUnitPrice order by sd.SaleHeaderId asc
end


USE [MalibanPosDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCustomerDeductionDetails]    Script Date: 7/7/2022 12:06:43 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date:  7/6/2022 12:37:29 PM
-- Description:	<Description,,>
-- Modified date:  7/6/2022 12:37:29 PM
-- =============================================
Create PROCEDURE [dbo].[sp_GetCustomerDeductionDetails] (@branchLocationId as int,@paid as bit,@schemeCode as int,@frmDate as Date,@toDate as Date)  
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select sh.EmployeeCode as CustomerNumber,
	    em.Number as CustomerId,
		Sum(sh.TotalAmount-sh.CustomerReturnAmount) as Total
		,em.CreditLimit as BasicSalary
		,em.FullName as FullName
		from SaleHeaders  as sh left join MI_Employer_Employees as em on sh.MI_Employer_EmployeeId=em.MI_Employer_EmployeeId
where sh.BranchLocationId=@branchLocationId and (em.SchemeCode=@schemeCode or @schemeCode=4) and sh.paid=@paid and (Convert(Date,sh.OrderDate) >= @frmDate and Convert(Date,sh.OrderDate) <= @toDate) Group by sh.EmployeeCode,em.Number,em.CreditLimit,em.FullName order by sh.EmployeeCode asc
end



USE [MalibanPosDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomerDeductionProcess]    Script Date: 7/7/2022 12:08:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: 3/8/2022 9:05 AM
-- Description:	<Description,,>
-- Modify date:	 7/6/2022 2:33:35

-- =============================================
ALTER PROCEDURE [dbo].[sp_CustomerDeductionProcess] (@branchLocationId as int,@fromDate as Date,@toDate as Date,@category as int) 
as
begin
select em.FullName,sh.EmployeeCode as CustomerEpf,em.Number as CustomerNumber,sum(sh.TotalAmount-sh.CustomerReturnAmount) as DeductionTotal from SaleHeaders  as sh left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo left join dbo.BranchLocations as b on b.BranchLocationId=@branchLocationId
where LEFT(em.Number, 2)=b.BranchCode and sh.BranchLocationId=@branchLocationId and (em.SchemeCode=@category or @category=4)  and (Convert(date,sh.OrderDate) >= @fromDate and Convert(date,sh.OrderDate) <= @toDate) Group by sh.EmployeeCode,em.Number,em.FullName
update SaleHeaders set Paid=1 
from (
select sh.SaleHeaderId as SaleHeaderId,sh.Paid as SaleHeadrPaid,sh.EmployeeCode from SaleHeaders  as sh left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo
where sh.BranchLocationId=@branchLocationId and (em.SchemeCode=@category or @category=4) and (Convert(date,sh.OrderDate) >= @fromDate and Convert(date,sh.OrderDate) <= @toDate) and sh.paid=0 Group by sh.SaleHeaderId,sh.Paid,sh.EmployeeCode
) B 
where SaleHeaders.SaleHeaderId=B.SaleHeaderId

update SaleDetails set Paid=1 
from (
select em.Number,sh.EmployeeCode,em.FullName,em.CreditLimit,sh.SaleHeaderId as SaleHeaderId,sd.Paid as SaleDetailPaid,sh.Paid as SaleHeadrPaid,PriceId,sd.ProductId,sd.BillDate,sd.CreateDate,sh.OrderTimeStamp,em.CreditBalance,em.PreviousCreditBalance from SaleDetails as sd left join SaleHeaders as sh on sd.SaleHeaderId=sh.SaleHeaderId left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo
where sh.BranchLocationId=@branchLocationId and (em.SchemeCode=@category or @category=4) and (Convert(date,sd.CreateDate) >= @fromDate and Convert(date,sd.CreateDate) <= @toDate) and (Convert(date,sh.OrderDate) >= @fromDate and Convert(date,sh.OrderDate) <= @toDate) and sd.paid=0) B 
where SaleDetails.SaleHeaderId=B.SaleHeaderId

UPDATE
      [MI_Employer_Employees] 
SET
      [MI_Employer_Employees].[CreditBalance] = 0,
	  [MI_Employer_Employees].[PreviousCreditBalance] = 0

FROM
    (
select sh.EmployeeCode,em.Number from SaleHeaders  as sh left join MI_Employer_Employees as em on sh.EmployeeCode=em.EPFNo
where sh.BranchLocationId=@branchLocationId and (em.SchemeCode=@category or @category=4) and (Convert(date,sh.OrderDate) >= @fromDate and Convert(date,sh.OrderDate) <= @toDate) and sh.paid=1 Group by sh.EmployeeCode,em.Number
    ) B
Where 
    [MI_Employer_Employees].Number  = B.Number


end


USE [Test]
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteUserPermissionById]    Script Date: 7/20/2022 3:05:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_DeleteUserPermissionById](@id as int) 
AS
BEGIN
DECLARE @TestVariable AS nvarchar(MAX)
SET @TestVariable = (select AccessControllerName from RolePermissions where Id=@id)
--Print @TestVariable
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
--IF @TestVariable is not null
--    BEGIN
--	       PRINT 'Failed,Try again ';
--    Print 'The error message is'
--END;
--ELSE
--BEGIN
BEGIN TRY
BEGIN tran
IF @TestVariable is not null 
    BEGIN
	Delete from RolePermissions where Id=@id
	SELECT 1
	end;
else
begin
        SELECT  -1;
END;
	COMMIT TRAN
--select * from RolePermissions
END TRY
BEGIN CATCH
	ROLLBACK TRAN
	--SELECT ERROR_MESSAGE() AS Description, ERROR_LINE() as Code
	--SELECT ERROR_MESSAGE() AS [Error Message]
 --        ,ERROR_LINE() AS ErrorLine
 --        ,ERROR_NUMBER() AS [Error Number]  
 --        ,ERROR_SEVERITY() AS [Error Severity]  
 --        ,ERROR_STATE() AS [Error State]  
 --THROW 
   --PRINT 'The error message is: ' + error_message()
END CATCH


--END;
	-- SET NOCOUNT ON added to prevent extra result sets from
END




USE [Test]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUserPermissionById]    Script Date: 7/20/2022 3:10:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: 5/5/2022 1:44 AM
-- Description:	<Description,,>
-- Modify date:	

-- =============================================
ALTER PROCEDURE [dbo].[sp_GetUserPermissionById](@id as int) 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT r.[Id]
      ,[Allow]
      ,[ApplicationRoleId]
      ,[ApplicationUserId]
      ,[AccessControllerName]
      ,m.Name as MainMenu
      ,sm.Name as SubMenu
      ,smi.Name as SubMenuItem
	  ,ro.Name as RoleName
	  ,u.UserName as UserName
  FROM [Test].[dbo].[RolePermissions] as r 
  left join Test.dbo.MainMenus as m on r.MainMenuId=m.MainMenuId 
  left join Test.dbo.Submenus as sm on r.SubmenuId=sm.SubmenuId
  left join Test.dbo.SubmenuItems as smi on r.SubmenuItemId=smi.SubmenuItemId
  left join Test.dbo.AspNetRoles as ro on r.ApplicationRoleId=ro.Id
  left join Test.dbo.AspNetUsers as u on r.ApplicationUserId=u.Id where ApplicationUserId=@id order by m.Name,sm.Name
END



USE [Test]
GO
/****** Object:  StoredProcedure [dbo].[sp_CreateUserPermissionById]    Script Date: 7/20/2022 3:11:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[sp_CreateUserPermissionById](@allow as bit,@submenuId as int,@submenuItemId as int,@applicationRoleId as int,@applicationUserId as int,@accessControllerName as nvarchar(MAX),@mainMenuId as int) 
AS
BEGIN
DECLARE @TestVariable AS nvarchar(MAX)
SET @TestVariable = (select AccessControllerName from RolePermissions where ApplicationUserId=@applicationUserId and AccessControllerName=@accessControllerName)
--Print @TestVariable
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
--IF @TestVariable is not null
--    BEGIN
--	       PRINT 'Failed,Try again ';
--    Print 'The error message is'
--END;
--ELSE
--BEGIN
BEGIN TRY
BEGIN tran
IF @TestVariable is null 
    BEGIN
	INSERT INTO RolePermissions ([Allow],[SubmenuId],[SubmenuItemId],[ApplicationRoleId],[ApplicationUserId],[AccessControllerName],[MainMenuId]) VALUES (@allow,@submenuId,@submenuItemId,@applicationRoleId,@applicationUserId,@accessControllerName,@mainMenuId)
	SELECT CAST(SCOPE_IDENTITY() as int)
	end;
else
begin
        SELECT  -1;
END;
	COMMIT TRAN
--select * from RolePermissions
END TRY
BEGIN CATCH
	ROLLBACK TRAN
	--SELECT ERROR_MESSAGE() AS Description, ERROR_LINE() as Code
	--SELECT ERROR_MESSAGE() AS [Error Message]
 --        ,ERROR_LINE() AS ErrorLine
 --        ,ERROR_NUMBER() AS [Error Number]  
 --        ,ERROR_SEVERITY() AS [Error Severity]  
 --        ,ERROR_STATE() AS [Error State]  
 --THROW 
   --PRINT 'The error message is: ' + error_message()
END CATCH


--END;
	-- SET NOCOUNT ON added to prevent extra result sets from
END



-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
alter PROCEDURE sp_GetProductBySupplierFilterByProduct(@supplierId int) 
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT p.[ProductId]
        ,[Barcode]
      ,[ProductName]
      FROM [Test].[dbo].[Products] as p where p.ProductId not in (select ProductId from [dbo].[SupplierProducts] as sp where sp.SupplierId=@supplierId) 
	  
  END
GO



//////////////////////////////////////////////////////////
Customer deduction process v1 start
/////////////////////////////////////////

USE [MalibanPosDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCustomerDeductionDetails_v1]    Script Date: 10/14/2022 10:06:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		Gnanam
-- Create date	14-Oct-2022
-- Description	Download the Dedection base on given date range and location
-- Modified 
-- =============================================
ALTER PROCEDURE [dbo].[sp_GetCustomerDeductionDetails_v1] (@branchLocationId as int,@schemeCode as int,@frmDate as Date,@toDate as Date)  

AS
BEGIN

	SELECT E.EPFNo 'CustomerNumber', E.Number 'CustomerId',E.FullName 'FullName' ,SUM(MD.CreditBalance) 'Total' , AVG(E.CreditLimit) 'BasicSalary'
	FROM MonthEndProcessHeaders MH 
	INNER JOIN MonthEndProcessDetails MD ON MD.MEPHeaderId=MD.MEPHeaderId
	INNER JOIN MI_Employer_Employees E ON E.MI_Employer_EmployeeId=MD.MI_Employer_EmployeeId
	WHERE MH.BranchLocationId=@branchLocationId AND MH.SchemeCode=@schemeCode AND (Convert(Date,MH.FromDate) >= @frmDate and Convert(Date,MH.ToDate) <= @toDate)
	GROUP BY E.EPFNo,E.Number,E.FullName
END






//////////////////////////////////////////////////////////
Customer deduction process v1 start
/////////////////////////////////////////



USE [MalibanPosDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_CustomerDeductionProcess_V1]    Script Date: 10/14/2022 9:56:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		Gnanam
-- Create date	14-Oct-2022
-- Description	MONTHS END PROCESS FOR DEDECTION CALCUATIONS
-- Modified 
-- =============================================
ALTER PROCEDURE [dbo].[sp_CustomerDeductionProcess_V1] (@branchLocationId as int,@fromDate as Date,@toDate as Date,@category as int,@UserID as int) 
as
begin

DECLARE @mepId bigint
DECLARE @SalesDT TABLE (tSalesOrderId bigint , tTotalSalesAmount decimal(18,2) , tTotalReturnAmount decimal (18,2) , tEmpNoid bigint, tCreditLimit float )

--SET @fromDate = '1/8/2022'
--SET @toDate = '10/10/2022'
--SET @branchLocationId=4
--set @category=2
--set @UserID=1

--GET THE SALES ORDER WITHIN GIVEN RANGE
INSERT INTO @SalesDT (tSalesOrderId,tEmpNoid,tCreditLimit)
SELECT SH.SaleHeaderId , SH.MI_Employer_EmployeeId , ISNULL(E.CreditLimit,0) FROM SaleHeaders SH 
INNER JOIN BranchLocations B ON B.BranchLocationId=SH.BranchLocationId
INNER JOIN MI_Employer_Employees E ON E.MI_Employer_EmployeeId=SH.MI_Employer_EmployeeId AND B.BranchCode=E.Location
WHERE (Convert(date,sh.OrderDate) >= @fromDate and Convert(date,sh.OrderDate) <= @toDate) 
AND B.BranchLocationId=@branchLocationId AND SH.Paid=0 and e.SchemeCode=@category

--GET THE TOTAL SALES AMOUNT , RETURN AMOUNT
;WITH TotalSalesDetails
	AS (
		SELECT SH.tSalesOrderId ,SUM(p.SalePrice * sd.Quantity) 'TOT' , SUM(p.SalePrice * sd.ReturnQuantity) 'RTOT'
		FROM [MalibanPosDB].[dbo].[SaleDetails] as sd inner join @SalesDT as sh on sd.SaleHeaderId=sh.tSalesOrderId 
		inner join Prices p on p.PriceId=sd.PriceId
		GROUP BY SH.tSalesOrderId
		)
	UPDATE S SET S.tTotalSalesAmount=T.TOT , S.tTotalReturnAmount=T.RTOT 
	FROM  @SalesDT S INNER JOIN TotalSalesDetails T ON S.tSalesOrderId=T.tSalesOrderId

IF EXISTS (SELECT * FROM @SalesDT)
	BEGIN
		--==========INSERT THE MONTHS END PROCESS HEADER=========
		INSERT INTO [MalibanPosDB].[dbo].[MonthEndProcessHeaders] (CreatedBy,CreateDate,FromDate,ToDate,BranchLocationId,TotalAmount,TotalReturnAmount,SchemeCode)
		SELECT @UserID,GETDATE(),@fromDate,@toDate,@branchLocationId,SUM(tTotalSalesAmount),SUM(tTotalReturnAmount),@category  FROM @SalesDT

		SET @mepId = SCOPE_IDENTITY();

		--==========INSERT THE MONTHS END PROCESS DEAILS =========
		INSERT INTO MonthEndProcessDetails (MI_Employer_EmployeeId,MEPHeaderId,CreditLimit,CreditBalance)
		SELECT tEmpNoid,@mepId,AVG(tCreditLimit), SUM(tTotalSalesAmount-tTotalReturnAmount)
		FROM @SalesDT
		GROUP BY tEmpNoid

		--==========RE UPDATE THE TOTAL AMOUNT AND MARKED PAID TO SALES HEADER
		UPDATE SH SET SH.TotalAmount=S.tTotalSalesAmount , SH.CustomerReturnAmount=S.tTotalReturnAmount , SH.Paid=1
		FROM @SalesDT S INNER JOIN SaleHeaders SH ON SH.SaleHeaderId=S.tSalesOrderId

		--===========MARKED AS PAID TO SALES DETAILS====================
		UPDATE SD SET SD.Paid=1
		FROM @SalesDT S INNER JOIN SaleDetails SD ON SD.SaleHeaderId=S.tSalesOrderId


		---RESET THE CREDIT BALANCE AND RE UPDATE FROM THE UNPAID SALES 
		UPDATE MI_Employer_Employees SET CreditBalance=0

		;WITH TotalSalesEmployee
			   AS (
					  SELECT SH.MI_Employer_EmployeeId, sum(p.SalePrice * sd.Quantity) 'TotAmt', sum(p.SalePrice * sd.ReturnQuantity) 'TotRtnAmt'
					  FROM [MalibanPosDB].[dbo].[SaleDetails] as sd inner join SaleHeaders as sh on sd.SaleHeaderId=sh.SaleHeaderId 
					  inner join Prices p on p.PriceId=sd.PriceId
					  where sh.Paid=0
					  group by SH.MI_Employer_EmployeeId
					  )
			   UPDATE E SET E.CreditBalance=T.TotAmt-T.TotRtnAmt
			   FROM MI_Employer_Employees E INNER JOIN TotalSalesEmployee T ON T.MI_Employer_EmployeeId=E.MI_Employer_EmployeeId

	END

END
























