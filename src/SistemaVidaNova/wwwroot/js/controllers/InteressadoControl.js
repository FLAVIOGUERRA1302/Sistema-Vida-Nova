angular.module('app')
.controller('InteressadoControl', ['$scope', 'InteressadoService', 'interessados', 'loadingDialod', 'ngDialog', function ($scope, InteressadoService, interessados, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.interessados = interessados;


    $scope.totalItems = InteressadoService.totalItems;
    $scope.currentPage = 1;

    $scope.ToExcel = function () {
        InteressadoService.toExcel($scope.valorPesquisa)
        /*.then(function () {            
        }, function (erros) {
            
        });*/
    };

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

    $scope.delete = function (interessado) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este Interessado?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            InteressadoService.Delete(interessado)
            .then(function () {
                //remove da lista
                var index = $scope.interessados.indexOf(interessado);
                $scope.interessados.splice(index, 1);
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
.controller('InteressadoUpdateControl', ['$scope', 'InteressadoService', 'interessado', 'loadingDialod','$location', function ($scope, InteressadoService, interessado, loadingDialod,$location) {
    loadingDialod.close();
    $scope.interessado = interessado;//angular.copy(interessado);
    $scope.random = new Date().getTime();
    
    
    $scope.salvar = function () {
        InteressadoService.Update($scope.interessado)
            .then(function (interessado) {
                $location.path('/Interessado');
            }, function (erros) {
                $scope.erros = erros;
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
                $scope.erros = erros;
            });
    }
    


}]);