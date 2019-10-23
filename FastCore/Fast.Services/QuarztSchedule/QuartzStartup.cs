using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Fast.Entities;
using Fast.Mapping;
using AutoMapper;
using System.Linq;

namespace Fast.Services
{
    public class QuartzStartup
    {
        private JobCenter _jobCenter;
        private FastDbContext _dbContext;
        private IMapper _mapper;

        public QuartzStartup(JobCenter jobCenter,
            IMapper mapper,
            FastDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _jobCenter = jobCenter;
        }

        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public async Task Start()
        {
            //执行中的任务
            var jobList = _dbContext.QuarztSchedule.Where(o => o.RunStatus == (int)JobStatus.执行任务中).ToList()
                 .Select(item => _mapper.Map<QuarztScheduleMapping>(item)).ToList();
            jobList.ForEach(async item =>
            {
                await _jobCenter.AddScheduleJobAsync(item);
            });
            await Task.FromResult(0);
        }


    }
}
