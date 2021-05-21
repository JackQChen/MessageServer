using System;
using System.ComponentModel;
using System.Linq;
using MessageLib.SocketBase;
using MessageLib.SocketBase.Metadata;
using MessageLib.SocketEngine;

namespace MessageServer.Extension
{
    [TypeConverter(typeof(PropertySorter))]
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
            this.CollectedTime = serviceStatus.CollectedTime;
            this.CurrentConnectionCount = serviceStatus.GetValue<int>(StatusInfoKeys.TotalConnections, 0);
            this.MaxConnectionCount = serviceStatus.GetValue<int>(StatusInfoKeys.MaxConnectionNumber, 0);
            this.TotalSentSize = serviceStatus.GetValue<long>(StatusInfoKeys.TotalSent, 0).ToFileSize();
            this.SendingSpeed = serviceStatus.GetValue<double>(StatusInfoKeys.SendingSpeed, 0).ToLong();
            this.SendingSpeedText = this.SendingSpeed.ToFileSize() + "/s";
            this.TotalReceivedSize = serviceStatus.GetValue<long>(StatusInfoKeys.TotalReceived, 0).ToFileSize();
            this.ReceivingSpeed = serviceStatus.GetValue<double>(StatusInfoKeys.ReceivingSpeed, 0).ToLong();
            this.ReceivingSpeedText = this.ReceivingSpeed.ToFileSize() + "/s";
            this.TotalHandledRequests = serviceStatus.GetValue<long>(StatusInfoKeys.TotalHandledRequests, 0);
            this.RequestHandlingSpeed = serviceStatus.GetValue<double>(StatusInfoKeys.RequestHandlingSpeed, 0).ToLong();
            this.RequestHandlingSpeedText = this.RequestHandlingSpeed + "T/s";
            this.CpuUsage = serverStatus.GetValue<float>(StatusInfoKeys.CpuUsage, 0).ToString("0.00") + "%";
            this.MemoryUsage = serverStatus.GetValue<long>(StatusInfoKeys.MemoryUsage, 0).ToFileSize();
        }

        [Browsable(false)]
        public DateTime CollectedTime { get; set; }

        [Category("连接"), Description("当前连接数"), DisplayName("当前连接数")]
        public int CurrentConnectionCount { get; set; }

        [Category("连接"), Description("最大连接数"), DisplayName("最大连接数")]
        public int MaxConnectionCount { get; set; }

        [Category("网络"), Description("累计发送"), DisplayName("累计发送"), PropertyOrder(4)]
        public string TotalSentSize { get; set; }

        [Browsable(false)]
        public long SendingSpeed { get; set; }

        [Category("网络"), Description("发送速度"), DisplayName("发送速度"), PropertyOrder(1)]
        public string SendingSpeedText { get; set; }

        [Category("网络"), Description("累计接收"), DisplayName("累计接收"), PropertyOrder(5)]
        public string TotalReceivedSize { get; set; }

        [Browsable(false)]
        public long ReceivingSpeed { get; set; }

        [Category("网络"), Description("接收速度"), DisplayName("接收速度"), PropertyOrder(2)]
        public string ReceivingSpeedText { get; set; }

        [Category("网络"), Description("累计处理"), DisplayName("累计处理"), PropertyOrder(6)]
        public long TotalHandledRequests { get; set; }

        [Browsable(false)]
        public long RequestHandlingSpeed { get; set; }

        [Category("网络"), Description("处理速度"), DisplayName("处理速度"), PropertyOrder(3)]
        public string RequestHandlingSpeedText { get; set; }

        [Category("资源"), Description("CPU使用率"), DisplayName("CPU使用率")]
        public string CpuUsage { get; set; }

        [Category("资源"), Description("内存使用率"), DisplayName("内存使用率")]
        public string MemoryUsage { get; set; }

    }
}
