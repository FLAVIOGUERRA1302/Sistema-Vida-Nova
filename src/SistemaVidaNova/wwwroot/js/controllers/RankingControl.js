angular.module('app')
.controller('EventosMaisProcuradosControl', ['$scope', 'EventosMaisProcuradosService', 'ngDialog',
    function ($scope, EventosMaisProcuradosService, ngDialog) {
        $scope.resultado = null;
        $scope.consultar = function () {
            var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
        EventosMaisProcuradosService.Read($scope.start, $scope.end)//start,end
        .then(function (resultado) {
            dialog.close();
            $scope.resultado = resultado;
            
        }, function (erros) {
            dialog.close();
        });
    }  

    }])

.controller('MelhoresDoadoresControl', ['$scope', 'MelhoresDoadoresService', 'ngDialog',
    function ($scope, MelhoresDoadoresService, ngDialog) {
        $scope.resultado = null;
        $scope.consultar = function () {
            var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            MelhoresDoadoresService.Read($scope.start, $scope.end)//start,end
            .then(function (resultado) {
                dialog.close();
                $scope.resultado = resultado;

            }, function (erros) {
                dialog.close();
            });
        }

    }])

.controller('FavorecidoMaisBeneficiadosControl', ['$scope', 'FavorecidoMaisBeneficiadosService', 'ngDialog',
    function ($scope, FavorecidoMaisBeneficiadosService, ngDialog) {
        $scope.resultado = null;
        $scope.consultar = function () {
            var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            FavorecidoMaisBeneficiadosService.Read($scope.start, $scope.end)//start,end
            .then(function (resultado) {
                dialog.close();
                $scope.resultado = resultado;

            }, function (erros) {
                dialog.close();
            });
        }

    }]);

