using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DefinitelyTypedNet;

namespace TVS.API.Models
{
    [TypeScript]
    public class AddressSearchModel
    {
        public string State { get; set; }
        public string City { get; set; }
        public string Locality { get; set; }
        public string HomeAddress { get; set; }
        public string OwnersLastName { get; set; }
        public string OwnersFirstName { get; set; }
    }
}
