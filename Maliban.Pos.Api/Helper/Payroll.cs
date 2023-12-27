using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Maliban.Pos.Api.DataAccess;
using Maliban.Pos.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Maliban.Pos.Api.Helper
{
  public class Payroll
  {
    private readonly IConfiguration _configuration;

    private string _connectionString;

    DbContextOptionsBuilder<DataContextP> _optionsBuilder;
    public Payroll(IConfiguration configuration)
    {
      _configuration = configuration;
      _optionsBuilder = new DbContextOptionsBuilder<DataContextP>();
      _connectionString = _configuration.GetConnectionString("PayrollConnection");
      _optionsBuilder.UseSqlServer(_connectionString);

    }
    public List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
    {

      using (var context = new DataContextP(_optionsBuilder.Options))
      {
        using (var command = context.Database.GetDbConnection().CreateCommand())
        {
          command.CommandText = query;
          command.CommandType = CommandType.Text;

          context.Database.OpenConnection();

          using (var result = command.ExecuteReader())
          {
            var entities = new List<T>();

            while (result.Read())
            {
              entities.Add(map(result));
            }

            return entities;
          }
        }
      }

    }


  }
}