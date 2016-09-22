var app = angular.module('app');
app.factory('VoluntarioService', ["$http", "$q", function ($http, $q) {
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

    s.Read = function (id) {
        var deferred = $q.defer();
        if (id === undefined) id = "";
        var req = {
            method: 'GET',
            url: '/api/Voluntario/' + id,
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
            url: '/api/Voluntario/' + voluntario.Id,
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

app.factory('InteressadoService', ["$http", "$q", function ($http, $q) {
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

    s.Read = function (id) {
        var deferred = $q.defer();
        if (id === undefined) id = "";
        var req = {
            method: 'GET',
            url: '/api/Interessado/' + id,
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
            url: '/api/Interessado/' + interessado.Id,
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

app.factory('EventoService', ["$http", "$q", function ($http, $q) {
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

    s.Read = function (id) {
        var deferred = $q.defer();
        if (id === undefined) id = "";
        var req = {
            method: 'GET',
            url: '/api/Evento/' + id,
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
            url: '/api/Evento/' + evento.Id,
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