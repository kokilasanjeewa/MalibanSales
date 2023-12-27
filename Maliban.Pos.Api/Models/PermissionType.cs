using System;
using System.ComponentModel.DataAnnotations;

namespace Maliban.Pos.Api.Models
{
    public class PermissionType
    {
        [Key]
        public int Id { get; set; }
        public string AccessControllerName { get; set; }
    }
}
