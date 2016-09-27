angular.module('app')
.controller('InteressadoControl', ['$scope', 'InteressadoService', 'interessados', 'loadingDialod', function ($scope, InteressadoService, interessados, loadingDialod) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.interessados = interessados;


    $scope.totalItems = InteressadoService.totalItems;
    $scope.currentPage = 1;

    $scope.pageChanged = function () {
        InteressadoService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
        .then(function (interessados) {
            $scope.interessados = interessados;
            $scope.totalItems = InteressadoService.totalItems;
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }


}])
.controller('InteressadoUpdateControl', ['$scope', 'InteressadoService', 'interessado', 'loadingDialod','$location', function ($scope, InteressadoService, interessado, loadingDialod,$location) {
    loadingDialod.close();
    $scope.interessado = interessado;//angular.copy(interessado);
    $scope.random = new Date().getTime();
    
    
    $scope.salvar = function () {
        InteressadoService.Update($scope.interessado)
            .then(function (interessado) {
                $location.path('/Interessado');
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
                $location.path('/Interessado')

            }, function (erros) {

            });
    }
    


}]);