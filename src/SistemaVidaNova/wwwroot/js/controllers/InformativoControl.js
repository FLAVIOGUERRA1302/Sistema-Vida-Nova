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
        .then(function (results) {
            $scope.progresso = 0;
            for (var i = 0; i < results.length;i++)
                $scope.informativo.attachments.push(results[i]);
        }, function (erros) {

        }, function (progresso) {
            $scope.progresso = progresso;
        });

    }
    $scope.Salvar = function () {
        InformativoService.Update($scope.informativo)
            .then(function () { });
    }

    $scope.AdicionarTodosUsuarios = function () {
        var carregando = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default'})
        UsuarioService.Read(null, 0, 1000000)//id,skip,take
        .then(function (usuarios) {
            $scope.informativo.usuarios = usuarios;
            $scope.Salvar();
            carregando.close();
            }, function () {

            });
    }

    $scope.AdicionarTodosVoluntarios = function () {
        var carregando = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' })
        VoluntarioService.Read(null, 0, 1000000)//id,skip,take
        .then(function (voluntarios) {
            $scope.informativo.voluntarios = voluntarios;
            $scope.Salvar();
            carregando.close();
        }, function () {

        });
    }

    $scope.AdicionarTodosDoadoresPF = function () {
        var carregando = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' })
        DoadorService.Read(null, 0, 1000000,'PF')//id,skip,take,tipo
        .then(function (doadoresFisicos) {
            $scope.informativo.doadoresFisicos = doadoresFisicos;
            $scope.Salvar();
            carregando.close();
        }, function () {

        });
    }

    $scope.AdicionarTodosDoadoresPJ = function () {
        var carregando = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' })
        DoadorService.Read(null, 0, 1000000, 'PJ')//id,skip,take,tipo
        .then(function (doadoresJuridicos) {
            $scope.informativo.doadoresJuridicos = doadoresJuridicos;
            $scope.Salvar();
            carregando.close();
        }, function () {

        });
    }


       
    $scope.enviar = function () {
        var enviando = ngDialog.open({ template: '/templates/enviando.html', className: 'ngdialog-theme-default' })
        InformativoService.Update($scope.informativo)
            .then(function () {
                InformativoService.Send($scope.informativo)
                    .then(function (novoInfor) {
                        $scope.informativo = novoInfor;
                        enviando.close();
                        ngDialog.open({ template: '/templates/Informativo/Sucesso.html', className: 'ngdialog-theme-default', })
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


    $scope.Detach = function (attachment) {
        InformativoService.Detach($scope.informativo, attachment)
            .then(function () {
                var index = $scope.informativo.attachments.indexOf(attachment);
                $scope.informativo.attachments.splice(index, 1);
            }, function () {

            });
    }

}])
;