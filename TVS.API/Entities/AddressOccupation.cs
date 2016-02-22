using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVS.API.Entities
{
    [Table("AddressOccupation")]
    public partial class AddressOccupation
    {
        public long Id { get; set; }

        public long AddressId { get; set; }

        public long PersonId { get; set; }

        public decimal? Rent { get; set; }

        [Column(TypeName = "date")]
        public DateTime OccupiedFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OccupiedTo { get; set; }

        public virtual Address Address { get; set; }

        public virtual Person Person { get; set; }
    }
}
