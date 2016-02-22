using System;
using System.Linq;
using TVS.API.Entities;

namespace TVS.API.Controllers
{
	public class PersonDTO
    {
		public int AddressOccupations_Count { get; set; }
		public int AddressOwnerships_Count { get; set; }
		public int DomainAspnetPersonMaps_Count { get; set; }
		public int PersonAttributes_Count { get; set; }
		public int PersonRatings_Count { get; set; }
		public System.Int64 Id { get; set; }
		public System.String Initial { get; set; }
		public System.String FirstName { get; set; }
		public System.String MiddleName { get; set; }
		public System.String LastName { get; set; }
		public System.DateTime DateOfBirth { get; set; }
		public System.String PlaceOfBirth { get; set; }
		public System.String AdhaarCard { get; set; }
		public System.String PAN { get; set; }
		public System.String IdentificationMark { get; set; }

        public static System.Linq.Expressions.Expression<Func<Person, PersonDTO>> SELECT =
            x => new  PersonDTO
            {
                AddressOccupations_Count = x.AddressOccupations.Count(),
                AddressOwnerships_Count = x.AddressOwnerships.Count(),
                DomainAspnetPersonMaps_Count = x.DomainAspnetPersonMaps.Count(),
                PersonAttributes_Count = x.PersonAttributes.Count(),
                PersonRatings_Count = x.PersonRatings.Count(),
                Id = x.Id,
                Initial = x.Initial,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                //DateOfBirth = x.DateOfBirth,
                PlaceOfBirth = x.PlaceOfBirth,
                AdhaarCard = x.AdhaarCard,
                PAN = x.PAN,
                IdentificationMark = x.IdentificationMark,
            };

	}
}