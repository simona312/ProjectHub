using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using ProjectHub.Api.Dtos;
using ProjectHub.Application.Dtos;
using ProjectHub.Application.Services;
using ProjectHub.Domin.Entites;
using System.Reflection;


namespace ProjectHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IValidator<CreateProjectTaskDto> _validator;
        private readonly IProjectTaskService _taskService;
        public ProjectTaskController(
            IValidator<CreateProjectTaskDto> validator,
            IProjectTaskService taskService)
        {
            _validator = validator;
            _taskService = taskService;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetAllTasks(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] int? projectId = null,
            [FromQuery] string? sortBy = "id",
            [FromQuery] string? sortDir = "asc")
        {
            var result = await _taskService.GetAllAsync(page, pageSize, search, projectId, sortBy, sortDir);
            //.Include(t => t.Project)
            //.ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpGet("by-project/{projectId:int}")]
        public async Task<IActionResult> GetTasksForProject(int projectId)
        {
            var tasks = await _taskService.GetByProjectIdAsync(projectId);
            return Ok(tasks);
        }


        //[HttpPost]
        //public async Task<IActionResult> CreateTask([FromBody] ProjectTask task)
        //{
        //    if (task == null)
        //        return BadRequest("Task data is missing.");

        //    var projectExists = await _context.Projects.AnyAsync(p => p.id == task.ProjectId);
        //    if (!projectExists)
        //        return BadRequest("Invalid ProjectId,");

        //    _context.ProjectTasks.Add(task);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetAllTasks), new { id = task.id }, task);



        //}

        [HttpPost]
        public async Task<IActionResult> Create(
    [FromBody] CreateProjectTaskDto dto)
        {
           
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }

            
            var appDto = new ProjectTaskCreateDto
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                ProjectId = dto.ProjectId
            };

            
            var id = await _taskService.CreateAsync(appDto);

         
            return Ok(new { id });
        }








        //var projectExists = await _context.Projects.AnyAsync(p => p.id == dto.ProjectId);
        //if (!projectExists)
        //    return BadRequest("Invalid ProjectId.");




        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] ProjectTaskUpdateDto dto,
            [FromServices] IValidator<ProjectTaskUpdateDto> validator)
        {
          
            var validation = await validator.ValidateAsync(dto);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(e => new
                {
                    field = e.PropertyName,
                    error = e.ErrorMessage
                }));
            }
            var task = await _taskService.GetByIdAsync(id);
            if (task is null)
                return NotFound();



            task.Title = dto.Title;
            task.Description = dto.Description;
            task.Status = dto.Status;
            task.Priority = dto.Priority;
            task.DueDate = dto.DueDate;

            var ok = await _taskService.UpdateAsync(task);
            if (!ok)
                return BadRequest("Update failed");

            //_context.Entry(updatedTask).State = EntityState.Modified;
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteTask(int id)
        {
            //var task = await _context.ProjectTasks.FindAsync(id);
            //if (task == null)
            var success = await _taskService.DeleteAsync(id);

               if(!success) return NotFound();

            //_context.ProjectTasks.Remove(task);
            //await _context.SaveChangesAsync();

            return NoContent();
        }
       
    }
}
