using System;
using System.Collections.Generic;
using System.Text;

namespace Fast.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class Quarzts
    {
        #region 组

        /// <summary>
        /// 
        /// </summary>
        public const string GROUP_NAME = "任务组";
         

        #endregion

        #region 名

        /// <summary>
        /// 
        /// </summary>
        public const string JOB_NAME = "任务X1";

         

        #endregion

        /// <summary>
        /// 任务初始数据
        /// </summary>
        public List<QuarztScheduleMapping> QuarztSchedules
        {
            get
            {
                List<QuarztScheduleMapping> list = new List<QuarztScheduleMapping>();
                //每天1点出发一次
                list.Add(new QuarztScheduleMapping()
                {
                    JobGroup = GROUP_NAME,
                    JobName = JOB_NAME,
                    CronExpress = "0 0 1 * * ?",
                    RunStatus = (int)JobStatus.初始值,
                    StarRunTime = DateTime.Now,
                    EndRunTime = DateTime.MaxValue.AddDays(-1)
                });
                  

                return list;
            }
        }
    }
}
