using Autofac;
using Quartz;
using Quartz.Impl;

namespace BargainBot.Jobs
{
    public class JobScheduler
    {
        private IScheduler _scheduler;

        public JobScheduler(ILifetimeScope container)
        {
            // construct a scheduler factory
            var stdSchedulerFactory = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            _scheduler = stdSchedulerFactory.GetScheduler();
            _scheduler.Start();

            var sched = new StdSchedulerFactory().GetScheduler();
            sched.JobFactory = new AutofacJobScheduler(container);

            Register();
        }

        //Can add object as parameter here if needed.
        public void Register()
        {

            var jobData = new JobDataMap
            {
                {"k", "hey it's a string object"}
            };

            //TODO: Problem with creating a DealJob with autofac
            var dealJob = JobBuilder.Create<DealJob>()
                    .WithIdentity("dealJob", "group1")
                    .SetJobData(jobData)
                    .Build();

            // create trigger
            var trigger = TriggerBuilder.Create()
                .WithIdentity("60SecondsTrigger", "group1")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(60).RepeatForever())
                .Build();

            // Schedule the job using the job and trigger 
            _scheduler.ScheduleJob(dealJob, trigger);
        }
    }
}