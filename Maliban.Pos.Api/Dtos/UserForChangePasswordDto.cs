using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class UserForChangePasswordDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Password {get;set;}
        
    }
}