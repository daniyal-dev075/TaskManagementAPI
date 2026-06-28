using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagementAPI.Application.DTOs.Projects
{
    public class CreateProjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
