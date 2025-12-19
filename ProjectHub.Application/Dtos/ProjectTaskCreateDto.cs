using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectHub.Domin.Enums;

namespace ProjectHub.Application.Dtos
{
    public class ProjectTaskCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectHub.Domin.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }
}
