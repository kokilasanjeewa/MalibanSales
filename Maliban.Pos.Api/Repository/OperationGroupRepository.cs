using Dapper;
using Maliban.Pos.Api.Controllers;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Syncfusion.XlsIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Maliban.Pos.Api.Repository
{
    public class OperationGroupRepository : IOperationGroupRepository
    {
        private readonly DapperContext _dpcontext;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnviroment;
        private readonly DataContext _context;

        [Obsolete]
        public OperationGroupRepository(DapperContext dpcontext, IHostingEnvironment hostingEnviroment, DataContext context)
        {
            _dpcontext = dpcontext;
            _hostingEnviroment = hostingEnviroment;
            _context = context;
        }
        public static bool FileInUse(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    return false;
                }
                //return false;
            }
            catch (IOException ex)
            {
                
                return true;
            }
        }
        private bool CodeExists(string code) =>
        _context.MI_Employer_Employees.Any(c => c.Number == code);

        [Obsolete]
        public async Task<IList<string>> InsertIntoEmployee()
        {
            int result;
            IList<string> errorList = new List<string>();
            string uploadsFolder = Path.Combine(_hostingEnviroment.WebRootPath, "excel");
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(uploadsFolder);
            // Loop over all files in C directory.
            string uniqueFileName = Path.GetFileName(fileEntries.First());
            FileInfo fi = new FileInfo(fileEntries.First());

            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (FileInUse(filePath))
            {
                return errorList;
            }
            List<DummyEmployee> list = new List<DummyEmployee>();

            //Creates a new instance for ExcelEngine
            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                //The workbook is opened.
                FileStream fileStream = new FileStream(filePath, FileMode.Append);
                IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
                IWorksheet worksheet = workbook.Worksheets[0];
                //Export worksheet data into Business Objects
                list = worksheet.ExportData<DummyEmployee>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);
                //output path
            }
            foreach (var item in list)
            {
                if (!CodeExists(item.EmployeeNumber))
                {
                    var classstudent = new MI_Employer_Employee
                    {
                        Number = item.EmployeeNumber,
                        FullName = item.Name,
                        EPFNo = item.EPFNo,
                        SchemeCode = item.SchemeCode,
                        Deleted = item.Deleted,
                        CreditLimit = item.CreditLimit,
                        Scheme = item.Scheme,
                        DateOfAppointment = item.DateOfAppointment,
                        DateResigned = item.DateResigned,
                        //CreditBalance = item.CreditBalance
                    };
                    _context.MI_Employer_Employees.Add(classstudent);
                    try
                    {
                        result = await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully saved the code {item.EPFNo} to employee table.");
                }
                if (CodeExists(item.EmployeeNumber) && item.Deleted == 1)
                {
                    /*var classstudent1 = new MI_Employer_Employee
                    {
                      SchemeCode = item.SchemeCode,
                      Deleted = item.Deleted,
                      CreditLimit = item.CreditLimit,
                      Scheme = item.Scheme,
                      DateResigned = item.DateResigned, 
                    }; */
                    var employee = _context.MI_Employer_Employees.Single(e => e.Number == item.EmployeeNumber);
                    employee.Deleted = item.Deleted;
                    _context.MI_Employer_Employees.Update(employee);
                    try
                    {
                        result = await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateException e)
                    {
                        errorList.Add(e.Message);
                    }
                    errorList.Add($"Successfully update the code {item.EPFNo} to employee table.");
                    errorList.Add($"duplicate employee exsits, the employee is {item.EPFNo} .");
                }
            }
            /*             DataTable TVP_MI_Employer_Employees = new DataTable();
                        TVP_MI_Employer_Employees.Columns.Add("MI_Employer_EmployeeUid", typeof(string));
                        TVP_MI_Employer_Employees.Columns.Add("Number", typeof(string));
                        TVP_MI_Employer_Employees.Columns.Add("Deleted", typeof(int));
                        TVP_MI_Employer_Employees.Columns.Add("DateOfAppointment", typeof(DateTime));
                        TVP_MI_Employer_Employees.Columns.Add("SchemeCode", typeof(int));
                        TVP_MI_Employer_Employees.Columns.Add("EPFNo", typeof(string));
                        TVP_MI_Employer_Employees.Columns.Add("DateResigned", typeof(DateTime));
                        TVP_MI_Employer_Employees.Columns.Add("FullName", typeof(string));
                        TVP_MI_Employer_Employees.Columns.Add("CreditLimit", typeof(decimal));
                        TVP_MI_Employer_Employees.Columns.Add("Scheme", typeof(string));
                        TVP_MI_Employer_Employees.Columns.Add("CreditBalance", typeof(decimal));
                        TVP_MI_Employer_Employees.Columns.Add("PreviousCreditBalance", typeof(decimal));
                        foreach (DummyEmployee item in list)
                        {
                            TVP_MI_Employer_Employees.Rows.Add(Guid.NewGuid(), item.EmployeeNumber.ToString(), item.Deleted.ToString(), item.DateOfAppointment, item.SchemeCode, item.EPFNo,
                            item.DateResigned, item.Name, item.CreditLimit, item.Scheme, 0, 0);
                        }
                        var procedureName = "InsertEmployees";
                        DynamicParameters parameters = new DynamicParameters();
                        parameters.Add("TVP_MI_Employer_Employees", TVP_MI_Employer_Employees.AsTableValuedParameter("MI_Employer_Employees"));
                        using (var connection = _dpcontext.CreateConnection())
                        {
                            var result = (await connection.QueryAsync<MI_Employer_Employee>
                                (procedureName, parameters, commandType: CommandType.StoredProcedure).ConfigureAwait(false)).AsList();
                            return result;
                        } */
            return errorList;
        }


    }
}
