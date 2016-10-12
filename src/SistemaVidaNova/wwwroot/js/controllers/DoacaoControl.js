angular.module('app')
.controller('DoacaoControl', ['$scope', 'loadingDialod', 'ngDialog', 'DoacaoDinheiroService', 'DoacaoSopaService', 'DoacaoObjetoService', 'doacoes',
    function ($scope, loadingDialod, ngDialog, DoacaoDinheiroService, DoacaoSopaService, DoacaoObjetoService, doacoes) {
    loadingDialod.close();
    var itensPorPagina = 10;
    $scope.unidadesDeMedida = unidadesDeMedida;
    $scope.doacoes = doacoes;
        //as datas chegaram em fomarto texto ISO
    var fixDatas = function () {
        for (var i = 0; i < $scope.doacoes.length; i++) {
            if (!($scope.doacoes[i].dataDaDoacao instanceof Date))
                $scope.doacoes[i].dataDaDoacao = Date.parse($scope.doacoes[i].dataDaDoacao);
            if($scope.doacoes[i].dataDeRetirada && !($scope.doacoes[i].dataDeRetirada instanceof Date))
                $scope.doacoes[i].dataDeRetirada = Date.parse($scope.doacoes[i].dataDeRetirada);
        }
    }

    fixDatas();

    $scope.destinos = destinos;
    $scope.unidadesDeMedida = unidadesDeMedida;
    
    $scope.tipo = 'DINHEIRO';

    $scope.totalItems = DoacaoDinheiroService.totalItems;
    $scope.currentPage = 1;
    

    $scope.pageChanged = function () {
        var dialog =ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
        var DoacaoService = {};
        switch ($scope.tipo) {
            case "DINHEIRO":
                DoacaoService = DoacaoDinheiroService;
                break;
            case "SOPA":
                DoacaoService = DoacaoSopaService;
                break;
            case "OBJETO":
                DoacaoService = DoacaoObjetoService;
                break;
        }

        DoacaoService.Read(null, ($scope.currentPage - 1) * itensPorPagina, itensPorPagina, $scope.valorPesquisa)//id,skip,take,filtro
            .then(function (doacoes) {
                $scope.doacoes = doacoes;
                $scope.totalItems = DoacaoService.totalItems;
                fixDatas();
                dialog.close();
            }, function (erros) {
                dialog.close();
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

    $scope.delete = function (doacao) {
        var DoacaoService = {};
        switch ($scope.tipo) {
            case "DINHEIRO":
                DoacaoService = DoacaoDinheiroService;
                break;
            case "SOPA":
                DoacaoService = DoacaoSopaService;
                break;
            case "OBJETO":
                DoacaoService = DoacaoObjetoService;
                break;
        }

        ngDialog.openConfirm({
            template: '\
                <p>Tem certeza que quer apagar este doação?</p>\
                <div class="ngdialog-buttons">\
                    <button type="button" class="ngdialog-button ngdialog-button-secondary" ng-click="closeThisDialog(0)">Não</button>\
                    <button type="button" class="ngdialog-button ngdialog-button-primary" ng-click="confirm(1)">Sim</button>\
                </div>',
            plain: true
        }).then(function () {

            DoacaoService.Delete(doacao)
            .then(function () {
                //remove da lista
                var index = $scope.doacoes.indexOf(doacao);
                $scope.doacoes.splice(index, 1);
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

.controller('DoacaoCreateDinheiroControl', ['$scope', 'DoacaoDinheiroService', 'DoadorService', '$location',
        function ($scope, DoacaoDinheiroService,DoadorService, $location) {
            $scope.doacao = {};
            $scope.tipo = 'PF'; //o doador começa como pessoa fisica
        
        $scope.salvar = function () {
            DoacaoDinheiroService.Create($scope.doacao)
                .then(function (doacao) {
                    $location.path('/Doacao')

                }, function (erros) {
                    $scope.erros = erros;
                });
        }

   
        

        $scope.getDoadores = function (val) {
            return DoadorService.Read(null, 0, 10,$scope.tipo, val);//id,skip, take, filtro

        };

    }])
.controller('DoacaoUpdateDinheiroControl', ['$scope', 'DoacaoDinheiroService', 'DoadorService', '$location', 'loadingDialod', 'ngDialog', 'doacao',
        function ($scope, DoacaoDinheiroService,DoadorService, $location,loadingDialod, ngDialog,doacao) {
            loadingDialod.close();
            doacao.dataDaDoacao = Date.parse(doacao.dataDaDoacao);
            $scope.doacao = doacao;

            $scope.tipo = doacao.doador.tipo; 
        
            $scope.salvar = function () {
                var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                DoacaoDinheiroService.Update($scope.doacao)
                .then(function (doacao) {
                    dialog.close();
                    $location.path('/Doacao')

                }, function (erros) {
                    dialog.close();
                    $scope.erros = erros;
                });
        }

   
        

        $scope.getDoadores = function (val) {
            return DoadorService.Read(null, 0, 10,$scope.tipo, val);//id,skip, take, filtro

        };

        }])

.controller('DoacaoCreateSopaControl', ['$scope', 'DoacaoSopaService', 'DoadorService','ItemService', '$location',
        function ($scope, DoacaoSopaService, DoadorService, ItemService,$location) {
            $scope.doacao = {};
            $scope.tipo = 'PF'; //o doador começa como pessoa fisica

            $scope.salvar = function () {
                DoacaoSopaService.Create($scope.doacao)
                    .then(function (doacao) {
                        $location.path('/Doacao')

                    }, function (erros) {
                        $scope.erros = erros;
                    });
            }




            $scope.getDoadores = function (val) {
                return DoadorService.Read(null, 0, 10, $scope.tipo, val);//id,skip, take, tipo, filtro

            };

            $scope.getItens= function (val) {
                return ItemService.Read(null, 0, 10, val,'SOPA');//id,skip, take, filtro,destino

            };

        }])
.controller('DoacaoUpdateSopaControl', ['$scope', 'DoacaoSopaService', 'DoadorService', '$location', 'loadingDialod', 'ngDialog', 'doacao','ItemService',
        function ($scope, DoacaoSopaService, DoadorService, $location, loadingDialod, ngDialog, doacao, ItemService) {
            loadingDialod.close();
            doacao.dataDaDoacao = Date.parse(doacao.dataDaDoacao);
            $scope.doacao = doacao;

            $scope.tipo = doacao.doador.tipo;

            $scope.salvar = function () {
                var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                DoacaoSopaService.Update($scope.doacao)
                .then(function (doacao) {
                    dialog.close();
                    $location.path('/Doacao')

                }, function (erros) {
                    dialog.close();
                    $scope.erros = erros;
                });
            }




            $scope.getDoadores = function (val) {
                return DoadorService.Read(null, 0, 10, $scope.tipo, val);//id,skip, take, filtro

            };

            $scope.getItens = function (val) {
                return ItemService.Read(null, 0, 10, val, 'SOPA');//id,skip, take, filtro,destino

            };

        }])


.controller('DoacaoCreateObjetoControl', ['$scope', 'DoacaoObjetoService', 'DoadorService', '$location','CepService',
        function ($scope, DoacaoObjetoService, DoadorService, $location, CepService) {
            $scope.doacao = { endereco:{}};
            $scope.tipo = 'PF'; //o doador começa como pessoa fisica
            $scope.ufs = ufs;
            $scope.salvar = function () {
                DoacaoObjetoService.Create($scope.doacao)
                    .then(function (doacao) {
                        $location.path('/Doacao')

                    }, function (erros) {
                        $scope.erros = erros;
                    });
            }




            $scope.getDoadores = function (val) {
                return DoadorService.Read(null, 0, 10, $scope.tipo, val);//id,skip, take, filtro

            };

            //quando alterar o doador busca o doador novamente com o endereço
            $scope.$watch('doacao.doador', function (newValue, oldValue) {
                if (newValue != oldValue && $scope.doacao.doador) {
                    DoadorService.Read($scope.doacao.doador.id)
                    .then(function (doador) {                        
                        $scope.doacao.endereco = {
                            estado: doador.endereco.estado,
                            cidade: doador.endereco.cidade,
                            bairro: doador.endereco.bairro,
                            cep: doador.endereco.cep,
                            logradouro: doador.endereco.logradouro,
                            numero: doador.endereco.numero,
                            complemento: doador.endereco.complemento,
                        }
                    }, function (erros) {                       
                    });

                }
                    
            });


            $scope.buscaCep = function () {
                if ($scope.doacao.endereco.cep) {
                    CepService.Pesquisa($scope.doacao.endereco.cep)
                    .then(function (endereco) {

                        if (endereco.estado)
                            $scope.doacao.endereco.estado = endereco.estado;
                        if (endereco.bairro)
                            $scope.doacao.endereco.bairro = endereco.bairro;
                        if (endereco.cidade)
                            $scope.doacao.endereco.cidade = endereco.cidade;
                        if (endereco.logradouro)
                            $scope.doacao.endereco.logradouro = endereco.logradouro;




                        $scope.msgErro = "";
                    }, function (erros) {
                        $scope.msgErro = "CEP não localizado";
                    });
                }
            }

        }])
.controller('DoacaoUpdateObjetoControl', ['$scope', 'DoacaoObjetoService', 'DoadorService', '$location', 'loadingDialod', 'ngDialog', 'doacao',
        function ($scope, DoacaoObjetoService, DoadorService, $location, loadingDialod, ngDialog, doacao) {
            loadingDialod.close();
            doacao.dataDaDoacao = Date.parse(doacao.dataDaDoacao);
            doacao.dataDeRetirada =new Date(doacao.dataDeRetirada);
            $scope.doacao = doacao;
            $scope.ufs = ufs;
            $scope.tipo = doacao.doador.tipo;

            $scope.salvar = function () {
                var dialog = ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                DoacaoObjetoService.Update($scope.doacao)
                .then(function (doacao) {
                    dialog.close();
                    $location.path('/Doacao')

                }, function (erros) {
                    dialog.close();
                    $scope.erros = erros;
                });
            }




            $scope.getDoadores = function (val) {
                return DoadorService.Read(null, 0, 10, $scope.tipo, val);//id,skip, take, filtro

            };
            //quando alterar o doador busca o doador novamente com o endereço
            $scope.$watch('doacao.doador', function (newValue, oldValue) {
                if (newValue != oldValue && $scope.doacao.doador) {
                    DoadorService.Read($scope.doacao.doador.id)
                    .then(function (doador) {
                        $scope.doacao.endereco = {
                            estado: doador.endereco.estado,
                            cidade: doador.endereco.cidade,
                            bairro: doador.endereco.bairro,
                            cep: doador.endereco.cep,
                            logradouro: doador.endereco.logradouro,
                            numero: doador.endereco.numero,
                            complemento: doador.endereco.complemento,
                        }
                    }, function (erros) {
                    });

                }

            });


            $scope.buscaCep = function () {
                if ($scope.doacao.endereco.cep) {
                    CepService.Pesquisa($scope.doacao.endereco.cep)
                    .then(function (endereco) {

                        if (endereco.estado)
                            $scope.doacao.endereco.estado = endereco.estado;
                        if (endereco.bairro)
                            $scope.doacao.endereco.bairro = endereco.bairro;
                        if (endereco.cidade)
                            $scope.doacao.endereco.cidade = endereco.cidade;
                        if (endereco.logradouro)
                            $scope.doacao.endereco.logradouro = endereco.logradouro;




                        $scope.msgErro = "";
                    }, function (erros) {
                        $scope.msgErro = "CEP não localizado";
                    });
                }
            }

        }])

