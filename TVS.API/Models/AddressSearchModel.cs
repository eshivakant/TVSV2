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




    [TypeScript]
    public class AddressRatingViewModel
    {
        public long OwnerId { get; set; }
        public string OwnersName { get; set; }
        public long ProviderId { get; set; }
        public string ProviderName { get; set; }
        public long AddressId { get; set; }
        public string AddressString { get; set; }
        public double AggregateScore { get; set; }
        public string Comments { get; set; }
        public List<ScoreViewModel> ScoreViewModels { get; set; }

    }

    [TypeScript]
    public class ScoreViewModel
    {
        public string Parameter { get; set; }
        public string ParameterDesc { get; set; }
        public int Score { get; set; }
    }
}
