
module Helpers {
    export  class CommonFunctions {
        public fullAddress(address: TVS.API.Entities.Address): string {
            var add = address.addressLine1 + ', ' + address.addressLine2 + ', ' + address.addressLine3 + ', ' + address.city + ', ' + address.state;
            if (address.postCode != undefined && address.postCode != null && address.postCode !== '') add = add + ', ' + address.postCode;
            add = add.replace("null", "").replace("  ", " ").replace(",,", ",").replace(", ,", ",");
            return add;
        }

        public fullName(person) {
            if (person.middleName == undefined) person.middleName = "";
            var name = person.initial + " " + person.firstName + " " + person.middleName + " " + person.lastName;
            return name.replace("  ", " ").replace("null", "").replace("  ", " ").replace(",,", ",").replace(", ,", ",");
        }

        public getScoreText(score: number) {

            if (score <= 1)
                return "Horrible";
            else if (score <= 2)
                return "Very Bad";
            else if (score <= 3)
                return "Bad";
            else if (score <= 4)
                return "Satisfactory";
            else if (score <= 5)
                return "Adequate";
            else if (score <= 6)
                return "Good";
            else if (score <= 7)
                return "Very Good";
            else if (score <= 8)
                return "Excellent";
            else if (score <= 9)
                return "Fanastic";
            else
                return "Exceptional!";

        }

    }
}