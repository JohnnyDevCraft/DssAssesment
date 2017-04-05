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

        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DueDate { get; set; }

        [Required]
        public bool Completed { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:G}")]
        public DateTime? CompletedAt { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}