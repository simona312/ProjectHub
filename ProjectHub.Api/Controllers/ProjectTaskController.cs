using Microsoft.AspNetCore.Mvc;
using ProjectHub.Application.Services;
using ProjectHub.Api.Dtos;
using ProjectHub.Domin.Entites;


namespace ProjectHub.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTaskController : ControllerBase
    {
        private readonly IProjectTaskService _taskService ;
        public ProjectTaskController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _taskService.GetAllAsync();
                //.Include(t => t.Project)
                //.ToListAsync();
            return Ok(tasks);
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
        public async Task<IActionResult> CreateTask([FromBody] CreateProjectTaskDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var projectExists = await _context.Projects.AnyAsync(p => p.id == dto.ProjectId);
            //if (!projectExists)
            //    return BadRequest("Invalid ProjectId.");

            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDaate,
                ProjectId = dto.ProjectId
            };

            task = await _taskService.CreateAsync(task);
            //await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllTasks), new { id = task.id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateTask(int id, [FromBody] ProjectTask updatedTask)
        {
            if (id != updatedTask.id)
                return BadRequest("Task ID mismatch.");

            var success = await _taskService.UpdateAsync(updatedTask);
            if (!success) return NotFound();

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
