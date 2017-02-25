using System;
using Quartz;

namespace BargainBot.Jobs
{
    public class DealJob : IJob
    {
        void IJob.Execute(IJobExecutionContext context)
        {
            var obj = context.JobDetail.JobDataMap["k"];
            Console.WriteLine($"Hello, Job executed and got {obj} back");
        }
    }
}