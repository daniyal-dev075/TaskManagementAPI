using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementAPI.Application.DTOs.Tasks
{
    public class UpdateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DueDate { get; set; }
        public int? AssignedToId { get; set; }
    }
}
