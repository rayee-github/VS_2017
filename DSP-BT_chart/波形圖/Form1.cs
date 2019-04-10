using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace 波形圖
{
    public partial class Form1 : Form
    {
        public int max = 40000;
        public int width = 500;
        public String filename="";

        int[] a1 = new int[50000];
        int[] a2 = new int[50000];
        int[] a3 = new int[50000];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Text = width.ToString();
            InitChart();
        }

        private void InitChart()
        {
            //定義圖表區域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("123");
            this.chart1.ChartAreas.Add(chartArea1);

            //X滾動軸
            this.chart1.ChartAreas[0].CursorX.AutoScroll = true;
            this.chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            this.chart1.ChartAreas[0].AxisX.ScaleView.SizeType = DateTimeIntervalType.Number;
            this.chart1.ChartAreas[0].AxisX.ScaleView.Zoom(0, width);
            this.chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            this.chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = 5;

            //定義存儲和顯示點的容器
            this.chart1.Series.Clear();
            Series series1 = new Series("level1");
            Series series2 = new Series("level2");
            Series series3 = new Series("level3");
            series1.ChartArea = "123";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);
            //設置圖表顯示樣式
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 260;
            this.chart1.ChartAreas[0].AxisX.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //設置標題
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "UV顯示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //設置圖表顯示樣式
            this.chart1.Series[0].Color = Color.Red;
            this.chart1.Series[1].Color = Color.Blue;
            this.chart1.Series[2].Color = Color.Green;
            //設置線寬度
            this.chart1.Series[0].BorderWidth = 3;
            this.chart1.Series[1].BorderWidth = 3;
            this.chart1.Series[2].BorderWidth = 3;

            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            this.chart1.Series[1].ChartType = SeriesChartType.Line;
            this.chart1.Series[2].ChartType = SeriesChartType.Line;

            this.chart1.Series[0].Points.Clear();
            this.chart1.Series[1].Points.Clear();
            this.chart1.Series[2].Points.Clear();
        }

        private void Start_bt_Click(object sender, EventArgs e)
        {
            String s;
            
            width = Int32.Parse(comboBox1.Text);
            InitChart();

            if (filename == "")
            {
                MessageBox.Show("找不到檔案");
                return;
            }

            using (StreamReader sr = new StreamReader(filename))
            {
                String line = sr.ReadToEnd();
                max = line.Length/6;
                for (int i = 0; i < max; i++)
                {
                    s = line.Substring(i * 6, 2);
                    a1[i] = Convert.ToInt32(s, 16);

                    s = line.Substring(i * 6 + 2, 2);
                    a2[i] = Convert.ToInt32(s, 16);

                    s = line.Substring(i * 6 + 4, 2);
                    a3[i] = Convert.ToInt32(s, 16);
                }
            }
            /*string[] day = new string[7] { "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日" };
            for (int i = 0; i < 7; i++)
            {
                chart1.Series[0].Points.AddXY(day[i], a1[i]);
                chart1.Series[1].Points.AddXY(day[i], a2[i]);
                chart1.Series[2].Points.AddXY(day[i], a3[i]);
            }*/
            for (int i = 0; i < max; i++)
            {
                chart1.Series[0].Points.AddXY(i, a1[i]);
                chart1.Series[1].Points.AddXY(i, a2[i]);
                chart1.Series[2].Points.AddXY(i, a3[i]);
            }
        }

        private void Select_file_bt_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog(); 
            if (string.IsNullOrEmpty(ofd.InitialDirectory))
                ofd.InitialDirectory = "//";

            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            ofd.Title = "請開啟文字檔案";

            if (ofd.ShowDialog(this) == DialogResult.Cancel)
                return;

            filename= ofd.FileName;
            label1.Text = filename;
        }
    }
}