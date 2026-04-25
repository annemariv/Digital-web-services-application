using System.ComponentModel.DataAnnotations;

namespace AdminsideWebApp.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        //By default status draft
        [Required]
        public string Status { get; set; } = "Draft";
        public string StatusDisplay => (Status ?? "").ToLower() switch
        {
            "draft" => "Mustand",
            "active" => "Aktiivne",
            "done" => "Tehtud",
            "inactive" => "Mitteaktiivne",
            _ => Status ?? ""
        };

        public string? UserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}
