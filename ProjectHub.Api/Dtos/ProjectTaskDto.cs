using ProjectHub.Domin.Enums;
using System.Diagnostics.Contracts;

namespace ProjectHub.Api.Dtos
{
    public class ProjectTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
