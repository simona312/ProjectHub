using ProjectHub.Domin.Enums;
using TaskStatus = ProjectHub.Domin.Enums.TaskStatus;



namespace ProjectHub.Api.Dtos

{
    public class CreateProjectTaskDto
    {
        public string Title { get; set; } ="";
        public string? Description { get; set; }
        public TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }

  

}
