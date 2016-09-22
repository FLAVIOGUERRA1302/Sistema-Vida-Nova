angular.module('app')
.controller('InteressadoControl', ['$scope', 'InteressadoService', '$uibModal', function ($scope, InteressadoService, $uibModal) {
    $scope.interessados = [];
    InteressadoService.Read().then(function (interessados) {
        $scope.interessados = interessados;
    });
    
    $scope.editar = function (interessado) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'InteressadoUpdate.html',
            controller: 'InteressadoUpdateControl',
            size: 'lg',
            resolve: {
                interessado: function () {
                    return interessado;
                }
            }
        });

        modalInstance.result.then(function (result) {
            var index = $scope.interessados.indexOf(interessado);
            $scope.interessados[index] = result;
        }, function () {
            //cancelou
        });

    }

    $scope.novo = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'InteressadoCreate.html',
            controller: 'InteressadoCreateControl',
            size: 'lg'
            
        });

        modalInstance.result.then(function (result) {            
            $scope.interessados.push(result);
        }, function () {
            //cancelou
        });

    }

}])
.controller('InteressadoUpdateControl', ['$scope', 'InteressadoService', '$uibModalInstance', 'interessado', function ($scope, InteressadoService, $uibModalInstance, interessado) {
    $scope.interessado = angular.copy(interessado);
    if (!($scope.interessado.dataNascimento instanceof Date))
        $scope.interessado.dataNascimento = new Date($scope.interessado.dataNascimento);
    $scope.salvar = function () {
        InteressadoService.Update($scope.interessado)
            .then(function (interessado) {
                //$scope.interessado = interessado;
                $uibModalInstance.close(interessado);
            }, function (erros) {
                //exibir erros
            });
    }
    $scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.valido = function () {
        
        return $scope.form.$valid;
    }
    
}])
.controller('InteressadoCreateControl', ['$scope', 'InteressadoService', '$uibModalInstance', function ($scope, InteressadoService, $uibModalInstance) {
    $scope.interessado = {};
    $scope.salvar = function () {
        InteressadoService.Create($scope.interessado)
            .then(function (interessado) {
                //$scope.interessado = interessado;
                $uibModalInstance.close(interessado);
            }, function (erros) {
                //exibir erros
            });
    }
    $scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.valido = function () {

        return $scope.form.$valid;
    }
}]);