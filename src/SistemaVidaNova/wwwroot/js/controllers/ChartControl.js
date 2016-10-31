angular.module('app')
.controller('ChartVoluntarioDiaDaSemanaControl', ['$scope', 'ChartService', 'ngDialog', '$location', "$timeout", function ($scope, ChartService, ngDialog, $location, $timeout) {
    
    var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
    //$scope.chartData = {};

    /*

    $scope.chartConfig = {        
        title : {
            text: 'Disponibilidade de voluntários', x: 'center'
        },
        debug: true,
        showXAxis: true,
        showYAxis: true,
        showLegend: true,
        stack: false,
        loading: "Carregando...",
        errorMsg :"Erro",
        calculable: true,
        theme: 'infographic', //'infographic', 'macarons', 'shine', 'dark', 'blue', 'green', 'red',
        center: ['50%', '50%'],
        radius: '80%',
        legend:{
            orient : 'verticle',
            x : 'left',
            y : 'top'
        },
        event :{
            echarts.config.EVENT.CLICK
    }
       
    };

   */

    var getOptions = function (labels, valores) {
        return {
            title: {
                text: 'Disponibilidade de voluntários',                
                x: 'center'
            },
            tooltip: {
                trigger: 'item',
                formatter: "{a} <br/>{b} : {c} ({d}%)"
            },
            legend: {
                orient: 'vertical',
                x: 'left',
                data: labels
            },
            
            calculable: true,
            series: [
                {
                    name: 'Disponibilidade',
                    type: 'pie',
                    radius: '80%',
                    center: ['50%', '50%'],
                    data: valores
                }
            ]
        };

    };

    var labels = [];
    var valores = [];

    $scope.eventos = [{
        type: echarts.config.EVENT.CLICK,
        fn: function (param) {

            //
            $timeout(
            $location.path('/Voluntario/Disponivel/' + labels[param.dataIndex]));
        }
    }];
    

    ChartService.Read('VoluntarioDiaDaSemana')
    .then(function (result) {
        labels = [];
        valores = [];
        for (var i = 0; i < result.data[0].datapoints.length; i++) {
            labels.push(result.data[0].datapoints[i].x);
            valores.push({
                value: result.data[0].datapoints[i].y,
                name: result.data[0].datapoints[i].x
            });
        }


        $scope.options = getOptions(labels, valores);

        dialog.close();

    }, function (erros) {
        $scope.erros = erros;
    });

}])
.controller('ChartDespesaPeriodoControl', ['$scope', 'ChartService', 'ngDialog', function ($scope, ChartService, ngDialog) {
    $scope.tipo = 'ASSOCIACAO';
    $scope.start = null;
    $scope.end = null;
    //var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
    //$scope.chartData = {};
    $scope.chartConfig = {
        title: {
            text: 'Despesas no período', x: 'center'
        },
        forceClear:true,
        debug: true,
        showXAxis: true,
        showYAxis: true,
        showLegend: false,
        stack: true,
        loading: "Carregando...",
        errorMsg: "Erro",
        calculable: true,
        theme: 'infographic', //'infographic', 'macarons', 'shine', 'dark', 'blue', 'green', 'red',
        center: ['60%', '10%'],        
        legend: {
            orient: 'verticle',
            x: 'right',
            y: 'center'
        },
       
       itemStyle: {
            normal: {
                color: function(params) {
                    // build a color map as your need.
                    var colorList = [
                      '#C1232B','#B5C334','#FCCE10','#E87C25','#27727B',
                       '#FE8463','#9BCA63','#FAD860','#F3A43B','#60C0DD',
                       '#D7504B', '#C6E579', '#F4E001', '#F0805A', '#26C0C0',
                       '#D1232B', '#C5C334', '#ECCE10', '#F87C25', '#37727B',
                       '#EE8463', '#ABCA63', '#AAD860', '#A3A43B', '#70C0DD',
                       '#E7504B', '#D6E579', '#E4E001', '#E0805A', '#36C0C0'
                    ];
                    return colorList[params.dataIndex]
                },
                label: {
                    show: true,
                    position: 'top',
                    formatter: '{b}\n{c}'
                }
            }
        }
    };

    $scope.advancedOptions = {
        grid: {
            borderWidth: 0,
            y: 80,
            y2: 100
        }
    };

    var read = function () {
        if ($scope.start != null && $scope.end != null) {
            var params = { filtro: $scope.tipo, start: $scope.start, end: $scope.end }
            ChartService.Read('DespesaPorItemNoPeriodo', params)
            .then(function (result) {
                $scope.chartData = result.data;

                
                //dialog.close();

            }, function (erros) {
                $scope.erros = erros;
            });
        }
    };

    $scope.$watch('start', function (newValue, oldValue) {
        read();
    });
    $scope.$watch('end', function (newValue, oldValue) {
        read();
    });

    $scope.$on('tipo', function (event, tipo) {
        $scope.tipo = tipo;
        read();
    });

}])
.controller('ChartDoacoesPeriodoControl', ['$scope', 'ChartService', 'ngDialog', '$filter', function ($scope, ChartService, ngDialog, $filter) {
    
    $scope.start = null;
    $scope.end = null;
    
    
    var getOptions = function (labels, valores) {
        return {
            title: {
                text: 'Doações por mês',
                subtext: "Doações em Dinheiro",
                x: 'center'
            },
            tooltip: {
                trigger: 'axis'
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
                                formatter: 'R${c}'
                            }
                        }
                
                        }
                }
            ]
        };
    }

    var read = function () {
        if ($scope.start != null && $scope.end != null) {

           
            
            var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            var params = {  start: $scope.start, end: $scope.end }
            ChartService.Read('DoacoesMensaisNoPeriodo', params)
            .then(function (result) {                
                var labels = result.labels;
                var valores = result.series[0];

                for (var i = 0; i < labels.length; i++)
                    labels[i] = $filter('date')(Date.parse(labels[i]), 'MMMM-yyyy');
               

                $scope.options = getOptions(labels, valores);
                dialog.close();                

            }, function (erros) {
                $scope.erros = erros;
            });
        }
    };

    $scope.$watch('start', function (newValue, oldValue) {
        read();
    });
    $scope.$watch('end', function (newValue, oldValue) {
        read();
    });

    

}])
.controller('ChartDoacoesPorDoadorPeriodoControl', ['$scope', 'ChartService', 'ngDialog', '$filter', function ($scope, ChartService, ngDialog, $filter) {

    $scope.start = null;
    $scope.end = null;


    var getOptions = function (labels, valores) {
        return {
            title: {
                text: 'Doações por mês',
                subtext: "Doações em Dinheiro",
                x: 'center'
            },
            tooltip: {
                trigger: 'axis'
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
                                formatter: 'R${c}'
                            }
                        }

                    }
                }
            ]
        };
    }

    var read = function () {
        if ($scope.start != null && $scope.end != null) {



            var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            var params = { start: $scope.start, end: $scope.end, id: $scope.doador.id };
            ChartService.Read('DoacoesMensaisPorDoadorNoPeriodo', params)
            .then(function (result) {
                var labels = result.labels;
                var valores = result.series[0];

                for (var i = 0; i < labels.length; i++)
                    labels[i] = $filter('date')(Date.parse(labels[i]), 'MMMM-yyyy');


                $scope.options = getOptions(labels, valores);
                dialog.close();

            }, function (erros) {
                $scope.erros = erros;
            });
        }
    };

    $scope.$watch('start', function (newValue, oldValue) {
        read();
    });
    $scope.$watch('end', function (newValue, oldValue) {
        read();
    });



}])
;