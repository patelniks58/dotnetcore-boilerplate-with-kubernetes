using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coop.Sample.Core.Models
{
    public partial class Owner
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(101)]
        public string FullName { get; set; }


        [InverseProperty("Owner")]
        public virtual ICollection<Car> Cars { get; set; } = new HashSet<Car>();
    }
}
