using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementAPI.Application.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public int? AssignedToId { get; set; }
    }
}
