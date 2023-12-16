using System.ComponentModel.DataAnnotations;

namespace CitiInfo.WebAPI.Models
{
    public class PointofInterestForCreatingDto
    {
        [Required(ErrorMessage = "Name point of interest is requied.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
