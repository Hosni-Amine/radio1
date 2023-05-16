//Fonctions pour le chargement des notification .
$(document).ready(function () {
    refresh_notification();

});
function refresh_notification() {
    var parentElement = $("#notification_list");
    var elementToClone = $("#notification_message");
    parentElement.empty();
    $.ajax({
        url: '/Appointment/EventsList',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            if (data.length != 0) {
                for (var i = 0; i < data.length; i++) {
                    const image_name = data[i].image_Name;
                    const inter = data[i].interpretation;
                    const Role = localStorage.getItem("Role");
                    var clonedElement = elementToClone.clone();

                    //date format and check 
                    var currentDate = new Date();
                    var formattedcurrentDate = currentDate.toLocaleDateString('fr-FR', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit',
                        second: '2-digit',
                        hour12: false
                    });
                    var date = new Date(data[i].date);
                    var formattedRV = date.toLocaleDateString('fr-FR', {
                        year: 'numeric',
                        month: '2-digit',
                        day: '2-digit',
                        hour: '2-digit',
                        minute: '2-digit',
                        second: '2-digit',
                        hour12: false
                    });

                    //adding details to the notification element
                    clonedElement.find("#noti_time").text("Le : " + formattedRV);
                    var patient = data[i].patient.nom + " " + data[i].patient.prenom;
                    if (data[i].secretaire.nom === null) {
                        clonedElement.find("#noti_owner").text("L'administrateur ");
                    }
                    else {
                        clonedElement.find("#noti_owner").text(data[i].secretaire.nom + " " + data[i].secretaire.prenom);
                    }
                    clonedElement.find("#noti_details").text(data[i].typeOperation.nom + " : " + data[i].examen);
                    clonedElement.find("#noti_for").text(patient);
                    var add = clonedElement.find("#event_details_for_add");
                    if (image_name === null && ( (Role === "Doctor") || (Role === "Technicien") ) ) {
                        clonedElement.css("background-color", "#ffe0002e");
                        add.css("background-color", "#cb6d00bf");
                        add.text("Ajouter une image");
                        add.attr("onclick", "event_details_for_add('" + data[i].id + "')");
                    }
                    else if (image_name === null) {
                        clonedElement.css("background-color", "#ffe0002e");
                        clonedElement.find("#notification_button").hide();
                    } 
                    else if (image_name != null && inter != null && ((Role === "Doctor") || (Role === "Technicien"))) {
                        clonedElement.css("background-color", "#54db763d");
                        add.css("background-color", "#1ed641b8");
                        add.text("Visualiser l'image");
                        add.attr("onclick", "View_Image('" + data[i].id + "')");
                    }
                    else if (image_name != null && ((Role === "Doctor") || (Role === "Technicien")) ) {
                        add.css("background-color", "#648fcc");
                        add.text("Visualiser l'image");
                        add.attr("onclick", "View_Image('" + data[i].id + "', true)");
                    }
                    else {
                        clonedElement.find("#notification_button").hide();
                    }
                    if (formattedRV > formattedcurrentDate) {
                        clonedElement.css("background-color", "#e4e8ee");
                        clonedElement.find("#notification_button").hide();
                        //adding event listener for the button
                        console.log(data[i]);
                    }
                    parentElement.append(clonedElement);
                }
            }
            //else {
            //    $('#success-modal-text').text("pas de Rendez-Vous trouvée !");
            //    $('#success-modal').modal('show');
            //    setTimeout(function () {
            //        $('#success-modal').modal('hide');
            //    }, 2500);
            //}
        },
        error: function (xhr) {
            CheckError(xhr);
        }
    });
}

