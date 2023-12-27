using Maliban.Pos.Api.Controllers;
using Maliban.Pos.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateUploadLunchTokenDto
    {
        public ICollection<SaleLunchByEmployee> Lines { get; set; }
        public int BranchLocationId { get; set; }
        public string CreatedBy { get; set; }
        public int Id { get; set; }
    }
}
