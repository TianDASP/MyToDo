using MyToDo.Extensions;
using Prism.DryIoc;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.ViewModels
{
    /// <summary>
    /// 为ViewModel添加导航前后的处理
    /// </summary>
    public class NavigationViewModel : BindableBase,  INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        private readonly IEventAggregator eventAggregator; 

        public NavigationViewModel(IContainerProvider containerProvider )
        {
            this.containerProvider = containerProvider;
            this.eventAggregator = this.containerProvider.Resolve<IEventAggregator>();
        }
        // 是否重用以前的窗口 即只初始化一次
        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        {
             
        }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        {
            //journal = navigationContext.NavigationService.Journal;
        }

        public void UpdateLoading(bool isOpen)
        {
            eventAggregator.UpdateLoading(new Common.Events.UpdateModel() { IsOpen = isOpen });
        }
    }
}
