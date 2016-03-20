using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("PersonDocument")]
    public class PersonDocument
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}