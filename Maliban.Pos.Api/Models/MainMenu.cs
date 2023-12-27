using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
    public class MainMenu
    {
        [Key]
        public int MainMenuId {get;set;}
        public string Name {get;set;}
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

    }
}