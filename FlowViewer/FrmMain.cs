using System;
using System.Runtime.InteropServices;
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

        int stepTimeSpan = 1, displayTimeSpan = 60;

        private void InitChart()
        {
            ((System.Windows.Controls.UserControl)this.chartFlow.Child).UseLayoutRounding = true;
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
                LabelFormatter = value => FormatLabel(new DateTime((long)value)),
                Separator = new Separator
                {
                    Step = TimeSpan.FromSeconds(stepTimeSpan).Ticks,
                    StrokeThickness = 0
                }
            });
            chartFlow.AxisY.Add(new Axis
            {
                Foreground = System.Windows.Media.Brushes.Black,
                LabelFormatter = value => FormatFileSize((long)value) + "/s",
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
        }

        string FormatLabel(DateTime dt)
        {
            if (dt.Second % 5 == 0)
                return dt.ToString("HH:mm:ss");
            else
                return string.Empty;
        }

        string FormatFileSize(long fileSize)
        {
            if (fileSize < 0)
                return "ErrorSize";
            else if (fileSize >= 1024 * 1024 * 1024)
                return string.Format("{0:########0.00}GB", fileSize / (1024d * 1024 * 1024));
            else if (fileSize >= 1024 * 1024)
                return string.Format("{0:####0.00}MB", fileSize / (1024d * 1024));
            else if (fileSize >= 1024)
                return string.Format("{0:####0.00}KB", fileSize / 1024d);
            else
                return string.Format("{0}B", fileSize);
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
                            var rate = strText.Split(',');
                            this.AddPoint(new DateTime(Convert.ToInt64(rate[0])), Convert.ToInt32(rate[1]), Convert.ToInt64(rate[2]), Convert.ToInt64(rate[3]));
                        }
                        break;
                }
            }
            else
                base.DefWndProc(ref m);
        }

        private void SetAxisLimits(DateTime now)
        {
            chartFlow.AxisX[0].MinValue = now.Ticks - TimeSpan.FromSeconds(displayTimeSpan).Ticks;
            chartFlow.AxisX[0].MaxValue = now.Ticks + TimeSpan.FromSeconds(stepTimeSpan).Ticks;
        }

        private void AddPoint(DateTime now, int connCount, long recvBytes, long sendBytes)
        {
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
            if (sendValues.Count > displayTimeSpan)
            {
                sendValues.RemoveAt(0);
                recvValues.RemoveAt(0);
                connValues.RemoveAt(0);
            }
        }
    }
}
