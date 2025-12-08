using System;
using System.Collections.Generic;

namespace ProjectHub.Api.Dtos
{
    public class ProjectDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<ProjectTaskDto> Tasks { get; set; } = new();
    }
}
