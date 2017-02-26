using System;
using Autofac;
using Quartz;
using Quartz.Spi;

namespace BargainBot.Jobs
{
    public class AutofacJobScheduler : IJobFactory
    {
        private readonly ILifetimeScope _container;

        public AutofacJobScheduler(ILifetimeScope container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)_container.Resolve(bundle.JobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}