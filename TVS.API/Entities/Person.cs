using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVS.API.Entities
{
    [Table("Person")]
    public partial class Person
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Person()
        {
            // ReSharper disable VirtualMemberCallInContructor
            AddressOccupations = new List<AddressOccupation>();
            AddressOwnerships = new List<AddressOwnership>();
            DomainAspnetPersonMaps = new List<DomainAspnetPersonMap>();
            PersonAttributes = new List<PersonAttribute>();
            PersonRatings = new List<PersonRating>();
            // ReSharper restore VirtualMemberCallInContructor

        }

        public long Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Initial { get; set; }

        [Required]
        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        [StringLength(250)]
        public string PlaceOfBirth { get; set; }

        [StringLength(50)]
        public string AdhaarCard { get; set; }

        [StringLength(50)]
        public string PAN { get; set; }

        [StringLength(250)]
        public string IdentificationMark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual IList<AddressOccupation> AddressOccupations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual IList<AddressOwnership> AddressOwnerships { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual IList<DomainAspnetPersonMap> DomainAspnetPersonMaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual IList<PersonAttribute> PersonAttributes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual IList<PersonRating> PersonRatings { get; set; }
    }
}
