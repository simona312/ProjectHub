using ProjectHub.Domin.Entites;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProjectHub.Application.Dtos;


namespace ProjectHub.Application.Services
{
    public interface IProjectService
    {
        Task<PagedResult<ProjectDto>> GetAllAsync(int page, int pageSize, string? search, string? sortBy, string? sortDir);
        Task<ProjectDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(ProjectDto dto);
        Task<bool> UpdateAsync(int id, ProjectDto dto);
        Task<bool> DeleteAsync(int id);



            
    }
}

