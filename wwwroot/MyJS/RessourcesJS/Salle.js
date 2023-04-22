
//**//Les fonction d'ajout permet d'ajouter une salle, des operations, affecter un medecin et le pdf de l'emplacement
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
                    $.ajax({
                        url: '/Salle/Submit_AddSalle',
                        type: 'POST',
                        data: { salle: salle, operations: operations },
                        success: function (response) {
                            if (response.success) {
                                add_pdf(pdf, response);
                                window.location.href = '/Salle/SalleList';
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
function add_pdf(pdf,response) {
    $.ajax({
        url: '/Salle/AddPDF',
        type: 'POST',
        data: pdf,
        processData: false,
        contentType: false,
        success: function (name) {
            $('#success-modal-text').text(response.message);
            $('#success-modal').modal('show');
            setTimeout(function () {
                $('#success-modal').modal('hide');
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
    const new_Emplacement = document.querySelector('.upload');

    if (pdfFileInput.files.length > 0) {
        const pdfFileName = pdfFileInput.files[0].name;
        new_Emplacement.innerHTML = pdfFileName;
    } else {
        new_Emplacement.innerHTML = 'Choisir un fichier';
    }
}
function displayFileNameedit() {
    const modal = document.getElementById("edit-salle-modal");
    const pdfFileInput = modal.querySelector("#file-edit");
    const new_Emplacement = modal.querySelector(".upload");
    if (pdfFileInput.files.length > 0) {
        const pdfFileName = pdfFileInput.files[0].name;
        new_Emplacement.innerHTML = pdfFileName;
    } else {
        new_Emplacement.innerHTML = 'Choisir un fichier';
    }
}
function Show_PDF(Name) {
    var pdfUrl = '../assets/Emplacement/' + Name;
    var PDFiframe = document.getElementById("PDFiframe");
    PDFiframe.setAttribute("src", pdfUrl);
    $('#pdf-modal').modal('show');
}
//Fonction d'ajout nouveau types d'operations pour une salle
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
    else {
        Toperations_salle.append(newOperation);
    }
}
//**//


//**//Fonctions pour les API des salles 
function delete_salle_btn(id) {
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

function edit_salle_btn(id, emplacement)
{
    var new_Emplacement = document.querySelector('.upload');
    new_Emplacement.innerHTML = emplacement;
    var nom = document.getElementById( id +'-nom');
    $('#Nom-edit').val(nom.textContent);
    $('#edit-salle-modal').modal('show');
    var editbtn = document.getElementById('submit-edit-btn');
    editbtn.onclick = function () {
        submit_edit_salle(id,emplacement);
    };
}
function submit_edit_salle(id, Old_Emplacement) {
    let pattern1 = /^.{6,}$/;
    var pdf = new FormData();
    var new_Emplacement = document.querySelector('.upload');
    var old_name= document.getElementById(id + '-nom');
    var old_emp = document.getElementById(id + '-emp');
    console.log("old_name.textContent" + old_name.textContent);
    console.log('Old_Emplacement'+Old_Emplacement);
    console.log("new_Emplacement.textContent"+new_Emplacement.textContent);
    pdf.append("pdf", document.getElementById("file-edit").files[0]);
    if (($('#Nom-edit').val() != old_name.textContent) || (pdf.get("pdf").name != null && pdf != null)) {
        if ($('#Nom-edit').val() && pattern1.test($('#Nom-edit').val())) {
            var salle =
            {
                Id: id,
                Nom: $('#Nom-edit').val(),
                Emplacement: new_Emplacement.textContent 
            };
            $.ajax({
                url: '/Salle/EditSalle',
                type: 'POST',
                data: { salle: salle, Old_Emplacement: Old_Emplacement},
                success: function (response) {
                    if (response.success) {
                        if ((pdf.get("pdf").name) != null && (pdf != null)) {
                            add_pdf(pdf, response);
                        }
                        old_name.textContent = salle.Nom;
                        old_emp.textContent = salle.Emplacement;
                        $('#success-modal-text').text(response.message);
                        $('#edit-salle-modal').modal('hide');
                        $('#success-modal').modal('show');
                        setTimeout(function () {
                            $('#success-modal').modal('hide');
                        }, 1500);
                    }
                    else {
                        $('#error-modal-text').text(response.message);
                        $('#edit-salle-modal').modal('hide');
                        $('#error-modal').modal('show');
                        setTimeout(function () {
                            $('#edit-salle-modal').modal('show');
                            $('#error-modal').modal('hide');
                        }, 2000);
                    }
                },
                error(xhr, status, error) {
                    console.log(error);
                }
            });
        }
        else {
            $('#error-modal-text').text("Choisir un nom qui contient au moin 6 caractere !");
            $('#edit-salle-modal').modal('hide');
            $('#error-modal').modal('show');
            setTimeout(function () {
                $('#edit-salle-modal').modal('show');
                $('#error-modal').modal('hide');
            }, 2000);
        }
    }
    else
    {
        $('#error-modal-text').text("Aucun changement !");
        $('#edit-salle-modal').modal('hide');
        $('#error-modal').modal('show');
        setTimeout(function () {
            $('#edit-salle-modal').modal('show');
            $('#error-modal').modal('hide');
        }, 1500);
    }
}
    
//**//


//**//Fonctions API pour la gestion des operations associée a une salle 
function operation_associee(id) {
    var addbtn = document.getElementById("add-op-btn");
    addbtn.setAttribute("onclick", "add_op_btn('" + id + "')");
    $.ajax({
        url: '/TypeOperation/TypeOperationList',
        type: 'GET',
        data: { SalleId: id },
        dataType: 'json',
        success: function (data) {
            var tbody = $('#operation-salle-table-body');
            tbody.empty();
            $.each(data.operations, function (index, operation) {
                var row = $('<tr style="background-color: #f4f5fa;">');
                row.append($('<td class="text-center" style="padding: 15px;">').text(operation.nom));
                row.append('<td class="text-center"></a><a href="#" onclick="delete_op_btn(' + operation.id + "," + operation.salleId + ')"><i class="fa fa-trash-alt m-r-5"></i> Supprimer </a></td>');
                tbody.append(row);
            });
            var countlist = document.getElementById(id + '-count');
            countlist.textContent = data.operations.length + " Type d'operation";
            $('#operation-salle-list-modal').modal('show');
        },
        error: function (xhr, status, error) {
            if (xhr.status == 403) {
                $('#error-modal-text').text("Tu n'a pas l'autorisation !");
                $('#error-modal').modal('show');
                setTimeout(function () {
                    $('#error-modal').modal('hide');
                }, 1500);
            }
        }
    });

}
function add_op_btn(salle_id) {
    var tbody = $('#operation-salle-table-body');
    var row = $('<tr style="background-color: #f4f5fa;">');
    row.append($('<td>').append($('<input>').attr('type', 'text').attr('id', 'for_operation_name')));
    row.append('<td class="text-center"><a href="#" onclick="submit_op_add(' + salle_id + ')"><i class="fa-solid fa-pen-to-square m-r-5"></i> Ajouter     |     </a><a href="#" onclick="operation_associee(' + salle_id + ')"><i class="fa fa-trash-alt m-r-5"></i>  Annuler </a></td>');
    tbody.append(row);
}
function submit_op_add(salle_id) {
    var nom = $('#for_operation_name').val();
    var op =
    {
        Nom: nom,
        Salleid: salle_id
    }
    $.ajax({
        url: '/TypeOperation/AddTypeOperation',
        type: 'POST',
        data: { operation: op },
        success: function (response) {
            if (response.success) {
                $('#success-modal-text').text("Type ajouter avec succées !");
                $('#success-modal').modal('show');
                $('#operation-salle-list-modal').modal('hide');
                setTimeout(function () {
                    $('#success-modal').modal('hide');
                    operation_associee(salle_id);
                }, 1500);
            }
            else {
                $('#error-modal-text').text(response.message);
                $('#error-modal').modal('show');
                setTimeout(function () {
                    $('#error-modal').modal('hide');
                    operation_associee(id);
                }, 1500);
            }
        },
        error: function (xhr, status, error) {
            $('#error-modal-text').text(status + "//" + error);
            $('#error-modal').modal('show');
            setTimeout(function () {
                $('#error-modal').modal('hide');
                operation_associee(id);
            }, 1500);
        }
    });
}
function delete_op_btn(op_id, salle_id) {
    $('#delete_modal').modal('show');
    $('#delete-text').text("Voulez-vous vraiment supprimer ce type d'operation ?");
    var deletebtn = document.getElementById('delete-modal-btn');
    deletebtn.onclick = function () {
        Submit_Delete_Operation(op_id, salle_id);
    };
}
function Submit_Delete_Operation(op_id, salle_id) {
    $.ajax({
        url: "/TypeOperation/DeleteTypeOperation/" + op_id,
        type: 'DELETE',
        success: function (response) {
            if (response.success) {
                $('#delete_modal').modal('hide');
                $('#success-modal-text').text(response.message);
                $('#success-modal').modal('show');
                $('#operation-salle-list-modal').modal('hide');
                setTimeout(function () {
                    $('#success-modal').modal('hide');
                    operation_associee(salle_id);
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
//**//



//**//Fonction d'affectaion de medecin
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
                $('#doctor-list-modal').modal('show');
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
                $('#doctor-list-modal').modal('hide');
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
//**//


//recherche dans liste des salle 
$(document).ready(function () {
    $('#search-salle-input').on('keyup', function () {
        var searchText = $(this).val().toLowerCase();
        $('#salle-table tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});
//Recherche dans la list des operations de salle
$(document).ready(function () {
    $('#operation-salle-list-modal input').on('keyup', function () {
        var searchText1 = $('#operation-salle-list-modal #search-input').val().toLowerCase();
        $('#operation-salle-list-modal tbody tr').filter(function () {
            var name = $(this).find('td:nth-child(1)').text().toLowerCase();
            $(this).toggle(name.indexOf(searchText1) > -1);
        });
    });
});

function goBack() {
    window.history.back();
}