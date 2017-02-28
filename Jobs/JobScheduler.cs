using Autofac;
using Quartz;
using Quartz.Impl;

namespace BargainBot.Jobs
{
    public class JobScheduler
    {
        private IScheduler _scheduler;

        public JobScheduler(IScheduler scheduler)
        {
            _scheduler = scheduler;
            // construct a scheduler factory
            //var stdSchedulerFactory = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            //_scheduler = stdSchedulerFactory.GetScheduler();
            //_scheduler.Start();

            Register();

            _scheduler.Start();
        }

        //Can add object as parameter here if needed.
        public void Register()
        {

            var jobData = new JobDataMap
            {
                {"k", "hey it's a string object"}
            };

            var dealJob = JobBuilder.Create<DealJob>()
                    .WithIdentity("dealJob", "group1")
                    .SetJobData(jobData)
                    .Build();

            // create trigger
            var trigger = TriggerBuilder.Create()
                .WithIdentity("60SecondsTrigger", "group1")
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(30).RepeatForever())
                .Build();

            // Schedule the job using the job and trigger 
            _scheduler.ScheduleJob(dealJob, trigger);
        }
    }
}