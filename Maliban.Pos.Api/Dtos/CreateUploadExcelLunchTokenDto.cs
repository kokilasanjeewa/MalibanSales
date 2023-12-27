using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateUploadExcelLunchTokenDto
    {
        
        public IFormFile Files { get; set; }
        public int BranchLocationId { get; set; }
        public string CreatedBy { get; set; }
        [Required]
        [Display(Name = "ApplicationUser Id")]
        public int Id { get; set; }
    }
}