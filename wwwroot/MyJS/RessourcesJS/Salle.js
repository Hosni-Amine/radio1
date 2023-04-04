
///Les fonction d'ajout permet d'ajouter une salle une operation affecter un medecin et le pdf de l'emplacement
function submit_add_salle() {
    var pdf = new FormData();
    let pattern1 = /^.{6,}$/;
    var Ops = $('#Toperations_salle').val();
    console.log($('#Nom_Salle').val());
    pdf.append("pdf", document.getElementById("file").files[0]);
    if ($('#Nom_Salle').val() && pattern1.test($('#Nom_Salle').val())) {
        if ($('#Responsable_salle').val() !== "") {
            if (Ops.length !== 0) {
                if ((pdf.get("pdf").name) != null && pdf != null) {
                    var operations = [];
                    Ops.forEach(function (op) {
                        var object = { nom: op };
                        operations.push(object);
                    });
                    var doc =
                    {
                        Id: parseInt($('#Responsable_salle').val())
                    }
                    var salle =
                    {
                        Nom: $('#Nom_Salle').val(),
                        Operations: operations,
                        Responsable: doc,
                        Emplacement: pdf.get("pdf").name
                    };
                    console.log(salle);
                    $.ajax({
                        url: '/Salle/Submit_AddSalle',
                        type: 'POST',
                        data: { salle: salle, operations: operations },
                        success: function (response) {
                            if (response.success) {
                                add_pdf(pdf, response);
                            }
                            else {
                                $('#error-modal-text').text(response.message);
                                $('#error-modal').modal('show');
                                console.log(response.message);
                                setTimeout(function () {
                                    $('#error-modal').modal('hide');
                                }, 1500);
                            }
                        },
                        error(xhr, status, error) {
                            console.log(error);
                        }
                    });
                }
                else {
                    $('#error-modal-text').text("Pas de fichier choisis !");
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#error-modal').modal('hide');
                    }, 1500);
                }
            }
            else {
                $('#error-modal-text').text("choisir au moin un type d'operation !");
                $('#error-modal').modal('show');
                setTimeout(function () {
                    $('#error-modal').modal('hide');
                }, 1500);
            }
        }
        else {
            $('#error-modal-text').text("Choisir Responsable pour la salle !");
            $('#error-modal').modal('show');
            setTimeout(function () {
                $('#error-modal').modal('hide');
            }, 1500);
        }
    }
    else {
        $('#error-modal-text').text("Choisir un nom qui contient au moin 6 caractere !");
        $('#error-modal').modal('show');
        setTimeout(function () {
            $('#error-modal').modal('hide');
        }, 1500);
    }
}

//Fonctions de gestion pour les PDF
function add_pdf(pdf, response) {
    $.ajax({
        url: '/Salle/AddPDF',
        type: 'POST',
        data: pdf,
        processData: false,
        contentType: false,
        success: function (name) {
            $('#add-salle-modal').modal('hide');
            $('#success-modal-text').text(response.message);
            $('#success-modal').modal('show');
            setTimeout(function () {
                $('#success-modal').modal('hide');
                window.location.href = '/Salle/SalleList';
            }, 1500);
        },
        error: function (xhr, status, error) {
            console.log(status);
            $('#error-modal-text').text("Salle crée sans emplacement ! ");
            $('#error-modal').modal('show');
        }
    });
}
function displayFileName() {
    const pdfFileInput = document.getElementById("file");
    const uploadLabel = document.querySelector('.upload');

    if (pdfFileInput.files.length > 0) {
        const pdfFileName = pdfFileInput.files[0].name;
        uploadLabel.innerHTML = pdfFileName;
    } else {
        uploadLabel.innerHTML = 'Choisir un fichier';
    }
}
function Show_PDF(Name) {
    var pdfUrl = '../assets/Emplacement/' + Name;
    var PDFiframe = document.getElementById("PDFiframe");
    PDFiframe.setAttribute("src", pdfUrl);
    $('#pdf-modal').modal('show');
}

