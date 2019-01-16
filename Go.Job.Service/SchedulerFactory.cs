﻿using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Go.Job.Service
{
    public sealed class SchedulerFactory
    {
        private readonly NameValueCollection _properties;


        public SchedulerFactory() : this(null)
        {

        }

        public SchedulerFactory(NameValueCollection properties)
        {
            _properties = properties;
        }

        public async Task CreateSchedulerAndStart()
        {

            if (_properties == null)
            {
                JobPoolManager.Scheduler = await new StdSchedulerFactory().GetScheduler();
            }
            else
            {
                JobPoolManager.Scheduler = await new StdSchedulerFactory(_properties).GetScheduler();
            }
            await JobPoolManager.Scheduler.Start();

            JobPoolManager.Scheduler.ListenerManager.AddJobListener(new MyJobListenerSupport(), KeyMatcher<JobKey>.KeyEquals(new JobKey("ScanJob", "ScanJob")));

            //创建扫描Job
            IJobDetail jobDetail = JobBuilder.Create<ScanJob>()
                .SetJobData(new JobDataMap(new Dictionary<string, string>
                {
                    {"name","ScanJob" }
                }))
                .WithIdentity("ScanJob", "ScanJob").Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("ScanJob", "ScanJob")
                .WithSimpleSchedule(s => s.WithIntervalInSeconds(10).RepeatForever().WithMisfireHandlingInstructionFireNow())
                //.WithSimpleSchedule(s => s.WithIntervalInMinutes(1).RepeatForever().WithMisfireHandlingInstructionNowWithExistingCount())
                //.WithSimpleSchedule(s => s.WithIntervalInSeconds(10).WithRepeatCount(1))
                .StartNow()
                //.StartAt(new DateTimeOffset(DateTime.Now.AddSeconds(5)))
                .Build();
            await JobPoolManager.Scheduler.ScheduleJob(jobDetail, trigger);
        }
    }
}