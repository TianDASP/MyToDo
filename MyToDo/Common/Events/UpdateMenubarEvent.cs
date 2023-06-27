using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Events
{
    public class UpdateMenubarModel
    {
        public string Title { get; set; }
        public string NameSpace { get; set; }
    }
    public class UpdateMenubarEvent : PubSubEvent<UpdateMenubarModel>
    {

    }
}
