using System;
using System.ComponentModel;
using MessageLib;

namespace MessageServer.Extension
{
    [TypeConverter(typeof(PropertySorter))]
    internal class ServiceInfo
    {
        TcpServer service;
        long lastSentSize, lastReceivedSize;

        public ServiceInfo(TcpServer tcpServer)
        {
            this.service = tcpServer;
            this.Backlog = this.service.OptionBacklog;
            this.MaxConnectionCount = this.service.OptionMaxConnectionCount;
            this.SendBufferSize = Convert.ToInt64(this.service.OptionSendBufferSize).ToFileSize();
            this.ReceiveBufferSize = Convert.ToInt64(this.service.OptionReceiveBufferSize).ToFileSize();
        }

        public void Update()
        {
            this.CollectedTime = DateTime.Now;
            this.CurrentConnectionCount = this.service.SessionCount;
            this.TotalSentSize = this.service.BytesSent.ToFileSize();
            this.TotalReceivedSize = this.service.BytesReceived.ToFileSize();
            this.SendingSpeed = this.service.BytesSent - this.lastSentSize;
            this.ReceivingSpeed = this.service.BytesReceived - this.lastReceivedSize;
            this.SendingSpeedText = this.SendingSpeed.ToFileSize() + "/s";
            this.ReceivingSpeedText = this.ReceivingSpeed.ToFileSize() + "/s";
            this.lastSentSize = this.service.BytesSent;
            this.lastReceivedSize = this.service.BytesReceived;
        }

        [Browsable(false)]
        public DateTime CollectedTime { get; set; }

        [Category("连接"), Description("挂起队列长度"), DisplayName("挂起队列长度"), PropertyOrder(3)]
        public int Backlog { get; set; }

        [Category("连接"), Description("当前连接数"), DisplayName("当前连接数"), PropertyOrder(1)]
        public int CurrentConnectionCount { get; set; }

        [Category("连接"), Description("最大连接数"), DisplayName("最大连接数"), PropertyOrder(2)]
        public int MaxConnectionCount { get; set; }

        [Category("网络"), Description("累计发送"), DisplayName("累计发送"), PropertyOrder(3)]
        public string TotalSentSize { get; set; }

        [Browsable(false)]
        public long SendingSpeed { get; set; }

        [Category("网络"), Description("发送速度"), DisplayName("发送速度"), PropertyOrder(1)]
        public string SendingSpeedText { get; set; }

        [Category("网络"), Description("累计接收"), DisplayName("累计接收"), PropertyOrder(4)]
        public string TotalReceivedSize { get; set; }

        [Browsable(false)]
        public long ReceivingSpeed { get; set; }

        [Category("网络"), Description("接收速度"), DisplayName("接收速度"), PropertyOrder(2)]
        public string ReceivingSpeedText { get; set; }

        [Category("缓存"), Description("发送缓存"), DisplayName("发送缓存"), PropertyOrder(1)]
        public string SendBufferSize { get; set; }

        [Category("缓存"), Description("接收缓存"), DisplayName("接收缓存"), PropertyOrder(2)]
        public string ReceiveBufferSize { get; set; }

    }
}
