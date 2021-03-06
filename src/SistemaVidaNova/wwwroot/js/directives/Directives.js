﻿var app = angular.module('app');
app.directive('calendar', function () {
    return {
        restrict: 'A',
        scope: {
            editable: '=?',
            events: '=?'
        },
        link: function (scope, element, attrs, ctrl) {
            element.fullCalendar({
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay,listMonth'
                },
                locale: 'pt-br',
                navLinks: true, // can click day/week names to navigate views
                businessHours: true, // display business hours
                editable: scope.editable,
                events: scope.events,
                eventClick: function (calEvent, jsEvent, view) {

                    if (calEvent.tipo == "DOACAO")
                        window.location.href = '/Doacao/Objeto/Visualizar/' + calEvent.id;
                    else
                        window.location.href = '/Evento/Visualizar/' + calEvent.id;

                }
            });
        }
    }
});

app.directive('listaErro', function () {
    return {
        restrict: 'E',
        scope: {
            lista: '=?'
        },
        template: '<div><p class="text-red" ng-repeat="erro in lista">{{erro}}</p></div>',
        link: function (scope, element, attrs, ctrl) {            
        }
    }
});

app.directive('goBack', ['$window',function ($window) {
    return {
        restrict: 'A',
        
        link: function (scope, element, attrs) {
            element.on('click', function () {
                $window.history.back();
            });
        }
    }
}]);


app.directive('echart', ['theme','$timeout',  function (theme,$timeout) {
    return {
        restrict: 'EA',
        //template: '<div></div>',
        scope: {
            
            options: '=?',
            tema: '=?',
            events: '=?',
            api: '='
        },
        link: function (scope, element, attrs) {
            var ndWrapper = element.find('div')[0],
            
            
            width, height, chart;

            function getSizes() {
                var parentWidth = element.width(),
                parentHeight = element.height();
                width =  parseInt(attrs.width) || parentWidth || 320;
                height = parseInt(attrs.height) || parentHeight || 240;

                ndWrapper.style.width = '100%';
                ndWrapper.style.height = height + 'px';
            }

            function aplicarOptions() {
                if (scope.options) {
                    //getSizes();
                    if (!chart) {
                        chart = echarts.init(element[0], theme.get(scope.tema || 'macarons'));
                    }
                    if(scope.api)
                        scope.api.chart = chart;

                    chart.clear();
                    if (scope.events) {
                        for (var i = 0; i < scope.events.length; i++) {
                            chart.on(scope.events[i].type, scope.events[i].fn);
                        }
                    }
                    chart.setOption(scope.options);
                    chart.resize();
                    window.onresize = chart.resize;
                    
                }
            }

            

            scope.$watch('options', function (newValue, oldValue) {
                $timeout(function () {
                    aplicarOptions();
                }, 500
               );
                
            });
            

        }


    }
}]);



app.directive('htmlToPdf', function () {
    return {
        restrict: 'A',
        scope: {

            nomeDoArquivo: '=?',
            orientacao: '=?'
          
        },
        controller: ['$element', '$scope', function ($element, $scope) {
            var ctrl = this;
            var specialElementHandlers = {
                '[ignore-to-pdf]': function (element, renderer) {
                    return true;
                }
            };
            ctrl.gerar = function () {
                var tipo = 'p';
                if ($scope.orientacao =='landscape')
                    tipo = 'l';

                html2canvas($element, {
                    onrendered: function (canvas) {
                        var imgData = canvas.toDataURL(
                            'image/png');
                        var doc = new jsPDF(tipo, 'mm');
                        doc.addImage(imgData, 'PNG', 10, 10);
                        doc.save($scope.nomeDoArquivo+'.pdf');
                    }
                });

                /*
                var pdf = new jsPDF('p', 'pt', 'letter')

                // source can be HTML-formatted string, or a reference
                // to an actual DOM element from which the text will be scraped.
                , source = $element[0]

                // we support special element handlers. Register them with jQuery-style
                // ID selector for either ID or node name. ("#iAmID", "div", "span" etc.)
                // There is no support for any other type of selectors
                // (class, of compound) at this time.
                , specialElementHandlers = {
                    // element with id of "bypass" - jQuery style selector
                    '#bypassme': function (element, renderer) {
                        // true = "handled elsewhere, bypass text extraction"
                        return true
                    }
                };

                margins = {
                    top: 80,
                    bottom: 60,
                    left: 40,
                    width: 522
                };


                pdf.fromHTML(
                    source // HTML string or DOM elem ref.
                    , margins.left // x coord
                    , margins.top // y coord
                    , {
                        'width': margins.width // max width of content on PDF
                        , 'elementHandlers': specialElementHandlers
                    },
                    function (dispose) {
                        // dispose: object with X, Y of the last line add to the PDF
                        //          this allow the insertion of new lines after html
                        pdf.save($scope.nomeDoArquivo + '.pdf');
                    },
                    margins
                  );

                  */
                //doc.save($scope.nomeDoArquivo + '.pdf');
            }

        }],        
        link: function (scope, element, attrs, ctrls) {
          
        }
    }
});

app.directive('gerarPdf',  function () {
    return {
        restrict: 'A',                
        require: ["^^htmlToPdf"],
        link: function (scope, element, attrs, ctrls) {             
            element.on('click', function () {
                ctrls[0].gerar();
            });
            
        }
    }
});

app.directive('gerarPdfAutomatico',['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        require: ["^^htmlToPdf"],
        link: function (scope, element, attrs, ctrls) {            
            $timeout(function () {
                ctrls[0].gerar()
            },1000);

        }
    }
}]);

app.directive('pdfA4', function () {
    return {
        restrict: 'A',
        scope: {

            nomeDoArquivo: '=?'

        },
        controller: ['$element', '$scope', function ($element, $scope) {
            var ctrl = this;

           
            ctrl.gerar = function () {                
                /*var element = $element.clone().width(750);
               
                
                content.css("position", "fixed");
                content.css("left", "-750px");
                var toRemove = element.find(".fora-do-pfd").detach();
                element.appendTo(content);
                content.appendTo($element);*/
                var content = $("<div>").width(750);
                var parent = $element.parent();
                content.appendTo(parent);
                $element.appendTo(content);                
                var toHide = $element.find(".fora-do-pfd")
                toHide.hide();
                html2canvas($element, {
                    onrendered: function (canvas) {
                        $element.appendTo(parent);
                        content.detach();
                        toHide.show();
                        var imgData = canvas.toDataURL(
                            'image/png');
                        var doc = new jsPDF('p', 'mm');
                        doc.addImage(imgData, 'PNG', 10, 10);
                        doc.save($scope.nomeDoArquivo + '.pdf');
                    }
                });

               
            }

        }],
        link: function (scope, element, attrs, ctrls) {
            
        }
    }
});




app.directive('pdfButton', function () {
    return {
        restrict: 'A',
        require: ["^^pdfA4"],
        link: function (scope, element, attrs, ctrls) {
            element.on('click', function () {
                ctrls[0].gerar();
            });

        }
    }
});




