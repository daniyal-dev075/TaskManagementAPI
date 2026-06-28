using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Application.DTOs.Projects;

namespace TaskManagementAPI.Application.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDto?> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(CreateProjectDto dto, int ownerId);
        Task<bool> DeleteAsync(int id);
    }
}
