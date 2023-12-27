using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Dtos
{
    public class ReturnDto
    {
        public int Result { get; set; }
        public string RefNum { get; set; }
        public int RefNumId { get; set; }
        public int Version { get; set; }
        public string Status { get; set; }
        public string MINNo { get; set; }
        public int MINHeaderId { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorState { get; set; }
        public string ErrorSeverity { get; set; }
        public string ErrorProcedure { get; set; }
        public string ErrorLine { get; set; }
        public string ErrorMessage { get; set; }
    }
}