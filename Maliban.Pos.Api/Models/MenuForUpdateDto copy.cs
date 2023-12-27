namespace Maliban.Pos.Api.Models
{
    public class MenuForUpdateDto
    {
        public bool Allow { get; set; } = true;
        public string AccessControllerName { get; set; }
        //public int ApplicationRoleId { get; set; }
        public int ApplicationUserId { get; set; }
        //public string[] Role  { get; set; }
    }
}