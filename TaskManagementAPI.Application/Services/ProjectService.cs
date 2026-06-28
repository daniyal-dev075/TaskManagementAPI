using System;
using System.Collections.Generic;
using System.Text;

using TaskManagementAPI.Application.DTOs.Projects;
using TaskManagementAPI.Application.Interfaces.Repositories;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Application.Interfaces;


namespace TaskManagementAPI.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IGenericRepository<Project> _projectRepository;

        public ProjectService(IGenericRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            var projects = await _projectRepository.GetAllAsync();
            return projects.Select(MapToDto);
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            return project == null ? null : MapToDto(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, int ownerId)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                OwnerId = ownerId
            };

            await _projectRepository.AddAsync(project);
            await _projectRepository.SaveChangesAsync();

            return MapToDto(project);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project == null) return false;

            project.IsDeleted = true;
            _projectRepository.Update(project);
            await _projectRepository.SaveChangesAsync();
            return true;
        }

        private static ProjectDto MapToDto(Project project) => new()
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            OwnerId = project.OwnerId,
            OwnerName = project.Owner?.FullName ?? string.Empty,
            CreatedAt = project.CreatedAt
        };
    }
}
