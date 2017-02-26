using System;
using Autofac;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace BargainBot.Jobs
{
    public class AutofacJobScheduler : SimpleJobFactory
    {
        private readonly ILifetimeScope _container;

        public AutofacJobScheduler(ILifetimeScope container)
        {
            _container = container;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return (IJob)_container.Resolve(bundle.JobDetail.JobType);
        }
    }
}