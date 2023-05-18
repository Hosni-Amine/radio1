
//Fonctions pour le display de l'ancien contenu
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
function display_Vocal(audioBlob_str) {
    var audioPlayer = document.getElementById("audioPlayer");
    audioPlayer.src = "/Appointment/Get_Inter_Vocal?audioBlob_str=" + encodeURIComponent(audioBlob_str);
}

//Fonction pour afficher la modal de l'ajout et l'edit
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
                if ( object.inter_Vocal != null ) {
                    display_Vocal(object.inter_Vocal);
                }
            } else {
                $("#inter_vocal").css("display", "none");
                $("#inter_papier").css("display", "block");
                $("#inter_text").css("display", "none");
                $("#Add_Inter").one("click", function () {
                    Add_inter_pdf_btn(id);
                });
            }
            $('#edit-interpret-modal #Id_RV_VW').val(id);
            $('#edit-interpret-modal').modal('show');
        },
        error: function (xhr) {
            $('#delete_modal').modal('hide');
            $('#RV-modal').modal('hide');
            CheckError(xhr);
        }
    });
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
                $('#edit-interpret-modal').modal('hide');
                if ((xhr.status === 403) || (xhr.status === 401)) {
                    CheckError(xhr);
                }
                else {
                    $('#error-modal-text').text("Erreur ! ");
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#error-modal').modal('hide');
                    }, 1500);
                }
            }
        });
    }
    else {
        $('#error-modal-text').text("Vous devez choisir un fichier PDF !");
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
            $('#edit-interpret-modal').modal('hide');
            CheckError(xhr);
            }
        });
    }
    else {
        $('#error-modal-text').text("Vous devez choisir au moin 4 Caractéres pour l'interpretation !");
        $('#error-modal').modal('show');
        $('#edit-interpret-modal').modal('hide');
        setTimeout(function () {
            $('#error-modal').modal('hide');
            $('#edit-interpret-modal').modal('show');
        }, 2000);
    }
}
function Add_inter_vocal_btn(audioBlob) {
    if ((audioBlob.type == 'audio/mp3') && (audioBlob.size >= 10000))
    {
        let Id = $('#edit-interpret-modal #Id_RV_VW').val();
        let filename = Id + "_recording.mp3";
        const record = new FormData();
        record.append("audio", audioBlob, filename);
        record.append("Id", Id);
        $.ajax({
            url: "/Appointment/Add_Inter_Vocal",
            type: "POST",
            data: record,
            processData: false,
            contentType: false,
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
                $('#edit-interpret-modal').modal('hide');
                if ((xhr.status === 403) || (xhr.status === 401)) {
                    CheckError(xhr);
                }
                else {
                    $('#error-modal-text').text("Erreur ! ");
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#edit-interpret-modal').modal('show');
                        $('#error-modal').modal('hide');
                    }, 1500);
                }
            }
        });
    }
    else
    {
        $('#error-modal-text').text("Vous devez enregistrer une vocale plus longue et nette !");
        $('#error-modal').modal('show');
        $('#edit-interpret-modal').modal('hide');
        setTimeout(function () {
            $('#error-modal').modal('hide');
            $('#edit-interpret-modal').modal('show');
        }, 2000);
    }
}

//Fonction pour l'enregistrement vocal
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
        const sendAudioBtn = document.getElementById("Add_Inter");
        sendAudioBtn.addEventListener("click", function () {
            Add_inter_vocal_btn(audioBlob);
        });
    }
    mediaRecorder.stream.getTracks().forEach(function (track) {
        track.stop();
    });
}