using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("DomainAspnetPersonMap")]
    public partial class DomainAspnetPersonMap
    {
        public long Id { get; set; }

        public long PersonId { get; set; }

        [Required]
        [StringLength(450)]
        public string AspnetId { get; set; }

        public virtual Person Person { get; set; }
    }
}
