using MirogramServer.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Mirogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public Frame AppFrame { get; set; }

        Thread CkeckServerAvailabilityThread;

        public Visibility OverlayVisisbility
        {
            get
            {
                return overlay.Visibility;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            App.ProgramWindows = this;
            AppFrame = mainFrame;
        }


        #region Navigate Methods :: Frame Navigation Methods
        public void FrameNavigate(Uri uri)
        {
            AppFrame.Navigate(uri);
        }
        public void FrameNavigate(Uri uri, object extraData)
        {
            AppFrame.Navigate(uri, extraData);
        }
        public void FrameNavigate(object content)
        {
            AppFrame.Navigate(content);
        }
        public void FrameNavigate(object content, object extraData)
        {
            AppFrame.Navigate(content, extraData);
        }



        #endregion FrameNavigationMethods

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = (WindowState != WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal;
            pathMax.Data = (WindowState != WindowState.Maximized) ? Geometry.Parse("M0,0L0,10L10,10L10,0L-0.5,0") : Geometry.Parse("M0,0L0,6L6,6L6,0L-0.5,0 M4,4L4,10L10,10L10,4L3,4");

        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;

        }

        #region OverlayManager
        public async void ShowOverlay(string text)
        {
            overlayText.Text = text;
            await overlay.ShowUsingLinearAnimationAsync();
        }
        public void HideOverlay()
        {
            overlay.HideUsingLinearAnimation();
        }
        public async void ShowOverlayTimed(string text, int millisecond)
        {
            ShowOverlay(text);
            await Task.Delay(millisecond);
            HideOverlay();
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TryConnecting();
        }


        public void TryConnecting()
        {
            CkeckServerAvailabilityThread = new Thread(() =>
            {
                bool hasError = true;
       
                while (hasError)
                {
                    try
                    {

                        App.NetworkManager.Connect();
                        App.NetworkManager.StartListeningToServer();
                        if (App.NetworkManager.LastConnectCallbackStatus)
                        {
                            Dispatcher.Invoke(() => { if(OverlayVisisbility == Visibility.Visible) HideOverlay(); });
                            hasError = false;
                        }
                        else throw new Exception();


                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            hasError = true;

                            Dispatcher.Invoke(() =>
                            {
                                if (overlay.Visibility == Visibility.Collapsed)
                                {
                                    ShowOverlay("در حال جستجوی سرور...");
                                }

                            });
                            App.NetworkManager.Disconnect();
                            Thread.Sleep(3000);
                        }
                        catch { return; }
                    }
                }

            })
            { Priority = ThreadPriority.AboveNormal, IsBackground = false };
            CkeckServerAvailabilityThread.Start();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                App.NetworkManager.Disconnect();
            }
            catch { }
            App.NetworkManager.RealeaseAllThreadManulaWaits();
            
        }
    }
}
