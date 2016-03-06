using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVS.API.Entities
{
    [Table("Address")]
    public partial class Address
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Address()
        {
            AddressOccupations = new HashSet<AddressOccupation>();
            AddressOwnerships = new HashSet<AddressOwnership>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(250)]
        public string AddressLine1 { get; set; }

        [StringLength(250)]
        public string AddressLine2 { get; set; }

        [StringLength(250)]
        public string AddressLine3 { get; set; }

        [Required]
        [StringLength(250)]
        public string City { get; set; }

        [StringLength(250)]
        public string State { get; set; }

        [Required]
        [StringLength(50)]
        public string PostCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddressOccupation> AddressOccupations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddressOwnership> AddressOwnerships { get; set; }

        public string FullAddress => ($"{AddressLine1}, {AddressLine2}, {AddressLine3}, {City}, {State}. Pin:{PostCode}")
            .Replace("  ", "").Replace(" , ", ", ").Replace(",,", ",");
    }
}
