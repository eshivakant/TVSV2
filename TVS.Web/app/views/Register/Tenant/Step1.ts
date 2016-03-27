class signUpTenantStep1Ctrl {


    public error: string;
    public info: string;
    public serviceBase: string;
    private searchResult: Array<TVS.API.Entities.Person>;
    private proceedToSave: boolean;
    public newPerson: TVS.API.Entities.Person;

    static $inject = ['$scope', '$http', '$location', '$q', 'ngAuthSettings', 'personData', '$timeout', 'Notification', 'ModalService'];

    constructor(
        private scope: ng.IScope,
        private http: ng.IHttpService,
        private location,
        private q,
        private ngAuthSettings,
        private personData,
        private timeout,
        private Notification,
        private ModalService
    ) {

        this.error = "";
        this.info = "";
        this.newPerson = new TVS.API.Entities.Person();
        this.serviceBase = ngAuthSettings.apiServiceBaseUri;
        this.proceedToSave = false;
        //this.init();
    }

   


    public showSearchPanel() {
        this.ModalService.showModal({
            templateUrl: "app/views/ModalViews/personSrchRsltModal.html",
            controller: 'personSrchRsltModalCtrl',
            controllerAs: 'vm',
            inputs: {
                title: "Matching people found...",
                people: this.searchResult
            }
        }).then((modal)=> {
            modal.element.modal();
            modal.close.then((result)=> {
                if (result != null) {
                    var commonFunc = new Helpers.CommonFunctions();
                    commonFunc.copyNonEmptyPersonAttributes(result, this.newPerson);
                }

                this.proceedToSave = true;
            });
        });
    }


    public searchAndSave() {

            this.http.post(serviceBase + '/api/Search/Landlord/Search', this.newPerson)
                .then((response)=> {
                    this.searchResult = <Array<TVS.API.Entities.Person>>response.data;
                    if (this.proceedToSave === false && this.searchResult != undefined && this.searchResult != null && this.searchResult.length > 0) {
                        this.showSearchPanel();
                        return;
                    } else {
                        this.savePerson();
                    }
                },
                ()=> {
                    this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 });
                });
     
    }
    

    public savePerson () {
            this.http.post(serviceBase + '/Api/Tenant/Save', this.newPerson)
                .success(response=> {
                    this.newPerson = <TVS.API.Entities.Person>response;
                    this.location.path('/tsignup2');
                })
                .error(() => { this.Notification.error({ message: 'Server Error Ocurred!', delay: 1000 }) });
        
    }


}
app.controller('signUpTenantStep1Ctrl', signUpTenantStep1Ctrl);

