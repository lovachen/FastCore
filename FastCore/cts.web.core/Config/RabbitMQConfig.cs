﻿using System;
using System.Collections.Generic;
using System.Text;

namespace cts.web.core.Config
{
    /// <summary>
    /// rabbitmq 配置
    /// </summary>
    [Serializable]
    public class RabbitMQConfig
    {
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 虚拟
        /// </summary>
        public string VirtualHost { get; set; }
    }
}