// Fonctions de j'ajout d'une image médicale
function event_details_for_add(id) {
    var add = $("#add_dicom_btn");
    add.attr("onclick", "submit_add_dicom_btn()");
    document.getElementById("RendezVous_Id").value = id;
    $('#attache-dicom-modal').modal('show');
}
function displayFileNameDicom() {
    const pdfFileInput = document.getElementById("file-dicom");
    const new_Emplacement = document.querySelector('.upload');

    if (pdfFileInput.files.length > 0) {
        const pdfFileName = pdfFileInput.files[0].name;
        new_Emplacement.innerHTML = pdfFileName;
    } else {
        new_Emplacement.innerHTML = 'Choisir un fichier';
    }
}
function submit_add_dicom_btn() {
    var dicomFile = $('#file-dicom')[0].files[0];
    if (dicomFile && dicomFile.name.endsWith(".dcm")) {
        var formData = new FormData($('#dicom_form')[0]);
        var spinner = $('#spinner');
        $('#spinner_text').css('display', 'block');
        spinner.show();
        // Send the AJAX request
        $.ajax({
            url: "/AzurePacs/AzureUpLoadStudy",
            type: "POST",
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                $('#spinner_text').css('display', 'none');
                spinner.hide();
                if (response.verification) {
                    refresh_notification();
                    $('#attache-dicom-modal').modal('hide');
                    $('#success-modal-text').text(response.msg);
                    $('#success-modal').modal('show');
                    setTimeout(function () {
                        $('#success-modal').modal('hide');
                        if (window.location.href.includes("/Patient/EventsListPatient")) {
                            window.location.reload();
                        }
                    }, 1500);
                }
                else {
                    refresh_notification();
                    $('#error-modal-text').text(response.msg);
                    $('#error-modal').modal('show');
                    $('#attache-dicom-modal').modal('hide');
                    setTimeout(function() {
                        $('#error-modal').modal('hide');
                        $('#attache-dicom-modal').modal('show');
                    }, 2000);
                }
            },
            error: function (xhr, status, error) {
                CheckError(xhr);
                refresh_notification();
                $('#spinner_text').css('display', 'none');
                spinner.hide();
                $('#error-modal-text').text("Erreur de serveur !");
                $('#error-modal').modal('show');
                $('#attache-dicom-modal').modal('hide');
                setTimeout(function () {
                    $('#error-modal').modal('hide');
                    $('#attache-dicom-modal').modal('show');
                }, 2000);
            }
        });
    }
    else {
        $('#error-modal-text').text("Choisir une image de type Dicom !");
        $('#error-modal').modal('show');
        $('#attache-dicom-modal').modal('hide');
        setTimeout(function () {
            $('#error-modal').modal('hide');
            $('#attache-dicom-modal').modal('show');
        }, 2000);
    }
}

//Fonctions pour la visualisation des images médicale
function View_Image(id) {
    var spinner = $('#spinner');
    spinner.show();
    $('#spinner_text').css('display', 'block');
    $.ajax({
        url: "/AzurePacs/Study",
        type: "GET",
        data: { RendezVous_Id: id },
        success: function () {
            $('#spinner_text').css('display', 'none');
            spinner.hide();
            $.ajax({
                url: "/AzurePacs/LaunchViewer",
                type: "GET",
                success: function () {
                    $('#success-modal-text').text("Tu peut consulter l'image dans le Viewer !");
                    $('#success-modal').modal('show');
                    setTimeout(function () {
                        RendezVous_Delails_Image(id);
                        $('#success-modal').modal('hide');
                    }, 1500);
                },
                error: function (xhr, status, error) {
                    CheckError(xhr);
                    $('#error-modal-text').text("Erreur de serveur !");
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#error-modal').modal('hide');
                    }, 2000);
                }
            });
        },
        error: function (xhr, status, error) {
            CheckError(xhr);
            $('#spinner_text').css('display', 'none');
            spinner.hide();
            $('#error-modal-text').text("Erreur de serveur !");
            $('#error-modal').modal('show');
            setTimeout(function () {
                $('#error-modal').modal('hide');
            }, 2000);
        }
    });
}
function Open_Viewer() {
    $.ajax({
        url: "/AzurePacs/LaunchViewer",
        type: "GET",
        success: function () {
        },
        error: function (xhr, status, error) {
            CheckError(xhr);
            $('#error-modal-text').text("Erreur de serveur !");
            $('#error-modal').modal('show');
            setTimeout(function () {
                $('#error-modal').modal('hide');
            }, 2000);
        }
    });
}

function RendezVous_Delails_Image(id) {
    $.ajax({
        url: "/Appointment/GetById/" + id,
        type: 'GET',
        success: function (object) {
            const dateStr = object.date;
            const formattedDate = new Intl.DateTimeFormat('fr-FR', {
                year: 'numeric',
                month: '2-digit',
                day: '2-digit',
                hour: '2-digit',
                minute: '2-digit',
                second: '2-digit',
                hour12: false
            }).format(new Date(dateStr));
            $("#interpret-modal #Id_RV_VW").val(id);
            $("#interpret-modal #Examen").text(object.examen);
            $("#interpret-modal #Nom_patient").text(object.patient.nom + ' ' + object.patient.prenom);
            $("#interpret-modal #Nom_doc").text(object.doctor.nom + ' ' + object.doctor.prenom);
            if (object.secretaire.nom != null) {
                $("#interpret-modal #Nom_sec").text(object.secretaire.nom + ' ' + object.secretaire.prenom);
            }
            else {
                $("#interpret-modal #Nom_sec").text("( Administrateur )");
            }
            $("#interpret-modal #Nom_app").text(object.appareil_NumSerie);
            $("#interpret-modal #Nom_tec").text(object.technicien.nom + ' ' + object.technicien.prenom);
            $("#interpret-modal #Nom_op").text(object.typeOperation.nom);
            $("#interpret-modal #Date").text(formattedDate);
            $('#interpret-modal').modal('show');
        },
        error: function (xhr) {
            CheckError(xhr);
            $('#error-modal-text').text("Erreur de serveur !");
            $('#error-modal').modal('show');
            setTimeout(function () {
                $('#error-modal').modal('hide');
            }, 2000);
        }
    });
}






