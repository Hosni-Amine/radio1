function delete_tech_btn(id) {
	$('#m-t-20').empty();
	var button = $('<button style="margin: 10px;">').attr('data-id', id).attr('type', 'submit').addClass('btn btn-danger').attr('id', 'delete-modal-btn').text('Oui').on('click', Submit_Delete_technicien);
	var link = $('<a style="margin: 10px;">').attr('href', '#').addClass('btn btn-white').attr('data-bs-dismiss', 'modal').text('Non');
	$('#m-t-20').append(button);
	$('#m-t-20').append(link);
	$('#delete-text').text("Voulez-vous vraiment supprimer ce Technicien ?");
	$('#delete_modal').modal('show');
}

function Submit_Delete_technicien() {
	var id = $('#delete-modal-btn').data('id');
	$.ajax({
		url: "/Technicien/DeleteTechnicien/" + id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#delete_modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/Technicien/TechnicienList';
				}, 2000);
			} else {
				console.log('Error in response:', response);
				$('#error-modal-text').text(response.message);
				$('#error-modal').modal('show');
			}
		},
		error: function (error) {
			console.log(error);
			// handle error response here
		}
	});
}


function add_tech_btn() {
	$('#add-tech-modal').modal('show');
	$('#add-tech-modal #addontime').attr('value', 'true');
}
$('#add-tech-form').on('submit', function (event) {
	event.preventDefault();
	console.log($('#add-tech-form').serialize());
	var Technicien = {
		Prenom: $('#add-tech-modal #Prenom').val(),
		Nom: $('#add-tech-modal #Nom').val(),
		Sexe: $('input[name="Sexe"]:checked').val(),
	};
	console.log(Technicien);
	if ($('#add-tech-modal #addontime').val() == 'true') {
		$.ajax({
			url: '/Technicien/AddTechnicien',
			type: 'POST',
			data: Technicien,
			success: function (data) {
				console.log('Success:', data);
				if (data.success) {
					$('#add-tech-modal #addontime').val('false');
					$('#add-tech-modal').modal('hide');
					$('#success-modal-text').text(data.message);
					$('#success-modal').modal('show');
					setTimeout(function () {
						window.location.href = '/Technicien/TechnicienList';
					}, 1500);

				}
				else if (($('#add-tech-modal #addontime').val() == 'true')) {
					console.log('Error in response:', data);
					$('#error-modal-text').text(data.message);
					$('#add-tech-modal').modal('hide');
					$('#error-modal').modal('show');
					setTimeout(function () {
						$('#error-modal').modal('hide');
						$('#add-tech-modal').modal('show');
					}, 1500);

				}
			},
			error: function (xhr, status, error) {
				console.log('Error:', xhr, status, error);
				$('#error-modal-text').text('An error occurred: ' + error);
				$('#error-modal').modal('show');
			}
		});
	}
});



function edit_tech_btn(id) {
	console.log(id);
	$.ajax({
		url: "/Technicien/GetTechnicienById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				console.log(data);
				console.log(data.nom)
				$("#edit-tech-modal #Id").val(data.id);
				$("#edit-tech-modal #Prenom").val(data.prenom);
				$("#edit-tech-modal #Nom").val(data.nom);
				if (data.sexe === "Homme") {
					document.getElementById('HSexet').checked = true;
				} else if (data.sexe === "Femme") {
					document.getElementById('FSexet').checked = true;
				}
				$('#edit-tech-modal').modal('show');
			}
			else {
				console.log(data);
				$('#edit-tech-modal').modal('hide');
				$('#error-modal-text').text("Erreur d'identification");
				$('#error-modal').modal('show');
			}
		},
		error: function (xhr, status, error) {
			console.log('Error:', xhr, status, error);
			$('#error-modal-text').text('An error occurred: ' + error);
			$('#error-modal').modal('show');
		}
	});
}
$('#edit-tech-form').on('submit', function (event) {
	event.preventDefault();
	if (document.getElementById('HSexet').checked) {
		sex = "Homme";
	}
	else if (document.getElementById('FSexet').checked) {
		sex = "Femme";
	}
	var technicien = {
		Id: $('#edit-tech-modal #Id').val(),
		Prenom: $('#edit-tech-modal #Prenom').val(),
		Nom: $('#edit-tech-modal #Nom').val(),
		Sexe: sex,
	};
	console.log(technicien);
	$.ajax({
		url: '/Technicien/SubmitEditTechnicien',
		type: 'POST',
		data: technicien,
		success: function (data) {
			console.log('Success:', data);
			if (data.success) {
				$('#edit-tech-modal').modal('hide');
				$('#success-modal-text').text(data.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/Technicien/TechnicienList';
				}, 1500);
			} else {
				console.log('Error in response:', data);
				$('#error-modal-text').text(data.message);
				$('#edit-tech-modal').modal('hide');
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					$('#edit-tech-modal').modal('show');
				}, 1500);
			}
		},
		error: function (xhr, status, error) {
			console.log('Error:', xhr, status, error);
			$('#error-modal-text').text('An error occurred: ' + error);
			$('#error-modal').modal('show');
			console.log
		}

	});
});



function profil_tech_btn(id) {
	$.ajax({
		url: "/Technicien/GetTechnicienById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				console.log(data);
				$("#profil-tech-modal #Name").text(data.nom + data.prenom);
				$("#profil-tech-modal #Nom").text(data.nom);
				$("#profil-tech-modal #Prenom").text(data.prenom);
				$("#profil-tech-modal #Sexe").text(data.sexe);
				$('#profil-tech-modal').modal('show');
			}
			else {
				console.log(data);
				$('#profil-tech-modal').modal('hide');
				$('#error-modal-text').text("Une erreur lors de de l'identification de Medecin");
				$('#error-modal').modal('show');
			}
		},
		error: function (xhr, status, error) {
			console.log('Error:', xhr, status, error);
			$('#error-modal-text').text('An error occurred: ' + error);
			$('#error-modal').modal('show');
		}
	});
}



function canceltech() {
	$('#add-tech-modal').modal('hide');
	$('#search-tech-modal').modal('hide');
	$('#edit-tech-modal').modal('hide');
}
function hideall() {
	$('#add-modal').modal('hide');
	$('#edit-modal').modal('hide');
	$('#success-modal').modal('hide');
	$('#error-modal').modal('hide');
}




$(document).ready(function () {
	$('#search-tech-input').on('keyup', function () {
		var searchText = $(this).val().toLowerCase();
		$('#tech-table tbody tr').filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
		});
	});
});




function search_tech() {
	const tableContent = document.getElementById('tech-table').innerHTML;
	document.getElementById("table-tech-copy").innerHTML = tableContent;
	$('#search-tech-modal').modal('show');
}
$(document).ready(function () {
	$('#search-tech-modal input').on('keyup', function () {
		var searchText1 = $('#search-tech-modal #search-nom').val().toLowerCase(); 
		var searchText2 = $('#search-tech-modal #search-prenom').val().toLowerCase(); 
		$('#search-tech-modal tbody tr').filter(function () {
			var name = $(this).find('td:nth-child(1)').text().toLowerCase(); 
			var prenom = $(this).find('td:nth-child(2)').text().toLowerCase(); 
			$(this).toggle(name.indexOf(searchText1) > -1 && prenom.indexOf(searchText2) > -1);
		});
	});
});