var app = angular.module('app', [
    'ngRoute',
    'ngAnimate',
    'ngFileUpload',
    'ngDialog',
    'ui.bootstrap',
    'color.picker',    
    'ui.utils.masks',    
    'idf.br-filters',
    'ngTagsInput',    
    'textAngular',
    'angular-echarts',
    'angular-echarts.theme',
    'validation.match'
    

]).config(function ($routeProvider, $locationProvider) {

    $routeProvider

        //inicio
        .when('/', {
            templateUrl: '/templates/Dashboard.html'
        })
        //------------Usuario----------
    .when('/Usuario', {
        templateUrl: '/templates/Usuario/List.html',
        controller: 'UsuarioControl',
        resolve: {

            usuarios: function (AccountService) {
                return AccountService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Usuario/Criar', {
            templateUrl: '/templates/Usuario/Create.html',
            controller: 'UsuarioCreateControl'

        })
            .when('/Usuario/Editar/:id', {
                templateUrl: '/templates/Usuario/Update.html',
                controller: 'UsuarioUpdateControl',
                resolve: {
                    usuario: function (AccountService, $route) {
                        return AccountService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Usuario/Visualizar/:id', {
            templateUrl: '/templates/Usuario/Detalhe.html',
            controller: 'UsuarioUpdateControl',
            resolve: {
                usuario: function (AccountService, $route) {
                    return AccountService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })



      //------------Voluntario----------
    .when('/Voluntario', {
        templateUrl: '/templates/Voluntario/List.html',
        controller: 'VoluntarioControl',
        resolve: {

            voluntarios: function (VoluntarioService) {
                return VoluntarioService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }

        }
    })
    .when('/Voluntario/Criar', {
        templateUrl: '/templates/Voluntario/Create.html',
        controller: 'VoluntarioCreateControl'

    })
        .when('/Voluntario/Editar/:id', {
            templateUrl: '/templates/Voluntario/Update.html',
            controller: 'VoluntarioUpdateControl',
            resolve: {
                voluntario: function (VoluntarioService, $route) {
                    return VoluntarioService.Read($route.current.params.id);
                }
                ,
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        .when('/Voluntario/Curso/:id', {
            templateUrl: '/templates/Voluntario/Curso.html',
            controller: 'VoluntarioUpdateControl',
            resolve: {
                voluntario: function (VoluntarioService, $route) {
                    return VoluntarioService.Read($route.current.params.id);
                }
                ,
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
    .when('/Voluntario/Visualizar/:id', {
        templateUrl: '/templates/Voluntario/Detalhe.html',
        controller: 'VoluntarioUpdateControl',
        resolve: {
            voluntario: function (VoluntarioService, $route) {
                return VoluntarioService.Read($route.current.params.id);
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
.when('/Voluntario/Disponivel/:diaDaSemana', {
    templateUrl: '/templates/Voluntario/Disponivel.html',
    controller: 'VoluntarioDisponivelControl',
    resolve: {
        voluntarios: function (VoluntarioService, $route) {
            
            return VoluntarioService.Read(null, 0, 10, null, $route.current.params.diaDaSemana);//id,skip,take,filtro,diaDaSemana
        },
        loadingDialod: function (ngDialog) {
            return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
        },
        diaDaSemana: function ($route) {
            return $route.current.params.diaDaSemana;
        }
    }
})


    

        //------------Interessado----------
    .when('/Interessado', {
        templateUrl: '/templates/Interessado/List.html',
        controller: 'InteressadoControl',
        resolve: {

            interessados: function (InteressadoService) {
                return InteressadoService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Interessado/Criar', {
            templateUrl: '/templates/Interessado/Create.html',
            controller: 'InteressadoCreateControl'

        })
            .when('/Interessado/Editar/:id', {
                templateUrl: '/templates/Interessado/Update.html',
                controller: 'InteressadoUpdateControl',
                resolve: {
                    interessado: function (InteressadoService, $route) {
                        return InteressadoService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Interessado/Visualizar/:id', {
            templateUrl: '/templates/Interessado/Detalhe.html',
            controller: 'InteressadoUpdateControl',
            resolve: {
                interessado: function (InteressadoService, $route) {
                    return InteressadoService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })



        //------------Evento----------
    .when('/Evento', {
        templateUrl: '/templates/Evento/List.html',
        controller: 'EventoControl',
        resolve: {

            eventos: function (EventoService) {
                return EventoService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Evento/Criar', {
            templateUrl: '/templates/Evento/Create.html',
            controller: 'EventoCreateControl'

        })
         .when('/Evento/Calendario', {
             templateUrl: '/templates/Evento/Calendario.html'

         })
            .when('/Evento/Editar/:id', {
                templateUrl: '/templates/Evento/Update.html',
                controller: 'EventoUpdateControl',
                resolve: {
                    evento: function (EventoService, $route) {
                        return EventoService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Evento/Visualizar/:id', {
            templateUrl: '/templates/Evento/Detalhe.html',
            controller: 'EventoUpdateControl',
            resolve: {
                evento: function (EventoService, $route) {
                    return EventoService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })

     //------------Doador----------
    .when('/Doador', {
        templateUrl: '/templates/Doador/List.html',
        controller: 'DoadorControl',
        resolve: {

            doadores: function (DoadorService) {
                return DoadorService.Read(null, 0, 10, 'PF');//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Doador/Criar/PF', {
            templateUrl: '/templates/Doador/CreatePf.html',
            controller: 'DoadorCreateControl',
            resolve: {
                doador: function () {
                    return { tipo: "PF" };
                }
            }

        })
        .when('/Doador/Criar/PJ', {
            templateUrl: '/templates/Doador/CreatePj.html',
            controller: 'DoadorCreateControl',
            resolve: {
                doador: function () {
                    return { tipo: "PJ" };
                }
            }

        })
            .when('/Doador/Editar/:id', {
                templateUrl: '/templates/Doador/Update.html',
                controller: 'DoadorUpdateControl',
                resolve: {
                    doador: function (DoadorService, $route) {
                        return DoadorService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Doador/Visualizar/:id', {
            templateUrl: '/templates/Doador/Detalhe.html',
            controller: 'DoadorUpdateControl',
            resolve: {
                doador: function (DoadorService, $route) {
                    return DoadorService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
     .when('/Doador/RelatorioPorEmail/:id', {
         templateUrl: '/templates/Doador/RelatorioDoacoesEmail.html',
         controller: 'DoadorRelatorioControl',
         resolve: {
             doador: function (DoadorService, $route) {
                 return DoadorService.Read($route.current.params.id);
             },
             loadingDialod: function (ngDialog) {
                 return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
             }
         }
     })


    

     //------------Favorecido----------
    .when('/Favorecido', {
        templateUrl: '/templates/Favorecido/List.html',
        controller: 'FavorecidoControl',
        resolve: {

            favorecidos: function (FavorecidoService) {
                return FavorecidoService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Favorecido/Criar', {
            templateUrl: '/templates/Favorecido/Create.html',
            controller: 'FavorecidoCreateControl'

        })
            .when('/Favorecido/Editar/:id', {
                templateUrl: '/templates/Favorecido/Update.html',
                controller: 'FavorecidoUpdateControl',
                resolve: {
                    favorecido: function (FavorecidoService, $route) {
                        return FavorecidoService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Favorecido/Visualizar/:id', {
            templateUrl: '/templates/Favorecido/Detalhe.html',
            controller: 'FavorecidoUpdateControl',
            resolve: {
                favorecido: function (FavorecidoService, $route) {
                    return FavorecidoService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })

     //------------email----------
    .when('/Informativo', {
        templateUrl: '/templates/Informativo/Create.html',
        controller: 'InformativoControl',
        resolve: {

            informativo: function (InformativoService) {
                return InformativoService.Read();
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })

     //------------Item----------
    .when('/Item', {
        templateUrl: '/templates/Item/List.html',
        controller: 'ItemControl',
        resolve: {

            itens: function (ItemService) {
                return ItemService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Item/Criar', {
            templateUrl: '/templates/Item/Create.html',
            controller: 'ItemCreateControl'

        })
            .when('/Item/Editar/:id', {
                templateUrl: '/templates/Item/Update.html',
                controller: 'ItemUpdateControl',
                resolve: {
                    item: function (ItemService, $route) {
                        return ItemService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Item/Visualizar/:id', {
            templateUrl: '/templates/Item/Detalhe.html',
            controller: 'ItemUpdateControl',
            resolve: {
                item: function (ItemService, $route) {
                    return ItemService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })


    //------------Despesa----------
    .when('/Despesa', {
        templateUrl: '/templates/Despesa/List.html',
        controller: 'DespesaControl',
        resolve: {

            despesas: function (DespesaService) {
                return DespesaService.Read(null, 0, 10, 'ASSOCIACAO');//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Despesa/Associacao/Criar', {
            templateUrl: '/templates/Despesa/Create.html',
            controller: 'DespesaCreateControl',
            resolve: {
                despesa: function () {
                    return { tipo: "ASSOCIACAO" };
                }
            }

        })
        .when('/Despesa/Sopa/Criar', {
            templateUrl: '/templates/Despesa/Create.html',
            controller: 'DespesaCreateControl',
            resolve: {
                despesa: function () {
                    return { tipo: "SOPA" };
                }
            }

        })
        .when('/Despesa/Favorecido/Criar', {
            templateUrl: '/templates/Despesa/Create.html',
            controller: 'DespesaCreateControl',
            resolve: {
                despesa: function () {
                    return { tipo: "FAVORECIDO" };
                }
            }

        })
            .when('/Despesa/Editar/:id', {
                templateUrl: '/templates/Despesa/Update.html',
                controller: 'DespesaUpdateControl',
                resolve: {
                    despesa: function (DespesaService, $route) {
                        return DespesaService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Despesa/Visualizar/:id', {
            templateUrl: '/templates/Despesa/Detalhe.html',
            controller: 'DespesaUpdateControl',
            resolve: {
                despesa: function (DespesaService, $route) {
                    return DespesaService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })

        .when('/Despesa/RelatorioAssociacao/:start/:end', {
            templateUrl: '/templates/Despesa/RelatorioAssociacao.html',
            controller: 'DespesaRelatorioControl',
            resolve: {
                dados: function (DespesaService, $route) {
                    return DespesaService.Relatorio($route.current.params.start, $route.current.params.end, 'ASSOCIACAO');
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        .when('/Despesa/RelatorioFavorecido/:start/:end', {
            templateUrl: '/templates/Despesa/RelatorioFavorecido.html',
            controller: 'DespesaRelatorioControl',
            resolve: {
                dados: function (DespesaService, $route) {
                    return DespesaService.Relatorio($route.current.params.start, $route.current.params.end, 'FAVORECIDO');
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        .when('/Despesa/RelatorioSopa/:start/:end', {
            templateUrl: '/templates/Despesa/RelatorioSopa.html',
            controller: 'DespesaRelatorioControl',
            resolve: {
                dados: function (DespesaService, $route) {
                    return DespesaService.Relatorio($route.current.params.start, $route.current.params.end, 'SOPA');
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })



    //------------Doacao----------
    .when('/Doacao', {
        templateUrl: '/templates/Doacao/List.html',
        controller: 'DoacaoControl',
        resolve: {

            doacoes: function (DoacaoDinheiroService) {
                return DoacaoDinheiroService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })
        .when('/Doacao/Dinheiro/Criar', {
            templateUrl: '/templates/Doacao/CreateDinheiro.html',
            controller: 'DoacaoCreateDinheiroControl'

        })
        .when('/Doacao/Objeto/Criar', {
            templateUrl: '/templates/Doacao/CreateObjeto.html',
            controller: 'DoacaoCreateObjetoControl'

        })
        .when('/Doacao/Sopa/Criar', {
            templateUrl: '/templates/Doacao/CreateSopa.html',
            controller: 'DoacaoCreateSopaControl'

        })


        .when('/Doacao/Dinheiro/Editar/:id', {
            templateUrl: '/templates/Doacao/UpdateDinheiro.html',
            controller: 'DoacaoUpdateDinheiroControl',
            resolve: {
                doacao: function (DoacaoDinheiroService, $route) {
                    return DoacaoDinheiroService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        .when('/Doacao/Objeto/Editar/:id', {
            templateUrl: '/templates/Doacao/UpdateObjeto.html',
            controller: 'DoacaoUpdateObjetoControl',
            resolve: {
                doacao: function (DoacaoObjetoService, $route) {
                    return DoacaoObjetoService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        .when('/Doacao/Sopa/Editar/:id', {
            templateUrl: '/templates/Doacao/UpdateSopa.html',
            controller: 'DoacaoUpdateSopaControl',
            resolve: {
                doacao: function (DoacaoSopaService, $route) {
                    return DoacaoSopaService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })


        .when('/Doacao/Dinheiro/Visualizar/:id', {
            templateUrl: '/templates/Doacao/DetalheDinheiro.html',
            controller: 'DoacaoUpdateDinheiroControl',
            resolve: {
                doacao: function (DoacaoDinheiroService, $route) {
                    return DoacaoDinheiroService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        .when('/Doacao/Objeto/Visualizar/:id', {
            templateUrl: '/templates/Doacao/DetalheObjeto.html',
            controller: 'DoacaoUpdateObjetoControl',
            resolve: {
                doacao: function (DoacaoObjetoService, $route) {
                    return DoacaoObjetoService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        .when('/Doacao/Sopa/Visualizar/:id', {
            templateUrl: '/templates/Doacao/DetalheSopa.html',
            controller: 'DoacaoUpdateSopaControl',
            resolve: {
                doacao: function (DoacaoSopaService, $route) {
                    return DoacaoSopaService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })

        .when('/Doacao/RelatorioDinheiro/:start/:end', {
            templateUrl: '/templates/Doacao/RelatorioDinheiro.html',
            controller: 'DoacaoDinheiroRelatorioControl',
            resolve: {
                dados: function (DoacaoDinheiroService, $route) {
                    return DoacaoDinheiroService.Relatorio($route.current.params.start, $route.current.params.end);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })


     //------------ModeloDeReceita----------
    .when('/ModeloDeDistribuicao', {
        templateUrl: '/templates/ModeloDeReceita/List.html',
        controller: 'ModeloDeReceitaControl',
        resolve: {

            modelos: function (ModeloDeReceitaService) {
                return ModeloDeReceitaService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }

        }
    })
    .when('/ModeloDeReceita/Criar', {
        templateUrl: '/templates/ModeloDeReceita/Create.html',
        controller: 'ModeloDeReceitaCreateControl'

    })
        .when('/ModeloDeReceita/Editar/:id', {
            templateUrl: '/templates/ModeloDeReceita/Update.html',
            controller: 'ModeloDeReceitaUpdateControl',
            resolve: {
                modelo: function (ModeloDeReceitaService, $route) {
                    return ModeloDeReceitaService.Read($route.current.params.id);
                }
                ,
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
    .when('/ModeloDeReceita/Visualizar/:id', {
        templateUrl: '/templates/ModeloDeReceita/Detalhe.html',
        controller: 'ModeloDeReceitaUpdateControl',
        resolve: {
            modelo: function (ModeloDeReceitaService, $route) {
                return ModeloDeReceitaService.Read($route.current.params.id);
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })

    //------------ResultadoSopa----------
    .when('/ResultadoSopa', {
        templateUrl: '/templates/ResultadoSopa/List.html',
        controller: 'ResultadoSopaControl',
        resolve: {

            resultados: function (ResultadoSopaService) {
                return ResultadoSopaService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }

        }
    })
    .when('/ResultadoSopa/Criar', {
        templateUrl: '/templates/ResultadoSopa/Create.html',
        controller: 'ResultadoSopaCreateControl'

    })
        .when('/ResultadoSopa/Editar/:id', {
            templateUrl: '/templates/ResultadoSopa/Update.html',
            controller: 'ResultadoSopaUpdateControl',
            resolve: {
                resultado: function (ResultadoSopaService, $route) {
                    return ResultadoSopaService.Read($route.current.params.id);
                }
                ,
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
    .when('/ResultadoSopa/Visualizar/:id', {
        templateUrl: '/templates/ResultadoSopa/Detalhe.html',
        controller: 'ResultadoSopaUpdateControl',
        resolve: {
            resultado: function (ResultadoSopaService, $route) {
                return ResultadoSopaService.Read($route.current.params.id);
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })

    //------------Estoque----------
    .when('/Estoque', {
        templateUrl: '/templates/Estoque/List.html',
        controller: 'EstoqueControl',
        resolve: {

            itens: function (EstoqueService) {
                return EstoqueService.Read(null, 0, 10);//id,skip,take
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }
    })

            .when('/Estoque/Ajustar/:id', {
                templateUrl: '/templates/Estoque/Update.html',
                controller: 'EstoqueUpdateControl',
                resolve: {
                    item: function (EstoqueService, $route) {
                        return EstoqueService.Read($route.current.params.id);
                    },
                    loadingDialod: function (ngDialog) {
                        return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                    }
                }
            })
        .when('/Estoque/Visualizar/:id', {
            templateUrl: '/templates/Estoque/Detalhe.html',
            controller: 'EstoqueUpdateControl',
            resolve: {
                item: function (EstoqueService, $route) {
                    return EstoqueService.Read($route.current.params.id);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })
        //Planejamento
     .when('/Planejamento', {
         templateUrl: '/templates/Planejamento/Planejamento.html',
         controller: 'PlanejamentoControl'

     })

    //ResultadosGerais
     .when('/ResultadosGerais', {
         templateUrl: '/templates/ResultadosGerais/Relatorio.html',
         controller: 'ResultadosGeraisControl'

     })

    .when('/ResultadosGerais/Beneficios/:start/:end', {
        templateUrl: '/templates/ResultadosGerais/RelatorioPdfBeneficios.html',
        controller: 'ResultadosGeraisRelatorioControl',
        resolve: {
            dados: function (ResultadosGeraisService, $route) {
                return ResultadosGeraisService.Read(Date.parseExact($route.current.params.start, 'dd-MM-yyyy'),Date.parseExact($route.current.params.end, 'dd-MM-yyyy') );
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }

    })

    .when('/ResultadosGerais/Balanco/:start/:end', {
        templateUrl: '/templates/ResultadosGerais/RelatorioPdfBalanco.html',
        controller: 'ResultadosGeraisRelatorioControl',
        resolve: {
            dados: function (ResultadosGeraisService, $route) {
                return ResultadosGeraisService.Read(Date.parseExact($route.current.params.start, 'dd-MM-yyyy'), Date.parseExact($route.current.params.end, 'dd-MM-yyyy'));
            },
            loadingDialod: function (ngDialog) {
                return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
            }
        }

    })

        .when('/Doacao/RelatorioDinheiro/:start/:end', {
            templateUrl: '/templates/Doacao/RelatorioDinheiro.html',
            controller: 'DoacaoDinheiroRelatorioControl',
            resolve: {
                dados: function (DoacaoDinheiroService, $route) {
                    return DoacaoDinheiroService.Relatorio($route.current.params.start, $route.current.params.end);
                },
                loadingDialod: function (ngDialog) {
                    return ngDialog.open({ template: '/templates/loading.html', className: 'ngdialog-theme-default' });
                }
            }
        })

    //Ranking
     .when('/EventosMaisProcurados', {
         templateUrl: '/templates/Ranking/EventosMaisProcurados.html',
         controller: 'EventosMaisProcuradosControl'

     })

    .when('/MelhoresDoadores', {
        templateUrl: '/templates/Ranking/MelhoresDoadores.html',
        controller: 'MelhoresDoadoresControl'

    })

    .when('/FavorecidoMaisBeneficiados', {
        templateUrl: '/templates/Ranking/FavorecidoMaisBeneficiados.html',
        controller: 'FavorecidoMaisBeneficiadosControl'

    })


    
    ;
    $locationProvider.html5Mode(true);
});


angular.module("ngLocale", [], ["$provide", function ($provide) {
    var PLURAL_CATEGORY = { ZERO: "zero", ONE: "one", TWO: "two", FEW: "few", MANY: "many", OTHER: "other" };
    $provide.value("$locale", {
        "DATETIME_FORMATS": {
            "AMPMS": [
              "AM",
              "PM"
            ],
            "DAY": [
              "domingo",
              "segunda-feira",
              "ter\u00e7a-feira",
              "quarta-feira",
              "quinta-feira",
              "sexta-feira",
              "s\u00e1bado"
            ],
            "ERANAMES": [
              "antes de Cristo",
              "depois de Cristo"
            ],
            "ERAS": [
              "a.C.",
              "d.C."
            ],
            "FIRSTDAYOFWEEK": 6,
            "MONTH": [
              "janeiro",
              "fevereiro",
              "mar\u00e7o",
              "abril",
              "maio",
              "junho",
              "julho",
              "agosto",
              "setembro",
              "outubro",
              "novembro",
              "dezembro"
            ],
            "SHORTDAY": [
              "dom",
              "seg",
              "ter",
              "qua",
              "qui",
              "sex",
              "s\u00e1b"
            ],
            "SHORTMONTH": [
              "jan",
              "fev",
              "mar",
              "abr",
              "mai",
              "jun",
              "jul",
              "ago",
              "set",
              "out",
              "nov",
              "dez"
            ],
            "STANDALONEMONTH": [
              "janeiro",
              "fevereiro",
              "mar\u00e7o",
              "abril",
              "maio",
              "junho",
              "julho",
              "agosto",
              "setembro",
              "outubro",
              "novembro",
              "dezembro"
            ],
            "WEEKENDRANGE": [
              5,
              6
            ],
            "fullDate": "EEEE, d 'de' MMMM 'de' y",
            "longDate": "d 'de' MMMM 'de' y",
            "medium": "d 'de' MMM 'de' y HH:mm:ss",
            "mediumDate": "d 'de' MMM 'de' y",
            "mediumTime": "HH:mm:ss",
            "short": "dd/MM/yy HH:mm",
            "shortDate": "dd/MM/yy",
            "shortTime": "HH:mm"
        },
        "NUMBER_FORMATS": {
            "CURRENCY_SYM": "R$",
            "DECIMAL_SEP": ",",
            "GROUP_SEP": ".",
            "PATTERNS": [
              {
                  "gSize": 3,
                  "lgSize": 3,
                  "maxFrac": 3,
                  "minFrac": 0,
                  "minInt": 1,
                  "negPre": "-",
                  "negSuf": "",
                  "posPre": "",
                  "posSuf": ""
              },
              {
                  "gSize": 3,
                  "lgSize": 3,
                  "maxFrac": 2,
                  "minFrac": 2,
                  "minInt": 1,
                  "negPre": "-\u00a4",
                  "negSuf": "",
                  "posPre": "\u00a4",
                  "posSuf": ""
              }
            ]
        },
        "id": "pt-br",
        "localeID": "pt_BR",
        "pluralCat": function (n, opt_precision) { if (n >= 0 && n <= 2 && n !== 2) { return PLURAL_CATEGORY.ONE; } return PLURAL_CATEGORY.OTHER; }
    });
}]);
  

var ufs = {
    'AC': 'Acre',
    'AL': 'Alagoas',
    'AP': 'Amapá',
    'AM': 'Amazonas',
    'BA': 'Bahia',
    'CE': 'Ceará',
    'DF': 'Distrito Federal',
    'ES': 'Espírito Santo',
    'GO': 'Goiás',
    'MA': 'Maranhão',
    'MT': 'Mato Grosso',
    'MS': 'Mato Grosso do Sul',
    'MG': 'Minas Gerais',
    'PA': 'Pará',
    'PB': 'Paraíba',
    'PR': 'Paraná',
    'PE': 'Pernambuco',
    'PI': 'Piauí',
    'RJ': 'Rio de Janeiro',
    'RN': 'Rio Grande do Norte',
    'RS': 'Rio Grande do Sul',
    'RO': 'Rondônia',
    'RR': 'Roraima',
    'SC': 'Santa Catarina',
    'SP': 'São Paulo',
    'SE': 'Sergipe',
    'TO': 'Tocantins'
};

var unidadesDeMedida = {
    "AMPOLA": "Ampola",
    "BALDE": "Balde",
    "BANDEJ": "Bandeja",
    "BARRA": "Barra",
    "BISNAG": "Bisnaga",
    "BLOCO": "Bloco",
    "BOBINA": "Bobina",
    "BOMB": "Bombona",
    "CAPS": "Capsula",
    "CART": "Cartela",
    "CENTO": "Cento",
    "CJ": "Conjunto",
    "CM": "Centimetro",
    "CM2": "Centimetro Quadrado",
    "CX": "Caixa",
    "DISP": "Display",
    "DUZIA": "Duzia",
    "EMBAL": "Embalagem",
    "FARDO": "Fardo",
    "FOLHA": "Folha",
    "FRASCO": "Frasco",
    "GALAO": "Galão",
    "GF": "Garrafa",
    "GRAMAS": "Gramas",
    "JOGO": "Jogo",
    "KG": "Quilograma",
    "KIT": "Kit",
    "LATA": "Lata",
    "LITRO": "Litro",
    "M": "Metro",
    "M2": "Metro Quadrado",
    "M3": "Metro Cúbico",
    "MILHEI": "Milheiro",
    "ML": "Mililitro",
    "MWH": "Megawatt Hora",
    "PACOTE": "Pacote",
    "PALETE": "Palete",
    "PARES": "Pares",
    "PC": "Peça",
    "POTE": "Pote",
    "K": "Quilate",
    "RESMA": "Resma",
    "ROLO": "Rolo",
    "SACO": "Saco",
    "SACOLA": "Sacola",
    "TAMBOR": "Tambor",
    "TANQUE": "Tanque",
    "TON": "Tonelada",
    "TUBO": "Tubo",
    "UNID": "Unidade",
    "VASIL": "Vasilhame",
    "VIDRO": "Vidro"

};

var destinos = {
    "ASSOCIACAO": "Associação",
    "FAVORECIDO": "Favorecido",
    "SOPA": "Sopa"
};


Date.prototype.toISOString = function () {
    return this.toString('yyyy-MM-ddTHH:mm:ss')
};

String.prototype.capitalize = function () {
    return this.charAt(0).toUpperCase() + this.slice(1).toLocaleLowerCase();
}