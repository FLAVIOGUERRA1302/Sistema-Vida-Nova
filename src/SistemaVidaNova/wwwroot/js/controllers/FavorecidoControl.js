
angular.module('app')
.controller('FavorecidoControl', ['$scope', 'FavorecidoService', 'favorecidos', 'loadingDialod', 'ngDialog', function ($scope, FavorecidoService, favorecidos, loadingDialod, ngDialog) {
    
    var itensPorPagina = 10;
    $scope.favorecidos = favorecidos;
    for (var i = 0; i < $scope.favorecidos.length; i++) {
        if (!($scope.favorecidos[i].dataNascimento instanceof Date))
            $scope.favorecidos[i].dataNascimento = Date.parse($scope.favorecidos[i].dataNascimento);

        if (!($scope.favorecidos[i].dataDeCadastro instanceof Date))
            $scope.favorecidos[i].dataDeCadastro = Date.parse($scope.favorecidos[i].dataDeCadastro);
    }
    loadingDialod.close();

    $scope.totalItems = FavorecidoService.totalItems;
    $scope.currentPage = 1;

     $scope.ToExcel = function () {
         FavorecidoService.toExcel( $scope.valorPesquisa)
        /*.then(function () {            
        }, function (erros) {
            
        });*/
    };

    $scope.pageChanged = function () {
        FavorecidoService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
        .then(function (favorecidos) {
            $scope.favorecidos = favorecidos;
            for (var i = 0; i < $scope.favorecidos.length; i++) {
                if (!($scope.favorecidos[i].dataNascimento instanceof Date))
                    $scope.favorecidos[i].dataNascimento = Date.parse($scope.favorecidos[i].dataNascimento);

                if (!($scope.favorecidos[i].dataDeCadastro instanceof Date))
                    $scope.favorecidos[i].dataDeCadastro = Date.parse($scope.favorecidos[i].dataDeCadastro);
            }
            $scope.totalItems = FavorecidoService.totalItems;
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

    $scope.delete = function (favorecido) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este Favorecido?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            FavorecidoService.Delete(favorecido)
            .then(function () {
                //remove da lista
                var index = $scope.favorecidos.indexOf(favorecido);
                $scope.favorecidos.splice(index, 1);
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
.controller('FavorecidoUpdateControl', ['$scope', 'FavorecidoService', 'favorecido', 'loadingDialod', '$location', 'CepService', function ($scope, FavorecidoService, favorecido, loadingDialod, $location, CepService) {
    loadingDialod.close();
    $scope.favorecido = favorecido;
   
    $scope.random = new Date().getTime();
    $scope.ufs = ufs;

    if (!($scope.favorecido.dataNascimento instanceof Date))
        $scope.favorecido.dataNascimento = Date.parse($scope.favorecido.dataNascimento);

    if (!($scope.favorecido.dataDeCadastro instanceof Date))
        $scope.favorecido.dataDeCadastro = Date.parse($scope.favorecido.dataDeCadastro);


   
    $scope.salvar = function () {
        FavorecidoService.Update($scope.favorecido)
            .then(function (favorecido) {
                $location.path('/Favorecido');
            }, function (erros) {
                $scope.erros = erros;
            });
    }

    $scope.buscaCep = function () {
        if ($scope.favorecido.cep) {
            CepService.Pesquisa($scope.favorecido.cep)
            .then(function (endereco) {

                if (endereco.estado)
                    $scope.favorecido.estado = endereco.estado;
                if (endereco.bairro)
                    $scope.favorecido.bairro = endereco.bairro;
                if (endereco.cidade)
                    $scope.favorecido.cidade = endereco.cidade;
                if (endereco.logradouro)
                    $scope.favorecido.logradouro = endereco.logradouro;




                $scope.msgErro = "";
            }, function (erros) {
                $scope.msgErro = "CEP não localizado";
            });
        }
    }

   


}])
.controller('FavorecidoCreateControl', ['$scope', 'FavorecidoService', '$location', 'CepService', function ($scope, FavorecidoService, $location, CepService) {

    $scope.favorecido = {}; // valores padrao   

    
    $scope.ufs = ufs;
    $scope.salvar = function () {
        FavorecidoService.Create($scope.favorecido)
            .then(function (favorecido) {
                $location.path('/Favorecido');

            }, function (erros) {
                $scope.erros = erros;
            });
    }
    $scope.buscaCep = function () {
        if ($scope.favorecido.cep) {
            CepService.Pesquisa($scope.favorecido.cep)
            .then(function (endereco) {

                if (endereco.estado)
                    $scope.favorecido.estado = endereco.estado;
                if (endereco.bairro)
                    $scope.favorecido.bairro = endereco.bairro;
                if (endereco.cidade)
                    $scope.favorecido.cidade = endereco.cidade;
                if (endereco.logradouro)
                    $scope.favorecido.logradouro = endereco.logradouro;




                $scope.msgErro = "";
            }, function (erros) {
                $scope.msgErro = "CEP não localizado";
            });
        }
    }


}]);