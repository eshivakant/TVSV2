using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using DefinitelyTypedNet;

namespace TVS.API.Entities
{
    [TypeScript]
    [Table("VerificationRequest")]
    public class VerificationRequest
    {
        public long Id { get; set; }

        public string WhoIsRequesting { get; set; }

        public long PersonId { get; set; }

        public long RequestorId { get; set; }

        public bool CrimeCheck { get; set; }

        public bool CivilCheck { get; set; }

        public bool CreditCheck { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VerificationDocument> Documents { get; set; }


    }
}