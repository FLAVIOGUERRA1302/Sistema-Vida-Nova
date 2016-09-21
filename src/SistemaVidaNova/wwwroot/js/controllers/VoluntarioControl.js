angular.module('app')
.controller('VoluntarioControl', ['$scope', 'VoluntarioService', '$uibModal', function ($scope, VoluntarioService, $uibModal) {
    $scope.voluntarios = [];
    VoluntarioService.Read().then(function (voluntarios) {
        $scope.voluntarios = voluntarios;
    });
    
    $scope.editar = function (voluntario) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'VoluntarioUpdate.html',
            controller: 'VoluntarioUpdateControl',
            size: 'lg',
            resolve: {
                voluntario: function () {
                    return voluntario;
                }
            }
        });

        modalInstance.result.then(function (result) {
            var index = $scope.voluntarios.indexOf(voluntario);
            $scope.voluntarios[index] = result;
        }, function () {
            //cancelou
        });

    }

    $scope.novo = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'VoluntarioCreate.html',
            controller: 'VoluntarioCreateControl',
            size: 'lg'
            
        });

        modalInstance.result.then(function (result) {            
            $scope.voluntarios.push(result);
        }, function () {
            //cancelou
        });

    }

}])
.controller('VoluntarioUpdateControl', ['$scope', 'VoluntarioService', '$uibModalInstance', 'voluntario', function ($scope, VoluntarioService, $uibModalInstance, voluntario) {
    $scope.voluntario = angular.copy(voluntario);
    if (!($scope.voluntario.dataNascimento instanceof Date))
        $scope.voluntario.dataNascimento = new Date($scope.voluntario.dataNascimento);
    $scope.salvar = function () {
        VoluntarioService.Update($scope.voluntario)
            .then(function (voluntario) {
                //$scope.voluntario = voluntario;
                $uibModalInstance.close(voluntario);
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
.controller('VoluntarioCreateControl', ['$scope', 'VoluntarioService', '$uibModalInstance', function ($scope, VoluntarioService, $uibModalInstance) {
    $scope.voluntario = {};
    $scope.salvar = function () {
        VoluntarioService.Create($scope.voluntario)
            .then(function (voluntario) {
                //$scope.voluntario = voluntario;
                $uibModalInstance.close(voluntario);
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