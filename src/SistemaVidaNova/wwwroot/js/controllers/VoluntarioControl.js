angular.module('app')
.controller('VoluntarioControl', ['$scope', 'VoluntarioService', '$uibModal', 'voluntarios', 'loadingDialod', function ($scope, VoluntarioService, $uibModal, voluntarios, loadingDialod) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.voluntarios = voluntarios;
    $scope.valorPesquisa = "";

    $scope.totalItems = VoluntarioService.totalItems;
    $scope.currentPage = 1;

       

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

}])
.controller('VoluntarioUpdateControl', ['$scope', 'VoluntarioService', 'voluntario', 'CepService', 'loadingDialod','$location', function ($scope, VoluntarioService, voluntario, CepService, loadingDialod,$location) {
    loadingDialod.close();
    $scope.voluntario = voluntario;//angular.copy(voluntario);
    $scope.random = new Date().getTime();
    $scope.ufs = ufs;
    if (!($scope.voluntario.dataNascimento instanceof Date))
        $scope.voluntario.dataNascimento = new Date($scope.voluntario.dataNascimento);
    $scope.salvar = function () {
        VoluntarioService.Update($scope.voluntario)
            .then(function (voluntario) {
                $location.path('/Voluntario' + interessado.id)
            }, function (erros) {
                //exibir erros
            });
    }
    
    

    $scope.Upload = function () {
        VoluntarioService.Upload($scope.voluntario, $scope.picFile)
            .then(function (voluntario) {
                $scope.random = new Date().getTime();
                $scope.progresso = 0;
                $scope.picFile = null;
        }, function (erros) {
            //exibir erros
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
.controller('VoluntarioCreateControl', ['$scope', 'VoluntarioService', 'CepService', '$location', function ($scope, VoluntarioService, CepService, $location) {
    $scope.voluntario = {};
    $scope.salvar = function () {
        VoluntarioService.Create($scope.voluntario)
            .then(function (voluntario) {
                $location.path('/Voluntario/Editar/' + voluntario.id)
                
            }, function (erros) {
                
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


    }]);