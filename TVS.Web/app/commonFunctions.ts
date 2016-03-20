

class CommonFunctions {
    public fullAddress(address: TVS.API.Entities.Address): string {
        var add = address.addressLine1 + ', ' + address.addressLine2 + ', ' + address.addressLine3 + ', ' + address.city + ', ' + address.state;
        if (address.postCode != undefined && address.postCode != null && address.postCode !== '') add = add + ', ' + address.postCode;
        add = add.replace("null", "").replace("  ", " ").replace(",,", ",").replace(", ,", ",");
        return add;
    }
}