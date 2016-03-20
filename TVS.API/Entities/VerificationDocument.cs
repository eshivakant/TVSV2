using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("VerificationDocument")]
    public class VerificationDocument
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}