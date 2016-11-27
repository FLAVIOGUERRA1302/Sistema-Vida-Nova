angular.module('app')

.controller('ResultadosGeraisControl', ['$scope', 'ResultadosGeraisService', 'ngDialog','$filter','$timeout',
    function ($scope, ResultadosGeraisService, ngDialog, $filter, $timeout) {
        $scope.unidadesDeMedida = unidadesDeMedida;
        $scope.podeExibir = false;
        $scope.resultado = null;
        $scope.api = {};
        var getOptions = function (labels, seriesName, seriesData,colors) {
            var series = [];

            for (var i = 0; i < seriesName.length; i++) {
                series.push({
                    name: seriesName[i],
                    type: 'bar',
                    data: seriesData[i],

                    itemStyle: {
                        normal: {
                            color :colors[i],
                            label: {
                                show: true,
                                position: 'top',

                                formatter: function (a) {
                                    return $filter('currency')(a.value, 'R$', 2)
                                }
                            }
                        }

                    }
                });
            }

            return {
                title: {
                    text: 'Doações x Despesas por mês',
                    x: 'center'
                },
                legend: {
                    orient: 'horizontal',
                    x: 'left',
                    data: seriesName
                },
                tooltip: {
                    trigger: 'axis',
                    formatter: function (a,b,c,d) {
                        return a[0].name + "<br/>" + a[0].seriesName + " : " + $filter('currency')(a[0].value, 'R$', 2)+
                            "<br/>" + a[1].seriesName + " : " + $filter('currency')(a[1].value, 'R$', 2)
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
                series: series
            };
        }


    $scope.consultar = function () {
        ResultadosGeraisService.Read($scope.start, $scope.end)//start,end
        .then(function (resultado) {
            $scope.resultado = resultado;
            
            if (resultado.chartData.labels.length > 0) {
                $scope.podeExibir = true;

                var labels = resultado.chartData.labels;
                var seriesName = resultado.chartData.seriesName
                var seriesData = resultado.chartData.series
                var colors = ['blue','red' ];

                for (var i = 0; i < labels.length; i++)
                    labels[i] = $filter('date')(Date.parse(labels[i]), 'MMMM-yyyy');


                $scope.options = getOptions(labels, seriesName, seriesData, colors)

            }
            else {
                $scope.podeExibir = false;
            }
        }, function (erros) {

        });
    }

    $scope.resizeChart = function () {
        if ($scope.api.chart)
            $timeout(function () {
                $scope.api.chart.resize()
            },500
                );
            
    }

  



    



    }])
.controller('ResultadosGeraisRelatorioControl', ['$scope', 'ResultadosGeraisService', 'ngDialog', '$filter', '$timeout', 'dados','loadingDialod',
function ($scope, ResultadosGeraisService, ngDialog, $filter, $timeout, dados, loadingDialod) {
        $scope.unidadesDeMedida = unidadesDeMedida;
        loadingDialod.close();
        $scope.resultado = dados;
        
        var getOptions = function (labels, seriesName, seriesData, colors) {
            var series = [];

            for (var i = 0; i < seriesName.length; i++) {
                series.push({
                    name: seriesName[i],
                    type: 'bar',
                    data: seriesData[i],

                    itemStyle: {
                        normal: {
                            color: colors[i],
                            label: {
                                show: true,
                                position: 'top',

                                formatter: function (a) {
                                    return $filter('currency')(a.value, 'R$', 2)
                                }
                            }
                        }

                    }
                });
            }

            return {
                title: {
                    text: 'Doações x Despesas por mês',
                    x: 'center'
                },
                tooltip: {
                    trigger: 'axis',
                    formatter: function (a) {
                        return a[0].name + "<br/>" + a[0].seriesName + " : " + $filter('currency')(a[0].value, 'R$', 2) +
                            "<br/>" + a[1].seriesName + " : " + $filter('currency')(a[1].value, 'R$', 2)
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
                series: series
            };
        }


        
        if (dados.chartData.labels.length > 0) {
                    $scope.podeExibir = true;

                    var labels = dados.chartData.labels;
                    var seriesName = dados.chartData.seriesName
                    var seriesData = dados.chartData.series
                    var colors = ['blue', 'red'];

                    for (var i = 0; i < labels.length; i++)
                        labels[i] = $filter('date')(Date.parse(labels[i]), 'MMMM-yyyy');


                    $scope.options = getOptions(labels, seriesName, seriesData, colors)

                }
                else {
                    $scope.podeExibir = false;
                }
           

   









    }]);

