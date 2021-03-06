﻿var app = angular.module('app');
app.factory('VoluntarioService', ["$http", "$q", "Upload", "$window", function ($http, $q, Upload, $window) {
    var s = {};

    s.Create = function (voluntario) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Voluntario',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(voluntario),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id,skip,take,filtro,diaDaSemana,semCurso) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Voluntario/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }

        if (diaDaSemana !== null && diaDaSemana !== undefined && diaDaSemana !== "") {
            req.params.diaDaSemana = diaDaSemana;
        }
        if (semCurso !== null && semCurso !== undefined && semCurso !== "") {
            req.params.semCurso = semCurso;
        }

        
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        

        return deferred.promise;
    }

    s.ReadMotorista = function ( skip, take, filtro) {
        var deferred = $q.defer();
        
        var req = {
            method: 'GET',
            url: '/api/Voluntario/',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }

        
        req.params.funcao = 'MOTORISTA';
        
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.ReadCursoAtrasado = function () {
        var deferred = $q.defer();
        
        var req = {
            method: 'GET',
            url: '/api/Voluntario/' ,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {semCurso:true,'skip': 0, 'take': 5};        
        
        $http(req).then(function successCallback(response) {
            s.totalItemsAtrasado = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (voluntario) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Voluntario/' + voluntario.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(voluntario),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (voluntario) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Voluntario/' + voluntario.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Upload = function (voluntario,foto) {
        var deferred = $q.defer();

        Upload.upload({
            //method: 'UPLOAD',
            url: '/api/Voluntario/' + voluntario.id,
            data: { file: foto }
        }).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        }, function (evt) {
            deferred.notify (Math.min(100, parseInt(100.0 *
                                     evt.loaded / evt.total)));
        });

        return deferred.promise;
    }

    
    s.toExcel = function (filtro,diaDaSemana,semCurso) {
        var turl = '/api/Voluntario/excel';
        var param = "?";
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            param += 'filtro=' + filtro;
        }
        if (diaDaSemana !== null && diaDaSemana !== undefined && diaDaSemana !== "") {
            param += ((param=="?"?"":"&") + 'diaDaSemana=' + diaDaSemana);
        }
        if (semCurso !== null && semCurso !== undefined && semCurso !== "") {
            param += ((param == "?" ? "" : "&") + 'semCurso=' + semCurso);
        }

        param = param == "?" ? "" : param;

        var url = encodeURI(turl + param)
        $window.open(url);
        /*
        var deferred = $q.defer();
        
        var req = {
            method: 'GET',
            url: '/api/Voluntario/excel',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            
            var blob = new Blob([response.data], {name:'voluntarios.xlsx', type: response.headers("Content-Type") });
            var objectUrl = URL.createObjectURL(blob);
            //$window.open(objectUrl);

            var anchor = angular.element('<a/>');
            anchor.attr({
                href: 'data:attachment/' + response.headers("Content-Type") + ';charset=utf-8,' + encodeURI(response.data),
                target: '_blank',
                download: 'filename.xlsx'
            })[0].click();


            deferred.resolve();
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;*/
    }

    return s;


}]);

app.factory('InteressadoService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (interessado) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Interessado',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(interessado),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Interessado/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (interessado) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Interessado/' + interessado.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(interessado),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (interessado) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Interessado/' + interessado.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro) {
        var turl = '/api/Interessado/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro='+filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);
        
       
    }

    return s;


}]);

app.factory('EventoService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (evento) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Evento',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(evento),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro,comParticipantes) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Evento/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }

        if (comParticipantes !== null && comParticipantes !== undefined && comParticipantes !== "") {
            req.params.comParticipantes = comParticipantes;
        }


        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            var hoje = new Date();
            for (var i = 0; i < response.data.length; i++) {
                if (!(response.data[i].start instanceof Date))
                    response.data[i].start = Date.parse(response.data[i].start);

                if (!(response.data[i].end instanceof Date))
                    response.data[i].end = Date.parse(response.data[i].end);

                response.data[i].semParticipantes = response.data[i].participantes == 0 && (Math.floor(response.data[i].start - hoje) / (1000 * 60 * 60 * 24) < 7)
            }


            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Update = function (evento) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Evento/' + evento.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(evento),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (evento) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Evento/' + evento.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro, comParticipantes) {
        var turl = '/api/Evento/excel';
        var param = "?";
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            param += 'filtro=' + filtro;
        }
        
        if (comParticipantes !== null && comParticipantes !== undefined && comParticipantes !== "") {
            param += ((param == "?" ? "" : "&") + 'comParticipantes=' + comParticipantes);
        }

        param = param == "?" ? "" : param;

        var url = encodeURI(turl + param)
        $window.open(url);
    }

    return s;


}]);

