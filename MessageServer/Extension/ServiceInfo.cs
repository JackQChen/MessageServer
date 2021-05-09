using System;
using System.ComponentModel;
using System.Linq;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Metadata;
using MessageLib.SocketEngine;

namespace MessageServer.Extension
{
    class ServiceInfo
    {
        private IPerformanceMonitor monitor;
        private string serviceName;

        public ServiceInfo(IPerformanceMonitor monitor, string serviceName)
        {
            this.monitor = monitor;
            this.serviceName = serviceName;
            this.monitor.OnStatusUpdate += Monitor_OnStatusUpdate;
        }

        private void Monitor_OnStatusUpdate(NodeStatus status)
        {
            var serverStatus = status.BootstrapStatus;
            var serviceStatus = status.InstancesStatus.First(p => p.Name == serviceName);
            this.CurrentConnectionCount = serviceStatus.GetValue<int>(StatusInfoKeys.TotalConnections, 0);
            this.MaxConnectionCount = serviceStatus.GetValue<int>(StatusInfoKeys.MaxConnectionNumber, 0);
            this.TotalSentSize = serviceStatus.GetValue<long>(StatusInfoKeys.TotalSent, 0).ToFileSize();
            this.SendingSpeed = Convert.ToInt64(serviceStatus.GetValue<double>(StatusInfoKeys.SendingSpeed, 0)).ToFileSize() + "/s";
            this.TotalReceivedSize = serviceStatus.GetValue<long>(StatusInfoKeys.TotalReceived, 0).ToFileSize();
            this.ReceivingSpeed = Convert.ToInt64(serviceStatus.GetValue<double>(StatusInfoKeys.ReceivingSpeed, 0)).ToFileSize() + "/s";
            this.CpuUsage = serverStatus.GetValue<float>(StatusInfoKeys.CpuUsage, 0).ToString("0.00") + "%";
            this.MemoryUsage = serverStatus.GetValue<long>(StatusInfoKeys.MemoryUsage, 0).ToFileSize();
        }

        [Category("\t服务状态"), Description("服务当前连接数"), DisplayName("当前连接数")]
        public int CurrentConnectionCount { get; set; }

        [Category("\t服务状态"), Description("服务最大连接数"), DisplayName("最大连接数")]
        public int MaxConnectionCount { get; set; }

        [Category("\t服务状态"), Description("累计发送"), DisplayName("累计发送")]
        public string TotalSentSize { get; set; }

        [Category("\t服务状态"), Description("发送速度"), DisplayName("发送速度")]
        public string SendingSpeed { get; set; }

        [Category("\t服务状态"), Description("累计接收"), DisplayName("累计接收")]
        public string TotalReceivedSize { get; set; }

        [Category("\t服务状态"), Description("接收速度"), DisplayName("接收速度")]
        public string ReceivingSpeed { get; set; }

        [Category("服务信息"), Description("CPU使用率"), DisplayName("CPU使用率")]
        public string CpuUsage { get; set; }

        [Category("服务信息"), Description("内存使用率"), DisplayName("内存使用率")]
        public string MemoryUsage { get; set; }

    }
}
