using ProjectHub.Domin.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHub.Application.Services
{
    public interface IProjectService
    {
        Task<List<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task<Project> CreateAsync(Project project);
        Task<bool> UpdateAsync(Project project);
        Task<bool> DeleteAsync(int id);
    }
}