app.factory('DoadorService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (doador) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Doador',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doador),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take,tipo, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Doador/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        if (tipo !== null && tipo !== undefined && tipo !== "") {
            req.params.tipo = tipo;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (doador) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Doador/' + doador.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doador),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (doador) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Doador/' + doador.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (tipo, filtro) {
        var turl = '/api/Doador/excel?tipo='+tipo;
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '&filtro='+filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);

    }
    s.EnviarRelatorioEmail = function (id,start,end) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Doador/EnviarRelatorioEmail/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = { 'start': start, 'end': end }
        
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }


    

    return s;


}]);

app.factory('FavorecidoService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (favorecido) {
        
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Favorecido',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(favorecido),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Favorecido/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (favorecido) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Favorecido/' + favorecido.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(favorecido),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (favorecido) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Favorecido/' + favorecido.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function ( filtro) {
        var turl = '/api/Favorecido/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);

    }

    return s;


}]);

app.factory('CepService', ["$http", "$q", function ($http, $q) {
    var s = {};

    

    s.Pesquisa = function (cep) {
        var deferred = $q.defer();
        
        var req = {
            method: 'GET',
            url: 'http://api.postmon.com.br/v1/cep/' + cep,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        
        $http(req).then(function successCallback(response) {            
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    return s;

}])


app.factory('UsuarioService', ["$http", "$q", "$window", function ($http, $q,$window) {
    var s = {};

    

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Usuario/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

   

   

    return s;


}]);

app.factory('ChartService', ["$http", "$q", function ($http, $q) {
    var s = {};
    s.Read = function (id, params) {
        var deferred = $q.defer();
        
        var req = {
            method: 'GET',
            url: '/api/Chart/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        if (params != undefined && params != null)
            req.params = params;
        
        $http(req).then(function successCallback(response) {            
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }





    return s;


}]);

app.factory('InformativoService', ["$http", "$q", "Upload", function ($http, $q, Upload) {
    var s = {};

   

    s.Read = function () {
        var deferred = $q.defer();
        
        var req = {
            method: 'GET',
            url: '/api/Informativo/',
            headers: {
                'Content-Type': 'application/json'
            },
            dataType: 'json'
        };
        
        $http(req).then(function successCallback(response) {            
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (informativo) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Informativo/' + informativo.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(informativo),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Send = function (informativo) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Informativo/Send/' + informativo.id,
            headers: {
                'Content-Type': 'application/json'
            },
            
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (informativo) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Informativo/' + informativo.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Attach = function (informativo, files) {
        var deferred = $q.defer();

        Upload.upload({
            //method: 'UPLOAD',
            url: '/api/Informativo/Attach/' + informativo.id,
            data: { files: files }
        }).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        }, function (evt) {
            deferred.notify(Math.min(100, parseInt(100.0 *
                                     evt.loaded / evt.total)));
        });

        return deferred.promise;
    }

    s.Detach = function (informativo, attachment) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Informativo/Detach/' + informativo.id + "/" + attachment.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    return s;


}]);

app.factory('DespesaService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (despesa) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Despesa',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(despesa),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, tipo, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Despesa/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        if (tipo !== null && tipo !== undefined && tipo !== "") {
            req.params.tipo = tipo;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (despesa) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Despesa/' + despesa.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(despesa),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (despesa) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Despesa/' + despesa.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (tipo, filtro) {
        var turl = '/api/Despesa/excel?tipo=' + tipo;
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '&filtro=' + filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);

    }

    s.Relatorio = function (start, end, tipo) {
        var deferred = $q.defer();
        
        var req = {
            method: 'GET',
            url: '/api/Despesa/Relatorio',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = { 'start': Date.parseExact(start, 'dd-MM-yyyy'), 'end': Date.parseExact(end, 'dd-MM-yyyy'), 'tipo': tipo };
       
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    return s;


}]);

app.factory('ItemService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (item) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Item',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(item),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro, destino) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Item/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        if (destino !== null && destino !== undefined && destino !== "") {
            req.params.destino = destino;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (item) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Item/' + item.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(item),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (item) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Item/' + item.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro,destino) {
        var turl = '/api/Item/excel';
        var query = "?"
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            query += 'filtro=' + filtro;
        }
        if (destino !== null && destino !== undefined && destino !== "") {
            query += ((query=="?"?"":"&")+'destino=' + destino);
        }
        
        var url = encodeURI(turl + query)

        $window.open(url);

    }

    return s;


}]);


app.factory('DoacaoDinheiroService', ["$http", "$q", "$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/DoacaoDinheiro',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doacao),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/DoacaoDinheiro/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.ReadPeriodo = function (start, end, idDoador) {
        var deferred = $q.defer();        
        var req = {
            method: 'GET',
            url: '/api/DoacaoDinheiro/' ,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = { start: start, end: end, idDoador: idDoador };
        
        
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/DoacaoDinheiro/' + doacao.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doacao),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/DoacaoDinheiro/' + doacao.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }


    s.toExcel = function (filtro) {
        var turl = '/api/DoacaoDinheiro/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);
    }

    s.Relatorio = function (start, end) {
        var deferred = $q.defer();

        var req = {
            method: 'GET',
            url: '/api/DoacaoDinheiro/Relatorio',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = { 'start': Date.parseExact(start, 'dd-MM-yyy'), 'end': Date.parseExact(end, 'dd-MM-yyy') };

        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }
    
    return s;


}]);


app.factory('DoacaoSopaService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/DoacaoSopa',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doacao),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/DoacaoSopa/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/DoacaoSopa/' + doacao.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doacao),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/DoacaoSopa/' + doacao.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro) {
        var turl = '/api/DoacaoSopa/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);
    }

    return s;


}]);

