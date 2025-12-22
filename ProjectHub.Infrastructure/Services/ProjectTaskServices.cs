using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProjectHub.Application.Services;
using ProjectHub.Domin.Entites;
using ProjectHub.Infrastructure.Data;
using ProjectHub.Application.Dtos;
using Microsoft.EntityFrameworkCore.Query;


namespace ProjectHub.Infrastructure.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly AppDbContext _context;

        public ProjectTaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<ProjectTaskDto>> GetAllAsync(
            int page, int pageSize,
            string? search, int? projectId,
            string? sortBy, string? sortDir)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var query = _context.ProjectTasks.AsQueryable();
             //filter:projectId
            if (projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId.Value);
            //filter:search(Title/Description)
            if(!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(s) ||
                (t.Description != null && t.Description.ToLower().Contains(s)));
            }

            //sortiranje
            query = query.OrderByDescending(t => t.Id);
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new ProjectTaskDto
                {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                Priority = t.Priority,
                DueDate = t.DueDate,
                ProjectId = t.ProjectId,
                CreatedAt = t.CreatedAt
            })
            .ToListAsync();

            return new PagedResult<ProjectTaskDto>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
        
           
        public async Task<List<ProjectTask>> GetByProjectIdAsync(int projectid)
        {
            return await _context.ProjectTasks
                .Where(testc => testc.ProjectId == projectid)
                .ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(int id)
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<int> CreateAsync(ProjectTaskCreateDto dto)
        {
            var task = new ProjectTask
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                ProjectId = dto.ProjectId
            };
            _context.ProjectTasks.Add(task);
            await _context.SaveChangesAsync();

            return task.Id;
            
           
        }

        public async Task<bool> UpdateAsync(ProjectTask task)
        {
            var existing = await _context.Projects.FindAsync(task.Id);
            if (existing == null)
                return false;
            existing.Name = existing.Name;
            existing.Description = existing.Description;
            existing.StartDate = existing.StartDate;
            existing.EndDate = existing.EndDate;

            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task == null) return false;

            _context.ProjectTasks.Remove(task);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}

