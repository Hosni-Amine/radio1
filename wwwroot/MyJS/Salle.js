//recherche de la liste des salle 
$(document).ready(function () {
    $('#search-salle-input').on('keyup', function () {
        var searchText = $(this).val().toLowerCase();
        $('#salle-table tbody tr').filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
    });
});

//Fonctions pour les API des salles 
function Add_Salle()
{
    $('#add-salle-modal').modal('show');
}
function add_pdf(pdf,response)
{
    $.ajax({
        url: '/Salle/AddPDF',
        type: 'POST',
        data: pdf,
        processData: false,
        contentType: false,
        success: function (name) {
            console.log(name);
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
function submit_add_salle() {
    var pdf = new FormData();
    pdf.append("pdf", document.getElementById("file").files[0]);
    if ((pdf.get("pdf").name) != null && pdf != null) {
        var str = $('#add-salle-modal #Responsable').val();
        var op =
        {
            Nom : $('#add-salle-modal #Toperation').val()
        };         
        var salle =
        {
            Nom: $('#add-salle-modal #NomSalle').val(),
            Emplacement: $('#add-salle-modal #NomSalle').val(),
            Emplacement: pdf.get("pdf").name
        };
        console.log(salle);
        console.log(str);
        console.log(pdf.get("pdf").name);
        $.ajax({
            url: '/Salle/AddSalle',
            type: 'POST',
            data: { salle: salle, str: str, operation: op },
            success: function (response) {
                if (response.success) {
                    console.log(response);
                    add_pdf(pdf, response);
                }
                else {
                    console.log(response);
                    $('#add-salle-modal').modal('hide');
                    $('#error-modal-text').text(response.message);
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#error-modal').modal('hide');
                        $('#add-salle-modal').modal('show');
                    }, 1500);
                }
            },
            error(xhr, status, error) {
                console.log(error);
            }
        });
    }
    else
    {
        $('#add-salle-modal').modal('hide');
        $('#error-modal-text').text("Pas de fichier choisis !");
        $('#error-modal').modal('show');
        setTimeout(function () {
            $('#error-modal').modal('hide');
            $('#add-salle-modal').modal('show');
        }, 1500);
    }
}

function delete_salle_btn(id)
{
    $('#m-t-20').empty();
    var button = $('<button style="margin: 10px;">').attr('data-id', id).attr('type', 'submit').addClass('btn btn-danger').attr('id', 'delete-modal-btn').text('Oui').on('click', Submit_Delete_salle);
    var link = $('<a style="margin: 10px;">').attr('href', '#').addClass('btn btn-white').attr('data-bs-dismiss', 'modal').text('Non');
    $('#m-t-20').append(button);
    $('#m-t-20').append(link);
    $('#delete-text').text("Voulez-vous vraiment supprimer cette salle ?");
    $('#delete_modal').modal('show');
    console.log(id);
}
function Submit_Delete_salle() {
    var id = $('#delete-modal-btn').data('id');
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
                console.log('Error in response:', response);
                $('#error-modal-text').text(response.message);
                $('#error-modal').modal('show');
            }
        },
        error: function (error) {
            console.log(error);
        }
    });
}

function doctor_list()
{
    $.ajax({
        url: '/Doctor/DoctorList',
        type: 'Get',
        success: function (data) {
            console.log(data);
			var operations = data.operations;
			if (operations.length !== 0) {
				var distinctNames = operations.filter((operation, index, self) =>
					index === self.findIndex((op) => (
						op.nom === operation.nom
					))
				);
				var tbody = $('#operation-table-body');
				var last_td = $('#Nom-Op');
				tbody.empty();
				$.each(distinctNames, function (index, operation) {
					var row = $('<tr id="' + operation.nom + '" style="background-color: #f4f5fa;">');
					row.append($('<td class="text-center" style="padding: 15px;">').text(operation.nom));
					row.append('<td class="text-center"></a><a href="#" id="(' + operation.nom + ')"><i class="fa-solid fa-pen-to-square m-r-5"></i> Salles associeé </a></td>');
					tbody.append(row);
				});
				$('#operation-list-modal').modal('show');
			}
			else {
				$('#success-modal-text').text("pas d'opération trouvée ajouter une salle d'abord !");
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

