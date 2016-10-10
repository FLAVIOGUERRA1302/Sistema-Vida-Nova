angular.module('app')
.controller('DespesaControl', ['$scope', 'DespesaService', 'despesas', 'loadingDialod', 'ngDialog', function ($scope, DespesaService, despesas, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.despesas = despesas;
    for (var i = 0; i < $scope.despesas.length; i++) {
        if (!($scope.despesas[i].dataDaCompra instanceof Date))
            $scope.despesas[i].dataDaCompra = new Date($scope.despesas[i].dataDaCompra);
    }

    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    
    $scope.tipo = 'ASSOCIACAO';

    $scope.totalItems = DespesaService.totalItems;
    $scope.currentPage = 1;
    

    $scope.pageChanged = function () {
        DespesaService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.tipo, $scope.valorPesquisa)//id,skip,take,tiopo,filtro
        .then(function (despesas) {
            $scope.despesas = despesas;
            $scope.totalItems = DespesaService.totalItems;
            for (var i = 0; i < $scope.despesas.length; i++) {
                if (!($scope.despesas[i].dataDaCompra instanceof Date))
                    $scope.despesas[i].dataDaCompra = new Date($scope.despesas[i].dataDaCompra);
            }
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

    $scope.Select=function(tipo){
        $scope.tipo = tipo;
        $scope.currentPage = 1;
        $scope.pageChanged();
    }

    $scope.delete = function (despesa) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este despesa?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            DespesaService.Delete(despesa)
            .then(function () {
                //remove da lista
                var index = $scope.despesas.indexOf(despesa);
                $scope.despesas.splice(index, 1);
            }, function (erros) {
                var corpo = "";
                angular.forEach(erros, function (value, key) {
                    corpo += '<div class="list-group">';
                    corpo += '<a href="#" class="list-group-item list-group-item-danger">' + key + '</a>';
                    for (var i = 0; i < value.length; i++) {
                        corpo += '<a href="#" class="list-group-item">' + value[i] + '</a>';
                    }
                    corpo += '</div>';
                });



                ngDialog.open({
                    template: '\
                <h1>Erro</h1>\
                '+ corpo,
                    plain: true
                })
            });
        }, function () {
            //não faz nada
        });
    }


}])
.controller('DespesaUpdateControl', ['$scope', 'DespesaService', 'despesa', 'loadingDialod', '$location', 'CepService','ItemService','FavorecidoService', function ($scope, DespesaService, despesa, loadingDialod, $location, CepService,ItemService,FavorecidoService) {
    loadingDialod.close();
    despesa.dataDaCompra = new Date(despesa.dataDaCompra);
    $scope.despesa = despesa;//angular.copy(despesa);
    $scope.random = new Date().getTime();
    
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;

    

    $scope.salvar = function () {
        DespesaService.Update($scope.despesa)
            .then(function (despesa) {
                $location.path('/Despesa');
            }, function (erros) {
                $scope.erros = erros;
            });
    }


    $scope.getItens = function (val) {
        return ItemService.Read(null, 0, 10, val, despesa.tipo);//id,skip, take, filtro, destino

    };

    $scope.getFavorecidos = function (val) {
        return FavorecidoService.Read(null, 0, 10, val);//id,skip, take, filtro

    };



}])
.controller('DespesaCreateControl', ['$scope', 'DespesaService','ItemService','FavorecidoService', '$location', 'CepService', 'despesa', function ($scope, DespesaService,ItemService,FavorecidoService, $location, CepService, despesa) {
    $scope.despesa = despesa;
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    $scope.salvar = function () {
        DespesaService.Create($scope.despesa)
            .then(function (despesa) {
                $location.path('/Despesa')

            }, function (erros) {
                $scope.erros = erros;
            });
    }

   
    $scope.getItens = function (val) {
        return ItemService.Read(null, 0, 10,val, despesa.tipo );//id,skip, take, filtro, destino
        
    };

    $scope.getFavorecidos = function (val) {
        return FavorecidoService.Read(null, 0, 10, val);//id,skip, take, filtro

    };

}]);