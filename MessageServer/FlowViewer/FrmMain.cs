using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace FlowViewer
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            InitChart();
        }

        ChartValues<MeasureModel> sendValues;
        ChartValues<MeasureModel> recvValues;
        ChartValues<MeasureModel> connValues;

        int currentCount = 5, stepCount = 5, displayCount = 30;

        Process serverProc;

        private void InitChart()
        {
            this.sendValues = new ChartValues<MeasureModel>();
            this.recvValues = new ChartValues<MeasureModel>();
            this.connValues = new ChartValues<MeasureModel>();
            Charting.For<MeasureModel>(Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)
                .Y(model => model.Value));
            chartFlow.Series = new SeriesCollection { 
                new LineSeries {
                    Values = sendValues, 
                    PointGeometry = null ,
                    Title = "发送速率",
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Stroke =new  System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff,0xFE,0xAE,0x51))
                },
                new LineSeries {
                    Values = recvValues, 
                    PointGeometry = null,
                    Title = "接收速率",
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Stroke =new  System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff,0x46,0xA8,0xF9))
                },
                new LineSeries {
                    Values = connValues, 
                    PointGeometry = null,
                    Title = "连接数",
                    ScalesYAt= 1,
                    Fill = System.Windows.Media.Brushes.Transparent,
                    Stroke =new  System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0xff,0x25,0x92,0x41))
                }
            };
            chartFlow.LegendLocation = LegendLocation.Top;
            chartFlow.AxisX.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.Black,
                LabelFormatter = value => new DateTime((long)value).ToString("HH:mm:ss"),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(stepCount).Ticks,
                    Stroke = System.Windows.Media.Brushes.Silver,
                    StrokeThickness = 1.5f
                }
            });
            chartFlow.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.Black,
                LabelFormatter = value => FormatFileSize(value),
                MinValue = 0,
                Separator = new Separator
                {
                    Stroke = System.Windows.Media.Brushes.LightGray,
                    StrokeThickness = 0.5f
                }
            });
            chartFlow.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.Black,
                MinValue = 0,
                Position = AxisPosition.RightTop,
                Separator = new Separator
                {
                    Stroke = System.Windows.Media.Brushes.Transparent
                }
            });
            SetAxisLimits(DateTime.Now);
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (serverProc != null)
                        if (serverProc.HasExited)
                            Application.Exit();
                    Thread.Sleep(10000);
                }
            });
        }

        string FormatFileSize(double fileSize)
        {
            if (fileSize < 0)
                return "ErrorSize";
            else if (fileSize >= 1024 * 1024 * 1024)
                return string.Format("{0:########0.00} GB/s", fileSize / (1024 * 1024 * 1024));
            else if (fileSize >= 1024 * 1024)
                return string.Format("{0:####0.00} MB/s", fileSize / (1024 * 1024));
            else if (fileSize >= 1024)
                return string.Format("{0:####0.00} KB/s", fileSize / 1024);
            else
                return string.Format("{0} B/s", fileSize);
        }

        protected override void DefWndProc(ref Message m)
        {
            if (m.Msg == 0x0C)
            {
                var strText = Marshal.PtrToStringUni(m.LParam);
                switch ((int)m.WParam)
                {
                    case 1:
                        {
                            this.serverProc = Process.GetProcessById(Convert.ToInt32(strText));
                        }
                        break;
                    case 2:
                        {
                            var rate = strText.Split(',');
                            this.AddPoint(Convert.ToInt32(rate[0]), Convert.ToInt64(rate[1]), Convert.ToInt64(rate[2]));
                        }
                        break;
                }
            }
            else
                base.DefWndProc(ref m);
        }

        private void SetAxisLimits(DateTime now)
        {
            chartFlow.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(0.5).Ticks;
            if (currentCount == 5)
            {
                currentCount = 0;
                chartFlow.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(displayCount).Ticks;
            }
            currentCount++;
        }

        private void AddPoint(int connCount, long recvBytes, long sendBytes)
        {
            var now = System.DateTime.Now;
            sendValues.Add(new MeasureModel
            {
                DateTime = now,
                Value = sendBytes
            });
            recvValues.Add(new MeasureModel
            {
                DateTime = now,
                Value = recvBytes
            });
            connValues.Add(new MeasureModel
            {
                DateTime = now,
                Value = connCount
            });
            SetAxisLimits(now);
            if (currentCount == 1)
            {
                var count = connValues.Count - displayCount;
                for (int i = 0; i < count; i++)
                {
                    sendValues.RemoveAt(0);
                    recvValues.RemoveAt(0);
                    connValues.RemoveAt(0);
                }
            }
        }
    }
}
