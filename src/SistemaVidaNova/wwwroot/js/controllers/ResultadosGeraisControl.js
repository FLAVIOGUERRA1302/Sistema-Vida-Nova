angular.module('app')
.controller('ResultadosGeraisControl', ['$scope', 'ResultadosGeraisService', 'ngDialog',
    function ($scope, ResultadosGeraisService, ngDialog) {
        $scope.unidadesDeMedida = unidadesDeMedida;
    
        $scope.resultado = null;
    $scope.consultar = function () {
        ResultadosGeraisService.Read($scope.start, $scope.end)//start,end
        .then(function (resultado) {
            $scope.resultado = resultado;
            
        }, function (erros) {

        });
    }

  



    



}]);

