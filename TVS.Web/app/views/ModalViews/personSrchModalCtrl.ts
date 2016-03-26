'use strict';
app.controller('personSrchModalCtrl', ['$scope', '$element', 'title', 'close', function ($scope, $element, title, close) {
    $scope.name = null;
    $scope.age = null;
    $scope.title = title;
    $scope.person = new TVS.API.Entities.Person();
    //  This close function doesn't need to use jQuery or bootstrap, because
    //  the button has the 'data-dismiss' attribute.
    $scope.search = function () {
        close($scope.person, 500); // close, but give 500ms for bootstrap to animate
    };
    //  This cancel function must use the bootstrap, 'modal' function because
    //  the doesn't have the 'data-dismiss' attribute.
    $scope.cancel = function () {
        //  Manually hide the modal.
        $element.modal('hide');
        //  Now call close, returning control to the caller.
        close($scope.person, 500); // close, but give 500ms for bootstrap to animate
    };
}]);





//app.controller(personSrchModalCtrl);

//class personSrchModalCtrl {
    
//    public person: TVS.API.Entities.Person;
//    public close:Function;
//    static $inject = ['$scope', '$element', 'title', 'close'];
//    constructor(
//        private scope: ng.IScope,
//        private element,
//        private title: string,
//        close
//    ) {
//        this.person = new TVS.API.Entities.Person();
//        this.close = () => { close(this.person, 500); };
//        //this.cancel = () => {
            
//        //};
//    }

//    public cancel() {
//        this.element.modal('hide');
//        this.close(this.person, 500);
//    }
//}
