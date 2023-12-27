using Maliban.Pos.Api.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maliban.Pos.Api.Service
{
    public class MyBackgroundService : IHostedService
    {
		private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MyBackgroundService> logger;
        public MyBackgroundService(ILogger<MyBackgroundService> logger,IServiceProvider serviceProvider)
        {
            this.logger = logger;
        	_serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
			logger.LogInformation("Hosted service starting");

			return Task.Factory.StartNew(async () =>
			{
				// loop until a cancalation is requested
				while (!cancellationToken.IsCancellationRequested)
				{
					logger.LogInformation("Hosted service executing - {0}", DateTime.Now);
		 
					try
					{
				 		 	 	using (var scope = _serviceProvider.CreateScope())
								{
									var svc = scope.ServiceProvider.GetRequiredService<IOperationGroupRepository>();
									await svc.InsertIntoEmployee();
								}  
						// wait for 1 minutes
						await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
					}
					catch (OperationCanceledException) { }
				}
			}, cancellationToken);
		}

        public Task StopAsync(CancellationToken cancellationToken)
        {
			logger.LogInformation("Hosted service stopping");
			return Task.CompletedTask;
		}
    }
}