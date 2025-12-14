using Microsoft.EntityFrameworkCore;
using System.Linq;
using ProjectHub.Application.Services;
using ProjectHub.Domin.Entites;
using ProjectHub.Infrastructure.Data;

namespace ProjectHub.Infrastructure.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly AppDbContext _context;

        public ProjectTaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProjectTask>> GetAllAsync()
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .ToListAsync();
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

        public async Task<ProjectTask> CreateAsync(ProjectTask task)
        {
            _context.ProjectTasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> UpdateAsync(ProjectTask task)
        {
            _context.ProjectTasks.Update(task);
            var result = await _context.SaveChangesAsync();
            return result > 0;
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

