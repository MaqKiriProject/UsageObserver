using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace MaqKiriAppTest
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        Point current_Point = new Point(0, 0);
        Point Mouse_Point = new Point(0, 0);
        private bool Mouse_Clicking = false;

        CPUObserver cpuObserver = new CPUObserver();

        public MainWindow()
        {
            InitializeComponent();
            RestoreWindowBounds();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            SaveWindowBounds();
            base.OnClosing(e);
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            current_Point = e.GetPosition(this);
            Mouse_Clicking = true;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            Mouse_Point = e.GetPosition(this);
            Debug.Print(Mouse_Point.ToString());
            if (Mouse_Clicking)
            {
                this.Top += Mouse_Point.Y - current_Point.Y;
                this.Left += Mouse_Point.X - current_Point.X;
            }

            cpuObserver.RefreshCpuUsage();
            usageBox.Text = cpuObserver.cpuUsage.ToString();
        }

        private void Window_MouseUp(object sender, MouseEventArgs e)
        {
            Mouse_Point = e.GetPosition(this);
            Mouse_Clicking = false;
        }

        private void SaveWindowBounds()
        {
            var settings = Properties.Settings.Default;
            settings.WindowMaximized = WindowState == WindowState.Maximized;
            WindowState = WindowState.Normal;
            settings.WindowLeft = this.Left;
            settings.WindowTop = this.Top;
            settings.WindowWidth = this.Width;
            settings.WindowHeight = this.Height;
            settings.Save();
        }
        private void RestoreWindowBounds()
        {
            var settings = Properties.Settings.Default;
            if (settings.WindowLeft >= 0 && (settings.WindowLeft + settings.WindowWidth) < SystemParameters.VirtualScreenWidth) this.Left = settings.WindowLeft;
            if (settings.WindowTop >= 0 && (settings.WindowTop + settings.WindowHeight) < SystemParameters.VirtualScreenHeight) this.Top = settings.WindowTop;
            if (settings.WindowWidth > 0 && settings.WindowWidth <= SystemParameters.WorkArea.Width) this.Width = settings.WindowWidth;
            if (settings.WindowHeight > 0 && settings.WindowHeight <= SystemParameters.WorkArea.Height) this.Height = settings.WindowHeight;
            if (settings.WindowMaximized) this.Loaded += (o, e) => WindowState = WindowState.Maximized;
        }

    }
}
