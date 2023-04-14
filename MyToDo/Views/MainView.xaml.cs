using DryIoc;
using MaterialDesignThemes.Wpf;
using MyToDo.Common;
using MyToDo.Extensions;
using Prism.Events;
using Prism.Regions;
using System;
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
        private bool mRestoreForDragMove = false;
        private double dpi倍数;
        private Point currentScrennLeftTop;



        public MainView(IRegionManager regionManager, IEventAggregator aggregator, IDialogHostService dialogHost)
        {
            InitializeComponent();
            简单Event初始化绑定();

            this.regionManager = regionManager;
            this.dialogHost = dialogHost;
            // 注册提示消息
            aggregator.RegisterMessage(arg =>
            {
                this?.Snackbar?.MessageQueue?.Enqueue(arg.Message);
            }); 

            // 注册等待消息窗口
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
        }


        public void 简单Event初始化绑定()
        {
            btnMin.Click += (s, e) => { this.WindowState = WindowState.Minimized; };
            btnMax.Click += (s, e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }
            };
            btnClose.Click += async (s, e) =>
            {
                var dialogResult = await dialogHost.Question("温馨提示", $"确认退出系统?");
                if (dialogResult?.Result == Prism.Services.Dialogs.ButtonResult.OK)
                {
                    this.Close();
                }
            };
            ColorZone.MouseMove += OnMouseMove;
            ColorZone.MouseLeftButtonDown += OnMouseLeftButtonDown;
            ColorZone.MouseLeftButtonUp += OnMouseLeftButtonUp;
            
            //ColorZone.MouseDoubleClick += (s, e) =>
            //{
            //    if (this.WindowState == WindowState.Maximized)
            //    {
            //        this.WindowState = WindowState.Normal;
            //    }
            //    else
            //    {
            //        this.WindowState = WindowState.Maximized;
            //    }
            //};
            menuBar.SelectionChanged += (s, e) =>
            {
                drawerHost.IsLeftDrawerOpen = false;
            };
            //this.SizeChanged += (s, e) =>
            //{
            //    var text = $"当前宽高:{Width},{Height}当前屏幕大小{MaxWidth},{MaxHeight}当前左上坐标{Left},{Top}";
            //    tt.Text = text ;
            //};
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (ResizeMode != ResizeMode.CanResize &&
                    ResizeMode != ResizeMode.CanResizeWithGrip)
                {
                    return;
                }

                WindowState = WindowState == WindowState.Maximized
                    ? WindowState.Normal
                    : WindowState.Maximized;

                Left = currentScrennLeftTop.X ;
                Top = currentScrennLeftTop.Y ;
            }
            else
            {
                mRestoreForDragMove = WindowState == WindowState.Maximized;
                DragMove();
            }
        }

        /// <summary>
        /// wpf自定义标题栏拖拽效果,可复用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mRestoreForDragMove)
            {
                mRestoreForDragMove = false;
                // 鼠标在当前屏幕的位置(实际分辨率)
                var point = PointToScreen(e.MouseDevice.GetPosition(this));
                //当前屏幕参数,左上角坐标 + 渲染分辨率大小(经过dpi调整后)
                //当前屏幕工作区(去除任务栏)参数,左上角坐标 + 渲染分辨率大小(经过dpi调整后)
                var screen = Screen.FromPoint(new System.Drawing.Point((int)point.X, (int)point.Y));
                // 分为左1/3 中间1/3 右1/3  zo'yz
                var pointToLeft = point.X - screen.Bounds.Left;
                var pointToRight = screen.Bounds.Left + screen.Bounds.Width - point.X;
                if (pointToLeft < RestoreBounds.Width * dpi倍数 / 2)
                {
                    // left与top是渲染位置  RestoreBounds也是渲染分辨率
                    Left = screen.Bounds.Left / dpi倍数;
                    Top = (point.Y - 10) / dpi倍数;
                }
                else if (pointToRight < RestoreBounds.Width * dpi倍数 / 2)
                {
                    var 右距 = (screen.Bounds.Left + screen.Bounds.Width - point.X);
                    Left = (point.X - (RestoreBounds.Width * dpi倍数 - 右距)) / dpi倍数;
                    Top = (point.Y - 10) / dpi倍数;
                }
                else
                {
                    Left = (point.X - (point.X - screen.Bounds.Left) / screen.Bounds.Width * RestoreBounds.Width * dpi倍数) / dpi倍数;
                    Top = (point.Y - 10) / dpi倍数;
                }
                WindowState = WindowState.Normal;
                DragMove(); 
            }
            //var text = $"当前宽高:{Width},{Height}当前屏幕大小{MaxWidth},{MaxHeight}当前左上坐标{Left},{Top}";
            //tt.Text = text;
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            mRestoreForDragMove = false;
        }

        /// <summary>
        /// 用于获取屏幕dpi倍数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void avatar_DpiChanged(object sender, RoutedEventArgs e)
        {
            Image image = (Image)sender;
            var x = VisualTreeHelper.GetDpi(image);
            dpi倍数 = x.PixelsPerDip;

            //Dpi.Text = x.PixelsPerDip.ToString();

            var hwndSource = (HwndSource)PresentationSource.FromVisual(this);
            if (hwndSource is null)
            {
                return;
            }
            var hWnd = hwndSource.Handle;
            //当前屏幕参数,左上角坐标 + 渲染分辨率大小(经过dpi调整后)
            //当前屏幕工作区(去除任务栏)参数,左上角坐标 + 渲染分辨率大小(经过dpi调整后)
            var screen = System.Windows.Forms.Screen.FromHandle(hWnd);
            this.MaxWidth = screen.WorkingArea.Width / dpi倍数;
            this.MaxHeight = screen.WorkingArea.Height / dpi倍数;
            currentScrennLeftTop = new Point(screen.Bounds.Left/dpi倍数, screen.Bounds.Top/dpi倍数);
        }

        private void BindDpiChangedAction(object sender, EventArgs e)
        {
            Image image = sender as Image;
            image.DpiChanged += avatar_DpiChanged;
        }


    }
}
