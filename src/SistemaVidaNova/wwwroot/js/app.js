var app = angular.module('app', [
    'ngRoute',
    'ngAnimate',
    'ngFileUpload',
    'ngDialog',
    'ui.bootstrap',
    'color.picker',    
    'ui.utils.masks',    
    'idf.br-filters'
    

]).config(function ($routeProvider, $locationProvider) {

    $routeProvider

      //------------Voluntario----------
    .when('/Voluntario', {
        templateUrl: '/templates/Voluntario/List.html',
        controller: 'VoluntarioControl',
        resolve: {

            voluntarios: function (VoluntarioService) {
                return VoluntarioService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
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
                ,
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
                }
            }
        })
    .when('/Voluntario/Visualizar/:id', {
        templateUrl: '/templates/Voluntario/Detalhe.html',
        controller: 'VoluntarioUpdateControl',
        resolve: {
            voluntario: function (VoluntarioService, $route) {
                return VoluntarioService.Read($route.current.params.id);
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
            }
        }
    })


        //------------Interessado----------
    .when('/Interessado', {
        templateUrl: '/templates/Interessado/List.html',
        controller: 'InteressadoControl',
        resolve: {

            interessados: function (InteressadoService) {
                return InteressadoService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
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
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
                    }
                }
            })
        .when('/Interessado/Visualizar/:id', {
            templateUrl: '/templates/Interessado/Detalhe.html',
            controller: 'InteressadoUpdateControl',
            resolve: {
                interessado: function (InteressadoService, $route) {
                    return InteressadoService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
                }
            }
        })



        //------------Evento----------
    .when('/Evento', {
        templateUrl: '/templates/Evento/List.html',
        controller: 'EventoControl',
        resolve: {

            eventos: function (EventoService) {
                return EventoService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
            }
        }
    })
        .when('/Evento/Criar', {
            templateUrl: '/templates/Evento/Create.html',
            controller: 'EventoCreateControl'

        })
         .when('/Evento/Calendario', {
             templateUrl: '/templates/Evento/Calendario.html'

         })
            .when('/Evento/Editar/:id', {
                templateUrl: '/templates/Evento/Update.html',
                controller: 'EventoUpdateControl',
                resolve: {
                    evento: function (EventoService, $route) {
                        return EventoService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
                    }
                }
            })
        .when('/Evento/Visualizar/:id', {
            templateUrl: '/templates/Evento/Detalhe.html',
            controller: 'EventoUpdateControl',
            resolve: {
                evento: function (EventoService, $route) {
                    return EventoService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
                }
            }
        })

     //------------Doador----------
    .when('/Doador', {
        templateUrl: '/templates/Doador/List.html',
        controller: 'DoadorControl',
        resolve: {

            doadores: function (DoadorService) {
                return DoadorService.Read(null, 0, 10,'PF');//id,skip,take
            },            
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
            }
        }
    })
        .when('/Doador/Criar/PF', {
            templateUrl: '/templates/Doador/CreatePf.html',
            controller: 'DoadorCreateControl',
            resolve: {
                doador: function () {
                    return { tipo: "PF" };
                }                
            }

        })
        .when('/Doador/Criar/PJ', {
            templateUrl: '/templates/Doador/CreatePj.html',
            controller: 'DoadorCreateControl',
            resolve: {
                doador: function () {
                    return { tipo: "PJ" };
                }
            }

        })
            .when('/Doador/Editar/:id', {
                templateUrl: '/templates/Doador/Update.html',
                controller: 'DoadorUpdateControl',
                resolve: {
                    doador: function (DoadorService, $route) {
                        return DoadorService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
                    }
                }
            })
        .when('/Doador/Visualizar/:id', {
            templateUrl: '/templates/Doador/Detalhe.html',
            controller: 'DoadorUpdateControl',
            resolve: {
                doador: function (DoadorService, $route) {
                    return DoadorService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
                }
            }
        })
       ;

    


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