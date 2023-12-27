using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Helper
{
    public class DatabaseConnectionCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public DatabaseConnectionCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
               // using var connection = new SqlConnection("Server=KOKILA-LAP\\SQLEXPRESS;Database=MalibanPosDB;User Id=sa;Password=Sqlserver2017;MultipleActiveResultSets=true;Trusted_Connection=True;TrustServerCertificate=true;Integrated Security=false;Connect Timeout=30;Encrypt=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
                using var command = connection.CreateCommand();
                command.CommandText = "Select 1";
                object result = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);

                return HealthCheckResult.Healthy("The status of the connection is open");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("The status of the connection is close"+ex.InnerException);
            }

        }
    }
}
