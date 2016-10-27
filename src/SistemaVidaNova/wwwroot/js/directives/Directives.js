var app = angular.module('app');
app.directive('calendar', function () {
    return {
        restrict: 'A',
        scope: {
            editable: '=?',
            events: '=?',
            eventClickEndPoint: "=?"
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

                    window.location.href = scope.eventClickEndPoint + calEvent.id;

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


app.directive('echart', ['theme',  function (theme) {
    return {
        restrict: 'EA',
        template: '<div></div>',
        scope: {
            
            options: '=?',
            tema: '=?'
        },
        link: function (scope, element, attrs) {
            var ndWrapper = element.find('div')[0],
            ndParent = element.parent()[0],
            parentWidth = ndParent.clientWidth,
            parentHeight = ndParent.clientHeight,
            width, height, chart;

            function getSizes() {
                width =  parseInt(attrs.width) || parentWidth || 320;
                height = parseInt(attrs.height) || parentHeight || 240;

                ndWrapper.style.width = width + 'px';
                ndWrapper.style.height = height + 'px';
            }

            function aplicarOptions() {
                if (scope.options) {
                    getSizes();
                    if (!chart) {
                        chart = echarts.init(ndWrapper, theme.get(scope.tema || 'macarons'));
                    }
                    chart.clear();
                    chart.setOption(scope.options);
                    chart.resize();
                }
            }

            scope.$watch('options', function (newValue, oldValue) {
                aplicarOptions();
            });
            

        }


    }
}]);

/*
app.directive('htmlToPdf', function () {
    return {
        restrict: 'A',
        scope: {

            nomeDoArquivo: '=?'
          
        },
        controller: ['$element', '$scope', function ($element, $scope) {
            var ctrl = this;
            var specialElementHandlers = {
                '[ignore-to-pdf]': function (element, renderer) {
                    return true;
                }
            };
            ctrl.gerar = function () {
                
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


                //doc.save($scope.nomeDoArquivo + '.pdf');
            }

        }],        
        link: function (scope, element, attrs, ctrls) {
          var a = 0;
        }
    }
})

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

*/



