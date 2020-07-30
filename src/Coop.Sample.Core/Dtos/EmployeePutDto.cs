using System.ComponentModel.DataAnnotations;

namespace Coop.Sample.Core.Dtos
{
    public class EmployeePutDto
    {
        [Required]
        public string LastName { get; set; }
    }
}
