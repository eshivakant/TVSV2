using System;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("AddressOwnership")]
    public partial class AddressOwnership
    {
        public long Id { get; set; }

        public long AddressId { get; set; }

        public long PersonId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OwnedFrom { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OwnedTo { get; set; }

        public virtual Address Address { get; set; }

        public virtual Person Person { get; set; }
    }
}
