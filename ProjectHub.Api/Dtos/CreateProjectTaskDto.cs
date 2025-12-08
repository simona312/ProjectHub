using System.ComponentModel.DataAnnotations;
using ProjectHub.Domin.Enums;
using TaskStatus = ProjectHub.Domin.Enums.TaskStatus;


namespace ProjectHub.Api.Dtos

{
    public class CreateProjectTaskDto
    {
        [Required]
        [MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Required]
        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public DateTime? DueDaate { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }

  

}
