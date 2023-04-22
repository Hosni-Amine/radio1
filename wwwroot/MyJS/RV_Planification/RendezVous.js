

//API pour les rendez-vous 
function Add_RendezVous_Btn()
{
    var pattern = /^[A-Za-zÀ-ÿ0-9\s]{4,}$/;
    var User_Id = localStorage.getItem("Id");
    if (localStorage.getItem("Role") === "Admin") {
        User_Id = 0;
    };
    var selectedAffectation = $('#Affectation option:selected').text();
    var startIndex = selectedAffectation.indexOf('Appareil : ') + 'Appareil : '.length;
    var endIndex = selectedAffectation.indexOf(' |', startIndex);
    var selectedAppareil = selectedAffectation.substring(startIndex, endIndex);
    if ($('#select_patient').val() != "") {
        if ($('#Description').val() && pattern.test($('#Description').val())) {
            if ($('#Type_Operation').val() != "") {
                if (!($('div#Hoursdiv').is(':hidden'))) {
                    if ($('#Heures').val() != "") {
                        rendezvous =
                        {
                            Date: $('#myDatepicker').val() + "T" + $('#Heures').val(),
                            Status: "Planifié",
                            TypeOperation: TypeOperation =
                            {
                                Nom: $('#Type_Operation').val()
                            },
                            Examen: $('#Description').val(),
                            doctor: doctor =
                            {
                                Id: 2
                            },
                            patient: patient =
                            {
                                Id: $('#select_patient').val()
                            },
                            secretaire: user =
                            {
                                Id: User_Id
                            },
                            technicien: technicien =
                            {
                                Id: 2
                            }
                        }
                        $.ajax({
                            url: '/Appointment/SubmitAddAppointment',
                            type: 'POST',
                            data: { rendezvous: rendezvous, selectedAppareil: selectedAppareil },
                            success: function (response) {
                                if (response.success) {
                                    $('#success-modal-text').text(response.message);
                                    $('#success-modal').modal('show');
                                    setTimeout(function () {
                                        $('#success-modal').modal('hide');
                                    }, 1500);
                                }
                                else {
                                    $('#error-modal-text').text(response.message);
                                    $('#error-modal').modal('show');
                                    setTimeout(function () {
                                        $('#error-modal').modal('hide');
                                    }, 2000);
                                }
                            },
                            error: function (xhr, status, error) {
                                $('#error-modal-text').text("Erreur du serveur !");
                                $('#error-modal').modal('show');
                                setTimeout(function () {
                                    $('#error-modal').modal('hide');
                                }, 1500);
                            }
                        });
                    }
                    else {
                        $('#error-modal-text').text("Choisir l'heure de rendez-vous !");
                        $('#error-modal').modal('show');
                        setTimeout(function () {
                            $('#error-modal').modal('hide');
                        }, 1500);
                    }
                }
                else {
                    $('#error-modal-text').text("Choisir une date de rendez-vous !");
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#error-modal').modal('hide');
                    }, 1500);
                }
            }
            else {
                $('#error-modal-text').text("Choisir un type d'operation !");
                $('#error-modal').modal('show');
                setTimeout(function () {
                    $('#error-modal').modal('hide');
                }, 1500);
            }
        }
        else {
            $('#error-modal-text').text("Choisir une description de l'examen qui contient au mois 4 caratére !");
            $('#error-modal').modal('show');
            setTimeout(function () {
                $('#error-modal').modal('hide');
            }, 1500);
        }
    }
    else {
        $('#error-modal-text').text("Choisir un patient !");
        $('#error-modal').modal('show');
        setTimeout(function () {
            $('#error-modal').modal('hide');
        }, 1500);
    }
}

function Delete_RendezVous_btn(id) {
    var deletebtn = document.getElementById('delete-modal-btn');
    deletebtn.onclick = function () {
        Submit_Delete_RendezVous(id);
    };
    $('#delete-text').text("Voulez-vous vraiment supprimer ce rendez-vous ?");
    $('#delete_modal').modal('show');
}
function Submit_Delete_RendezVous(id) {
    $.ajax({
        url: "/Appointment/DeleteAppointment/" + id,
        type: 'DELETE',
        success: function (response) {
            if (response.success) {
                $('#delete_modal').modal('hide');
                $('#RV-modal').modal('hide');
                $('#success-modal-text').text(response.message);
                $('#success-modal').modal('show');
                setTimeout(function () {
                    window.location.href = '/Appointment/AppointmentList';
                }, 1500);
            } else {
                $('#RV-modal').modal('hide');
                $('#error-modal-text').text(response.message);
                $('#error-modal').modal('show');
                setTimeout(function () {
                    $('#error-modal').modal('hide');
                    $('#RV-modal').modal('show');
                }, 1500);
            }
        },
        error: function (xhr) {
            $('#delete_modal').modal('hide');
            $('#RV-modal').modal('hide');
            CheckError(xhr);
        }
    });
}

