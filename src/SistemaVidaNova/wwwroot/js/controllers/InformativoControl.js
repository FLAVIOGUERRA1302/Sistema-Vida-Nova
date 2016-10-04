angular.module('app')
.controller('InformativoControl', [
    '$scope',
     'InformativoService',
     'UsuarioService',
     'VoluntarioService',
     'DoadorService',     
     'informativo',
     'loadingDialod',
     'ngDialog',
     '$route',
    function (
        $scope,
        InformativoService,
        UsuarioService,
        VoluntarioService,
        DoadorService,     
        informativo,
        loadingDialod,
        ngDialog,
        $route) {

    loadingDialod.close();
    
    $scope.informativo = informativo;
    $scope.progress = 0;

    $scope.loadUsuarios = function (query) {
        return UsuarioService.Read(null, 0, 5, query)//id,skip,take,filtro
    }

    $scope.loadVoluntarios = function (query) {
        return VoluntarioService.Read(null, 0, 5, query)//id,skip,take,filtro        
    }

    $scope.loadDoadoresFisicos = function (query) {
        return DoadorService.Read(null, 0, 5,'PF', query)//id,skip,take,tipo,filtro        
    }
    $scope.loadDoadoresJuridicos = function (query) {
        return DoadorService.Read(null, 0, 5, 'PJ', query)//id,skip,take,tipo,filtro        
    }
    $scope.uploadFiles = function (file, errFiles) {
        InformativoService.Attach($scope.informativo,file)
        .then(function (result) {

        }, function (erros) {

        }, function (progresso) {
            $scope.progresso = progresso;
        });

    }
       
    $scope.enviar = function () {
        InformativoService.Update($scope.informativo)
            .then(function () {
                InformativoService.Send($scope.informativo)
                    .then(function () {
                    }, function () {

                    });
            }, function () {

            });
        
    }
    

    $scope.delete = function () {
        
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este Informativo?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            InformativoService.Delete($scope.informativo)
            .then(function () {
                $route.reload();
            }, function () {

            });
        }, function () {
            //não faz nada
        });
    }

}])
;