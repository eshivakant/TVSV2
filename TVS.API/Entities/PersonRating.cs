using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("PersonRating")]
    public partial class PersonRating
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PersonRating()
        {
            RatingBreakdowns = new HashSet<RatingBreakdown>();
        }

        public long Id { get; set; }

        public long PersonId { get; set; }

        public long AddressId { get; set; }

        public long ProviderId { get; set; }

        public int AverageScore { get; set; }

        public string Comments { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RatingPeriodStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime? RatingPeriodEnd { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreated { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateUpdated { get; set; }

        public virtual Person Person { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatingBreakdown> RatingBreakdowns { get; set; }

        public virtual Address Address { get; set; }

    }
}
