angular.module('app')
.controller('EventoControl', ['$scope', 'EventoService', '$uibModal', function ($scope, EventoService, $uibModal) {
    $scope.eventos = [];
    EventoService.Read().then(function (eventos) {
        $scope.eventos = eventos;
    });
    
    $scope.editar = function (evento) {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'EventoUpdate.html',
            controller: 'EventoUpdateControl',
            size: 'lg',
            resolve: {
                evento: function () {
                    return evento;
                }
            }
        });

        modalInstance.result.then(function (result) {
            var index = $scope.eventos.indexOf(evento);
            $scope.eventos[index] = result;
        }, function () {
            //cancelou
        });

    }

    $scope.novo = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'EventoCreate.html',
            controller: 'EventoCreateControl',
            size: 'lg'
            
        });

        modalInstance.result.then(function (result) {            
            $scope.eventos.push(result);
        }, function () {
            //cancelou
        });

    }

}])
.controller('EventoUpdateControl', ['$scope', 'EventoService', '$uibModalInstance', 'evento', function ($scope, EventoService, $uibModalInstance, evento) {
    $scope.evento = angular.copy(evento);

    $scope.options = {
        required: true,
        format: 'hex',
        hue: true,
        alpha: false,
        swatchOnly: true,
        case: 'upper'
    }

    if (!($scope.evento.dataNascimento instanceof Date))
        $scope.evento.dataNascimento = new Date($scope.evento.dataNascimento);
    $scope.salvar = function () {
        EventoService.Update($scope.evento)
            .then(function (evento) {
                //$scope.evento = evento;
                $uibModalInstance.close(evento);
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
.controller('EventoCreateControl', ['$scope', 'EventoService', '$uibModalInstance', function ($scope, EventoService, $uibModalInstance) {
    $scope.evento = { valorDeEntrada: 0, valorArrecadado: 0, color: '#1A82C7', textColor: '#FFFFFF' }; // valores pad~rao   

    $scope.options = {
        required: true,
        format: 'hex',
        hue: true,
        alpha: false,
        swatchOnly: true,
        case: 'upper',
        disabled:false
    }


    $scope.salvar = function () {
        EventoService.Create($scope.evento)
            .then(function (evento) {
                //$scope.evento = evento;
                $uibModalInstance.close(evento);
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