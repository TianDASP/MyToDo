using MyToDo.Common;
using MyToDo.Common.Models;
using MyToDo.Extensions;
using MyToDo.Service;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    class MainViewModel : BindableBase ,IConfigureService
    {
        private readonly SharedData sharedData;
        private readonly IRegionManager regionManager; 

        public MainViewModel(SharedData sharedData,IRegionManager regionManager,IContainerProvider container )
        {
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            GoForwardCommand = new DelegateCommand(GoForward);
            LogoutCommand = new DelegateCommand(() =>
            {
                App.Logout(container);
            });
            this.sharedData = sharedData;
            this.regionManager = regionManager;
            CreateMenuBar();
        }
         

        private void GoBack()
        {
            GoBackCommand = new DelegateCommand(() =>
            {
                if (journal!=null && journal.CanGoBack)
                {
                    journal.GoBack();
                }
            });
        }

        private void GoForward()
        {
            GoForwardCommand = new DelegateCommand(() =>
            {
                if (journal != null && journal.CanGoForward)
                {
                    journal.GoForward();
                }
            });
        }

        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.NameSpace))
                return;
            // 导航到 obj对应的View
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, callback=>
            {
                journal = callback.Context.NavigationService.Journal;
            }); 
        }

        public ObservableCollection<MenuBar> MenuBars { get; set; } = new ObservableCollection<MenuBar>();
        public DelegateCommand<MenuBar> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand GoForwardCommand { get; private set; }
        public DelegateCommand LogoutCommand { get;private set; }
        private IRegionNavigationJournal journal;
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value;RaisePropertyChanged();  }
        }

        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookOutline", Title = "待办事项", NameSpace = "ToDoView" });
            MenuBars.Add(new MenuBar() { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
        }

        // 配置首页初始化参数
        public void Confige()
        {
            UserName = AppSession.UserName;
            sharedData.Init();
            regionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView");
        }
    }
}
