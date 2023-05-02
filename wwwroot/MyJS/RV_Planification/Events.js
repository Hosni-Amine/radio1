


var calendarEl = document.getElementById('calendar');
var currentDate = new Date();
var calendar = new FullCalendar.Calendar(calendarEl, {
    slotMinTime: '08:00',
    slotMaxTime: '18:00',
    headerToolbar: {
        left: 'prev,today next',
        center: 'title',
        right: 'dayGridMonth,dayGridWeek,listDay'
    },
    eventClick: function (info) {
        event_details(info.event.extendedProps.object, info.event.start);
    },
    views: {
        dayGridMonth: {
            dayMaxEvents: 4
        }
    },
    initialView: 'listDay',
    initialDate: currentDate,
    navLinks: true,
    nowIndicator: true,
    locale: 'fr',
    eventDidMount: function (info) {
        if (info.event.extendedProps.status === 'Terminé')
        {
            info.el.style.backgroundColor = '#21ff0063';
        }
        else if (info.event.extendedProps.status === 'Planifié') {
            info.el.style.backgroundColor = '#ffea003d';
        }
        else if (info.event.extendedProps.status === 'Annulé') {
            info.el.style.backgroundColor = '#ff000045';
        }
        else if (info.event.extendedProps.status === 'En cours') {
            info.el.style.backgroundColor = '#0064ff47';
        }    
    }

});
document.addEventListener('DOMContentLoaded', function () {
    $.ajax({
        url: '/Appointment/EventsList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    var event = data[i];
                    var newEvent = {
                        title: event.typeOperation.nom + ' : ' + event.examen + '  ( ' + event.status + ' )',
                        start: event.date,
                        extendedProps: {
                            status: event.status,
                            object: event
                        }
                    };
                    calendar.addEvent(newEvent);
                }
            }
            else {
                $('#success-modal-text').text("pas de Rendez-Vous trouvée !");
                $('#success-modal').modal('show');
                setTimeout(function () {
                    $('#success-modal').modal('hide');
                }, 2500);
            }
        },
        error: function (xhr) {
            CheckError(xhr);
        }
    });
    calendar.render();

});

