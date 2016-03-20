using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("RoleParameter")]
    public partial class RoleParameter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoleParameter()
        {
            RatingBreakdowns = new HashSet<RatingBreakdown>();
        }

        public long Id { get; set; }

        [Required]
        [StringLength(450)]
        public string RoleId { get; set; }

        [Required]
        [StringLength(250)]
        public string ParameterName { get; set; }

        public string Description { get; set; }
               

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RatingBreakdown> RatingBreakdowns { get; set; }
    }
}
