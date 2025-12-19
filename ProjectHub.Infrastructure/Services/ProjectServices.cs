using Microsoft.EntityFrameworkCore;
using ProjectHub.Application.Services;
using ProjectHub.Domin.Entites;
using ProjectHub.Infrastructure.Data;
using ProjectHub.Application.Dtos;
using System.Dynamic;

namespace ProjectHub.Infrastructure.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;

        public ProjectService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PagedResult<ProjectDto>> GetAllAsync(
         int page,
         int pageSize,
         string? search,
         string? sortBy,
          string? sortDir
)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.Projects.AsQueryable();

            // filtering
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(s) ||
                    (p.Description != null && p.Description.ToLower().Contains(s))
                );
            }

            // sorting
            bool desc = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase);

            query = (sortBy?.ToLower()) switch
            {
                "name" => desc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "createdat" => desc ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                "taskcount" => desc ? query.OrderByDescending(p => p.Tasks.Count) : query.OrderBy(p => p.Tasks.Count),
                _ => query.OrderBy(p => p.Id)
            };

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                    TaskCount = p.Tasks.Count
                })
                .ToListAsync();

            return new PagedResult<ProjectDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (project == null)
                return null;

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                TaskCount = project.Tasks.Count
            };
        }

        public async Task<int> CreateAsync(ProjectDto dto)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = dto.CreatedAt,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return project.Id;
        }

        public async Task<bool> UpdateAsync(int id, ProjectDto dto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return false;
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.StartDate = dto.StartDate;
            project.EndDate = dto.EndDate;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
                
            if (project == null)
                return false;
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }





    }
}
