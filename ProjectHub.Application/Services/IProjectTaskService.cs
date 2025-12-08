using ProjectHub.Domin.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHub.Application.Services
{
    public interface IProjectTaskService
    {
        Task<List<ProjectTask>> GetAllAsync();
        Task<ProjectTask?> GetByIdAsync(int id);
        Task<List<ProjectTask>> GetByProjectIdAsync(int projectId);
        Task<ProjectTask> CreateAsync(ProjectTask task);
        Task<bool> UpdateAsync(ProjectTask task);
        Task<bool> DeleteAsync(int id);
    }
}

