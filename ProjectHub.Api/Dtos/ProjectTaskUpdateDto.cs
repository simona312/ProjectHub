using ProjectHub.Domin.Enums;

namespace ProjectHub.Api.Dtos
{
    public class ProjectTaskUpdateDto
    {
     
            public string Title { get; set; } = string.Empty;
            public string? Description { get; set; }
            public ProjectHub.Domin.Enums.TaskStatus Status { get; set; }
            public TaskPriority Priority { get; set; }
            public DateTime? DueDate { get; set; }

        }
    }

