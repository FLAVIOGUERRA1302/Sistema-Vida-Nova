var app = angular.module('app');
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

    s.Read = function (id,skip,take,filtro) {
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

    
    s.toExcel = function (filtro) {
        var turl = '/api/Voluntario/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
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

    s.Read = function (id, skip, take, filtro) {
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
        $http(req).then(function successCallback(response) {
            s.totalItems = parseInt(response.headers('totalItems'));
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

    s.toExcel = function (filtro) {
        var turl = '/api/Evento/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
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

    s.toExcel = function (tipo, filtro) {
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

    s.toExcel = function (filtro) {
        var turl = '/api/Item/excel';
        if (filtro !== null && filtro !== undefined && filtro !== "") {
            turl += '?filtro=' + filtro;
        }
        var url = encodeURI(turl)
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

app.factory('ModeloDeReceitaService', ["$http", "$q", function ($http, $q) {
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