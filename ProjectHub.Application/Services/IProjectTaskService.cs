using ProjectHub.Application.Dtos;
using ProjectHub.Domin.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ProjectHub.Application.Services
{
    public interface IProjectTaskService
    {
        Task<PagedResult<ProjectTaskDto>> GetAllAsync(
            int page, int pageSize,
            string? search, int? projectId,
            string? sortBy, string? sortDir);
        Task<ProjectTask?> GetByIdAsync(int id);
        Task<List<ProjectTask>> GetByProjectIdAsync(int projectId);
        Task<int> CreateAsync(ProjectTaskCreateDto dto);
        Task<bool> UpdateAsync(ProjectTask task);
        Task<bool> DeleteAsync(int id);
    }
}

