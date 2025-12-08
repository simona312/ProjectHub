using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ProjectHub.Api.Dtos
{
    public class CreateProjectDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        
    }
}
