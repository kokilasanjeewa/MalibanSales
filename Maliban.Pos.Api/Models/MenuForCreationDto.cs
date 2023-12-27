namespace Maliban.Pos.Api.Models
{
    public class MenuForCreationDto
    {
        public bool Allow { get; set; }=true;
        public int MainMenuId { get; set; }
        public int SubmenuId { get; set; }
        public int SubmenuItemId { get; set; }
        public string AccessControllerName { get; set; }
        public int ApplicationRoleId { get; set; }
        public int ApplicationUserId { get; set; }
        public string[] Role  { get; set; }
    }
}