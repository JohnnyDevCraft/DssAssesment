using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoList.Web.Models
{
    public class JsonNoticeViewModel<T>
    {
        public string Result { get; set; }
        public T Value { get; set; }
        public IList<string> ValidationResults { get; set; }

    }
}