function Edit_RendezVous_btn(nom_op,id_op)
{
    flatpickr("#myDatepicker_edit", {
        minDate: "today",
        onChange: function (selectedDates, dateStr, instance) {
            var hours = [{ value: '', label: 'Select Hour' }, { value: '08:00:00', label: '8:00 am' }, { value: '09:00:00', label: '9:00 am' }, { value: '10:00:00', label: '10:00 am' }, { value: '11:00:00', label: '11:00 am' }, { value: '12:00:00', label: '12:00 pm' }, { value: '13:00:00', label: '1:00 pm' }, { value: '14:00:00', label: '2:00 pm' }, { value: '15:00:00', label: '3:00 pm' }, { value: '16:00:00', label: '4:00 pm' }, { value: '17:00:00', label: '5:00 pm' }, { value: '18:00:00', label: '6:00 pm' }];
            var heuresSelect = document.getElementById('Heures_edit');
            heuresSelect.innerHTML = '';
            var selectedValue = JSON.parse(document.getElementById("Affectation_edit").value);
            if (selectedValue && selectedDates.length > 0) {
                var selectedDate = selectedDates[0].toISOString().slice(0, 10);
                var filteredAppointments = selectedValue.filter(function (appointment) {
                    return appointment.startsWith(selectedDate);
                });
                for (var i = 0; i < hours.length; i++) {
                    var option = document.createElement('option');
                    option.value = hours[i].value;
                    option.text = hours[i].label;
                    var fullDateTime = selectedDate + 'T' + hours[i].value;
                    if (filteredAppointments.includes(fullDateTime)) {
                        option.disabled = true;
                    }
                    heuresSelect.add(option);

                    var Hoursdiv = $('#edit-RendezVous-modal #Hoursdiv');
                    Hoursdiv.show();
                }
            }
        }
    });
    var Hoursdiv = $('#edit-RendezVous-modal #Hoursdiv');
    Hoursdiv.hide();
    $.ajax({
        url: '/Appointment/GetDisponibility',
        type: 'GET',
        data: { operation : nom_op },
        success: function (dispos) {
            var select = $('#edit-RendezVous-modal #Affectation_edit');
            select.empty();
            if (dispos.length != 0) {
                for (var i = 0; i < dispos.length; i++) {
                    console.log(JSON.stringify(dispos[i].dates));
                    if (dispos[i].typeOperation_Id == id_op)
                    {
                        var option = $('<option>');
                        option.val(JSON.stringify(dispos[i].dates));
                        select.append(option);
                    }
                }
                var myDatepicker = $('#edit-RendezVous-modal #div_select_date');
                myDatepicker.show();
            }
        },
        error: function (xhr, status, error) {
        }
    });
    $('#edit-RendezVous-modal').modal('show');

}

//Fonction qui initialise le select option des patient disponible
function Set_Patients_List(check1) {
    if (!check || !check1) {
        check = true;
        $.ajax({
            url: '/Patient/PatientListJson',
            type: 'GET',
            success: function (patient) {
                var select_patient = document.getElementById('select_patient');
                select_patient.innerHTML = '';
                var option = document.createElement('option');
                option.text = 'Choisir un patient ';
                option.value = '';
                select_patient.add(option);
                if (patient.length != 0) {
                    for (var i = 0; i < patient.length; i++) {
                        var option = document.createElement('option');
                        option.value = patient[i].id;
                        option.text = 'Patient : ' + patient[i].nom + '  ' + patient[i].prenom + ' | Telephone : ' + patient[i].telephone;
                        select_patient.add(option);
                    }
                }
            },
            error: function (xhr, status, error) {
            }
        });
    }
}

var check = false;
$(document).ready(function () {

    $('#Type_Operation').on('change', function () {
        var Hoursdiv = $('#Hoursdiv');
        Hoursdiv.hide();
        var selectedOperation = $(this).val();
        $.ajax({
            url: '/Appointment/GetDisponibility',
            type: 'GET',
            data: { operation: selectedOperation},
            success: function (dispos) {
                var div = $('#div_select');
                var select = $('#Affectation');
                select.empty();
                if (dispos.length != 0) {
                    for (var i = 0; i < dispos.length; i++) {
                        var option = $('<option>');
                        option.val(JSON.stringify(dispos[i].dates));
                        console.log(JSON.stringify(dispos[i].dates));
                        option.text('Appareil : ' + dispos[i].nom_Appareil + ' | Salle : ' + dispos[i].nom_Salle + ' | Medecin : ' + dispos[i].nom_Doctor);
                        select.append(option);
                        div.show();
                        var myDatepicker = $('#div_select_date');
                        myDatepicker.show();
                    }
                }
            },
            error: function (xhr, status, error) {
            }
        });
    });

    $('#Affectation').off('change').on('change', function () {
        var Hoursdiv = $('#Hoursdiv');
        Hoursdiv.hide();
    });

    flatpickr("#myDatepicker", {
        minDate: "today",
        onChange: function (selectedDates, dateStr, instance) {
            var hours = [{ value: '', label: 'Select Hour' }, { value: '08:00:00', label: '8:00 am' }, { value: '09:00:00', label: '9:00 am' }, { value: '10:00:00', label: '10:00 am' }, { value: '11:00:00', label: '11:00 am' }, { value: '12:00:00', label: '12:00 pm' }, { value: '13:00:00', label: '1:00 pm' }, { value: '14:00:00', label: '2:00 pm' }, { value: '15:00:00', label: '3:00 pm' }, { value: '16:00:00', label: '4:00 pm' }, { value: '17:00:00', label: '5:00 pm' }, { value: '18:00:00', label: '6:00 pm' }];
            var heuresSelect = document.getElementById('Heures');
            heuresSelect.innerHTML = '';
            var selectedValue = JSON.parse(document.getElementById("Affectation").value);
            if (selectedValue && selectedDates.length > 0) {
                var selectedDate = selectedDates[0].toISOString().slice(0, 10);
                var filteredAppointments = selectedValue.filter(function (appointment) {
                    return appointment.startsWith(selectedDate);
                });
                for (var i = 0; i < hours.length; i++) {
                    var option = document.createElement('option');
                    option.value = hours[i].value;
                    option.text = hours[i].label;
                    var fullDateTime = selectedDate + 'T' + hours[i].value;
                    if (filteredAppointments.includes(fullDateTime)) {
                        option.disabled = true;
                    }
                    heuresSelect.add(option);

                    var Hoursdiv = $('#Hoursdiv');
                    Hoursdiv.show();
                }
            }
        }
    });
    Set_Patients_List(true);

  
});




