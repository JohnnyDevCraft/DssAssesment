using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ToDoList.Web.Models
{
    public sealed class TaskEditViewModel
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

        public TaskEditViewModel()
        { }

        public TaskEditViewModel(ITask task)
        {
            Id = task.Id;
            Name = task.Name;
            Description = task.Description;
            Priority = task.Priority;
            DueDate = task.DueDate;
            Completed = task.IsCompleted;
            CompletedAt = task.CompletedAt;
        }
    }
}