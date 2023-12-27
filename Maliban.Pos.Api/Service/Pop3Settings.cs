using System.Net.Security;
namespace Maliban.Pos.Api.Service
{
    public class Pop3Settings
    {
        public int PopPort { get; set; }
        public string PopServer { get; set; }
        public string PopUsername { get; set; }
        public string PopPassword { get; set; }

    }
}