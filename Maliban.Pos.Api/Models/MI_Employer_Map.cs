using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Models
{
	public class MI_Employer_Map
	{
		[Key]
		public long MIEmployerMapId { get; set; }
		[Required]
		public int MI_Employer_EmployeeId { get; set; }
	    public string EPFNo { get; set; }
		[Required]
		public int BranchLocationId { get; set; }
		public BranchLocation BranchLocation { get; set; }
		public virtual MI_Employer_Employee MI_Employer_Employee { get; set; }
	}
}
