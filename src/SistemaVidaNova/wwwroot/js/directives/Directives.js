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

