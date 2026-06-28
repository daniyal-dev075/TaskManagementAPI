using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementAPI.Application.DTOs.Tasks
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public int? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }
    }
}
