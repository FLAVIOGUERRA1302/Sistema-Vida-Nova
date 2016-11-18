angular.module('app')
.controller('VoluntarioControl', ['$scope', 'VoluntarioService', '$uibModal', 'voluntarios', 'loadingDialod', 'ngDialog', function ($scope, VoluntarioService, $uibModal, voluntarios, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.voluntarios = voluntarios;
    $scope.valorPesquisa = "";

    $scope.totalItems = VoluntarioService.totalItems;
    $scope.currentPage = 1;

       

    $scope.ToExcel = function () {
        VoluntarioService.toExcel($scope.valorPesquisa)
        /*.then(function () {            
        }, function (erros) {
            
        });*/
    };

    $scope.pageChanged = function () {
        VoluntarioService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
        .then(function (voluntarios) {
            $scope.voluntarios = voluntarios;
            $scope.totalItems = VoluntarioService.totalItems;
        }, function (erros) {

        });
    };
   
    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

    $scope.delete = function (voluntario) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este Voluntário?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            VoluntarioService.Delete(voluntario)
            .then(function () {
                //remove da lista
                var index = $scope.voluntarios.indexOf(voluntario);
                $scope.voluntarios.splice(index, 1);
            }, function () {
                ngDialog.open({
                template: '<h1>Não é possível apagar este voluntário</h1>',
                plain: true
            })
            });
        }, function () {
            //não faz nada
        });
    }

}])
.controller('VoluntarioUpdateControl', ['$scope', 'VoluntarioService', 'voluntario', 'CepService', 'loadingDialod','$location','$rootScope', function ($scope, VoluntarioService, voluntario, CepService, loadingDialod,$location,$rootScope) {
    loadingDialod.close();
    $scope.voluntario = voluntario;//angular.copy(voluntario);
    $scope.random = new Date().getTime();
    $scope.ufs = ufs;
    if (!($scope.voluntario.dataNascimento instanceof Date))
        $scope.voluntario.dataNascimento = Date.parse($scope.voluntario.dataNascimento);
    if (!($scope.voluntario.dataCurso instanceof Date))
        $scope.voluntario.dataCurso = Date.parse($scope.voluntario.dataCurso);
    if (!($scope.voluntario.dataAgendamentoCurso instanceof Date) && $scope.voluntario.dataAgendamentoCurso!= null)
        $scope.voluntario.dataAgendamentoCurso = Date.parse($scope.voluntario.dataAgendamentoCurso);
    $scope.salvar = function () {
        VoluntarioService.Update($scope.voluntario)
            .then(function (voluntario) {
                $rootScope.$broadcast('voluntario.update', voluntario);
                $location.path('/Voluntario' )
            }, function (erros) {
                $scope.erros = erros;
            });
    }
    
    

    $scope.Upload = function () {
        VoluntarioService.Upload($scope.voluntario, $scope.picFile)
            .then(function (voluntario) {
                $scope.random = new Date().getTime();
                $scope.progresso = 0;
                $scope.picFile = null;
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
                <h1>Erro, tente novamente</h1>\
                '+ corpo,
                plain: true
            })
        }, function (progresso) {
            $scope.progresso = progresso;
        });
    }

    $scope.buscaCep = function () {
        if ($scope.voluntario && $scope.voluntario.endereco && $scope.voluntario.endereco.cep) {
            CepService.Pesquisa($scope.voluntario.endereco.cep)
            .then(function (endereco) {

                if (endereco.estado)
                    $scope.voluntario.endereco.estado = endereco.estado;
                if (endereco.bairro)
                    $scope.voluntario.endereco.bairro = endereco.bairro;
                if (endereco.cidade)
                    $scope.voluntario.endereco.cidade = endereco.cidade;
                if (endereco.logradouro)
                    $scope.voluntario.endereco.logradouro = endereco.logradouro;




                $scope.msgErro = "";
            }, function (erros) {
                $scope.msgErro = "CEP não localizado";
            });
        }
    }
    
}])
.controller('VoluntarioCreateControl', ['$scope', 'VoluntarioService', 'CepService', '$location', 'ngDialog', function ($scope, VoluntarioService, CepService, $location, ngDialog) {
    $scope.voluntario = {};
    $scope.salvar = function () {
        VoluntarioService.Create($scope.voluntario)
            .then(function (voluntario) {
                $location.path('/Voluntario/');
                
            }, function (erros) {
                $scope.erros = erros;
            });
    }
    $scope.ufs = ufs;
    $scope.buscaCep = function () {
        if ($scope.voluntario && $scope.voluntario.endereco && $scope.voluntario.endereco.cep) {
            CepService.Pesquisa($scope.voluntario.endereco.cep)
            .then(function (endereco) {

                if (endereco.estado)
                    $scope.voluntario.endereco.estado = endereco.estado;
                if (endereco.bairro)
                    $scope.voluntario.endereco.bairro = endereco.bairro;
                if (endereco.cidade)
                    $scope.voluntario.endereco.cidade = endereco.cidade;
                if (endereco.logradouro)
                    $scope.voluntario.endereco.logradouro = endereco.logradouro;




                $scope.msgErro = "";
            }, function (erros) {
                $scope.msgErro = "CEP não localizado";
            });
        }
    }


}])
.controller('VoluntarioNotificacaoControl', ['$scope', 'VoluntarioService',  function ($scope, VoluntarioService) {
    $scope.voluntarios = [];
    $scope.semAgendamento = "";

    var read = function () {
        VoluntarioService.ReadCursoAtrasado()
        .then(function (voluntarios) {            
            $scope.voluntarios = voluntarios;
            $scope.semAgendamento = VoluntarioService.totalItemsAtrasado;
            //corrige de texto para Date as datas
            var umAnoAtras = Date.today().addYears(-1);
            angular.forEach(voluntarios, function (voluntario, key) {
                voluntario.dataCurso = Date.parse(voluntario.dataCurso);
                
            });

        }, function (erros) {

        });
    }

    $scope.$on('voluntario.update', function (event, data) {
        read();
    });

    read();

}])

.controller('VoluntarioDisponivelControl', ['$scope', 'VoluntarioService', '$uibModal', 'voluntarios', 'loadingDialod', 'ngDialog', 'diaDaSemana', function ($scope, VoluntarioService, $uibModal, voluntarios, loadingDialod, ngDialog, diaDaSemana) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.voluntarios = voluntarios;
    $scope.valorPesquisa = "";
    $scope.diaDaSemana = diaDaSemana;
    $scope.totalItems = VoluntarioService.totalItems;
    $scope.currentPage = 1;



    $scope.ToExcel = function () {
        VoluntarioService.toExcel($scope.valorPesquisa,$scope.diaDaSemana)
        /*.then(function () {            
        }, function (erros) {
            
        });*/
    };

    $scope.pageChanged = function () {
        VoluntarioService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa, $scope.diaDaSemana)//id,skip,take,filtro
        .then(function (voluntarios) {
            $scope.voluntarios = voluntarios;
            $scope.totalItems = VoluntarioService.totalItems;
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

    $scope.delete = function (voluntario) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este Voluntário?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            VoluntarioService.Delete(voluntario)
            .then(function () {
                //remove da lista
                var index = $scope.voluntarios.indexOf(voluntario);
                $scope.voluntarios.splice(index, 1);
            }, function () {

            });
        }, function () {
            //não faz nada
        });
    }

}]);