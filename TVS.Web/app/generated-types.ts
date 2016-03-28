
module TypeScriptBuilder {
	export interface IDictionaryString<TValue> {
		[key: string]: TValue;
	}
}

module TVS.API.Models {
	export class AddressSearchModel {
		public state: string;
		public city: string;
		public locality: string;
		public homeAddress: string;
		public ownersLastName: string;
		public ownersFirstName: string;
	}
	export class AddressRatingViewModel {
		public ownerId: number;
		public ownersName: string;
		public providerId: number;
		public providerName: string;
		public addressId: number;
		public addressString: string;
		public aggregateScore: number;
		public comments: string;
		public scoreViewModels: Array<TVS.API.Models.ScoreViewModel>;
	}
	export class ScoreViewModel {
		public parameter: string;
		public parameterDesc: string;
		public score: number;
	}
	export class TenantRegistration {
		public person: TVS.API.Entities.Person;
		public previousLandlords: Array<TVS.API.Entities.Person>;
	}
	export class LandlordRegistration {
		public person: TVS.API.Entities.Person;
		public ownedAddresses: Array<TVS.API.Entities.Address>;
	}
	export class PersonAddressFlatModel {
		public person: TVS.API.Entities.Person;
		public address: TVS.API.Entities.Address;
		public ownedFrom: Date;
		public ownedTo: Date;
		public occupiedFrom: Date;
		public occupiedTo: Date;
		public searchDone: boolean;
	}
	export class PhotoViewModel {
		public name: string;
		public created: Date;
		public modified: Date;
		public size: number;
	}
	export class ReportMoveModel {
		public hasEndDate: boolean;
		public address: TVS.API.Entities.Address;
		public moveInDate: Date;
		public moveOutDate: Date;
		public landlord: TVS.API.Entities.Person;
	}
	export class VerificationRequestDto {
		public person: TVS.API.Entities.Person;
		public verificationRequest: TVS.API.Entities.VerificationRequest;
	}
}

module TVS.API.Entities {
	export class Address {
		public id: number;
		public addressLine1: string;
		public addressLine2: string;
		public addressLine3: string;
		public city: string;
		public state: string;
		public postCode: string;
		public addressOccupations: Array<TVS.API.Entities.AddressOccupation>;
		public addressOwnerships: Array<TVS.API.Entities.AddressOwnership>;
		public fullAddress: string;
	}
	export class AddressOccupation {
		public id: number;
		public addressId: number;
		public personId: number;
		public rent: number;
		public occupiedFrom: Date;
		public occupiedTo: Date;
		public address: TVS.API.Entities.Address;
		public person: TVS.API.Entities.Person;
	}
	export class AddressOwnership {
		public id: number;
		public addressId: number;
		public personId: number;
		public ownedFrom: Date;
		public ownedTo: Date;
		public address: TVS.API.Entities.Address;
		public person: TVS.API.Entities.Person;
	}
	export class PersonDocument {
		public id: number;
		public url: string;
		public description: string;
	}
	export class VerificationDocument {
		public id: number;
		public url: string;
		public description: string;
	}
	export class DomainAspnetPersonMap {
		public id: number;
		public personId: number;
		public aspnetId: string;
		public person: TVS.API.Entities.Person;
	}
	export class Log {
		public id: number;
		public aspnetId: string;
		public lat: number;
		public long: number;
		public ipAddress: string;
		public method: string;
	}
	export class Person {
		public id: number;
		public initial: string;
		public firstName: string;
		public middleName: string;
		public lastName: string;
		public dateOfBirth: Date;
		public placeOfBirth: string;
		public adhaarCard: string;
		public pan: string;
		public identificationMark: string;
		public addressOccupations: Array<TVS.API.Entities.AddressOccupation>;
		public addressOwnerships: Array<TVS.API.Entities.AddressOwnership>;
		public domainAspnetPersonMaps: Array<TVS.API.Entities.DomainAspnetPersonMap>;
		public personAttributes: Array<TVS.API.Entities.PersonAttribute>;
		public personRatings: Array<TVS.API.Entities.PersonRating>;
		public personDocuments: Array<TVS.API.Entities.PersonDocument>;
		public fullName: string;
	}
	export class PersonAttribute {
		public id: number;
		public roleAttributeId: number;
		public personId: number;
		public stringValue: string;
		public intValue: number;
		public floatValue: number;
		public dateValue: Date;
		public person: TVS.API.Entities.Person;
		public roleAttribute: TVS.API.Entities.RoleAttribute;
	}
	export class PersonRating {
		public id: number;
		public personId: number;
		public addressId: number;
		public providerId: number;
		public averageScore: number;
		public comments: string;
		public ratingPeriodStart: Date;
		public ratingPeriodEnd: Date;
		public dateCreated: Date;
		public dateUpdated: Date;
		public person: TVS.API.Entities.Person;
		public ratingBreakdowns: Array<TVS.API.Entities.RatingBreakdown>;
		public address: TVS.API.Entities.Address;
	}
	export class RatingBreakdown {
		public id: number;
		public personRatingId: number;
		public roleParameterId: number;
		public score: number;
		public personRating: TVS.API.Entities.PersonRating;
		public roleParameter: TVS.API.Entities.RoleParameter;
	}
	export class RoleAttribute {
		public id: number;
		public roleId: string;
		public attribute: string;
		public description: string;
		public valueType: string;
		public personAttributes: Array<TVS.API.Entities.PersonAttribute>;
	}
	export class RoleParameter {
		public id: number;
		public roleId: string;
		public parameterName: string;
		public description: string;
		public ratingBreakdowns: Array<TVS.API.Entities.RatingBreakdown>;
	}
	export class VerificationRequest {
		public id: number;
		public whoIsRequesting: string;
		public personId: number;
		public requestorId: number;
		public crimeCheck: boolean;
		public civilCheck: boolean;
		public creditCheck: boolean;
		public documents: Array<TVS.API.Entities.VerificationDocument>;
	}
}
