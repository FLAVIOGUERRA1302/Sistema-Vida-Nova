angular.module('app')
.controller('VoluntarioControl', ['$scope', 'VoluntarioService', '$uibModal', 'voluntarios', function ($scope, VoluntarioService, $uibModal, voluntarios) {
    var itensPorPagina = 10;
    $scope.voluntarios = voluntarios;
   

    $scope.totalItems = VoluntarioService.totalItems;
    $scope.currentPage = 1;

    $scope.pageChanged = function () {
        VoluntarioService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina)//id,skip,take
        .then(function (voluntarios) {
            $scope.voluntarios = voluntarios;
            $scope.totalItems = VoluntarioService.totalItems;
        }, function (erros) {
            
        });
    };
   

}])
.controller('VoluntarioUpdateControl', ['$scope', 'VoluntarioService', 'voluntario', function ($scope, VoluntarioService,  voluntario) {
    $scope.voluntario = angular.copy(voluntario);
    if (!($scope.voluntario.dataNascimento instanceof Date))
        $scope.voluntario.dataNascimento = new Date($scope.voluntario.dataNascimento);
    $scope.salvar = function () {
        VoluntarioService.Update($scope.voluntario)
            .then(function (voluntario) {
                //$scope.voluntario = voluntario;
                $uibModalInstance.close(voluntario);
            }, function (erros) {
                //exibir erros
            });
    }
    $scope.cancelar = function () {
        $uibModalInstance.dismiss('cancel');
    }
    $scope.valido = function () {
        
        return $scope.form.$valid;
    }
    
}])
.controller('VoluntarioCreateControl', ['$scope', 'VoluntarioService', 'CepService', function ($scope, VoluntarioService, CepService) {
    $scope.voluntario = {};
    $scope.salvar = function () {
        VoluntarioService.Create($scope.voluntario)
            .then(function (voluntario) {
                
                
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