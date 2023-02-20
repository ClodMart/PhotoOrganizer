using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PhotoOrganizer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region ResizeWindows
        bool ResizeInProcess = false;
        private void Resize_Init(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = true;
                senderRect.CaptureMouse();
            }
        }

        private void Resize_End(object sender, MouseButtonEventArgs e)
        {
            Rectangle senderRect = sender as Rectangle;
            if (senderRect != null)
            {
                ResizeInProcess = false; ;
                senderRect.ReleaseMouseCapture();
            }
        }

        private void Resizeing_Form(object sender, MouseEventArgs e)
        {
            if (ResizeInProcess)
            {
                double temp = 0;
                Rectangle senderRect = sender as Rectangle;
                Window mainWindow = senderRect.Tag as Window;
                if (senderRect != null)
                {
                    double width = e.GetPosition(mainWindow).X;
                    double height = e.GetPosition(mainWindow).Y;
                    senderRect.CaptureMouse();
                    if (senderRect.Name.Contains("right", StringComparison.OrdinalIgnoreCase))
                    {
                        width += 5;
                        if (width > 0)
                            mainWindow.Width = width;
                    }
                    if (senderRect.Name.Contains("left", StringComparison.OrdinalIgnoreCase))
                    {
                        width -= 5;
                        temp = mainWindow.Width - width;
                        if ((temp > mainWindow.MinWidth) && (temp < mainWindow.MaxWidth))
                        {
                            mainWindow.Width = temp;
                            mainWindow.Left += width;
                        }
                    }
                    if (senderRect.Name.Contains("bottom", StringComparison.OrdinalIgnoreCase))
                    {
                        height += 5;
                        if (height > 0)
                            mainWindow.Height = height;
                    }
                    if (senderRect.Name.ToLower().Contains("top", StringComparison.OrdinalIgnoreCase))
                    {
                        height -= 5;
                        temp = mainWindow.Height - height;
                        if ((temp > mainWindow.MinHeight) && (temp < mainWindow.MaxHeight))
                        {
                            mainWindow.Height = temp;
                            mainWindow.Top += height;
                        }
                    }
                }
            }
        }
        #endregion

        private bool Normal = true;
        private void Reduce_Click(object sender, RoutedEventArgs e)
        {
            //Window Win = Application.Current.MainWindow;
            Window Win = Window.GetWindow((Button)sender);
            //var Win = VisualTreeHelper.GetParent(this);
            //while (!(Win is Window))
            //{
            //    Win = VisualTreeHelper.GetParent(Win);
            //}
            Win.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            //WindowCollection windows = Application.Current.Windows;
            //int WindowNumber = windows.Count - 1;
            //Window Win = windows[WindowNumber];
            Window Win = Window.GetWindow((Button)sender);
            Win?.Close(); 
        }

        private void Resize_Click(object sender, RoutedEventArgs e)
        {
            //WindowCollection windows = Application.Current.Windows;
            //int WindowNumber = windows.Count - 1;
            //Window Win = windows[WindowNumber];
            Window Win = Window.GetWindow((Button)sender);
            if(Win.WindowState == WindowState.Normal)
            {
                Win.WindowState = WindowState.Maximized;
            }
            else if(Win.WindowState == WindowState.Maximized)
            {
                Win.WindowState = WindowState.Normal;
            }
            //Win.WindowState = Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Window Win = Window.GetWindow((TextBlock)sender);
            if (e.ChangedButton == MouseButton.Left)
                Win.DragMove();
        }
    }
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }
}
