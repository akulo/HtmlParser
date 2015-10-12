
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
            Chart.defaults.global.maintainAspectRatio = false;
        });
}());


(function () {
    angular.module('html-parser-app').controller('HomeController', HomeController);

    HomeController.$inject = ['$http', '$scope'];

    function HomeController($http, $scope) {

        var vm = this;

        vm.url = null;

        vm.show = false;

        vm.alert = null;

        vm.words = {
            total: 0,
            topWords: {
                labels: [],
                data: []
            }
        };

        vm.carouselIndex = 0;

        vm.images = [];

        vm.isAlert = function () {
            return (vm.alert != null && vm.alert.length > 0);
        }

        vm.isWordsFound = function () {
            return (vm.words.total > 0);
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
                vm.reset();

                $http(req).then(function (response) {
                    vm.show = true;

                    vm.words.total = response.data.words.total;
                    //use lodash to transform data
                    vm.words.topWords.labels = _.pluck(response.data.words['top-words'], 'word');
                    vm.words.topWords.data = [_.pluck(response.data.words['top-words'], 'count')];

                    //transform and reset images array
                    vm.images = _.map(response.data.images, function (v, k) {
                        return {
                            src: v.url,
                            text: v.text
                        };
                    });
                }, function (error) {
                    vm.show = false;
                    if (error.data.Message) {
                        //display error message
                        vm.alert = error.data.Message;
                    } else {
                        vm.alert = error.data;
                    }
                    
                    console.log(error);
                });
            }
        };

        //reset show and alert
        vm.activate = function () {
            vm.reset();
        };

        vm.reset = function () {
            _.each(vm.charts, function (chart) {
                if (chart)
                    chart.destroy();
            });
            vm.charts = [];

            vm.show = false;
            vm.alert = null;
            vm.words = {
                total: 0,
                topWords: {
                    labels: [],
                    data: []
                }
            };

            vm.images = [];

            vm.carouselIndex = 0;
        };

        vm.activate();

        vm.charts = [];

        $scope.$on('create', function (event, chart) {
            vm.charts.push(chart);
        });

    }

}());
