using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Application.DTOs.Tasks;
using TaskManagementAPI.Application.Interfaces.Repositories;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Domain.Enums;
using TaskManagementAPI.Application.Interfaces;
namespace TaskManagementAPI.Application.Services

{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskItemDto?> GetByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdWithDetailsAsync(id);
            return task == null ? null : MapToDto(task);
        }

        public async Task<PagedResultDto<TaskItemDto>> GetPagedAsync(int projectId, string? status, int page, int pageSize)
        {
            var (items, totalCount) = await _taskRepository.GetPagedAsync(projectId, status, page, pageSize);

            return new PagedResultDto<TaskItemDto>
            {
                Items = items.Select(MapToDto).ToList(),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<TaskItemDto> CreateAsync(CreateTaskDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                ProjectId = dto.ProjectId,
                AssignedToId = dto.AssignedToId,
                Status = TaskState.Todo
            };

            await _taskRepository.AddAsync(task);
            await _taskRepository.SaveChangesAsync();

            return MapToDto(task);
        }

        public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return false;

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;
            task.AssignedToId = dto.AssignedToId;

            if (Enum.TryParse<TaskState>(dto.Status, true, out var parsedStatus))
                task.Status = parsedStatus;

            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return false;

            if (!Enum.TryParse<TaskState>(status, true, out var parsedStatus))
                return false;

            task.Status = parsedStatus;
            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) return false;

            task.IsDeleted = true; // soft delete
            _taskRepository.Update(task);
            await _taskRepository.SaveChangesAsync();
            return true;
        }

        private static TaskItemDto MapToDto(TaskItem task) => new()
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            DueDate = task.DueDate,
            ProjectId = task.ProjectId,
            AssignedToId = task.AssignedToId,
            AssignedToName = task.AssignedTo?.FullName
        };
    }
}
