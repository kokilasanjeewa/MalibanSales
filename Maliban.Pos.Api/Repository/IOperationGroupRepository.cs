using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maliban.Pos.Api.Controllers;
using Maliban.Pos.Api.Models;

namespace Maliban.Pos.Api.Repository
{
     public interface IOperationGroupRepository
    {
        Task<IList<string>> InsertIntoEmployee();

    }
    public class DummyEmployee
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
        public DummyEmployee()
        {

        }
        #endregion
    }
}
