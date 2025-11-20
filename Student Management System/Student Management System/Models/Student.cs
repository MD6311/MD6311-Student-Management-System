using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    public class Student
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Enrollment No")]
        public string EnrollmentNo { get; set; } = string.Empty;

        [Required]
        public string Semester { get; set; } = string.Empty;

        [Required]
        public string Field { get; set; } = string.Empty;

        [Required]
        [Display(Name = "GR Number")]
        public string GRNumber { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile must be 10 digits")]
        public string Mobile { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@marwadiuniversity\.ac\.in$", ErrorMessage = "Institute email must be @marwadiuniversity.ac.in")]
        [Display(Name = "Institute Email")]
        public string InstituteEmail { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Personal Email")]
        public string PersonalEmail { get; set; } = string.Empty;

        [Required]
        [MinLength(5)]
        public string Address { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; } = DateTime.Now;

        public string Notes { get; set; } = string.Empty;
    }
}