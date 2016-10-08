angular.module('app')
.controller('ItemControl', ['$scope', 'ItemService', 'itens', 'loadingDialod', 'ngDialog', function ($scope, ItemService, itens, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.itens = itens;
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;

    $scope.totalItems = ItemService.totalItems;
    $scope.currentPage = 1;

    $scope.pageChanged = function () {
        ItemService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
        .then(function (itens) {
            $scope.itens = itens;
            $scope.totalItems = ItemService.totalItems;
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

    $scope.delete = function (item) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este Item?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            ItemService.Delete(item)
            .then(function () {
                //remove da lista
                var index = $scope.itens.indexOf(item);
                $scope.itens.splice(index, 1);
            }, function (erros) {                
                ngDialog.open({
                    template: '\
                <h1>Erro</h1>\
                 <p>Este item já está sendo utilizado e não pode ser apagado</p>  \
                ',
                    plain: true
                })

            });
        }, function () {
            //não faz nada
        });
    }


}])
.controller('ItemUpdateControl', ['$scope', 'ItemService', 'item', 'loadingDialod','$location', function ($scope, ItemService, item, loadingDialod,$location) {
    loadingDialod.close();
    $scope.item = item;//angular.copy(item);
    $scope.random = new Date().getTime();
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    
    $scope.salvar = function () {
        ItemService.Update($scope.item)
            .then(function (item) {
                $location.path('/Item');
            }, function (erros) {
                $scope.erros = erros;
            });
    }



}])
.controller('ItemCreateControl', ['$scope', 'ItemService', '$location', 'ngDialog', '$window', function ($scope, ItemService, $location, ngDialog, $window) {
    $scope.item = {};
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    $scope.salvar = function () {
        var dados = "";
        dados += "<p>Destino: " + destinos[$scope.item.destino] + '</p>';
        dados += "<p>Item: " + $scope.item.nome + '</p>';
        dados += "<p>Unidade de media: " + unidadesDeMedida[$scope.item.unidadeDeMedida] + '</p>';
        ngDialog.openConfirm({
            template: '\
                <h2>Os dados estão corretos?</h2>'
                +dados+    
                '<div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            //salva
            ItemService.Create($scope.item)
            .then(function (item) {
                $window.history.back();
            }, function (erros) {
                $scope.erros = erros;
            });
        }, function () {
            //não faz nada
        });


        
    }
    


}]);