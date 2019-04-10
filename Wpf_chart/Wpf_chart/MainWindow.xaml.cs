using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Events;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.IO;

namespace Wpf_chart
{
    public partial class MainWindow :Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public String filename = "";

        int scrollbar_view=0;//scrollbar value
        int[] v1 = new int[50000];//level 1
        int[] v2 = new int[50000];//level 2
        int[] v3 = new int[50000];//level 3
        int max;//max X count
        int zoom_size = 200;//diplay X size

        public MainWindow()
        {
            InitializeComponent();
            Chart.Visibility = Visibility.Hidden;
            scrollbar1.Visibility = Visibility.Hidden;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            scrollbar1.Height = Chart.ActualWidth - 60;
            scrollbar1.Margin = new Thickness(0, 0, 0, -(Chart.ActualWidth + 10 - 60));
        }

        private void b1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (string.IsNullOrEmpty(ofd.InitialDirectory))
                ofd.InitialDirectory = @"";

            ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            ofd.Title = "請開啟文字檔案";

            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                filename = ofd.FileName;
                label1.Content = filename;
            }
        }

        private void b2_Click(object sender, RoutedEventArgs e)
        {
            if (filename == "")
            {
                MessageBox.Show("請選擇檔案");
                return;
            }
            Chart.Visibility = Visibility.Visible;
            scrollbar1.Visibility = Visibility.Visible;
            GetFileValue();
        }

        private void GetFileValue()
        {
            string s;

            using (StreamReader sr = new StreamReader(filename))
            {
                String line = sr.ReadToEnd();
                max = line.Length / 6;
                for (int i = 0; i < max; i++)
                {
                    s = line.Substring(i * 6, 2);
                    v1[i] = Convert.ToInt32(s, 16);

                    s = line.Substring(i * 6 + 2, 2);
                    v2[i] = Convert.ToInt32(s, 16);

                    s = line.Substring(i * 6 + 4, 2);
                    v3[i] = Convert.ToInt32(s, 16);
                }
            }
            scrollbar1.Height =  Chart.ActualWidth-60;
            scrollbar1.Margin = new Thickness(0,0,0, -(Chart.ActualWidth+10-60));
            scrollbar1.Maximum = max- zoom_size;
            Show_chart();
        }

        private void Show_chart()
        {
            Labels = new string[max];
            
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "level 1",
                    Values = new ChartValues<int> { },
                    Fill = Brushes.Transparent,
                    PointGeometry = DefaultGeometries.None
                },
                new LineSeries
                {
                    Title = "level 2",
                    Values = new ChartValues<int> { },
                    Fill = Brushes.Transparent,
                    PointGeometry = DefaultGeometries.None
                },
                new LineSeries
                {
                    Title = "level 3",
                    Values = new ChartValues<int> { },
                    Fill = Brushes.Transparent,
                    PointGeometry = DefaultGeometries.None
                }
            };

            for (int i = scrollbar_view; i < scrollbar_view + zoom_size; i++)
            {
                SeriesCollection[0].Values.Add(v1[i]);
                SeriesCollection[1].Values.Add(v2[i]);
                SeriesCollection[2].Values.Add(v3[i]);
                Labels[i- scrollbar_view] = i.ToString();
            }
            YFormatter = value => value.ToString();
            DataContext = this;
        }

        private void ScrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scrollbar_view = (int)scrollbar1.Value;

            DataContext = null;//clear chart
            Show_chart();
        }
    }
}
