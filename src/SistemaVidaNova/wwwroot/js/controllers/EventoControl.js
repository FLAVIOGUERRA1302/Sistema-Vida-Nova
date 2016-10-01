
angular.module('app')
.controller('EventoControl', ['$scope', 'EventoService', 'eventos', 'loadingDialod', function ($scope, EventoService, eventos, loadingDialod) {
    
    var itensPorPagina = 10;
    $scope.eventos = eventos;
    for (var i = 0; i < $scope.eventos.length; i++) {
        if (!($scope.eventos[i].start instanceof Date))
            $scope.eventos[i].start = new Date($scope.eventos[i].start);

        if (!($scope.eventos[i].end instanceof Date))
            $scope.eventos[i].end = new Date($scope.eventos[i].end);
    }
    loadingDialod.close();

    $scope.totalItems = EventoService.totalItems;
    $scope.currentPage = 1;

    $scope.pageChanged = function () {
        EventoService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
        .then(function (eventos) {
            $scope.eventos = eventos;
            for (var i = 0; i < $scope.eventos.length; i++) {
                if (!($scope.eventos[i].start instanceof Date))
                    $scope.eventos[i].start = new Date($scope.eventos[i].start);

                if (!($scope.eventos[i].end instanceof Date))
                    $scope.eventos[i].end = new Date($scope.eventos[i].end);
            }
            $scope.totalItems = EventoService.totalItems;
        }, function (erros) {

        });
    };

    $scope.pesquisar = function () {
        $scope.pageChanged();
    }

    $scope.delete = function (evento) {
        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este evento?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {
            EventoService.Delete(evento)
            .then(function () {
                //remove da lista
                var index = $scope.eventos.indexOf(evento);
                $scope.eventos.splice(index, 1);
            }, function () {

            });
        }, function () {
            //não faz nada
        });
    }


}])
.controller('EventoUpdateControl', ['$scope', 'EventoService', 'evento', 'loadingDialod', '$location', 'VoluntarioService', 'InteressadoService', function ($scope, EventoService, evento, loadingDialod, $location, VoluntarioService, InteressadoService) {
    loadingDialod.close();
    $scope.evento = evento;//angular.copy(evento);
    if ($scope.evento.voluntarios == null)
        $scope.evento.voluntarios = [];
    if ($scope.evento.interessados == null)
        $scope.evento.interessados = [];
    $scope.random = new Date().getTime();


    if(!($scope.evento.start instanceof Date))
        $scope.evento.start = new Date($scope.evento.start);

    if(!($scope.evento.end instanceof Date))
        $scope.evento.end = new Date($scope.evento.end);


    $scope.options = {
        required: true,
        format: 'hex',
        hue: true,
        alpha: false,
        swatchOnly: true,
        case: 'upper',
        disabled: false
    }

    $scope.salvar = function () {
        EventoService.Update($scope.evento)
            .then(function (evento) {
                $location.path('/Evento');
            }, function (erros) {
                //exibir erros
            });
    }

    $scope.getVoluntarios = function (val) {
        return VoluntarioService.Read(null, 0, 10, val)//id,skip,take,filtro
        .then(function (voluntarios) {
            return voluntarios;
        }, function (erros) {

        });
    };

    $scope.getInteressados = function (val) {
        return InteressadoService.Read(null, 0, 10, val)//id,skip,take,filtro
        .then(function (interessados) {
            return interessados;
        }, function (erros) {

        });
    };

    $scope.addVoluntario = function ($item, $model, $label, $event) {
        var v = $scope.voluntarioSelected;
        $scope.voluntarioSelected = null;
        for (var i = 0; i < $scope.evento.voluntarios.length; i++) {
            if ($scope.evento.voluntarios[i].id == $item.id)
                return;
        }
        $scope.evento.voluntarios.push(v);
    }

    $scope.addInteressado = function ($item, $model, $label, $event) {
        var v = $scope.interessadoSelected;
        $scope.interessadoSelected = null;
        for (var i = 0; i < $scope.evento.interessados.length; i++) {
            if ($scope.evento.interessados[i].id == $item.id)
                return;
        }
        $scope.evento.interessados.push(v);
    }

    $scope.removerVoluntario = function (voluntario) {
        var index = $scope.evento.voluntarios.indexOf(voluntario);
        $scope.evento.voluntarios.splice(index, 1);
    }

    $scope.removerInteressado = function (interessado) {
        var index = $scope.evento.interessados.indexOf(interessado);
        $scope.evento.interessados.splice(index, 1);
    }



}])
.controller('EventoCreateControl', ['$scope', 'EventoService', '$location', function ($scope, EventoService, $location) {    

    $scope.evento = { valorDeEntrada: 0, valorArrecadado: 0, color: '#1A82C7', textColor: '#FFFFFF' }; // valores pad~rao   

    $scope.options = {
        required: true,
        format: 'hex',
        hue: true,
        alpha: false,
        swatchOnly: true,
        case: 'upper',
        disabled: false
    }

    $scope.salvar = function () {
        EventoService.Create($scope.evento)
            .then(function (evento) {
                $location.path('/Evento/Editar/'+evento .id);

            }, function (erros) {

            });
    }



}]);