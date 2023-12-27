using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
    public class SubmenuItem
    {
        [Key]
        public int SubmenuItemId {get;set;}
        public string Name {get;set;}
        [ForeignKey("Submenu")]
        public int SubMenuId {get;set;}
        public virtual Submenu Submenu {get;set;}
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

    }
}