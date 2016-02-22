using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TVS.API.Entities
{
    [Table("PersonAttribute")]
    public partial class PersonAttribute
    {
        public long Id { get; set; }

        public long RoleAttributeId { get; set; }

        public long PersonId { get; set; }

        [StringLength(250)]
        public string StringValue { get; set; }

        public long? IntValue { get; set; }

        public decimal? FloatValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateValue { get; set; }

        public virtual Person Person { get; set; }

        public virtual RoleAttribute RoleAttribute { get; set; }
    }
}
