var app = angular.module('app', [
    'ngAnimate',
    'ui.bootstrap',
    'color.picker',
    'ngRoute',
    'ui.utils.masks'

]).config(function ($routeProvider, $locationProvider) {

    $routeProvider
        .when('/', {
            templateUrl: '/templates/Home/Index.html'

        })
        .when('/Interessado', {
            templateUrl: '/templates/Interesado/List.html',
            controller: 'InteressadoControl',
            resolve: {
                
                interessados: function (InteressadoService) {
                    return InteressadoService.Read();
                }
            }
        })
        .when('/Interessado/:id', {
            templateUrl: '/templates/Interesado/Detalhe.html',
            controller: 'InteressadoControl',
            resolve: {
                interessados: function (InteressadoService, $route) {
                    return InteressadoService.Read($route.current.params.id);
                }
            }
        })
    .when('/Voluntario', {
        templateUrl: '/templates/Voluntario/List.html',
        controller: 'VoluntarioControl',
        resolve: {

            voluntarios: function (VoluntarioService) {
                return VoluntarioService.Read();
            }
        }
    })
    .when('/Voluntario/Criar', {
        templateUrl: '/templates/Voluntario/Create.html',
        controller: 'VoluntarioCreateControl'        
        
    })
        .when('/Voluntario/Editar/:id', {
            templateUrl: '/templates/Voluntario/Update.html',
            controller: 'VoluntarioUpdateControl',
            resolve: {
                voluntario: function (VoluntarioService, $route) {
                    return VoluntarioService.Read($route.current.params.id);
                }
            }
        })
    .when('/Voluntario/:id', {
        templateUrl: '/templates/Voluntario/Detalhe.html',
        controller: 'VoluntarioUpdateControl',
        resolve: {
            voluntario: function (VoluntarioService, $route) {
                return VoluntarioService.Read($route.current.params.id);
            }
        }
    })






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


    $locationProvider.html5Mode(true);
});



  

var ufs = {
    'AC': 'Acre',
    'AL': 'Alagoas',
    'AP': 'Amapá',
    'AM': 'Amazonas',
    'BA': 'Bahia',
    'CE': 'Ceará',
    'DF': 'Distrito Federal',
    'ES': 'Espírito Santo',
    'GO': 'Goiás',
    'MA': 'Maranhão',
    'MT': 'Mato Grosso',
    'MS': 'Mato Grosso do Sul',
    'MG': 'Minas Gerais',
    'PA': 'Pará',
    'PB': 'Paraíba',
    'PR': 'Paraná',
    'PE': 'Pernambuco',
    'PI': 'Piauí',
    'RJ': 'Rio de Janeiro',
    'RN': 'Rio Grande do Norte',
    'RS': 'Rio Grande do Sul',
    'RO': 'Rondônia',
    'RR': 'Roraima',
    'SC': 'Santa Catarina',
    'SP': 'São Paulo',
    'SE': 'Sergipe',
    'TO': 'Tocantins'
};