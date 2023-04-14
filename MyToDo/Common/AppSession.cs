using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common
{
    public static class AppSession
    {
        public static string UserName { get; set; }
        public static string Account { get; set; }
        public static int Id { get; set; }
        public static string Token { get; set; }
    }
}
