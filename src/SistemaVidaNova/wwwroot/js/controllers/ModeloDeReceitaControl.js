angular.module('app')
.controller('ModeloDeReceitaControl', ['$scope', 'loadingDialod', 'ngDialog', 'ModeloDeReceitaService', 'modelos',
    function ($scope, loadingDialod, ngDialog, ModeloDeReceitaService, modelos) {
        loadingDialod.close();
        var itensPorPagina = 10;
        $scope.unidadesDeMedida = unidadesDeMedida;
        $scope.modelos = modelos;

        

        $scope.totalItems = ModeloDeReceitaService.totalItems;
        $scope.currentPage = 1;


        $scope.pageChanged = function () {
            var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });

            ModeloDeReceitaService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
                .then(function (modelos) {
                    $scope.modelos = modelos;
                    $scope.totalItems = ModeloDeReceitaService.totalItems;
                    dialog.close();
                }, function (erros) {
                    dialog.close();
                });

        };

        $scope.pesquisar = function () {
            $scope.pageChanged();
        }


        $scope.delete = function (modelo) {

            ngDialog.openConfirm({
                template: '\
                <p>Tem certeza que quer apagar este modelo?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
                plain: true
            }).then(function () {

                ModeloDeReceitaService.Delete(modelo)
                .then(function () {
                    //remove da lista
                    var index = $scope.modelos.indexOf(modelo);
                    $scope.modelos.splice(index, 1);
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




    }])

.controller('ModeloDeReceitaCreateControl', ['$scope', 'ModeloDeReceitaService', 'ItemService', '$location', 'ngDialog',
        function ($scope, ModeloDeReceitaService, ItemService, $location, ngDialog) {
            $scope.modelo = {itens:[]};


            $scope.salvar = function () {
                var dialog = ngDialog.open({ template: '/templates/salvando.html', className: 'ngdialog-theme-default' });
                ModeloDeReceitaService.Create($scope.modelo)
                    .then(function (modelo) {
                        dialog.close();
                        $location.path('/ModeloDeReceita')
                    }, function (erros) {
                        $scope.erros = erros;
                        dialog.close();
                    });
            }



            $scope.getItens = function (val) {
                return ItemService.Read(null, 0, 10, val, 'SOPA');//id,skip, take, filtro,destino

            };

            $scope.addItem = function () {
                if ($scope.itemSelected) {
                    for (var i = 0; i < $scope.modelo.itens.length; i++) {
                        if ($scope.itemSelected.id == $scope.modelo.itens[i].item.id) {
                            $scope.itemSelected = null;
                            return;
                        }
                    }
                    $scope.modelo.itens.push({ item: $scope.itemSelected, quantidade: 0 });
                    $scope.itemSelected = null;
                }
            }

            $scope.removerItem = function (item) {
                var index = $scope.modelo.itens.indexOf(item);
                $scope.modelo.itens.splice(index, 1);
            }


            

        }])
.controller('ModeloDeReceitaUpdateControl', ['$scope', 'ModeloDeReceitaService', 'ItemService', '$location', 'loadingDialod', 'ngDialog', 'modelo',
        function ($scope, ModeloDeReceitaService, ItemService, $location, loadingDialod, ngDialog, modelo) {
            loadingDialod.close();
            $scope.unidadesDeMedida = unidadesDeMedida;
            $scope.modelo = modelo;

            

            $scope.salvar = function () {
                var dialog = ngDialog.open({ template: '/templates/salvando.html', className: 'ngdialog-theme-default' });
                ModeloDeReceitaService.Update($scope.modelo)
                .then(function (modelo) {
                    dialog.close();
                    $location.path('/ModeloDeReceita')

                }, function (erros) {
                    dialog.close();
                    $scope.erros = erros;
                });
            }



            $scope.getItens = function (val) {
                return ItemService.Read(null, 0, 10, val, 'SOPA');//id,skip, take, filtro,destino

            };

            $scope.addItem = function () {
                if ($scope.itemSelected) {
                    for (var i = 0; i < $scope.modelo.itens.length; i++) {
                        if ($scope.itemSelected.id == $scope.modelo.itens[i].item.id) {
                            $scope.itemSelected = null;
                            return;
                        }
                    }
                    $scope.modelo.itens.push({ item: $scope.itemSelected, quantidade: 0 });
                    $scope.itemSelected = null;
                }
            }

            $scope.removerItem = function (item) {
                var index = $scope.modelo.itens.indexOf(item);
                $scope.modelo.itens.splice(index, 1);
            }

            

        }]);

