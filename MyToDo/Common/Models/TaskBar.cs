using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 任务栏
    /// </summary>
    public class TaskBar : BindableBase
    {
        private string icon;
        private string title;
        private string content;
        private string color;
        private string target;
         
        /// <summary>
        /// 首页Icon
        /// </summary>
        public string Icon
        {
            get => icon;
            set => SetProperty<string>(ref icon, value);
        }

        public string Title
        {
            get => title;
            set => SetProperty<string>(ref title, value);
        }

        public string Content
        {
            get => content;
            set => SetProperty<string>(ref content, value);
        }

        public string Color
        {
            get=> color;
            set=>SetProperty<string>(ref color, value);
        }
        /// <summary>
        /// 触发目标,要跳转的页面
        /// </summary>
        public string Target
        {
            get => target;
            set => SetProperty<string>(ref target, value);
        }
    }
}
