using DryIoc;
using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Service;
using MyToDo.ViewModels;
using MyToDo.ViewModels.Dialogs;
using MyToDo.Views;
using MyToDo.Views.Dialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MyToDo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainView>();
        }

        /// <summary>
        /// 注销当前用户
        /// </summary>
        public static void Logout(IContainerProvider container)
        {
            Current.MainWindow.Hide();
            var dialog = container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", (res) =>
            {
                if (res.Result != ButtonResult.OK)
                {
                    Application.Current.Shutdown();
                    return;
                }
                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                {
                    service.Confige();
                }
                Current.MainWindow.Show();
            });
        }
        protected override void OnInitialized()
        {
            var dialog = Container.Resolve<IDialogService>();
            dialog.ShowDialog("LoginView", (res) =>
            {
                if (res.Result != ButtonResult.OK)
                {
                    Application.Current.Shutdown();
                    return;
                }

                var service = App.Current.MainWindow.DataContext as IConfigureService;
                if (service != null)
                {
                    service.Confige();
                }
                base.OnInitialized();
            });
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.GetContainer().RegisterInstance(new Uri("http://localhost:5000/"), serviceKey: "webUrl");
            containerRegistry.GetContainer().Register<HttpRestClient>(made: Parameters.Of.Type<Uri>(serviceKey: "webUrl"));

            //containerRegistry.RegisterInstance(new HttpRestClient(containerRegistry.GetContainer().Resolve<Uri>("webUrl"))); 
             
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IToDoService, ToDoService>();
            containerRegistry.Register<IMemoService, MemoService>();
            containerRegistry.Register<IDialogHostService, DialogHostService>();


            containerRegistry.RegisterForNavigation<IndexView, IndexViewModel>();
            containerRegistry.RegisterForNavigation<MemoView, MemoViewModel>();
            containerRegistry.RegisterForNavigation<ToDoView, ToDoViewModel>();
            containerRegistry.RegisterForNavigation<SettingsView, SettingsViewModel>();
            containerRegistry.RegisterForNavigation<SkinView, SkinViewModel>();
            containerRegistry.RegisterForNavigation<AboutView>();
            containerRegistry.RegisterForNavigation<AddToDoView, AddToDoViewModel>();
            containerRegistry.RegisterForNavigation<AddMemoView, AddMemoViewModel>();
            containerRegistry.RegisterForNavigation<MsgView, MsgViewModel>();

            containerRegistry.RegisterDialog<MsgView2, MsgView2Model>();
            containerRegistry.RegisterDialog<LoginView, LoginViewModel>();
            // containerRegistry.RegisterSingleton<ProgressView>(() => new ProgressView());
            // containerRegistry.RegisterInstance(typeof(ProgressView), new ProgressView());
            //containerRegistry.RegisterInstance<ProgressView>();
            containerRegistry.RegisterSingleton<SharedData>();
        }
    }
}
