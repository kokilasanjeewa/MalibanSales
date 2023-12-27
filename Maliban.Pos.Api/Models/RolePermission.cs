using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maliban.Pos.Api.Models
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }
        public bool Allow { get; set; }
        [ForeignKey("MainMenu")]
        public int MainMenuId { get; set; }
        [ForeignKey("Submenu")]
        public int SubmenuId { get; set; }
        public virtual MainMenu MainMenu { get; set; }
        public virtual Submenu Submenu { get; set; }
        [ForeignKey("SubmenuItem")]
        public int SubmenuItemId { get; set; }
        public virtual SubmenuItem SubmenuItem { get; set; }
        public string AccessControllerName { get; set; }
        public int ApplicationRoleId { get; set; }
        public virtual ApplicationRole ApplicationRole { get; set; }
        public int ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}
