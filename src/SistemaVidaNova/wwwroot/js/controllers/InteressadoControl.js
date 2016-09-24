angular.module('app')
.controller('InteressadoControl', ['$scope', 'InteressadoService',  'interessados', function ($scope, InteressadoService,  interessados) {
    var itensPorPagina = 10;
    $scope.interessados = interessados;


    $scope.totalItems = InteressadoService.totalItems;
    $scope.currentPage = 1;

    $scope.pageChanged = function () {
        InteressadoService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina)//id,skip,take
        .then(function (interessados) {
            $scope.interessados = interessados;
            $scope.totalItems = InteressadoService.totalItems;
        }, function (erros) {

        });
    };


}])
.controller('InteressadoUpdateControl', ['$scope', 'InteressadoService', 'interessado',  function ($scope, InteressadoService, interessado) {
    $scope.interessado = interessado;//angular.copy(interessado);
    $scope.random = new Date().getTime();
    
    
    $scope.salvar = function () {
        InteressadoService.Update($scope.interessado)
            .then(function (interessado) {

            }, function (erros) {
                //exibir erros
            });
    }



}])
.controller('InteressadoCreateControl', ['$scope', 'InteressadoService',  '$location', function ($scope, InteressadoService,  $location) {
    $scope.interessado = {};
    $scope.salvar = function () {
        InteressadoService.Create($scope.interessado)
            .then(function (interessado) {
                $location.path('/Interessado/Editar/' + interessado.id)

            }, function (erros) {

            });
    }
    


}]);