using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Domain.Entities;

namespace TaskManagementAPI.Application.Interfaces.Repositories
{
    public interface ITaskRepository : IGenericRepository<TaskItem>
    {
        Task<TaskItem?> GetByIdWithDetailsAsync(int id);
        Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedAsync(
            int projectId, string? status, int page, int pageSize);
    }
}
