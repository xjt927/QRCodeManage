using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QRCodeManage
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 日志委托
        /// </summary>
        public static Action<string> LogAction;

        public MainWindow()
        {
            //绑定委托
            LogAction = LogInfo;
            InitializeComponent();
        }

        /// <summary>
        /// 创建二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreatQRCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                LogAction("开始创建二维码！");
                string printCantentText = PrintCantent.Text;
                QRCoderHelper qrCoderHelper = new QRCoderHelper();
                //(.+?)(?:(?![\\s\\S])|[\\s\\S]$)
                string strRegStr = @"(.+?(?:(?![\s\S])|[\s\S]$))";
                string uidRegStr = @"uid=(.+)";
                Regex reg = new Regex(strRegStr, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                Regex uidReg = new Regex(uidRegStr, RegexOptions.Multiline | RegexOptions.IgnoreCase);

                var matchesValues = reg.Matches(printCantentText);
                var qrCantentList = new List<Tuple<string, string>>();
                foreach (Match match in matchesValues)
                {
                    string a = match.Value.Replace("\r", "");
                    string uid = uidReg.Match(a).Groups[1].Value;
                    qrCantentList.Add(new Tuple<string, string>(a, uid));
                }

                //var qrCantentList = (from Match match in matchesValues
                //                     select match.Value.Replace("\r", "")
                //                         into machStr
                //                         let uid = uidReg.Match(machStr).Value
                //                         select new Tuple<string, string>(machStr, uid)).ToList();

                if (qrCantentList.Count == 0)
                {
                    MessageBox.Show("没有生成内容");
                }

                qrCoderHelper.Create_CodeImages(qrCantentList, 21);
            }
            catch (Exception ex)
            {
                LogAction(ex.ToString());
            }
        }

        /// <summary>
        /// 打开二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenQRCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"\BarCode_Images";
                System.Diagnostics.Process.Start(currentPath);
            }
            catch (Exception ex)
            {
                LogAction(ex.ToString());
            }
        }


        /// <summary>
        /// 日志输出
        /// </summary>
        private void LogInfo(string strLog)
        {
            try
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    //this.Visibility = Visibility.Visible;
                    string log = DateTime.Now + " " + strLog + "\r\n";
                    LogBox.Text = log + LogBox.Text;
                }));

                Log2File(strLog);
                System.GC.Collect();
            }
            catch (Exception ex)
            {
                Log2File(ex.ToString());
                System.GC.Collect();
            }
        }


        /// <summary> 
        /// 日志输出到本地
        /// </summary>
        /// <param name="log"></param>
        public static void Log2File(string log)
        {
            try
            {
                string path = Directory.GetCurrentDirectory() + "\\logfile.txt";
                using (var sWriter = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sWriter.WriteLine(DateTime.Now + " " + log);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
