
(function () {

    angular.module('html-parser-app', ['ui.router', 'ngSanitize', 'ngAnimate', 'angularSpinner', 'chart.js', 'angular-carousel'])
        .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

            $urlRouterProvider.when('', '/home');
            $urlRouterProvider.otherwise("/home");
            $stateProvider
                .state('root', {
                    url: "",
                    templateUrl: 'www/views/root.html'

                })
                .state('root.home', {
                    url: "/home",
                    controller: 'HomeController as ctrl',
                    templateUrl: 'www/views/home.html'
                })
        }])
        .directive('usSpinner', ['$http', '$rootScope', function ($http, $rootScope) {
            return {
                link: function (scope, elm, attrs)
                {
                    $rootScope.spinnerActive = false;
                    scope.isLoading = function () {
                        return $http.pendingRequests.length > 0;
                    };

                    scope.$watch(scope.isLoading, function (loading)
                    {
                        $rootScope.spinnerActive = loading;
                        if(loading){
                            elm.removeClass('ng-hide');
                        }else{
                            elm.addClass('ng-hide');
                        }
                    });
                }
            }
        }])
        .run(function ($rootScope, $location) {

        });
}());


(function () {
    angular.module('html-parser-app').controller('HomeController', HomeController);

    HomeController.$inject = ['$http', 'usSpinnerService'];

    function HomeController($http, spinner) {

        var vm = this;

        vm.url = null;

        vm.show = false;

        vm.alert = null;

        vm.words = {
            labels: [],
            data: []
        };

        vm.carouselIndex = 0;

        vm.images = [];

        vm.isAlert = function () {
            return (vm.alert != null && vm.alert.length > 0);
        }

        vm.isWordsFound = function () {
            return (vm.words.data.length > 0);
        }

        vm.isImagesFound = function () {
            return (vm.images.length > 0);
        }

        vm.onEnter = function (keyEvent) {
            if (keyEvent.which === 13) {
                vm.parse();
            }
        };

        vm.parse = function () {
            if (vm.url) {
                //create post request
                var req = {
                    method: 'POST',
                    url: '/api/parser',
                    headers: {
                        'Content-Type': "text/json"
                    },
                    data: { url: vm.url }
                }
                vm.show = false;
                vm.alert = null;

                $http(req).then(function (response) {
                    vm.show = true;
                    //use lodash to transform data
                    vm.words.labels = _.pluck(response.data.words, 'word');
                    vm.words.data = [_.pluck(response.data.words, 'count')];

                    //reset images carousel to first slide 
                    vm.carouselIndex = 0;

                    //transform and reset images array
                    vm.images = _.map(response.data.images, function (v, k) {
                        return {
                            src: v.url,
                            text: v.text
                        };
                    });
                }, function (error) {
                    vm.show = false;
                    //display error message
                    vm.alert = error.data.Message;
                    console.log(error);
                });
            }
        };

        //reset show and alert
        vm.activate = function () {
            vm.show = false;
            vm.alert = null;
        };

        vm.activate();

    }

}());
