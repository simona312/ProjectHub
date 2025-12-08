using Microsoft.AspNetCore.Mvc;
using ProjectHub.Api.Dtos;
using ProjectHub.Application.Services;

using ProjectHub.Domin.Entites;

namespace ProjectHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET: api/Project
        [HttpGet]
        public async Task<IActionResult> GetProjects()
        {
            var projects = await _projectService.GetAllAsync();

            var result = projects.Select(p => new ProjectDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Tasks = p.Tasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = (int)t.Status,
                    Priority = (int)t.Priority,
                    DueDate = t.DueDate
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        // GET: api/Project/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return NotFound();

            var result = new ProjectDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Tasks = project.Tasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = (int)t.Status,
                    Priority = (int)t.Priority,
                    DueDate = t.DueDate
                }).ToList()
            };

            return Ok(result);
        }

        // POST: api/Project
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            project = await _projectService.CreateAsync(project);

            var result = new ProjectDetailsDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Tasks = project.Tasks.Select(t => new ProjectTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = (int)t.Status,
                    Priority = (int)t.Priority,
                    DueDate = t.DueDate
                }).ToList()
            };

            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, result);
        }

        // PUT: api/Project/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] CreateProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = new Project
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            var success = await _projectService.UpdateAsync(project);
            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Project/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var success = await _projectService.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
