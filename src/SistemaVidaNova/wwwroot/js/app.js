var app = angular.module('app', [
    'ngAnimate',
    'ui.bootstrap',
    'color.picker',
    'ngRoute',
    'ui.utils.masks',
    'ngFileUpload'

]).config(function ($routeProvider, $locationProvider) {

    $routeProvider
        
       
    .when('/Voluntario', {
        templateUrl: '/templates/Voluntario/List.html',
        controller: 'VoluntarioControl',
        resolve: {

            voluntarios: function (VoluntarioService) {
                return VoluntarioService.Read(null,0,10);//id,skip,take
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



    .when('/Interessado', {
        templateUrl: '/templates/Interessado/List.html',
        controller: 'InteressadoControl',
        resolve: {

            interessados: function (InteressadoService) {
                return InteressadoService.Read(null, 0, 10);//id,skip,take
            }
        }
    })
        .when('/Interessado/Criar', {
            templateUrl: '/templates/Interessado/Create.html',
            controller: 'InteressadoCreateControl'

        })
            .when('/Interessado/Editar/:id', {
                templateUrl: '/templates/Interessado/Update.html',
                controller: 'InteressadoUpdateControl',
                resolve: {
                    interessado: function (InteressadoService, $route) {
                        return InteressadoService.Read($route.current.params.id);
                    }
                }
            })
        .when('/Interessado/:id', {
            templateUrl: '/templates/Interessado/Detalhe.html',
            controller: 'InteressadoUpdateControl',
            resolve: {
                interessado: function (InteressadoService, $route) {
                    return InteressadoService.Read($route.current.params.id);
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