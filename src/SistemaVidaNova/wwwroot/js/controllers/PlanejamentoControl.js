angular.module('app')
.controller('PlanejamentoControl', ['$scope', 'PlanejamentoService', 'ngDialog', 'ModeloDeReceitaService', function ($scope, PlanejamentoService, ngDialog, ModeloDeReceitaService) {
    
    $scope.modelos = [];
    //$scope.chartData = {};
    $scope.podeExibir = false;
    var getOptions = function (labels, valores) {
        return {
            title: {
                text: 'Planejamento',
                subtext: 'Quantos dias por item',
                x: 'center'
            },
            tooltip: {
                trigger: 'axis'
            },
            /*legend: {
                data: ['2011年', '2012年']
            },*/

            calculable: true,
            xAxis: [
                {
                    type: 'value',
                    boundaryGap: [0, 0.01],
                    axisLabel: {
                        formatter: '{value} dias'
                    }
                }
            ],
            yAxis: [
                {
                    type: 'category',
                    data: labels
                }
            ],
            series: [
                {
                    name: 'Dias',
                    type: 'bar',
                    data: valores
                }
            ]
        };
    }


    $scope.getModelos = function (val) {
        return ModeloDeReceitaService.Read(null, 0, 10, val);//id,skip, take, filtro

    };

    $scope.addModelo = function () {
        if ($scope.itemSelected) {
            for (var i = 0; i < $scope.modelos.length; i++) {
                if ($scope.itemSelected.id == $scope.modelos[i].modelo.id) {
                    $scope.itemSelected = null;
                    return;
                }
            }
            $scope.modelos.push({ modelo: $scope.itemSelected, quantidade: 1 });
            $scope.itemSelected = null;
        }
    }

    $scope.removerModelo = function (modelo) {
        var index = $scope.modelos.indexOf(modelo);
        $scope.modelos.splice(index, 1);
    }


    $scope.visualizar = function () {
        var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
        PlanejamentoService.Consulta($scope.modelos)
           .then(function (result) {
               dialog.close();
               
                   
                var labels = [];
                var valores = [];
                angular.forEach(result, function (value, key) {
                    labels.push(key);
                    valores.push(value);

                });
                if (valores.length > 0) {
                    $scope.podeExibir = true;
                   $scope.options = getOptions(labels, valores);


                   
               } else {
                   $scope.podeExibir = false;
                   ngDialog.openConfirm({
                        template: '\
                            <div class="text-center"><h1>Nada a exibir</h1></div>\
                            <div class="ngdialog-buttons">\
                                <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Ok</button>\
                            </div>',
                        plain: true
                    });

               }
           }, function (erros) {
               $scope.erros = erros;
           });
    }

}])

;