app.factory('DoacaoObjetoService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/DoacaoObjeto',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doacao),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/DoacaoObjeto/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/DoacaoObjeto/' + doacao.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(doacao),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (doacao) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/DoacaoObjeto/' + doacao.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro) {
        var turl = '/api/DoacaoObjeto/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);
    }

    return s;


}]);

app.factory('ModeloDeReceitaService', ["$http", "$q", "$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (modelo) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/ModeloDeReceita',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(modelo),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/ModeloDeReceita/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (modelo) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/ModeloDeReceita/' + modelo.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(modelo),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (modelo) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/ModeloDeReceita/' + modelo.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro) {
        var turl = '/api/ModeloDeReceita/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);
    }

    return s;


}]);

app.factory('ResultadoSopaService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

    s.Create = function (resultado) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/ResultadoSopa',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(resultado),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/ResultadoSopa/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (resultado) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/ResultadoSopa/' + resultado.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(resultado),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (resultado) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/ResultadoSopa/' + resultado.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro) {
        var turl = '/api/ResultadoSopa/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
        $window.open(url);
    }

    

    return s;


}]);

app.factory('EstoqueService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};

   

    s.Read = function (id, skip, take, filtro, somenteNegativos) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Estoque/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        if (somenteNegativos !== null && somenteNegativos !== undefined && somenteNegativos !== "") {
            req.params.somenteNegativos = somenteNegativos;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (item) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Estoque/' + item.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(item),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro, somenteNegativos) {
        var turl = '/api/Estoque/excel';
        var qp = "?"
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            qp += 'filtro=' + filtro;
        }

        if (somenteNegativos !== null && somenteNegativos !== undefined && somenteNegativos !== "") {
            qp +=(qp=="?"?"":"&")+ 'somenteNegativos=' + somenteNegativos;
        }

        var url = encodeURI(turl + qp);
        $window.open(url);

    }

    return s;


}]);

