using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    /// <summary>
    /// 备忘录实体Dto
    /// </summary>
    public class MemoDto : BaseDto<int>
    {
        private string title;
        private string content; 

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
    }
}
