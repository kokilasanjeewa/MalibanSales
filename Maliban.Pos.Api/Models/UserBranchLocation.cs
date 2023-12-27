using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Models
{
    public class UserBranchLocation : BaseModel
    {
        [Key]
        public int UserBranchLocationId { get; set; }
        public string UserBranchLocationUid { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public int BranchLocationId { get; set; }
        public virtual BranchLocation BranchLocation { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        [Required]
        [ForeignKey("ApplicationUser")]
        public int Id { get; set; }
    }
}
