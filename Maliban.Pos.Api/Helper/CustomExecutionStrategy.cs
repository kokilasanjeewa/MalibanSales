using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Maliban.Pos.Api.Helper
{
    public class CustomExecutionStrategy : ExecutionStrategy
  {
      public CustomExecutionStrategy(DbContext context) : base(context, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
      {
 
      }
 
      public CustomExecutionStrategy(ExecutionStrategyDependencies dependencies) : base(dependencies, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
      {
 
      }
 
      public CustomExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) :
          base(context, maxRetryCount, maxRetryDelay)
      {
 
      }
 
      protected override bool ShouldRetryOn(Exception exception)
      {
          return exception.GetType() == typeof(DbUpdateException);
      }
  }
}