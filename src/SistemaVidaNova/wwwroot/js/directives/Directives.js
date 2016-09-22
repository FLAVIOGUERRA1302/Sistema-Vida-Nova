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
})