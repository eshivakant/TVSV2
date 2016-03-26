class personSrchRsltModalCtrl {
    public title: string;
    public searchResult: any;
    public person :TVS.API.Entities.Person;
    static $inject = ['$scope', '$element', 'title', 'close', 'people'];
    constructor(scope, private  $element, title, private close, people) {
        this.searchResult = people;
        this.person = new TVS.API.Entities.Person();
        this.title = title;
    }

    public search() {
        this.close(this.person, 500);
    }

    public cancel() {
        this.close(null, 500);
    }


    public fullAddress(address: TVS.API.Entities.Address): string {
        var commonFunc = new Helpers.CommonFunctions();
        return commonFunc.fullAddress(address);
    }

    public fullName(person) {
        var commonFunc = new Helpers.CommonFunctions();
        return commonFunc.fullName(person);
    }

    public select(person) {
        this.person = person;
        this.close(this.person, 500);
  

    }


}


app.controller('personSrchRsltModalCtrl', personSrchRsltModalCtrl);

