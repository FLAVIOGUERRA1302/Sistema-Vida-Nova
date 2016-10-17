angular.module('app')
.controller('EstoqueControl', ['$scope', 'EstoqueService', 'itens', 'loadingDialod', 'ngDialog', function ($scope, EstoqueService, itens, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.itens = itens;
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    $scope.negativos = false;

    $scope.totalItems = EstoqueService.totalItems;
    $scope.currentPage = 1;

    $scope.pageChanged = function () {
        EstoqueService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa, $scope.negativos)//id,skip,take,filtro
        .then(function (itens) {
            $scope.itens = itens;
            $scope.totalItems = EstoqueService.totalItems;
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

   


}])
.controller('EstoqueUpdateControl', ['$scope', 'EstoqueService', 'item', 'loadingDialod','$location', function ($scope, EstoqueService, item, loadingDialod,$location) {
    loadingDialod.close();
    $scope.item = item;//angular.copy(item);
    $scope.random = new Date().getTime();
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    
    $scope.salvar = function () {
        EstoqueService.Update($scope.item)
            .then(function (item) {
                $location.path('/Estoque');
            }, function (erros) {
                $scope.erros = erros;
            });
    }



}])
