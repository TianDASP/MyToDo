using DryIoc;
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using Prism.Events;
using Prism.Regions;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MyToDo.Views
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : System.Windows.Window
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogHostService dialogHost;
        private double dpi倍数;

        public MainView(IRegionManager regionManager, IEventAggregator aggregator, IDialogHostService dialogHost)
        {
            InitializeComponent();

            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };


            this.regionManager = regionManager;
            this.dialogHost = dialogHost;
            // 注册提示消息
            aggregator.RegisterMessage(arg =>
            {
                this?.Snackbar?.MessageQueue?.Enqueue(arg.Message);
            });

            // 注册等待消息(刷新)窗口
            aggregator.Register(async arg =>
            {
                MainViewDialogHost.IsOpen = arg.IsOpen;
                if (MainViewDialogHost.IsOpen)
                {
                    // 把首页设为进度条
                    MainViewDialogHost.DialogContent = new ProgressView();

                }
                // 最多持续2s
                await Task.Delay(2000);
                MainViewDialogHost.IsOpen = false;
            });

            //注册menubar更新
            //aggregator.RegisterUpdateMenubarEvent(arg =>
            //{
            //    var selected = menuBar.Items.Cast<MenuBar>().FirstOrDefault(x => x.NameSpace == arg.NameSpace);
            //    System.Windows.MessageBox.Show("消息中心触发 " + "即将导航至" + arg.Title + "  " + arg.NameSpace + "=====selected:" + selected.Title);
            //    menuBar.SelectedItem = selected; 
            //});
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        private void btnMax_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(this);
                rootGrid.Margin = new Thickness(0);
            }
            else
            {
                SystemCommands.MaximizeWindow(this); 
                rootGrid.Margin = new Thickness(8 / dpi倍数);
            }
        }

        private async void btnClose_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = await dialogHost.Question("温馨提示", $"确认退出系统?");
            if (dialogResult?.Result == Prism.Services.Dialogs.ButtonResult.OK)
            {
                SystemCommands.CloseWindow(this);
            }
        }
         

        private void avatar_DpiChanged(object sender, System.Windows.DpiChangedEventArgs e)
        {
            Image image = (Image)sender;
            var x = VisualTreeHelper.GetDpi(image);
            dpi倍数 = x.PixelsPerDip; 
        }

        private void BindDpiChangedAction(object sender, EventArgs e)
        {
            Image image = sender as Image;
            image.DpiChanged += avatar_DpiChanged;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            var window = sender as System.Windows.Window;
            if(window != null)
            {
                if(window.WindowState == WindowState.Maximized)
                { 
                    rootGrid.Margin = new Thickness(8 / dpi倍数);
                }
                else
                {

                    rootGrid.Margin = new Thickness(0);
                }
            }
        }
    }
}
