var app = angular.module('app', [
    'ngAnimate',
    'ui.bootstrap',
    'color.picker',
    'ngRoute'
    
])/*.config(function($routeProvider, $locationProvider) {
  
    $routeProvider
    .when('/Evento/Detalhe/:id', {
        templateUrl: '/Evento/Detalhe',
        controller: 'EventoControl',
        resolve: {
            // I will cause a 1 second delay
            delay: function ($q, $timeout) {
                var delay = $q.defer();
                $timeout(delay.resolve, 1000);
                return delay.promise;
            }
        }
    }).when('/novarota', {
        templateUrl: '/Evento/Calendario',
        controller: 'EventoControl'
        
    });

    
    //$locationProvider.html5Mode(true);
})*/;

  

