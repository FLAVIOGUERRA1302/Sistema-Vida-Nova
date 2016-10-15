angular.module('app')
.controller('ResultadoSopaControl', ['$scope', 'loadingDialod', 'ngDialog', 'ResultadoSopaService', 'resultados',
    function ($scope, loadingDialod, ngDialog, ResultadoSopaService, resultados) {
        loadingDialod.close();
        var itensPorPagina = 10;
        $scope.unidadesDeMedida = unidadesDeMedida;
        $scope.resultados = resultados;

        

        $scope.totalItems = ResultadoSopaService.totalItems;
        $scope.currentPage = 1;


        $scope.pageChanged = function () {
            var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });

            ResultadoSopaService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
                .then(function (resultados) {
                    $scope.resultados = resultados;
                    $scope.totalItems = ResultadoSopaService.totalItems;
                    dialog.close();
                }, function (erros) {
                    dialog.close();
                });

        };

        $scope.pesquisar = function () {
            $scope.pageChanged();
        }


        $scope.delete = function (resultado) {

            ngDialog.openConfirm({
                template: '\
                <p>Tem certeza que quer apagar este resultado?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
                plain: true
            }).then(function () {

                ResultadoSopaService.Delete(resultado)
                .then(function () {
                    //remove da lista
                    var index = $scope.resultados.indexOf(resultado);
                    $scope.resultados.splice(index, 1);
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

.controller('ResultadoSopaCreateControl', ['$scope', 'ResultadoSopaService','ModeloDeReceitaService', 'ItemService', '$location', 'ngDialog',
        function ($scope, ResultadoSopaService,ModeloDeReceitaService, ItemService, $location, ngDialog) {
            $scope.resultado = {itens:[]};


            $scope.salvar = function () {
                var dialog = ngDialog.open({ template: '/templates/salvando.html', className: 'ngdialog-theme-default' });
                ResultadoSopaService.Create($scope.resultado)
                    .then(function (resultado) {
                        dialog.close();
                        $location.path('/ResultadoSopa')
                    }, function (erros) {
                        $scope.erros = erros;
                        dialog.close();
                    });
            }

            $scope.getModelos = function (val) {
                return ModeloDeReceitaService.Read(null, 0, 10, val);//id,skip, take, filtro

            };

            $scope.getItens = function (val) {
                return ItemService.Read(null, 0, 10, val, 'SOPA');//id,skip, take, filtro,destino

            };

            $scope.addItem = function () {
                if ($scope.itemSelected) {
                    for (var i = 0; i < $scope.resultado.itens.length; i++) {
                        if ($scope.itemSelected.id == $scope.resultado.itens[i].item.id) {
                            $scope.itemSelected = null;
                            return;
                        }
                    }
                    $scope.resultado.itens.push({ item: $scope.itemSelected, quantidade: 0 });
                    $scope.itemSelected = null;
                }
            }

            $scope.removerItem = function (item) {
                var index = $scope.resultado.itens.indexOf(item);
                $scope.resultado.itens.splice(index, 1);
            }

            //quando alterar o modelo busca todos os itens do smodelo e adiciona
            $scope.$watch('resultado.modeloDeReceita', function (newValue, oldValue) {
                if (newValue != oldValue && $scope.resultado.modeloDeReceita) {
                    ModeloDeReceitaService.Read($scope.resultado.modeloDeReceita.id)
                    .then(function (modelo) {
                        //não adiciona repetido
                        for (var i = 0; i < modelo.itens.length; i++) {
                            var repetido = false;
                            for (var j = 0; j < $scope.resultado.itens.length; j++) {
                                repetido = (modelo.itens[i].item.id == $scope.resultado.itens[j].item.id) || repetido;                                    
                            }
                            if (!repetido)
                                $scope.resultado.itens.push({ item: modelo.itens[i].item, quantidade: modelo.itens[i].quantidade });
                        }
                        
                    }, function (erros) {
                    });

                }

            });


            

        }])
.controller('ResultadoSopaUpdateControl', ['$scope', 'ResultadoSopaService','ModeloDeReceitaService', 'ItemService', '$location', 'loadingDialod', 'ngDialog', 'resultado',
        function ($scope, ResultadoSopaService,ModeloDeReceitaService, ItemService, $location, loadingDialod, ngDialog, resultado) {
            loadingDialod.close();
            $scope.unidadesDeMedida = unidadesDeMedida;
            resultado.data = Date.parse(resultado.data);
            $scope.resultado = resultado;

            

            $scope.salvar = function () {
                var dialog = ngDialog.open({ template: '/templates/salvando.html', className: 'ngdialog-theme-default' });
                ResultadoSopaService.Update($scope.resultado)
                .then(function (resultado) {
                    dialog.close();
                    $location.path('/ResultadoSopa')

                }, function (erros) {
                    dialog.close();
                    $scope.erros = erros;
                });
            }

            $scope.getModelos = function (val) {
                return ModeloDeReceitaService.Read(null, 0, 10, val);//id,skip, take, filtro,destino

            };


            $scope.getItens = function (val) {
                return ItemService.Read(null, 0, 10, val, 'SOPA');//id,skip, take, filtro,destino

            };

            $scope.addItem = function () {
                if ($scope.itemSelected) {
                    for (var i = 0; i < $scope.resultado.itens.length; i++) {
                        if ($scope.itemSelected.id == $scope.resultado.itens[i].item.id) {
                            $scope.itemSelected = null;
                            return;
                        }
                    }
                    $scope.resultado.itens.push({ item: $scope.itemSelected, quantidade: 0 });
                    $scope.itemSelected = null;
                }
            }

            $scope.removerItem = function (item) {
                var index = $scope.resultado.itens.indexOf(item);
                $scope.resultado.itens.splice(index, 1);
            }

            //quando alterar o modelo busca todos os itens do smodelo e adiciona
            $scope.$watch('resultado.modeloDeReceita', function (newValue, oldValue) {
                if (newValue != oldValue && $scope.resultado.modeloDeReceita) {
                    ModeloDeReceitaService.Read($scope.resultado.modeloDeReceita.id)
                    .then(function (modelo) {
                        //não adiciona repetido
                        for (var i = 0; i < modelo.itens.length; i++) {
                            var repetido = false;
                            for (var j = 0; j < $scope.resultado.itens.length; j++) {
                                repetido = (modelo.itens[i].item.id == $scope.resultado.itens[j].item.id) || repetido;
                            }
                            if (!repetido)
                                $scope.resultado.itens.push({ item: modelo.itens[i].item, quantidade: modelo.itens[i].quantidade });
                        }

                    }, function (erros) {
                    });

                }

            });

        }]);

