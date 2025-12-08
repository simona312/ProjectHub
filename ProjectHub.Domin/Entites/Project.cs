using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectHub.Domin.Enums;
using System.Text.Json.Serialization;

namespace ProjectHub.Domin.Entites
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ICollection<ProjectTask> Tasks { get; set; } = new List<ProjectTask>();

    }
}
