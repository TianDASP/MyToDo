using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 待办实体dto
    /// </summary>
    public class ToDoDto : BaseDto<int>
    { 
        private string title;
        private string content;
        private bool status = false; 
         
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set => SetProperty(ref title, value);
        }
         /// <summary>
         /// 内容
         /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; RaisePropertyChanged(); }
        } 
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status
        {
            get { return status; }
            set { status = value;  RaisePropertyChanged();}
        }


    }
}
