using Quartz;
using Quartz.Impl;

namespace BargainBot.Jobs
{
    public class JobScheduler
    {
        //TODO: Add deal object as parameter?
        public void Register()
        {
            // construct a scheduler factory
            var _schedFact = new StdSchedulerFactory();

            // get a scheduler, start the schedular before triggers or anything else
            var sched = _schedFact.GetScheduler();
            sched.Start();

            var jobData = new JobDataMap
            {
                {"k", "hey it's a string object"}
            };

            // create job
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
            sched.ScheduleJob(dealJob, trigger);
        }
    }
}