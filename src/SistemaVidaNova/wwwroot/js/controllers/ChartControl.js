angular.module('app')
.controller('ChartVoluntarioDiaDaSemanaControl', ['$scope', 'ChartService', 'ngDialog', function ($scope, ChartService, ngDialog) {
    
    var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default', });
    //$scope.chartData = {};
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
        calculable: true,
        theme: 'infographic' //'infographic', 'macarons', 'shine', 'dark', 'blue', 'green', 'red'
    };

   

    ChartService.Read('VoluntarioDiaDaSemana')
    .then(function (result) {        
        $scope.chartData = result.data;

        dialog.close();

    }, function (erros) {
        $scope.erros = erros;
    });

}])
;