app.factory('PlanejamentoService', ["$http", "$q","$window", function ($http, $q,$window) {
    var s = {};    
    s.Consulta = function (modelos) {
        var m = [];
        for (var i = 0; i < modelos.length; i++)
            m.push({ id: modelos[i].modelo.id, quantidade: modelos[i].quantidade });
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Planejamento',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(m),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }



    return s;


}]);


app.factory('AccountService', ["$http", "$q", "$window", function ($http, $q, $window) {
    var s = {};

    s.Create = function (user) {
        var deferred = $q.defer();
        var req = {
            method: 'POST',
            url: '/api/Account',
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(user),
            dataType: 'json'
        };

        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Read = function (id, skip, take, filtro,isAdmin,isAtivo) {
        var deferred = $q.defer();
        if (id === undefined || id === null) id = "";
        var req = {
            method: 'GET',
            url: '/api/Account/' + id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = {};
        if (skip !== undefined && take !== undefined) {
            req.params = { 'skip': skip, 'take': take };
        }
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            req.params.filtro = filtro;
        }
        if (isAdmin !== null && isAdmin !== undefined && isAdmin !== "") {
            req.params.isAdmin = isAdmin;
        }

        if (isAtivo !== null && isAtivo !== undefined && isAtivo !== "") {
            req.params.isAtivo = isAtivo;
        }
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

    s.Update = function (user) {
        var deferred = $q.defer();
        var req = {
            method: 'PUT',
            url: '/api/Account/' + user.id,
            headers: {
                'Content-Type': 'application/json'
            },
            data: JSON.stringify(user),
            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.Delete = function (user) {
        var deferred = $q.defer();
        var req = {
            method: 'DELETE',
            url: '/api/Account/' + user.id,
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        $http(req).then(function successCallback(response) {
            deferred.resolve("OK");
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });

        return deferred.promise;
    }

    s.toExcel = function (filtro,isAdmin,isAtivo) {
        var turl = '/api/Account/excel';
        var param = "?"
        
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            param += 'filtro=' + filtro;
        }
        if (isAdmin !== null && isAdmin !== undefined && isAdmin !== "") {
            param += ((param == "?" ? "" : "&") + 'isAdmin=' + isAdmin);
        }

        if (isAtivo !== null && isAtivo !== undefined && isAtivo !== "") {
            param += ((param == "?" ? "" : "&") + 'isAtivo=' + isAtivo);
        }
        


        var url = encodeURI(turl + param)
        $window.open(url);


    }

    return s;


}]);


app.factory('ResultadosGeraisService', ["$http", "$q", "$window", function ($http, $q, $window) {
    var s = {};

   
    s.Read = function (start, end) {
        var deferred = $q.defer();
        var req = {
            method: 'GET',
            url: '/api/ResultadosGerais/',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };

        

        req.params = { start: start, end: end };


        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }

   

    s.toExcel = function (start, end) {
        var turl = '/api/ResultadosGerais/excel?start=' + start.toISOString() + "&end=" + end.toISOString();
        
        var url = encodeURI(turl)
        $window.open(url);
    }

    return s;


}]);



app.factory('EventosMaisProcuradosService', ["$http", "$q", "$window", function ($http, $q, $window) {
    var s = {};


    s.Read = function (start, end) {
        var deferred = $q.defer();
        var req = {
            method: 'GET',
            url: '/api/EventosMaisProcurados/',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = { start: start, end: end };


        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            for (var i = 0; i < response.data.length; i++) {
                response.data[i].DataInicio = Date.parse(response.data[i].DataInicio);
                response.data[i].DataFim = Date.parse(response.data[i].DataFim);
            }

            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }



    s.toExcel = function (start, end) {
        var turl = '/api/EventosMaisProcurados/excel?start=' + start.toISOString() + "&end=" + end.toISOString();

        var url = encodeURI(turl)
        $window.open(url);
    }

    return s;


}]);

app.factory('MelhoresDoadoresService', ["$http", "$q", "$window", function ($http, $q, $window) {
    var s = {};


    s.Read = function (start, end) {
        var deferred = $q.defer();
        var req = {
            method: 'GET',
            url: '/api/MelhoresDoadores/',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = { start: start, end: end };


        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
            
            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }



    s.toExcel = function (start, end) {
        var turl = '/api/MelhoresDoadores/excel?start=' + start.toISOString() + "&end=" + end.toISOString();

        var url = encodeURI(turl)
        $window.open(url);
    }

    return s;


}]);

app.factory('FavorecidoMaisBeneficiadosService', ["$http", "$q", "$window", function ($http, $q, $window) {
    var s = {};


    s.Read = function (start, end) {
        var deferred = $q.defer();
        var req = {
            method: 'GET',
            url: '/api/FavorecidosComMaisGasto/',
            headers: {
                'Content-Type': 'application/json'
            },

            dataType: 'json'
        };
        req.params = { start: start, end: end };


        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));

            deferred.resolve(response.data);
        }, function errorCallback(response) {
            deferred.reject(response.data);
        });



        return deferred.promise;
    }



    s.toExcel = function (start, end) {
        var turl = '/api/FavorecidosComMaisGasto/excel?start=' + start.toISOString() + "&end=" + end.toISOString();

        var url = encodeURI(turl)
        $window.open(url);
    }

    return s;


}]);