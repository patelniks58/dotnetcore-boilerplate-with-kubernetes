using System;
using System.ComponentModel.DataAnnotations;

namespace Coop.Sample.Core.Dtos
{
    public class EmployeePostDto
    {
        [Required]
        public DateTime? BirthDate { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [RegularExpression("^[MF]$")]
        public string Gender { get; set; }
    }
}
