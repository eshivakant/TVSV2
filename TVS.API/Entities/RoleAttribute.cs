using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("RoleAttribute")]
    public partial class RoleAttribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoleAttribute()
        {
            PersonAttributes = new HashSet<PersonAttribute>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(450)]
        public string RoleId { get; set; }

        [StringLength(100)]
        public string Attribute { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(10)]
        public string ValueType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonAttribute> PersonAttributes { get; set; }
    }
}
