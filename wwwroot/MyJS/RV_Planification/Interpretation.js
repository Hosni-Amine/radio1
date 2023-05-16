function Show_PDF(Name) {
    var pdfUrl = '../assets/Interpretation_PDF/' + Name;
    var PDFiframe = document.getElementById("PDFiframe");
    PDFiframe.setAttribute("src", pdfUrl);
    $('#pdf-modal').modal('show');
}
function display_PDF_inter() {
    const pdfFileInput = document.getElementById("file-inter");
    const new_Emplacement = document.querySelector('#pdf_upload_label');
    if (pdfFileInput.files.length > 0) {
        const pdfFileName = pdfFileInput.files[0].name;
        new_Emplacement.innerHTML = pdfFileName;
    } else {
        new_Emplacement.innerHTML = 'Choisir une interpretation';
    }
}


//Interpretation API
function show(id, str) {
    $.ajax({
        url: "/Appointment/GetById/" + id,
        type: 'GET',
        success: function (object) {
            if (str === "text") {
                $("#inter_vocal").css("display", "none");
                $("#inter_papier").css("display", "none");
                $("#inter_text").css("display", "block");
                if (Nom != null) {
                    $('#Interpretation').text(object.interpretation);
                }
                $("#Add_Inter").one("click", function () {
                    Add_inter_text_btn(id);
                });
            } else if (str === "vocal") {
                $("#inter_vocal").css("display", "block");
                $("#inter_papier").css("display", "none");
                $("#inter_text").css("display", "none");
                $("#Add_Inter").one("click", function () {
                    Add_inter_vocal_btn();
                });
            } else {
                $("#inter_vocal").css("display", "none");
                $("#inter_papier").css("display", "block");
                $("#inter_text").css("display", "none");
                $("#Add_Inter").one("click", function () {
                    Add_inter_pdf_btn(id);
                });
            }
            Inter_btn(id);
        },
        error: function (xhr) {
            $('#delete_modal').modal('hide');
            $('#RV-modal').modal('hide');
            CheckError(xhr);
        }
    });
}


//Fonction pour afficher la modal de l'ajout et edit
function Inter_btn(id)
{
    $('#edit-interpret-modal #Id_RV_VW').val(id);
    $('#edit-interpret-modal').modal('show');
}

//Fonctions pour Ajouter les différent types d'interprétation
function Add_inter_pdf_btn() {
    let Id = $('#edit-interpret-modal #Id_RV_VW').val();
    var pdf = new FormData();
    const pdfFile = document.getElementById("file-inter").files[0];
    if (pdfFile != null) {
        pdf.append("pdf", pdfFile, Id + "_" + pdfFile.name);
        $.ajax({
            url: '/Appointment/AddPDF',
            type: 'POST',
            data: pdf,
            processData: false,
            contentType: false,
            success: function (response) {
                $('#success-modal-text').text(response.message);
                $('#edit-interpret-modal').modal('hide');
                $('#success-modal').modal('show');
                setTimeout(function () {
                    $('#success-modal').modal('hide');
                    window.location.reload();
                }, 1500);
            },
            error: function (xhr, status, error) {
                console.log(status);
                $('#error-modal-text').text("Erreur ! ");
                $('#error-modal').modal('show');
                setTimeout(function () {
                    $('#edit-interpret-modal').modal('show');
                    $('#error-modal').modal('hide');
                }, 1500);
            }
        });
    }
    else {
        $('#error-modal-text').text("Pas de fichier choisis !");
        $('#edit-interpret-modal').modal('hide');
        $('#error-modal').modal('show');
        setTimeout(function () {
            $('#edit-interpret-modal').modal('show');
            $('#error-modal').modal('hide');
        }, 1500);
    }
}
function Add_inter_text_btn() {
    let Id = $('#edit-interpret-modal #Id_RV_VW').val();
    let inter = $('#edit-interpret-modal #Interpretation').val();
    if (inter.length > 4) {
        rendezvous =
        {
            Id: Id,
            Interpretation: inter
        }
        $.ajax({
            url: '/Appointment/Edit_Inter_Text',
            type: 'POST',   
            data: { RV: rendezvous }, 
            success: function (response) {
                if (response.success) {
                    $('#success-modal-text').text(response.message);
                    $('#success-modal').modal('show');
                    $('#edit-interpret-modal').modal('hide');
                    setTimeout(function () {
                        $('#success-modal').modal('hide');
                        window.location.reload();
                    }, 2000);
                } 
                else {  
                    $('#edit-RendezVous-modal').modal('hide');
                    $('#edit-interpret-modal').modal('hide');
                    $('#error-modal-text').text(response.message);
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#error-modal').modal('hide');
                        $('#edit-interpret-modal').modal('show');
                    }, 2000);
                } 
            },
            error: function (xhr, status, error) {
                CheckError(xhr);
            }
        });
    }
    else {
        $('#error-modal-text').text("Tu dois choisir au moin 4 Caractéres pour l'interpretation !");
        $('#error-modal').modal('show');
        $('#edit-interpret-modal').modal('hide');
        setTimeout(function () {
            $('#error-modal').modal('hide');
            $('#edit-interpret-modal').modal('show');
        }, 2000);
    }
}
function Add_inter_vocal_btn() {
}    

//Fonction pour le record
let mediaRecorder;
let chunks = [];
function startRecording() {
    $('#start_Recording').text('Enregistrement en cours...');
    navigator.mediaDevices.getUserMedia({ audio: true })
        .then(function (stream) {
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.ondataavailable = function (e) {
                chunks.push(e.data);
            }
            mediaRecorder.start();
        });
}
function stopRecording() {
    $('#start_Recording').text("Commencer l'enregistrement");
    const audioPlayer = document.getElementById("audioPlayer");
    mediaRecorder.stop();
    mediaRecorder.onstop = function () {
        const audioBlob = new Blob(chunks, { type: "audio/mp3" });
        chunks = [];
        const audioUrl = URL.createObjectURL(audioBlob);
        audioPlayer.src = audioUrl;
        const sendAudioBtn = document.createElement("Add_Inter");
        sendAudioBtn.addEventListener("click", function () {
            Add_inter_vocal_btn(audioBlob);
        });
    }
    mediaRecorder.stream.getTracks().forEach(function (track) {
        track.stop();
    });
}