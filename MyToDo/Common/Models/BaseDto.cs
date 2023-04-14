using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class BaseDto<Tkey> : BindableBase  
    {
		private Tkey id;
		private DateTime createDate;
		private DateTime updateTime; 

		public Tkey Id
		{
			get { return id; }
			set { id = value; }
		}

		public DateTime CreateDate
		{
			get { return createDate; }
			set { createDate = value; }
		}

		public DateTime UpdateTime
		{
			get { return updateTime; }
			set { updateTime = value; }
		}   
	}
}
