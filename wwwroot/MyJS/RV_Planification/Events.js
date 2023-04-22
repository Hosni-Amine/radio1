


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

function event_details(object, date)
{
    const formattedDate = date.toLocaleDateString('fr-FR', {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
        second: '2-digit',
        hour12: false
    });
    $("#RV-modal #Examen").text(object.examen);
    $("#RV-modal #Nom_patient").text(object.patient.nom + ' ' + object.patient.prenom);
    $("#RV-modal #Nom_doc").text(object.doctor.nom + ' ' + object.doctor.prenom);
    if (object.secretaire.nom != null) {
        $("#RV-modal #Nom_sec").text(object.secretaire.nom + ' ' + object.secretaire.prenom);
    }
    else {
        $("#RV-modal #Nom_sec").text("( Administrateur )");
    }
    $("#RV-modal #Nom_app").text(object.appareil_NumSerie);
    $("#RV-modal #Nom_tec").text(object.technicien.nom + ' ' + object.technicien.prenom);
    $("#RV-modal #Nom_op").text(object.typeOperation.nom);
    $("#RV-modal #Date").text(formattedDate);
    if (object.status === "Planifié") {
        $("#RV-modal #Status").text(object.status).css("color", "#d3a600");
    }
    else if (object.status === "Annulé") {
        $("#RV-modal #Status").text(object.status).css("color", "#d00000");
    }
    else if (object.status === 'Terminé') {
        $("#RV-modal #Status").text(object.status).css("color", "#16ac00");
    }
    else if (object.status === 'En cours') {
        $("#RV-modal #Status").text(object.status).css("color", "#2200ad");
    }  
    $('#RV-modal').modal('show');
    var button = document.getElementById("Delete_RendezVous");
    button.setAttribute("onclick", "Delete_RendezVous_btn('" + object.id + "')");
    var button = document.getElementById("Edit_RendezVous");
    $("#RV-modal #Id").text(object.id);
    button.setAttribute("onclick", "Edit_RendezVous_btn('" + object.typeOperation.nom + "', '" + object.typeOperation.id + "')");
}