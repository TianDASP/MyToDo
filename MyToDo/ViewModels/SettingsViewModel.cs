using MyToDo.Common.Models;
using MyToDo.Extensions;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    class SettingsViewModel : INavigationAware
    {
        public SettingsViewModel(IRegionManager regionManager, IContainerProvider containerProvider)
        {
            CreateMenuBar();
            NavigateCommand = new DelegateCommand<MenuBar>(Navigate); 
            this.regionManager = regionManager;
            this.containerProvider = containerProvider;

            this.eventAggregator = this.containerProvider.Resolve<IEventAggregator>();
        }

        public ObservableCollection<MenuBar> MenuBars { get; set; } = new ObservableCollection<MenuBar>();
        public DelegateCommand<MenuBar> NavigateCommand { get; }

        private readonly IRegionManager regionManager;
        private readonly IContainerProvider containerProvider;
        public readonly IEventAggregator eventAggregator;


        private void Navigate(MenuBar obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.NameSpace))
                return;
            // 导航到 obj对应的View
            regionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(obj.NameSpace);
        }

        void CreateMenuBar()
        {
            MenuBars.Add(new MenuBar() { Icon = "Palette", Title = "个性化", NameSpace = "SkinView" });
            MenuBars.Add(new MenuBar() { Icon = "Cog", Title = "系统设置", NameSpace = "" });
            MenuBars.Add(new MenuBar() { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" }); 
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

            //eventAggregator.SendMenubarUpdateMessage("设置", "SettingsView");
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        { 
        }
    }
}
