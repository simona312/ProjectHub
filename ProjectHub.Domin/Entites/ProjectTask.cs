using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHub.Domin.Enums;
using System.Text.Json.Serialization;

namespace ProjectHub.Domin.Entites
{
    public  class ProjectTask : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public ProjectHub.Domin.Enums.TaskStatus Status { get; set; } = ProjectHub.Domin.Enums.TaskStatus.Todo;
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }

        [JsonIgnore] 
        public Project? Project { get; set; }
    }
}
