angular.module('app')
.controller('UsuarioControl', ['$scope', 'AccountService', 'usuarios', 'loadingDialod', 'ngDialog', function ($scope, AccountService, usuarios, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.usuarios = usuarios;


    $scope.totalItems = AccountService.totalItems;
    $scope.currentPage = 1;

    $scope.ToExcel = function () {
        AccountService.toExcel($scope.valorPesquisa, $scope.adminPesquisa, $scope.ativoPesquisa)
        /*.then(function () {            
        }, function (erros) {
            
        });*/
    };

    $scope.pageChanged = function () {
        AccountService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa, $scope.adminPesquisa, $scope.ativoPesquisa)//id,skip,take,filtro
        .then(function (usuarios) {
            $scope.usuarios = usuarios;
            $scope.totalItems = AccountService.totalItems;
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

    $scope.delete = function (usuario) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este Usuario?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            AccountService.Delete(usuario)
            .then(function () {
                //remove da lista
                var index = $scope.usuarios.indexOf(usuario);
                $scope.usuarios.splice(index, 1);
            }, function (erros) {
                ngDialog.open({
                    template: '<h1>Não é possível apagar este usuário</h1>'             ,
                    plain: true
                })

            });
        }, function () {
            //não faz nada
        });
    }


}])
.controller('UsuarioUpdateControl', ['$scope', 'AccountService', 'usuario', 'loadingDialod','$location', function ($scope, AccountService, usuario, loadingDialod,$location) {
    loadingDialod.close();
    $scope.usuario = usuario;
    $scope.random = new Date().getTime();
    
    
    $scope.salvar = function () {
        AccountService.Update($scope.usuario)
            .then(function (usuario) {
                $location.path('/Usuario');
            }, function (erros) {
                $scope.erros = erros;
            });
    }



}])
.controller('UsuarioCreateControl', ['$scope', 'AccountService',  '$location', function ($scope, AccountService,  $location) {
    $scope.usuario = {};
    $scope.salvar = function () {
        AccountService.Create($scope.usuario)
            .then(function (usuario) {
                $location.path('/Usuario')

            }, function (erros) {
                $scope.erros = erros;
            });
    }
    


}]);