using DefinitelyTypedNet;
using TVS.API.Entities;

namespace TVS.API.Models
{
    [TypeScript]
    public class VerificationRequestDto
    {
        public Person Person { get; set; }
        public VerificationRequest VerificationRequest { get; set; }
    }
}