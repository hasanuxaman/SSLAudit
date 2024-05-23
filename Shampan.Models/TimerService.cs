using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Shampan.Models
{
    public class TimerService : IHostedService, IDisposable
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteFunction, null, TimeSpan.FromMinutes(1), TimeSpan.FromMilliseconds(-1));
            return Task.CompletedTask;
        }

        private void ExecuteFunction(object state)
        {
            // Place the logic you want to execute after 1 minute here
            Console.WriteLine("Executing function after 1 minute");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

    public class RepeatingFunctionExecutionService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Place the logic you want to execute every minute here
                ExecuteFunction();

                // Delay for 1 minute
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private void ExecuteFunction()
        {
            // Place the logic you want to execute every minute here
            Console.WriteLine("Executing function every minute");
        }
    }

    public class DailyFunctionExecutionService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var currentTime = DateTime.Now;
            var nextExecutionTime = GetNextExecutionTime();

            // Calculate the initial delay until the next execution time
            var initialDelay = nextExecutionTime > currentTime
                ? nextExecutionTime - currentTime
                : nextExecutionTime.AddDays(1) - currentTime;

            // Wait for the initial delay
            await Task.Delay(initialDelay, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                // Execute the function
                ExecuteFunction();

                // Wait for 24 hours until the next execution
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private DateTime GetNextExecutionTime()
        {
            var now = DateTime.Now;
            var nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, 11, 10, 0);
            //var nextExecutionTime = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Local);

            // If it's already past 12 AM for today, schedule for the next day
            if (now > nextExecutionTime)
                nextExecutionTime = nextExecutionTime.AddDays(1);

            return nextExecutionTime;
        }

        private void ExecuteFunction()
        {
            // Place the logic you want to execute once a day here



            Console.WriteLine("Executing function once a day");
        }
    }

    public class ScheduledFunctionExecutionService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var scheduledTime = new DateTime(now.Year, now.Month, now.Day, 12, 9, 0);

                // If it's past the scheduled time for today, schedule it for the next day
                if (now > scheduledTime)
                    scheduledTime = scheduledTime.AddDays(1);

                var delay = scheduledTime - now;

                // Wait for the specified delay
                await Task.Delay(delay, stoppingToken);

                // Execute the function
                ExecuteScheduledFunction();

                // Wait for 24 hours for the next execution
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        //private void ExecuteScheduledFunction()
        public void ExecuteScheduledFunction()
        {
            // Place the logic you want to execute every day at 11:10 AM here
            Console.WriteLine("Executing function every day at 11:10 AM");
        }
    }


}
