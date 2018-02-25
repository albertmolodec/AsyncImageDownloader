using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Threading;
using System.Net;
using System.ComponentModel;


namespace ImageDownloader
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string DataPath = "..//..//Data//";
        private string url1;
        private string url2;
        private string url3;
        private bool IsImage1Downloaded = false;
        private bool IsImage2Downloaded = false;
        private bool IsImage3Downloaded = false;
        private int ImageNumber = 0;

        

        public MainWindow()
        {
            InitializeComponent();
            CreateImageFolder();

            List<Image> ImageList = new List<Image>();
            ImageList.Add(ImageBox1);
            ImageList.Add(ImageBox2);
            ImageList.Add(ImageBox3);

        }

        private void StartButton1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartButton2_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void StartButton3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopButton3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartAllButton1_Click(object sender, RoutedEventArgs e)
        {
            List<string> urls = new List<string>();
            if (IsURLCorrect(TextBox1.Text))
                urls.Add(TextBox1.Text);
            if (IsURLCorrect(TextBox2.Text))
                urls.Add(TextBox2.Text);
            if (IsURLCorrect(TextBox3.Text))
                urls.Add(TextBox3.Text);

            long TotalLength = 0;

            foreach (var url in urls)
            {
                TotalLength += GetContentLength(url);
            }

            MainProgressBar.Maximum = TotalLength;

            foreach (var url in urls)
            {
                Downloading(url);
            }

        }

        //private void DrawImage()
        // Не получается сказать программе, какую картинку нужно отрисовывать. Как передать параметр потоку? Сложно.
        //{
        //    ImageSourceConverter converter = new ImageSourceConverter();
        //    ImageSource imageSource = (ImageSource)converter.ConvertFromString(DataPath + Path.GetFileName(url));
        //    ImageBox.Source = imageSource;
        //}

        private void CreateImageFolder()
        {
            if (!Directory.Exists("..//..//Data//"))
                Directory.CreateDirectory("..//..//Data//");
        }

        private bool IsURLCorrect(string url)
        {
            if (string.IsNullOrWhiteSpace(url) || url == "Enter URL...")
                return false;
            try
            {
                WebRequest req = WebRequest.Create(url);
                req.Method = "HEAD";

                using (WebResponse resp = req.GetResponse())
                {
                    if (url.Contains(".jpg") || url.Contains(".gif") || url.Contains(".jpeg") || url.Contains(".png") || url.Contains(".bmp"))
                        return true;
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private long GetContentLength(string url)
        {
            long ContentLength = 0;

            WebRequest req = WebRequest.Create(url);
            req.Method = "HEAD";

            try
            {
                using (WebResponse resp = req.GetResponse())
                {
                    ContentLength = long.Parse(resp.Headers.Get("Content-Length"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return ContentLength;
        }

        private void Downloading(string url)
        {
            WebClient webload = new WebClient();

            webload.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webload.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webload.DownloadFileAsync(new Uri(url), DataPath + ImageNumber + "." + url.Split('.').Last());
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            MainProgressBar.Value += e.BytesReceived;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                if (MainProgressBar.Maximum == MainProgressBar.Value)
                    MessageBox.Show("Downloading is completed");

            }
        }
    }
}
