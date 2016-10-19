angular.module('app')
.controller('PlanejamentoControl', ['$scope', 'PlanejamentoService', 'ngDialog', 'ModeloDeReceitaService', function ($scope, PlanejamentoService, ngDialog, ModeloDeReceitaService) {
    
    $scope.modelos = [];
    //$scope.chartData = {};
    
    var getOptions = function (labels, valoes) {
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
                    boundaryGap: [0, 0.01]
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
                    data: valoes
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
               var labels = [];
               var valores = [];
               angular.forEach(result, function (value, key) {
                   labels.push(key);
                   valores.push(value);

               });


               $scope.options = getOptions(labels, valores);


               dialog.close();

           }, function (erros) {
               $scope.erros = erros;
           });
    }

}])

;