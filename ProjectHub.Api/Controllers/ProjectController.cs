using Microsoft.AspNetCore.Mvc;
using ProjectHub.Application.Dtos;
using ProjectHub.Application.Services;



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
        public async Task<IActionResult> GetAllAsync(


                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 10,
                [FromQuery] string? search = null,
                [FromQuery] string? sortBy = "id",
                [FromQuery] string? sortDir = "asc")
        {
           
            var result = await _projectService.GetAllAsync(page, pageSize, search, sortBy,sortDir);
            return Ok(result);
        }



        // GET: api/Project/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return NotFound();
            return Ok(project);

        }



        // POST: api/Project
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var id = await _projectService.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }


        // PUT: api/Project/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProject(int id, [FromBody] ProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var success = await _projectService.UpdateAsync(id, dto);
            {
                if (!success)
                    return NotFound();
                return NoContent();
            }
        }


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


