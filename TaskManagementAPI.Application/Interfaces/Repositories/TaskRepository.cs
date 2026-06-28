using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Domain.Enums;
using TaskManagementAPI.Infrastructure.Persistence;

namespace TaskManagementAPI.Application.Interfaces.Repositories
{
    public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext context) : base(context) { }

        public async Task<TaskItem?> GetByIdWithDetailsAsync(int id) =>
            await _dbSet
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<(IEnumerable<TaskItem> Items, int TotalCount)> GetPagedAsync(
            int projectId, string? status, int page, int pageSize)
        {
            var query = _dbSet.Where(t => t.ProjectId == projectId);

            if (!string.IsNullOrEmpty(status) && Enum.TryParse<TaskState>(status, true, out var parsedStatus))
            {
                query = query.Where(t => t.Status == parsedStatus);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .Include(t => t.AssignedTo)
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
