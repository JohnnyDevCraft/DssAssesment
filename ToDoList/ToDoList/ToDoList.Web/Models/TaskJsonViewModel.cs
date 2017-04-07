using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ToDoList.Web.Models
{
    public class TaskJsonViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        [Required]
        public int Priority { get; set; }
        
        public string DueDate { get; set; }
        
        public bool Completed { get; set; }
        
        public string CompletedAt { get; set; }

        [Required]
        public string CategoryId { get; set; }

        public string Category { get; set; }
    }
}