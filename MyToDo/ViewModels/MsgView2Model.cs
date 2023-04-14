using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace MyToDo.ViewModels
{
    public class MsgView2Model : BindableBase, IDialogAware
    {
        public MsgView2Model()
        {
            CancelCommand = new DelegateCommand(Cancel);
        }

        private void Cancel()
        {
            RequestClose(new DialogResult(ButtonResult.OK));
        }

        public string Title { get; set; } = "温馨提示";
        public string Content { get; set; } = "确认删据吗";

        public event Action<IDialogResult> RequestClose;
        public DelegateCommand CancelCommand { get; set; }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
