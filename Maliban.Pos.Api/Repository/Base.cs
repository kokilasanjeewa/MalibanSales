using System;
using Microsoft.EntityFrameworkCore;

namespace Maliban.Pos.Api.Repository
{
  public abstract class Base : IBase
  {
    public int Id { get; set; }

    public DateTimeOffset Created { get; set; }

    public DateTimeOffset? LastUpdated { get; set; }

    public DateTimeOffset? Deleted { get; set; }
    public static void OnModelCreating<T>(ModelBuilder modelBuilder)
      where T : class, IBase
    {
      modelBuilder.Entity<T>().HasKey(entity => entity.Id);

    }
  }
}