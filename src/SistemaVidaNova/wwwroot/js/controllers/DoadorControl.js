angular.module('app')
.controller('DoadorControl', ['$scope', 'DoadorService', 'doadores', 'loadingDialod', 'ngDialog', function ($scope, DoadorService, doadores, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.doadores = doadores;
    
    $scope.tipo = 'PF';

    $scope.totalItems = DoadorService.totalItems;
    $scope.currentPage = 1;
    

    $scope.pageChanged = function () {
        DoadorService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.tipo, $scope.valorPesquisa)//id,skip,take,tiopo,filtro
        .then(function (doadores) {
            $scope.doadores = doadores;
            $scope.totalItems = DoadorService.totalItems;
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

    $scope.delete = function (doador) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este doador?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            DoadorService.Delete(doador)
            .then(function () {
                //remove da lista
                var index = $scope.doadores.indexOf(doador);
                $scope.doadores.splice(index, 1);
            }, function () {
             
            });
        }, function () {
            //não faz nada
        });
    }


}])
.controller('DoadorUpdateControl', ['$scope', 'DoadorService', 'doador', 'loadingDialod', '$location', 'CepService', function ($scope, DoadorService, doador, loadingDialod, $location, CepService) {
    loadingDialod.close();
    $scope.doador = doador;//angular.copy(doador);
    $scope.random = new Date().getTime();
    
    $scope.ufs = ufs;

    $scope.buscaCep = function () {
        if ($scope.doador && $scope.doador.endereco && $scope.doador.endereco.cep) {
            CepService.Pesquisa($scope.doador.endereco.cep)
            .then(function (endereco) {

                if (endereco.estado)
                    $scope.doador.endereco.estado = endereco.estado;
                if (endereco.bairro)
                    $scope.doador.endereco.bairro = endereco.bairro;
                if (endereco.cidade)
                    $scope.doador.endereco.cidade = endereco.cidade;
                if (endereco.logradouro)
                    $scope.doador.endereco.logradouro = endereco.logradouro;



                $scope.msgErro = "";
            }, function (erros) {
                $scope.msgErro = "CEP não localizado";
            });
        }
    }

    $scope.salvar = function () {
        DoadorService.Update($scope.doador)
            .then(function (doador) {
                $location.path('/Doador');
            }, function (erros) {
                //exibir erros
            });
    }



}])
.controller('DoadorCreateControl', ['$scope', 'DoadorService', '$location', 'CepService', 'doador', function ($scope, DoadorService, $location, CepService, doador) {
    $scope.doador = doador;
    $scope.salvar = function () {
        DoadorService.Create($scope.doador)
            .then(function (doador) {
                $location.path('/Doador')

            }, function (erros) {

            });
    }

    $scope.ufs = ufs;

    $scope.buscaCep = function () {
        if ($scope.doador && $scope.doador.endereco && $scope.doador.endereco.cep) {
            CepService.Pesquisa($scope.doador.endereco.cep)
            .then(function (endereco) {

                if (endereco.estado)
                    $scope.doador.endereco.estado = endereco.estado;
                if (endereco.bairro)
                    $scope.doador.endereco.bairro = endereco.bairro;
                if (endereco.cidade)
                    $scope.doador.endereco.cidade = endereco.cidade;
                if (endereco.logradouro)
                    $scope.doador.endereco.logradouro = endereco.logradouro;



                $scope.msgErro = "";
            }, function (erros) {
                $scope.msgErro = "CEP não localizado";
            });
        }
    }
    


}]);