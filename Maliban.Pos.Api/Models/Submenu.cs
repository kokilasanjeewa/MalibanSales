using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
    public class Submenu
    {
        [Key]
        public int SubmenuId {get;set;}
        public string Name {get;set;}
        public int MainMenuId {get;set;}
         [ForeignKey("MainMenuId")]
        public virtual MainMenu MainMenu {get;set;}
        public virtual ICollection<RolePermission> RolePermissions { get; set; }
    }
}