angular.module('app')
.controller('DespesaControl', ['$scope', 'DespesaService', 'despesas', 'loadingDialod', 'ngDialog', function ($scope, DespesaService, despesas, loadingDialod, ngDialog) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.despesas = despesas;
    for (var i = 0; i < $scope.despesas.length; i++) {
        if (!($scope.despesas[i].dataDaCompra instanceof Date))
            $scope.despesas[i].dataDaCompra = Date.parse($scope.despesas[i].dataDaCompra);
    }

    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    
    $scope.tipo = 'ASSOCIACAO';

    $scope.totalItems = DespesaService.totalItems;
    $scope.currentPage = 1;
    
    $scope.ToExcel = function () {
        DespesaService.toExcel($scope.tipo, $scope.valorPesquisa)
        /*.then(function () {            
        }, function (erros) {
            
        });*/
    };

    $scope.pageChanged = function () {
        var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
        $scope.despesas = [];
        DespesaService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.tipo, $scope.valorPesquisa)//id,skip,take,tiopo,filtro
        .then(function (despesas) {
            $scope.despesas = despesas;
            $scope.totalItems = DespesaService.totalItems;
            for (var i = 0; i < $scope.despesas.length; i++) {
                if (!($scope.despesas[i].dataDaCompra instanceof Date))
                    $scope.despesas[i].dataDaCompra = Date.parse($scope.despesas[i].dataDaCompra);
            }
            dialog.close();
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

    $scope.delete = function (despesa) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este despesa?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            DespesaService.Delete(despesa)
            .then(function () {
                //remove da lista
                var index = $scope.despesas.indexOf(despesa);
                $scope.despesas.splice(index, 1);
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


    $scope.$watch('tipo', function (newValue, oldValue) {
        if (newValue != oldValue)
            $scope.$broadcast('tipo', $scope.tipo);
    });


}])
.controller('DespesaUpdateControl', ['$scope', 'DespesaService', 'despesa', 'loadingDialod', '$location', 'CepService','ItemService','FavorecidoService', function ($scope, DespesaService, despesa, loadingDialod, $location, CepService,ItemService,FavorecidoService) {
    loadingDialod.close();
    despesa.dataDaCompra = Date.parse(despesa.dataDaCompra);
    $scope.despesa = despesa;//angular.copy(despesa);
    $scope.random = new Date().getTime();
    
    $scope.despesa.valorTotal = $scope.despesa.valorUnitario * $scope.despesa.quantidade;

    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;

    

    $scope.salvar = function () {
        DespesaService.Update($scope.despesa)
            .then(function (despesa) {
                $location.path('/Despesa');
            }, function (erros) {
                $scope.erros = erros;
            });
    }


    $scope.getItens = function (val) {
        return ItemService.Read(null, 0, 10, val, despesa.tipo);//id,skip, take, filtro, destino

    };

    $scope.getFavorecidos = function (val) {
        return FavorecidoService.Read(null, 0, 10, val);//id,skip, take, filtro

    };

    $scope.$watch('despesa.quantidade', function (newValue, oldValue) {
        $scope.despesa.valorTotal = $scope.despesa.valorUnitario * $scope.despesa.quantidade;
    });

    $scope.$watch('despesa.valorUnitario', function (newValue, oldValue) {
        $scope.despesa.valorTotal = $scope.despesa.valorUnitario * $scope.despesa.quantidade;
    });
    $scope.$watch('despesa.valorTotal', function (newValue, oldValue) {
        if ($scope.despesa.quantidade > 0 && $scope.despesa.valorTotal > 0) {
            $scope.despesa.valorUnitario = $scope.despesa.valorTotal / $scope.despesa.quantidade;
        }
        else {
            $scope.despesa.valorUnitario = 0;
        }
    });



}])
.controller('DespesaCreateControl', ['$scope', 'DespesaService','ItemService','FavorecidoService', '$location', 'CepService', 'despesa', function ($scope, DespesaService,ItemService,FavorecidoService, $location, CepService, despesa) {
    $scope.despesa = despesa;
    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    $scope.salvar = function () {
        DespesaService.Create($scope.despesa)
            .then(function (despesa) {
                $location.path('/Despesa')

            }, function (erros) {
                $scope.erros = erros;
            });
    }

   
    $scope.getItens = function (val) {
        return ItemService.Read(null, 0, 10,val, despesa.tipo );//id,skip, take, filtro, destino
        
    };

    $scope.getFavorecidos = function (val) {
        return FavorecidoService.Read(null, 0, 10, val);//id,skip, take, filtro

    };

    $scope.$watch('despesa.quantidade', function (newValue, oldValue) {
        $scope.despesa.valorTotal = $scope.despesa.valorUnitario * $scope.despesa.quantidade;
    });

    $scope.$watch('despesa.valorUnitario', function (newValue, oldValue) {
        $scope.despesa.valorTotal = $scope.despesa.valorUnitario * $scope.despesa.quantidade;
    });
    $scope.$watch('despesa.valorTotal', function (newValue, oldValue) {
        if($scope.despesa.quantidade>0 && $scope.despesa.valorTotal>0  ){
            $scope.despesa.valorUnitario = $scope.despesa.valorTotal / $scope.despesa.quantidade;
        }
        else{
            $scope.despesa.valorUnitario = 0;
        }
    });

}])
.controller('DespesaRelatorioControl', ['$scope', 'DespesaService', 'dados', '$routeParams', 'loadingDialod', '$filter', function ($scope, DespesaService, dados, $routeParams, loadingDialod, $filter) {

    var getOptions = function (labels, valores) {
        return {
            title: {
                text: 'Despesas no período',                
                x: 'center'
            },
            tooltip: {
                trigger: 'axis',
                formatter: function (a) {
                    return a[0].name +"<br/>" +a[0].seriesName +" : " + $filter('currency')(a[0].value, 'R$', 2)
                }
            },


            calculable: true,
            xAxis: [
                {
                    type: 'category',
                    data: labels
                }
            ],
            yAxis: [
                {
                    type: 'value',
                    axisLabel: {
                        formatter: 'R$ {value} '
                    }
                }
            ],
            series: [
                {
                    name: 'Valor',
                    type: 'bar',
                    data: valores,

                    itemStyle: {
                        normal: {
                            label: {
                                show: true,
                                position: 'top',
                                formatter: function (a) {
                                    return $filter('currency')(a.value, 'R$ ', 2)
                                }
                            }
                        }

                    }
                }
            ]
        };
    }

    $scope.dados = dados;
    $scope.start = Date.parse($routeParams.start);
    $scope.end = Date.parse($routeParams.end);
       
    var labels = [];
    var valores = [];

    angular.forEach(dados, function (value, key) {
        labels.push(key);
        valores.push(value);
    });


    $scope.options = getOptions(labels, valores);

    loadingDialod.close();
}]);