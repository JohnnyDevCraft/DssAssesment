using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList
{
    public interface ICategory
    {
        int Id { get; }
        string Name { get; }
        string Description { get; }
    }
}
