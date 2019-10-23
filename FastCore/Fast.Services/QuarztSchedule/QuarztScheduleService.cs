using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Fast.Entities;
using Fast.Mapping;
using cts.web.core;

namespace Fast.Services
{
    public class QuarztScheduleService : BaseService
    {
        private FastDbContext _dbContext;
        private IMapper _mapper;
        private JobCenter _jobCenter;

        public QuarztScheduleService(FastDbContext dbContext,
            IMapper mapper,
            JobCenter jobCenter)
        {
            _jobCenter = jobCenter;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        /// <summary>
        /// 获取所有任务调度
        /// </summary>
        /// <returns></returns>
        public List<QuarztScheduleMapping> GetTaskList()
        {
            return _dbContext.QuarztSchedule.ToList()
                .Select(item => _mapper.Map<QuarztScheduleMapping>(item)).OrderBy(o => o.JobGroup).ThenBy(o => o.JobName).ToList();
        }

        /// <summary>
        /// 开启任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (bool Status, string Message) Start(Guid id)
        {
            var item = _dbContext.QuarztSchedule.Find(id);
            if (item == null)
                return Fail("任务不存在");

            var res = _jobCenter.AddScheduleJobAsync(_mapper.Map<QuarztScheduleMapping>(item)).Result;
            if (res.Status)
            {
                item.RunStatus = (int)JobStatus.执行任务中;
                _dbContext.SaveChanges();
            }
            return res;
        }


        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public (bool Status, string Message) Stop(Guid id)
        {
            var item = _dbContext.QuarztSchedule.Find(id);
            if (item == null)
                return Fail("任务不存在");
            var res = _jobCenter.StopScheduleJobAsync(item.JobGroup, item.JobName).Result;
            if (res.Status)
            {
                item.RunStatus = (int)JobStatus.暂停任务中;
                _dbContext.SaveChanges();
            }
            return res;
        }


        /// <summary>
        /// 初始化任务，讲任务写入数据库
        /// </summary>
        public void Init()
        {
            var oldList = _dbContext.QuarztSchedule.ToList();
            var schedules = new Quarzts().QuarztSchedules;
            foreach (var del in oldList)
            {
                if (!schedules.Any(o => o.JobName == del.JobName && o.JobGroup == del.JobGroup))
                {
                    _dbContext.QuarztSchedule.Remove(del);
                }
            }
            schedules.ForEach(item =>
            {
                if (!oldList.Any(o => o.JobName == item.JobName && o.JobGroup == item.JobGroup))
                {
                    var entity = _mapper.Map<Entities.QuarztSchedule>(item);
                    entity.Id = CombGuid.NewGuid();
                    _dbContext.QuarztSchedule.Add(entity);
                }
            });
            _dbContext.SaveChanges();
        }


    }
}
