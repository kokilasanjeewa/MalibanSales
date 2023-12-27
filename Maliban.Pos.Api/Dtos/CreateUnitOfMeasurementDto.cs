using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Dtos
{
    public class CreateUnitOfMeasurementDto
    {
        [Required]
        public string UOM { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Create By ")]
        public string CreatedBy { get; set; }
    }
}