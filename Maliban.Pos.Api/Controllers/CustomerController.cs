using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Maliban.Pos.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Syncfusion.XlsIO;
using Newtonsoft.Json;
using System.Collections.Generic;
using Maliban.Pos.Api.Dtos;
using System.ComponentModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using static Maliban.Pos.Api.Models.Status;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Maliban.Pos.Api.Helper;
using Maliban.Pos.Api.Helpers;
using System.Runtime.InteropServices;
using Dapper;

namespace Maliban.Pos.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]
    public class CustomerController : BaseController
    {
        private readonly IConfiguration _config;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly ILog logger;
        private readonly IRepositoryWrapper _repoWrapper;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly DapperContext _dpContext;
        private readonly ISqlDataAccess _db;

        [Obsolete]
        public CustomerController(IConfiguration config, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signinManager, IMapper mapper, DataContext context, ILog logger, IHostingEnvironment hostingEnviroment, IRepositoryWrapper repoWrapper, DapperContext dpContext, ISqlDataAccess db) : base(config, userManager, signinManager, mapper, context, logger, hostingEnviroment)
        {
            _config = config;
            _userManager = userManager;
            _signinManager = signinManager;
            _mapper = mapper;
            _context = context;
            this.logger = logger;
            _repoWrapper = repoWrapper;
            _hostingEnviroment = hostingEnviroment;
            _dpContext = dpContext;
            _db = db;
        }
        /*                   /// <summary>
           /// Get Customers 
           /// </summary>
           /// <param name="syncallcustomers"></param>
         //  [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
           [Authorize(Policy = "UserAccess")]
           [HttpGet("syncallcustomers", Name = "SyncAllCustomers")]
           [MapToApiVersion("1.1")]
           public async Task<IActionResult> SyncAllCustomers([Optional]bool status, int BranchLocationId)
           {
               return Ok();
           } */
        /// <summary>
        /// Update Customer
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int? id)
        {
            // Check that the record exists.
            var entity = await _context.MI_Employer_Employees.Where(s => s.MI_Employer_EmployeeId == id).FirstOrDefaultAsync<MI_Employer_Employee>();

            if (entity == null)
            {
                return BadRequest("You've unable to find record with id '" + id + "'.");
            }
            return Ok(entity);
        }
        /// <summary>
        /// Update Customer
        /// </summary> 
        /// <param name="{id}"></param> 
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int? id, [FromForm] UpdateCustomerDto updateCustomerDto)
        {
            // Check that the record exists.
            var entity = await _context.MI_Employer_Employees.Where(s => s.MI_Employer_EmployeeId == id).SingleAsync<MI_Employer_Employee>();

            if (entity == null)
            {
                return BadRequest("You've unable to find record with id '" + id + "'.");
            }
            // Update changes if any of the properties have been modified.
            _context.Entry(entity).CurrentValues.SetValues(updateCustomerDto);
            _context.Entry(entity).State = EntityState.Modified;
            try
            {

                if (_context.Entry(entity).Properties.Any(property => property.IsModified))
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                //return BadRequest(e);
                return Conflict(ex);
            }
            return Ok(entity);

        }
        /// <summary>
        /// Get Customers 
        /// </summary>
        /// <param name="getallcustomers"></param>
        //  [TypeFilter(typeof(CustomAuthorizationFilterAttribute))]
        [Authorize(Policy = "UserAccess")]
        [HttpGet("getallcustomers", Name = "GetAllCustomers")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllCustomers([Optional] bool status, [FromQuery] UserParams userParams, int BranchLocationId)
        {
            var customers = await _repoWrapper.Employee.GetEmployersAsync(userParams, BranchLocationId);
            // Headers get 
            Request.Headers.TryGetValue("BranchLocationId", out var traceValue);

            // var suppliersToReturn = _mapper.Map<IEnumerable<SupplierListDto>>(suppliers.AsEnumerable());
            if (customers.Count() == 0)
            {
                return NoContent();
            }
            switch (status)
            {
                case true:
                    var resulttrue = customers.Where(p => p.Deleted == 1);
                    Response.AddPagination(customers.CurrentPage, customers.PageSize, customers.TotalCount, customers.TotalPages);
                    return Ok(resulttrue);
                case false:
                    var resultfalse = customers;//.Where(p => p.Deleted == 0);
                    Response.AddPagination(customers.CurrentPage, customers.PageSize, customers.TotalCount, customers.TotalPages);
                    return Ok(resultfalse);
                //default:
                //    Response.AddPagination(customers.CurrentPage, customers.PageSize, customers.TotalCount, customers.TotalPages);
                //    return Ok(customers);
            }

        }
        /// <summary>
        /// Search Employee By EmpCode 
        /// </summary>
        /// <param name="postmapemployeebyempcode"></param>
        // [ResponseCache(Duration = 60)]
        [HttpPost("postmapemployeebyempcode", Name = "PostMapEmployeeByEmpCode")]
        public async Task<IActionResult> PostMapEmployeeByEmpCode([FromForm]CreateBranchMapLocation createBranchMapLocation)
        {
            var branchCode = await _context.BranchLocations.Where(b => b.BranchLocationId == createBranchMapLocation.branchCodeId).FirstOrDefaultAsync();
            var tomapBranchCode = await _context.BranchLocations.Where(b => b.BranchLocationId == createBranchMapLocation.tomapBranchCodeId).FirstOrDefaultAsync();
            using var connection = _dpContext.CreateConnection();
            var procedureName1 = "sp_AddEmployeeToMapByLocation";
            var parameters = new DynamicParameters();
            parameters.Add("branchCode", branchCode.BranchCode, DbType.String);
            parameters.Add("epfNo", createBranchMapLocation.epfNo, DbType.Int32);
            parameters.Add("tomapBranchCode", tomapBranchCode.BranchCode, DbType.String);
            var mapEmployees = (await connection.QueryAsync<EmployeeDto>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
            .ConfigureAwait(false)).AsList();
            if (mapEmployees.Count > 0)
            {
                return Ok(mapEmployees);
            }
            return NotFound($"Invalid branch id is {createBranchMapLocation.branchCodeId}");
        }
        /// <summary>
        /// Search Employee By EmpCode 
        /// </summary>
        /// <param name="searchmap/{branchlocationId}"></param>
        // [ResponseCache(Duration = 60)]
        [HttpGet("searchmap/{branchlocationId}", Name = "SearchMapEmployeeByEmpCode")]
        public async Task<IActionResult> SearchMapEmployeeByEmpCode(int branchlocationId)
        {
            using var connection = _dpContext.CreateConnection();
            var procedureName1 = "sp_GetEmployeeMapByLocation";
            var parameters = new DynamicParameters();
            parameters.Add("branchLocationId", branchlocationId, DbType.Int32);
            var mapEmployees = (await connection.QueryAsync<EmployeeDto>(procedureName1, parameters, commandType: CommandType.StoredProcedure)
            .ConfigureAwait(false)).AsList();
            if (mapEmployees.Count > 0)
            {
                return Ok(mapEmployees);
            }
            return NotFound($"Invalid branch id is {branchlocationId}");
        }
        /// <summary>
        /// Search Employee By EmpCode 
        /// </summary>
        /// <param name="search/{branchlocationId}/{empcode}"></param>
        // [ResponseCache(Duration = 60)]
        [HttpGet("search/{branchCode}/{empcode}", Name = "SearchEmployeeByEmpCode")]
        public async Task<IActionResult> SearchEmployeeByEmpCode(string empcode, string branchCode)
        {
            string code = empcode.TrimStart(new Char[] { '0' });
            Utility utility = new Utility(_repoWrapper, _context);
            var branch = await _context.BranchLocations.Where(b => b.BranchCode == branchCode).FirstOrDefaultAsync();
            if (await _context.MI_Employer_Employees.AnyAsync(c => c.EPFNo == code && c.Deleted == 0 && c.Number.Contains(branch.BranchCode)))
            {
                var employeeid = _context.MI_Employer_Employees.Where(m => m.EPFNo == code && m.Location == branch.BranchCode).Select(c => new { c.MI_Employer_EmployeeId }).FirstOrDefault();
                var employee = _context.MI_Employer_Employees.Where(e => e.MI_Employer_EmployeeId == employeeid.MI_Employer_EmployeeId).FirstOrDefault();
                employee.CreditLimit = utility.CreditBalanceCalculation(employee.Number, branch.BranchLocationId);
                var emp = _mapper.Map<EmployeeDto>(employee);
                return Ok(emp);
            }
            return NotFound($"Invalid employee {empcode}");

        }
        /// <summary>
        /// Get All District 
        /// </summary>
        /// <param name="getallprocessdate"></param>
        [HttpGet("searchdate/{branchCode}/{schemeCode}", Name = "GetAllProcessDate")]
        [MapToApiVersion("1.1")]
        public async Task<IActionResult> GetAllProcessDate(int schemeCode, int branchCode)
        {
            if (schemeCode!>0 && branchCode>0)
            {
                var reports = await _context.MonthEndProcessHeaders.Where(m => m.BranchLocationId== branchCode && m.SchemeCode==schemeCode).ToListAsync();
                var reportDates = _mapper.Map<IEnumerable<ReportDate>>(reports);
                return Ok(reportDates);
            }
               return NotFound("Don't have any Dates.");
        }
        /// <summary>
        /// Get Employee By EmpCode 
        /// </summary>
        /// <param name="{branchlocationId}/{empcode}"></param>
        // [ResponseCache(Duration = 60)]
        [HttpGet("{branchlocationId}/{empcode}", Name = "GetEmployeeByEmpCode")]
        public async Task<IActionResult> GetEmployeeByEmpCode(string empcode, int branchlocationId)
        {
            #region
           /* string code = empcode.TrimStart(new Char[] { '0' });
            Utility utility = new Utility(_repoWrapper, _context);
            var branch = _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == branchlocationId).Single<BranchLocation>();
            var employees = await _repoWrapper.Employee.FindByCondition(c => c.EPFNo == code && c.Deleted == 0).ToListAsync();

            if (employees == null)
            {
                return NotFound($"Invalid employee {code}");
            }
            if (await _repoWrapper.VerifyEmpCode(code) && employees.Any(c => c.Number.Contains(branch.BranchCode)))
            {
                var emp = _mapper.Map<EmployeeDto>(employees.Where(c => c.Number.Contains(branch.BranchCode)).FirstOrDefault());
                emp.CreditLimit = utility.CreditBalanceCalculation(emp.Number, branchlocationId);
                if (emp != null)
                {
                    return Ok(emp);
                }
            }
            if (await _repoWrapper.VerifyEmpCode(code) && ((employees.Any(c => c.Number.Contains("TR")) && branch.BranchCode == "HO") || (employees.Any(c => c.Number.Contains("WW")) && branch.BranchCode == "BD")) || (employees.Any(e => e.Location == branch.BranchCode)))
            {
                var emp = _mapper.Map<EmployeeDto>(employees.Where(c => c.Number.Contains("WW") || c.Number.Contains("TR") || (c.EPFNo == code && c.Location == branch.BranchCode)).FirstOrDefault());
                emp.CreditLimit = utility.CreditBalanceCalculation(emp.Number, branchlocationId);
                if (emp != null)
                {
                    return Ok(emp);
                }
            }
            return NotFound($"Invalid employee {code}");*/
            #endregion
             string code = empcode.TrimStart(new Char[] { '0' });
            Utility utility = new Utility(_repoWrapper, _context);
             var branch = await _context.BranchLocations.Where(b => b.BranchLocationId == branchlocationId).FirstOrDefaultAsync();
            if (await _context.MI_Employer_Maps.AnyAsync(m => m.EPFNo == code && m.BranchLocationId == branch.BranchLocationId))
            {
                var employeeid = await _context.MI_Employer_Maps.Where(m => m.EPFNo == code && m.BranchLocationId == branch.BranchLocationId).Select(c => new { c.MI_Employer_EmployeeId }).FirstOrDefaultAsync();
                var employee = await _context.MI_Employer_Employees.Where( e => e.MI_Employer_EmployeeId==employeeid.MI_Employer_EmployeeId && e.Deleted == 0).FirstOrDefaultAsync();
                if (employee == null) { return NotFound($"Invalid employee {empcode}"); }
                employee.CreditLimit = utility.CreditBalanceCalculation(employee.Number, branchlocationId);
                var emp = _mapper.Map<EmployeeDto>(employee);
                return Ok(emp);
            }
            if (await _context.MI_Employer_Employees.AnyAsync(c => c.EPFNo == code && c.Deleted == 0 && c.Number.Contains(branch.BranchCode)))
            {
                var employee = _context.MI_Employer_Employees.Where(m => m.EPFNo == code && m.Location == branch.BranchCode && m.Deleted == 0).FirstOrDefault();
                employee.CreditLimit = utility.CreditBalanceCalculation(employee.Number, branchlocationId);
                var emp = _mapper.Map<EmployeeDto>(employee);
                return Ok(emp);
            }
          
            return NotFound($"Invalid employee {empcode}");

        }
        /// <summary>
        /// Get Employee By EmpCode 
        /// </summary>
        /// <param name="employee/{saleHeaderId}/{branchlocationId}"></param>
        [HttpGet("employee/{saleHeaderId}/{branchlocationId}", Name = "GetEmployeeBySaleHeader")]
        public async Task<IActionResult> GetEmployeeBySaleHeader(int saleHeaderId, int branchlocationId)
        {
            var EmployeeCode = await _context.SaleHeaders.Where(s => s.SaleHeaderId == saleHeaderId && s.BranchLocationId == branchlocationId).Select(c => new { c.EmployeeCode, c.BranchLocationId, c.MI_Employer_EmployeeId }).FirstOrDefaultAsync();
            if (EmployeeCode == null)
            {
                return NotFound($"Please check your bill number? {saleHeaderId}");
            }
            var branch = _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == branchlocationId).Single<BranchLocation>();
            var employee = await _repoWrapper.Employee.FindByCondition(c => c.MI_Employer_EmployeeId == EmployeeCode.MI_Employer_EmployeeId && c.EPFNo == EmployeeCode.EmployeeCode).FirstOrDefaultAsync() ?? null;

            if (employee != null)
            {
                var emp = _mapper.Map<EmployeeDto>(employee);
                return Ok(emp);
            }
            //var employee = await _repoWrapper.Employee.GetEmployeeAsync(EmployeeCode.EmployeeCode);
            /*             if (await _repoWrapper.VerifyEmpCode(employee.EPFNo) && employee.Number.Contains(branch.BranchCode))
                        {
                            if (employee != null)
                            {
                                return Ok(employee);
                            }
                        }
                        if (await _repoWrapper.VerifyEmpCode(employee.EPFNo) && employee.Number.Contains("TR"))

                        {
                            if (employee != null)
                            {
                                return Ok(employee);
                            }
                        } */
            return NotFound($"Invalid employee {employee.EPFNo}");
        }
        /// <summary>
        ///  Dummy classstudent upload.
        /// </summary>
        /// <param name="uploadexcel"></param>
        [HttpPost("uploadexcel")]
        [Obsolete]
        public async Task<IActionResult> UploadExcel([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile classstudentExcel = createUploadExcelDto.Files;
            if (classstudentExcel.Length > 0)
            {
                string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
                string uniqueFileName = classstudentExcel.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var iNeedToLearnAboutDispose = new FileStream(filePath, FileMode.Create))
                {
                    await classstudentExcel.CopyToAsync(iNeedToLearnAboutDispose);
                }
                return Ok("Successfully uploaded to the folder");
            }
            return BadRequest("Sorry, File does not exist or content type mismatch.");
        }
        /// <summary>
        ///   update lunch token to the system
        /// </summary>
        /// <param name="updatelunchtoken"></param>
        [HttpPost("updatelunchtoken")]
        [Obsolete]
        public async Task<IActionResult> UpdateLunchToken([FromForm] CreateUploadExcelLunchTokenDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            int result;
               IList<string> errorList = new List<string>();
            IList<SaleLunchByEmployee> users = new List<SaleLunchByEmployee>();
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                users = worksheet.ExportData<SaleLunchByEmployee>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
            }
            //output path
            foreach (var user in users)
            {
                decimal ComputeGrandTotalValue=0;
                var branch = await _repoWrapper.BranchLocation.FindByCondition(b => b.BranchLocationId == createUploadExcelDto.BranchLocationId).FirstOrDefaultAsync();
                var customer = await _repoWrapper.Employee.FindByCondition(c => c.Number == user.EmployeeNumber && c.Number.Contains(branch.BranchCode) && c.Deleted ==0).FirstOrDefaultAsync();
                if (customer != null)
                {
                    IList<SaleDetail> lineCollection = new List<SaleDetail>();
                    var getAvailableStock = _db.LoadData<CartItemList, dynamic>("dbo.sp_GetSalesByFIFO", new { xBranchlocation = createUploadExcelDto.BranchLocationId, xBarcode = user.BarCode, xQty = user.Quantity }).Result.AsQueryable();
                    foreach(var item in getAvailableStock)
                    {
                        // var stockDetailss = await _context.TransactionDetails.Where(s => s.IsDeleted == false && s.BranchLocationId == createUploadExcelDto.BranchLocationId && s.dQuantity-s.dIssueQuantity > 0 && s.Barcode == user.BarCode).GroupBy(t => new { t.ProductId, t.dSalePrice,t.dCostPrice, t.Barcode, t.ProductName, t.ProductExpireDate }).Select(p => new { ProductId = p.Key.ProductId, dNetQuantity = p.Sum(t => t.dQuantity-t.dIssueQuantity), dSalePrice = p.Key.dSalePrice, dCostPrice= p.Key.dCostPrice, Barcode = p.Key.Barcode, ProductName = p.Key.ProductName, ProductExpireDate = p.Key.ProductExpireDate }).FirstOrDefaultAsync();
                        var product = await _repoWrapper.Product.GetProductAsync(user.BarCode);
                        decimal stockQty = await _repoWrapper.ShopStock.StockDetailQuantityAsync(product.ProductId, createUploadExcelDto.BranchLocationId, item.dSalePrice, item.dCostPrice);
                        //IPriceRepository GetPriceAsync
                       // var price = await _repoWrapper.Price.GetPriceAsync(product.ProductId, createUploadExcelDto.BranchLocationId, item.dSalePrice);
                        var price = await _repoWrapper.Price.FindByCondition(p => p.ProductId == item.ProductId && p.BranchLocationId == createUploadExcelDto.BranchLocationId && p.PriceId == item.PriceId).FirstOrDefaultAsync();

                        lineCollection.Add(new SaleDetail
                        {
                            Product = product,
                            PriceId = price.PriceId,
                            ProductId = product.ProductId,
                            ItemPrice = price.SalePrice,
                            // ItemDiscount = price.DiscountPrecentage,
                            ItemDiscount = 0,
                            Quantity = user.Quantity,
                            ReturnQuantity = 0,
                            StockQuantity = stockQty,
                            Total = (price.SalePrice) * (user.Quantity),
                            SubTotal = (price.SalePrice) * (user.Quantity),
                            BillDate = user.OrderDate,
                            ProductExpireDate = item.ProductExpireDate,
                            CreateDate = user.OrderDate,
                            CreatedBy = createUploadExcelDto.CreatedBy,
                            ActiveBill = false,
                            TransactionDetailId=item.TransactionDetailId
                        });
                        ComputeGrandTotalValue += (price.SalePrice) * (user.Quantity);
                    }
                
                    var saleitem = new SaleHeader
                    {
                        PaymentId = 1,
                        CreatedBy = createUploadExcelDto.CreatedBy,
                        TotalAmount = ComputeGrandTotalValue,
                        Id = createUploadExcelDto.Id,
                        BranchLocationId = branch.BranchLocationId,
                        MI_Employer_EmployeeId = customer.MI_Employer_EmployeeId,
                        EmployeeCode = customer.EPFNo,
                        OrderDate = user.OrderDate,
                        OrderTimeStamp = user.OrderDate,
                        Lines = lineCollection
                    };

                    var strategy = _context.Database.CreateExecutionStrategy();
                    await strategy.ExecuteAsync(async () =>
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                _context.AttachRange(saleitem.Lines);
                                _context.SaleHeaders.Add(saleitem);
                                result = _context.SaveChanges();
                                if (result > 0)
                                {
                                    foreach (var tr in getAvailableStock)
                                    {
                                        var updateTransaction = await _context.TransactionDetails.Where(t => t.TransactionDetailId == tr.TransactionDetailId && t.dQuantity - t.dIssueQuantity > 0).FirstOrDefaultAsync();
                                        if (updateTransaction.dQuantity - updateTransaction.dIssueQuantity >= tr.dNetQuantity)
                                        {
                                            updateTransaction.dIssueQuantity += tr.dNetQuantity;
                                            _context.TransactionDetails.Update(updateTransaction);
                                            _context.SaveChanges();
                                        }
                                        logger.Information("{\"BranchLocationId\":" + updateTransaction.BranchLocationId + ",\"Barcode\":\"" + updateTransaction.Barcode +
                                            "\",\"ProductId\":" + updateTransaction.ProductId + ",\"MI_Employer_EmployeeId\":" + saleitem.MI_Employer_EmployeeId + ",\"PriceId\":" + updateTransaction.PriceId + ",\"dQuantity\":" + updateTransaction.dQuantity +
                                            ",\"dIssueQuantity\":" + updateTransaction.dIssueQuantity + ",\"dSaleQuantity\":" + tr.dNetQuantity + ",\"dReturnQuantity\":" + 0 +
                                            ",\"TransactionDetailId\":" + updateTransaction.TransactionDetailId + ",\"CreateDate\":\"" + DateTime.Now.ToString() + "\"}");
                                    }
                                    await _repoWrapper.ShopStock.UpdateSaleTransaction(lineCollection.ToList(), createUploadExcelDto.BranchLocationId, createUploadExcelDto.CreatedBy);
                                    await _repoWrapper.ShopStock.UpdateEmployeeBalanceAsync(customer.MI_Employer_EmployeeId, ComputeGrandTotalValue, createUploadExcelDto.CreatedBy);
                                }
                                transaction.Commit();
                                errorList.Add($"Successfully saved the code {customer.EPFNo} to employee table.");

                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                logger.Error("EmployeeCode" + user.EmployeeNumber + string.Join(',', lineCollection) + e.InnerException + e.Message + e.HelpLink + e.HResult + e.StackTrace);
                                errorList.Add("EmployeeCode" + user.EmployeeNumber + string.Join(',', lineCollection) + e.Message);
                                //return BadRequest(e);
                            }
                        }

                    });
                }
                else
                {
                    logger.Error("EmployeeCode" + user.EmployeeNumber);
                    errorList.Add("EmployeeCode" + user.EmployeeNumber);
                }

            }


            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy classstudent initialize to the system
        /// </summary>
        /// <param name="initialemployee"></param>
        [HttpPost("initialemployee")]
        [Obsolete]
        public async Task<IActionResult> InitialEmployee([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(uploadsFolder);
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Loop over all files in C directory.
            //Copy File names to Model collection.
            /*     List<FileModel> files = new List<FileModel>();
                foreach (string fileEntry in fileEntries)
                {
                    files.Add(new FileModel { FileName = Path.GetFileName(fileEntry) });
                } */
            //   string uniqueFileName = Path.GetFileName(fileEntries.Last());
            //string uniqueFileName = employeeExcel.FileName;
            //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);
                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<Employee> users = worksheet.ExportData<Employee>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                fileStream.Close();
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, users);

                }

            }
            var result = await Initialize(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath);
            return Ok(result);
        }
        private async Task<ActionResult> Initialize(DataContext context, string jsonfilename, string wwwrootpath)
        {
            int result;
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind1>>(JSON);
            IList<SchemeList> schemeList = new List<SchemeList> {
                new SchemeList {Scheme="Worker",SchemeCode=1},
                new SchemeList {Scheme="Staff",SchemeCode=2},
                new SchemeList {Scheme="Executive",SchemeCode=3}
            };
            foreach (var item in root)
            {
                var schme = schemeList.Where(s => s.SchemeCode == item.SchemeCode || s.Scheme == item.Scheme.ToUpper()).Select(s => new { s.SchemeCode, s.Scheme }).FirstOrDefault();
                if (!CodeExists(item.EmployeeNumber) && schme != null)
                {
                    var classstudent = new MI_Employer_Employee
                    {
                        Number = item.EmployeeNumber,
                        FullName = item.Name,
                        EPFNo = item.EPFNo,
                        SchemeCode = schme.SchemeCode,
                        Deleted = item.Deleted,
                        CreditLimit = item.CreditLimit,
                        Scheme = schme.Scheme,
                        DateOfAppointment = item.DateOfAppointment,
                        DateResigned = item.DateResigned,
                        Location = item.EmployeeNumber.Substring(0, 2),
                        //CreditBalance = item.CreditBalance
                    };
                    context.MI_Employer_Employees.Add(classstudent);
                    try
                    {
                        result = await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully saved the code {item.EPFNo} to employee table.");
                }
                if ((CodeExists(item.EmployeeNumber) && item.Deleted == 1))
                {
                    /*var classstudent1 = new MI_Employer_Employee
                    {
                      SchemeCode = item.SchemeCode,
                      Deleted = item.Deleted,
                      CreditLimit = item.CreditLimit,
                      Scheme = item.Scheme,
                      DateResigned = item.DateResigned, 
                    }; */
                    var employee = context.MI_Employer_Employees.Single(e => e.Number == item.EmployeeNumber);
                    employee.Deleted = item.Deleted;
                    context.MI_Employer_Employees.Update(employee);
                    try
                    {
                        result = await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully update the code {item.EPFNo} to employee table.");
                    errorList.Add($"duplicate employee exsits, the employee is {item.EPFNo} .");
                }
            }
            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy product initialize to the system
        /// </summary>
        /// <param name="initialproduct"></param>
        [HttpPost("initialproduct")]
        [Obsolete]
        public async Task<IActionResult> InitialProduct([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummyProduct> product = worksheet.ExportData<DummyProduct>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, product);

                }

            }
            var result = await InitializeP(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath);
            return Ok(result);
        }
        private async Task<ActionResult> InitializeP(DataContext context, string jsonfilename, string wwwrootpath)
        {
            int result;
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind2>>(JSON);
            foreach (var item in root)
            {
                if (!ProductCodeExists(item.Barcode))
                {
                    var product = new Product
                    {
                        CreatedBy = item.CreatedBy,
                        Barcode = item.Barcode,
                        ProductName = item.ProductName,
                    };
                    await context.Products.AddAsync(product);
                    try
                    {
                        result = await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully saved the code {item.Barcode} to product table.");
                }
                if (ProductCodeExists(item.Barcode))
                {
                    errorList.Add($"duplicate product exsits, the product is {item.Barcode} .");
                }
            }
            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy product initialize to the system
        /// </summary>
        /// <param name="initialproductupdate"></param>
        [HttpPost("initialproductupdate")]
        [Obsolete]
        public async Task<IActionResult> InitialProductUpdate([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummyProduct> product = worksheet.ExportData<DummyProduct>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, product);

                }

            }
            var result = await InitializePU(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath);
            return Ok(result);
        }
        private async Task<ActionResult> InitializePU(DataContext context, string jsonfilename, string wwwrootpath)
        {
            int result;
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind2>>(JSON);
            foreach (var item in root)
            {
                if (ProductCodeExists(item.Barcode))
                {
                    var product = await context.Products.Where(p => p.Barcode == item.Barcode).FirstOrDefaultAsync();
                    product.ProductName = item.ProductName;
                    context.Products.Update(product);
                    try
                    {
                        result = await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully update the code {item.Barcode} to product table.");
                }
                if (ProductCodeExists(item.Barcode))
                {
                    errorList.Add($"duplicate product exsits, the product is {item.Barcode} .");
                }
            }
            return Ok(errorList);
        }

        /// <summary>
        ///  Dummy Supplier initialize to the system
        /// </summary>
        /// <param name="initialsupplier"></param>
        [HttpPost("initialsupplier")]
        [Obsolete]
        public async Task<IActionResult> InitialSupplier([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummySupplier> supplier = worksheet.ExportData<DummySupplier>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, supplier);

                }

            }
            var result = await InitializeS(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath);
            return Ok(result);
        }
        private async Task<ActionResult> InitializeS(DataContext context, string jsonfilename, string wwwrootpath)
        {
            int result;
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind3>>(JSON);

            foreach (var item in root)
            {
                if (!SupplierCodeExists(item.SupplierName,item.BranchLocationId))
                {
                    var supplier = new Supplier
                    {
                        CreatedBy = item.CreatedBy,
                        SupplierName = item.SupplierName,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Address1 = item.Address1,
                        BranchLocationId = item.BranchLocationId
                    };
                    await context.Suppliers.AddAsync(supplier);
                    try
                    {
                        result = await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully saved the code {item.SupplierName} to employee table.");
                }
                if (SupplierCodeExists(item.SupplierName,item.BranchLocationId))
                {
                    errorList.Add($"duplicate employee exsits, the employee is {item.SupplierName} .");
                }
            }
            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy Supplier initialize to the system
        /// </summary>
        /// <param name="initialsupplierproduct"></param>
        [HttpPost("initialsupplierproduct")]
        [Obsolete]
        public async Task<IActionResult> InitialSupplierProduct([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummySupplierProduct> supplier = worksheet.ExportData<DummySupplierProduct>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, supplier);

                }

            }
            var result = await InitializeSPD(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath);
            return Ok(result);
        }
        private async Task<ActionResult> InitializeSPD(DataContext context, string jsonfilename, string wwwrootpath)
        {
            int result;
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind8>>(JSON);
            foreach (var item in root)
            {
                var product = await context.Products.Where(p => p.Barcode == item.BarCode).SingleAsync<Product>();
                if (!SupplierProductCodeExists(item.SupplierId, item.BranchLocationId, product.ProductId))
                {
                    var supplier = new SupplierProduct
                    {
                        SupplierId = item.SupplierId,
                        ProductId = product.ProductId,
                        BranchLocationId = item.BranchLocationId
                    };
                    await context.SupplierProducts.AddAsync(supplier);
                    try
                    {
                        result = await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully saved the code {item.BarCode} {item.SupplierId}  {item.BranchLocationId} to supplier product table.");
                }
                if (SupplierCodeExists(item.BarCode,item.BranchLocationId))
                {
                    errorList.Add($"duplicate supplier product exsits, the supplier product is {item.BarCode} {item.SupplierId}  {item.BranchLocationId} .");
                }
            }
            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy product initialize to the system
        /// </summary>
        /// <param name="initialsupplierprice"></param>
        [HttpPost("initialsupplierprice")]
        [Obsolete]
        public async Task<IActionResult> InitialSuppierPrice([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummySupplierPrice> product = worksheet.ExportData<DummySupplierPrice>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, product);

                }

            }
            var result = await InitializeSP(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath);
            return Ok(result);
        }
        private async Task<ActionResult> InitializeSP(DataContext context, string jsonfilename, string wwwrootpath)
        {
            int result;
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind4>>(JSON);
            foreach (var item in root)
            {
                var product = context.Products.Single(p => p.Barcode == item.BarCode);
                if (!await context.SupplierPrices.AnyAsync(sp => sp.ProductId == product.ProductId && sp.SupplierId == item.SupplierId && sp.BranchLocationId == item.BranchLocationId && sp.Active == true))
                {
                    var supplierprice = new SupplierPrice
                    {
                        CreatedBy = item.CreatedBy,
                        BranchLocationId = item.BranchLocationId,
                        SupplierId = item.SupplierId,
                        ProductId = product.ProductId,
                        UnitPrice = item.UnitPrice,
                        MaximumRetailPrice = item.MaximumRetailPrice,
                        StatusType = StatusType.Add,
                        PriceDate = DateTime.Now,
                        DiscountPrecentage = 0,
                        DiscountPrice = 0,
                        PromotionPrice = 0,
                        PromotionPrecentage = 0,
                        Active = true,
                        IsDeleted = false,
                        TaxPrecentage = 0,
                        TaXMethod = 0,
                        CreateDate = DateTime.Now
                    };
                    await context.SupplierPrices.AddAsync(supplierprice);
                    try
                    {
                        result = await context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully saved the supplierprice {item.SupplierId},{product.ProductId},{item.BranchLocationId} to employee table.");
                }
                if (await context.SupplierPrices.AnyAsync(sp => sp.ProductId == product.ProductId && sp.SupplierId == item.SupplierId && sp.BranchLocationId == item.BranchLocationId && sp.Active == true))
                {
                    errorList.Add($"duplicate supplier price exsits, the employee is {item.SupplierId},{product.ProductId},{item.BranchLocationId} .");
                }
            }
            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy product initialize to the system
        /// </summary>
        /// <param name="initialpurchaseorder"></param>
        [HttpPost("initialpurchaseorder")]
        [Obsolete]
        public async Task<IActionResult> InitialPurchaseOrder([FromForm] CreateUploadExcelDto createUploadExcelDto, [FromForm] CreatePurchaseOrderHeaderDto createPurchaseOrderHeaderDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummyPurchaseOrder> product = worksheet.ExportData<DummyPurchaseOrder>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, product);

                }

            }
            var result = await InitializePO(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath, createPurchaseOrderHeaderDto);
            return Ok(result);
        }
        private async Task<IActionResult> InitializePO(DataContext context, string jsonfilename, string wwwrootpath, CreatePurchaseOrderHeaderDto createPurchaseOrderHeaderDto)
        {
            int result;
            var purrchaseorderheader = _mapper.Map<PurchaseOrderHeader>(createPurchaseOrderHeaderDto);
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind5>>(JSON);
            IList<PurchaseOrderDetail> purrchaseorderdetails = new List<PurchaseOrderDetail>();
            foreach (var item in root)
            {
                var product = context.Products.Single(p => p.Barcode == item.BarCode);
                var supplierprice = await context.SupplierPrices.Where(sp => sp.ProductId == product.ProductId && sp.BranchLocationId == item.BranchLocationId).FirstOrDefaultAsync();
                var purrchaseorderdetail = new PurchaseOrderDetail
                {
                    CreatedBy = item.CreatedBy,
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    Amount = item.Amount,
                    CurrentQuantity = 0,
                    ReturnQuantity = 0,
                    BonusQuantity = 0,
                    GRNQuantity = 0,
                    FreeQuantity = 0,
                    Active = true,
                    IsDeleted = false,
                    SupplierPriceId = supplierprice.SupplierPriceId,
                    CreateDate = DateTime.Now
                };
                if (!await context.PurchaseOrderDetails.AnyAsync(sp => sp.ProductId == product.ProductId && sp.SupplierPriceId == supplierprice.SupplierPriceId && sp.Active == true))
                {
                    purrchaseorderdetails.Add(purrchaseorderdetail);
                    errorList.Add($"successfully data exsits {product.ProductId},{item.Quantity},{supplierprice.SupplierPriceId},{item.Amount} to purchaseorderdetails table.");
                }
                else
                {
                    errorList.Add($"duplicate data exsits {product.ProductId},{item.Quantity},{supplierprice.SupplierPriceId},{item.Amount} to purchaseorderdetails table.");
                }
            }
            purrchaseorderheader.PurchaseOrderDetails = purrchaseorderdetails;
            _context.AttachRange(purrchaseorderheader.PurchaseOrderDetails);
            await _context.PurchaseOrderHeaders.AddAsync(purrchaseorderheader);
            try
            {
                result = await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                errorList.Add(e.Message);
            }
            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy product initialize to the system
        /// </summary>
        /// <param name="initialgoodreceipt"></param>
        [HttpPost("initialgoodreceipt")]
        [Obsolete]
        public async Task<IActionResult> InitialGoodReceipt([FromForm] CreateUploadExcelDto createUploadExcelDto, [FromForm] CreateGrnHeaderDto createGrnHeaderDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummyGoodReceipt> product = worksheet.ExportData<DummyGoodReceipt>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, product);

                }

            }
            var result = await InitializeGRN(_context, uniqueFileName.Replace("xlsx", "json"), _hostingEnviroment.WebRootPath, createGrnHeaderDto);
            return Ok(result);
        }
        private async Task<ActionResult> InitializeGRN(DataContext context, string jsonfilename, string wwwrootpath, CreateGrnHeaderDto createGrnHeaderDto)
        {
            int result;
            var goodReceptNoteHeader = _mapper.Map<GoodReceptNoteHeader>(createGrnHeaderDto);
            IList<string> errorList = new List<string>();
            context.Database.EnsureCreated();
            string folder = Path.Combine(wwwrootpath, "data");
            string filename = jsonfilename;
            string path = Path.Combine(folder, filename);
            var JSON = System.IO.File.ReadAllText(path);
            var root = JsonConvert.DeserializeObject<IList<M203StructureKind6>>(JSON);
            IList<GoodReceptNoteDetail> goodReceptNoteDetails = new List<GoodReceptNoteDetail>();
            IList<DummySalePrice> salePriceDetails = new List<DummySalePrice>();
            foreach (var item in root)
            {
                var product = await context.Products.Where(p => p.Barcode == item.BarCode).FirstOrDefaultAsync();
                var DummySalePrice = new DummySalePrice
                {
                    SalePrice = item.SalePrice,
                    ProductId = product.ProductId
                };
                salePriceDetails.Add(DummySalePrice);
            }
            foreach (var item in root)
            {
                var goodreceptNoteDetail = new GoodReceptNoteDetail
                {
                    CreatedBy = item.CreatedBy,
                    CostPrice = item.CostPrice,
                    RetailPrice = item.RetailPrice,
                    PricePrecentage = 0,
                    ItemQuantity = item.ItemQuantity,
                    BalanceItemQuantity = 0,
                    FreeItemQuantity = 0,
                    InvoiceItemDate = DateTime.Now,
                    GroupDiscountAmount = 0,
                    ProductExpiryDate = DateTime.Now.AddYears(10),
                    BatchNumber = 0,
                    ProductId = item.ProductId,
                    SupplierPriceId = item.SupplierPriceId,
                    CreateDate = DateTime.Now,
                    PurchaseOrderHeaderId = item.PurchaseOrderHeaderId
                };
                if (!await context.GoodReceptNoteDetails.AnyAsync(sp => sp.ProductId == item.ProductId && sp.SupplierPriceId == item.SupplierPriceId && sp.Active == true))
                {
                    goodReceptNoteDetails.Add(goodreceptNoteDetail);
                    errorList.Add($"successfully data exsits {item.ProductId},{item.ItemQuantity},{item.SupplierPriceId},{item.PurchaseOrderHeaderId} to purchaseorderdetails table.");
                }
                else
                {
                    errorList.Add($"duplicate data exsits {item.ProductId},{item.ItemQuantity},{item.SupplierPriceId},{item.PurchaseOrderHeaderId} to purchaseorderdetails table.");
                }
            }
            goodReceptNoteHeader.Lines = goodReceptNoteDetails;
            _context.AttachRange(goodReceptNoteHeader.Lines);
            await _context.GoodReceptNoteHeaders.AddAsync(goodReceptNoteHeader);
            try
            {
                result = await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                errorList.Add(e.Message);
            }
            foreach (var item in goodReceptNoteDetails)
            {
                var salePrice = salePriceDetails.Where(s => s.ProductId == item.ProductId).FirstOrDefault();
                if (salePrice == null)
                {
                    Price price = new Price
                    {
                        UnitPrice = item.CostPrice,
                        MaximumRetailPrice = item.RetailPrice,
                        SalePrice = await _repoWrapper.Price.CalculatePriceV1Async(item.CostPrice, item.RetailPrice, createGrnHeaderDto.BranchLocationId),
                        ProductId = item.ProductId,
                        BranchLocationId = createGrnHeaderDto.BranchLocationId,
                        CreatedBy = item.CreatedBy,
                    };
                    await _repoWrapper.Price.UpdatePriceAsync(price);
                    ShopStock shopStock = new ShopStock
                    {
                        UnitsInStock = item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity),
                        ReorderLevel = ReorderLevel.Available,
                        ProductExpireDate = item.ProductExpiryDate,
                        CreateDate = DateTime.Now,
                        CreatedBy = item.CreatedBy,
                        ProductId = item.ProductId,
                        PriceId = price.PriceId,
                        Price = price,
                        BranchLocationId = createGrnHeaderDto.BranchLocationId,
                        BranchLocation = item.GoodReceptNoteHeader.BranchLocation,
                    };
                    await _repoWrapper.ShopStock.UpdateShopStockAsync(shopStock);
                }
                if (salePrice != null)
                {
                    Price price = new Price
                    {
                        UnitPrice = item.CostPrice,
                        MaximumRetailPrice = item.RetailPrice,
                        SalePrice = salePrice.SalePrice,
                        ProductId = item.ProductId,
                        BranchLocationId = createGrnHeaderDto.BranchLocationId,
                        CreatedBy = item.CreatedBy,
                    };
                    await _repoWrapper.Price.UpdatePriceAsync(price);
                    ShopStock shopStock = new ShopStock
                    {
                        UnitsInStock = item.ItemQuantity + Convert.ToInt32(item.FreeItemQuantity),
                        ReorderLevel = ReorderLevel.Available,
                        ProductExpireDate = item.ProductExpiryDate,
                        CreateDate = DateTime.Now,
                        CreatedBy = item.CreatedBy,
                        ProductId = item.ProductId,
                        PriceId = price.PriceId,
                        Price = price,
                        BranchLocationId = createGrnHeaderDto.BranchLocationId,
                        BranchLocation = item.GoodReceptNoteHeader.BranchLocation,
                    };
                    await _repoWrapper.ShopStock.UpdateShopStockAsync(shopStock);
                }
            }

            return Ok(errorList);
        }
        /// <summary>
        ///  Dummy product initialize to the system
        /// </summary>
        /// <param name="initialpriceupdate"></param>
        [HttpPost("initialpriceupdate")]
        [Obsolete]
        public IActionResult InitialPriceUpdate([FromForm] CreateUploadExcelDto createUploadExcelDto)
        {
            // get insertfile from the form
            IFormFile employeeExcel = createUploadExcelDto.Files;
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            string uniqueFileName = employeeExcel.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Open);

                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                IList<DummySalePriceUpdate> price = worksheet.ExportData<DummySalePriceUpdate>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
                string exportFolder = Path.Combine(_hostingEnviroment.WebRootPath, "data");
                //open file stream
                using (StreamWriter file = new StreamWriter(Path.Combine(exportFolder, uniqueFileName.Replace("xlsx", "json"))))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    //serialize object directly into file stream
                    serializer.Serialize(file, price);

                }

            }
            return Ok();
        }
        private async Task<decimal> CreditBalanceCalculation(string number)
        {
            var companySetting = await _context.CalculationMasters.Where(c => c.BranchLocationId == 2).FirstOrDefaultAsync();
            var customer = await _repoWrapper.Employee.FindByCondition(c => c.Number == number).FirstOrDefaultAsync();
            decimal creditBalance = 0;
            var str = DateTime.Now.Month.ToString() + "/11/" + DateTime.Now.Year.ToString();
            var end = DateTime.Now.Month.ToString() + "/20/" + DateTime.Now.Year.ToString();
            DateTime dt;
            DateTime.TryParse(str, out dt);
            DateTime dt1;
            DateTime.TryParse(end, out dt1);
            //get difference of two dates
            var today = DateTime.Now.Date;
            var diffOfDates = today - dt;
            int days = diffOfDates.Days;
            if (today >= dt && today <= dt1)
            {
                creditBalance = (100 * days) + 100;
                return creditBalance;
            }
            creditBalance = customer.CreditLimit - customer.CreditBalance;
            return creditBalance;
        }
        private bool CodeExists(string code) =>
        _context.MI_Employer_Employees.Any(c => c.Number == code);
        private bool ProductCodeExists(string code) =>
          _context.Products.Any(c => c.Barcode == code);
        private bool SupplierCodeExists(string code, int branchId) =>
          _context.Suppliers.Any(c => c.SupplierName == code && c.BranchLocationId == branchId);
        private bool SupplierProductCodeExists(int supplierId, int branchId, Int64 productId) =>
          _context.SupplierProducts.Any(c => c.BranchLocationId == branchId && c.ProductId == productId && c.SupplierId == supplierId);
    }
    class SchemeList
    {
        public int SchemeCode { get; set; }
        public String Scheme { get; set; }
    }
    class DummyGoodReceipt
    {
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int ItemQuantity { get; set; }
        public int ProductId { get; set; }
        public int SupplierPriceId { get; set; }
        public string CreatedBy { get; set; }
        public int PurchaseOrderHeaderId { get; set; }
        public string BarCode { get; set; }
        public decimal SalePrice { get; set; }

    }
    class DummyPurchaseOrder
    {
        public decimal Amount { get; set; }
        public int BranchLocationId { get; set; }
        public int Quantity { get; set; }
        public string BarCode { get; set; }
        public string CreatedBy { get; set; }
    }
    class DummySupplierPrice
    {
        public decimal UnitPrice { get; set; }
        public decimal MaximumRetailPrice { get; set; }
        public string BarCode { get; set; }
        public int BranchLocationId { get; set; }
        public int SupplierId { get; set; }
        public string CreatedBy { get; set; }

    }
    class DummySupplier
    {
        public string SupplierName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string CreatedBy { get; set; }
        public int BranchLocationId { get; set; }

    }
    public class FileModel
    {
        public string FileName { get; set; }
    }
    class DummySalePrice
    {
        public long ProductId { get; set; }
        public decimal SalePrice { get; set; }

    }
    class DummySalePriceUpdate
    {
        public decimal CostPrice { get; set; }
        public string BarCode { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal SalePrice { get; set; }
        public string ModifiedBy { get; set; }
        public int BranchLocationId { get; set; }

    }
    class DummyProduct
    {
        public string CreatedBy { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }

    }
    class DummySupplierProduct
    {
        public string BarCode { get; set; }
        public int BranchLocationId { get; set; }
        public int SupplierId { get; set; }
    }
    public class SaleLunchByEmployee
    {
        public string BarCode { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int Quantity { get; set; }
    }
    class Employee
    {
        #region Pr0perties
        public string EmployeeNumber { get; set; }
        public string EPFNo { get; set; }
        public string Name { get; set; }
        public string Scheme { get; set; }
        public int SchemeCode { get; set; }
        public decimal CreditLimit { get; set; }
        public int Deleted { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public DateTime DateResigned { get; set; }
        // public decimal CreditBalance { get; set; }
        #endregion
        #region Intialization
        public Employee()
        {

        }
        #endregion
    }
    class M203StructureKind1
    {
        public string EmployeeNumber { get; set; }
        public string EPFNo { get; set; }
        public string Name { get; set; }
        public string Scheme { get; set; }
        public decimal CreditLimit { get; set; }
        public int Deleted { get; set; }
        public int SchemeCode { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public DateTime DateResigned { get; set; }
        // public decimal CreditBalance { get; set; }


    }
    class M203StructureKind2
    {
        public string CreatedBy { get; set; }
        public string Barcode { get; set; }
        public string ProductName { get; set; }

    }
    class M203StructureKind3
    {
        public string SupplierName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string CreatedBy { get; set; }
        public int BranchLocationId { get; set; }
    }
    class M203StructureKind4
    {
        public decimal UnitPrice { get; set; }
        public decimal MaximumRetailPrice { get; set; }
        public string BarCode { get; set; }
        public int BranchLocationId { get; set; }
        public int SupplierId { get; set; }
        public string CreatedBy { get; set; }


    }
    class M203StructureKind5
    {
        public decimal Amount { get; set; }
        public int BranchLocationId { get; set; }
        public int Quantity { get; set; }
        public string BarCode { get; set; }
        public string CreatedBy { get; set; }

    }
    class M203StructureKind6
    {
        public decimal CostPrice { get; set; }
        public decimal RetailPrice { get; set; }
        public int ItemQuantity { get; set; }
        public int ProductId { get; set; }
        public int SupplierPriceId { get; set; }
        public string CreatedBy { get; set; }
        public int PurchaseOrderHeaderId { get; set; }
        public string BarCode { get; set; }
        public decimal SalePrice { get; set; }
    }
    class M203StructureKind7
    {
        public decimal CostPrice { get; set; }
        public string BarCode { get; set; }
        public decimal SalePrice { get; set; }
        public string ModifiedBy { get; set; }
        public int BranchLocationId { get; set; }

    }
    class M203StructureKind8
    {
        public string BarCode { get; set; }
        public int BranchLocationId { get; set; }
        public int SupplierId { get; set; }
    }
}