var app = angular.module('app');
app.factory('VoluntarioService', ["$http", "$q", function ($http,$q) {
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