using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHub.Domin.Enums;

namespace ProjectHub.Application.Dtos
{
    public class ProjectTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectHub.Domin.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
