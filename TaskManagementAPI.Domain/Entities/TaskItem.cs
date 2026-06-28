using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Domain.Enums;

namespace TaskManagementAPI.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskState Status { get; set; } = TaskState.Todo;
        public DateTime? DueDate { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;

        public int? AssignedToId { get; set; }
        public User? AssignedTo { get; set; }
    }
}
