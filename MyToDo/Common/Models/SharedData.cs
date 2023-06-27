 
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 多个页面共享的数据
    /// </summary>
    public class SharedData : BindableBase
    {
        public SharedData()
        {
            Init();
        }
        public bool IsToDoDtosInited;
        public bool IsMemoDtosInited;
        public ObservableCollection<ToDoDto> RealToDoDtos { get; set; } = new ObservableCollection<ToDoDto>();
        public ObservableCollection<MemoDto> RealMemoDtos { get; set; } = new ObservableCollection<MemoDto>();

        public IRegionNavigationJournal journal { get; set; } 
        public void Init()
        {
            IsToDoDtosInited = false;
            IsMemoDtosInited = false;
            RealToDoDtos.Clear();
            RealMemoDtos.Clear();
        }
    }
}
