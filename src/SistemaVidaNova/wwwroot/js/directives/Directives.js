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




