using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common
{
    public interface IDialogHostAware : IDialogAware
    {
        /// <summary>
        /// 所属的Dialog名称
        /// </summary>
        string DialogHostName { get; set; }
       // void OnDialogOpened(IDialogParameters parameters);

        DelegateCommand SaveCommand { get;set; }
        DelegateCommand CancelCommand { get;set; }
    }
}
