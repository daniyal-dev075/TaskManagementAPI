using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Application.DTOs.Tasks;

namespace TaskManagementAPI.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskItemDto?> GetByIdAsync(int id);
        Task<PagedResultDto<TaskItemDto>> GetPagedAsync(int projectId, string? status, int page, int pageSize);
        Task<TaskItemDto> CreateAsync(CreateTaskDto dto);
        Task<bool> UpdateAsync(int id, UpdateTaskDto dto);
        Task<bool> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
    }
}