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
using System.Windows.Threading;

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
        DispatcherTimer PaintingTimer = new DispatcherTimer();

        private int WindowPadding = 15;
        private ChoseArea status;

        private enum ChoseArea
        {
            None = 0,
            Top = 1,
            Bottom = 2,
            Left = 4,
            Right = 8
        };


        public MainWindow()
        {
            InitializeComponent();
            RestoreWindowBounds();
            PaintingTimer.Interval = TimeSpan.FromMilliseconds(10);
            PaintingTimer.Tick += new EventHandler(Window_Painting);
            PaintingTimer.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            PaintingTimer.Stop();
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
            if (Mouse_Clicking)
            {
                this.Top += Mouse_Point.Y - current_Point.Y;
                this.Left += Mouse_Point.X - current_Point.X;
            }
            status = ChoseArea.None;
            if (Mouse_Point.Y < WindowPadding) status |= ChoseArea.Top;
            if (Mouse_Point.Y > this.Height - WindowPadding) status |= ChoseArea.Bottom;
            if (Mouse_Point.X < WindowPadding) status |= ChoseArea.Left;
            if (Mouse_Point.X > this.Width - WindowPadding) status |= ChoseArea.Right;
            ChangeCursorOnWindow((int)status);

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

        private void ChangeCursorOnWindow(int status)
        {
            switch(status)
            {
                case (int)ChoseArea.Top:
                case (int)ChoseArea.Bottom:
                    this.Cursor = Cursors.SizeNS;
                    break;
                case (int)ChoseArea.Left:
                case (int)ChoseArea.Right:
                    this.Cursor = Cursors.SizeWE;
                    break;
                case (int)ChoseArea.Top + (int)ChoseArea.Left:
                case (int)ChoseArea.Bottom + (int)ChoseArea.Right:
                    this.Cursor = Cursors.SizeNWSE;
                    break;
                case (int)ChoseArea.Top + (int)ChoseArea.Right:
                case (int)ChoseArea.Bottom + (int)ChoseArea.Left:
                    this.Cursor = Cursors.SizeNESW;
                    break;
                default:
                    this.Cursor = Cursors.Arrow;
                    break;
            }
        }

        public void Window_Painting(object sender,EventArgs e)
        {
            //描画のテストのため、ここに退避しました。
            cpuObserver.RefreshCpuUsage();
            usageBox.Text = cpuObserver.cpuUsage.ToString();
            //
        }

    }
}