//Fonction d'ajout nouveau types d'operations
function new_operation_set() {
    var link_new = $('#new-operation');
    var element = document.getElementById('new_operation_set');
    element.parentNode.removeChild(element);
    var label = $('<label>Ajouter un nouveau type <span class="login-danger">*</span></label>');
    var input = $('<input class="form-control" type="text" name="" id="new_operation_to_set" placeholder="Nouveau type ">');
    var newlink = $('<a style="margin : 15px; " href="#" onclick="submit_new_operation_set()" id=""><i class="fa-solid fa-pen-to-square m-r-5"></i>Ajouter </a>');
    link_new.append(label, input, newlink);
}
function submit_new_operation_set() {
    var new_operation_to_set = $('#new_operation_to_set').val();
    var newOperation = $('<option>').val(new_operation_to_set).text(new_operation_to_set).attr('selected', true);
    var Toperations_salle = $('#Toperations_salle');
    if (Toperations_salle.find('option[value="' + new_operation_to_set + '"]').length) {
        $('#error-modal-text').text("Ce type d'operation deja disponible !");
        $('#error-modal').modal('show');
        setTimeout(function () {
            $('#error-modal').modal('hide');
        }, 1500);
    }
    else
    {
        Toperations_salle.append(newOperation);
    }
}

//Fonctions pour les API des salles 
function delete_salle_btn(id)
{
    var deletebtn = document.getElementById('delete-modal-btn');
    deletebtn.onclick = function () {
        Submit_Delete_salle(id);
    };
    $('#delete-text').text("Voulez-vous vraiment supprimer cette salle ?");
    $('#delete_modal').modal('show');
}
function Submit_Delete_salle(id) {
    $.ajax({
        url: "/Salle/DeleteSalle/" + id,
        type: 'DELETE',
        success: function (response) {
            if (response.success) {
                $('#delete_modal').modal('hide');
                $('#success-modal-text').text(response.message);
                $('#success-modal').modal('show');
                setTimeout(function () {
                    window.location.href = '/Salle/SalleList';
                }, 2000);
            } else {
                $('#error-modal-text').text(response.message);
                $('#error-modal').modal('show');
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function changer_affectation(salle_id)
{
    $.ajax({
        url: '/Doctor/DoctorListJson',
        type: 'Get',
        success: function (doctors) {
            if (doctors.length !== 0) {
				var tbody = $('#doctor-table-body');
				tbody.empty();
                $.each(doctors, function (index, doctor) {
                    var row = $('<tr id="' + doctor.nom + doctor.Prenom + '" style="background-color: #f4f5fa;">');
                    row.append($('<td class="text-center" style="padding: 15px;">').text(doctor.matricule + " : " + doctor.nom+' '+doctor.prenom));
                    row.append('<td class="text-center"></a><a href="#" onclick="AffecterDocteur(' + salle_id + ',' + doctor.id + ')"><i class="fa-solid fa-pen-to-square m-r-5"></i> Affecter </a></td>');
					tbody.append(row);
				});
				$('#operation-list-modal').modal('show');
			}
			else {
				$('#success-modal-text').text("pas de Medecins trouvée ajouter une Medecin d'abord !");
				$('#success-modal').modal('show');
				setTimeout(function () {
					$('#success-modal').modal('hide');
				}, 2500);
			}
		},
        error(xhr, status, error) {
            console.log(error);
        }
    });


    
}
function AffecterDocteur(salle_id , id)
{
    $.ajax({
        url: "/Salle/SalleAffectation/",
        data: { salle_id : salle_id , id : id },
        type: 'POST',
        success: function (response) {
            if (response.success) {
                $('#operation-list-modal').modal('hide');
                $('#success-modal-text').text(response.message);
                $('#success-modal').modal('show');
                setTimeout(function () {
                    window.location.href = '/Salle/SalleList';
                }, 2000);
            } else {
                $('#error-modal-text').text(response.message);
                $('#error-modal').modal('show');
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}

//recherche dans liste des salle 
$(document).ready(function () {
    $('#search-salle-input').on('keyup', function () {
        var searchText = $(this).val().toLowerCase();
        $('#salle-table tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});