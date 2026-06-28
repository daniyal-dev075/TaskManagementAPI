using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.Application.DTOs.Projects;
using TaskManagementAPI.Application.Interfaces;

namespace TaskManagementAPI.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET /api/projects
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ownerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var roleClaim = User.FindFirstValue(ClaimTypes.Role);

            if (ownerIdClaim == null) return Unauthorized();
            int ownerId = int.Parse(ownerIdClaim);

            var projects = await _projectService.GetAllAsync();
            
            if (roleClaim == "Admin")
            {
                return Ok(projects);
            }

            var myProjects = projects.Where(p => p.OwnerId == ownerId).ToList();
            return Ok(myProjects);
        }

        // GET /api/projects/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        // POST /api/projects
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
        {
            var ownerIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (ownerIdClaim == null) return Unauthorized();

            int ownerId = int.Parse(ownerIdClaim);
            var project = await _projectService.CreateAsync(dto, ownerId);
            return CreatedAtAction(nameof(GetById), new { id = project.Id }, project);
        }

        // DELETE /api/projects/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _projectService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}