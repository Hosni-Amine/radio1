
//Fonctions API pour les techniciens
function add_tech_btn() {
	$('#add-tech-modal').modal('show');
	$('#add-tech-modal #addontime').attr('value', 'true');
}
$('#add-tech-form').on('submit', function (event) {
	event.preventDefault();
	console.log($('#add-tech-form').serialize());
	var technicien = {
		Prenom: $('#add-tech-modal #Prenom').val(),
		Nom: $('#add-tech-modal #Nom').val(),
		Email: $('#add-tech-modal #Email').val(),
		Sexe: $('input[name="Sexe"]:checked').val(),
	};
	console.log(technicien);
	if ($('#add-tech-modal #addontime').val() == 'true') {
		$.ajax({
			url: '/technicien/Addtechnicien',
			type: 'POST',
			data: technicien,
			success: function (data) {
				console.log('Success:', data);
				if (data.success) {
					$('#add-tech-modal #addontime').val('false');
					$('#add-tech-modal').modal('hide');
					$('#success-modal-text').text(data.message);
					$('#success-modal').modal('show');
					setTimeout(function () {
						window.location.href = '/technicien/technicienList';
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


function delete_tech_btn(id) {
	var deletebtn = document.getElementById('delete-modal-btn');
	deletebtn.onclick = function () {
		Submit_Delete_technicien(id);
	};
	$('#delete-text').text("Voulez-vous vraiment supprimer ce technicien ?");
	$('#delete_modal').modal('show');
}
function Submit_Delete_technicien(id) {
	$.ajax({
		url: "/technicien/Deletetechnicien/" + id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#delete_modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/technicien/technicienList';
				}, 2000);
			} else {
				console.log('Error in response:', response);
				$('#search-tech-modal').modal('hide');
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

function edit_tech_btn(id) {
	console.log(id);
	$.ajax({
		url: "/technicien/GettechnicienById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				console.log(data);
				console.log(data.nom)
				$("#edit-tech-modal #Id").val(data.id);
				$("#edit-tech-modal #Prenom").val(data.prenom);
				$("#edit-tech-modal #Nom").val(data.nom);
				$("#edit-tech-modal #Email").val(data.email);
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
		Email: $('#edit-tech-modal #Email').val(),
		Sexe: sex,
	};
	$.ajax({
		url: '/technicien/SubmitEdittechnicien',
		type: 'POST',
		data: technicien,
		success: function (data) {
			console.log('Success:', data);
			if (data.success) {
				$('#edit-tech-modal').modal('hide');
				$('#success-modal-text').text(data.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/technicien/technicienList';
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
		url: "/technicien/GettechnicienById/",
		type: "GET",
		data: {id : id },
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






//Rechercher dans la liste des techniciens
$(document).ready(function () {
	$('#search-tech-input').on('keyup', function () {
		var searchText = $(this).val().toLowerCase();
		$('#tech-table tbody tr').filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
		});
	});
});
//Rechercher avancée avec modal
function search_tech() {
	const tableContent = document.getElementById('tech-table').innerHTML;
	document.getElementById("table-tech-copy").innerHTML = tableContent;
	$('#search-tech-modal').modal('show');
}
$(document).ready(function () {
	$('#search-tech-modal input').on('keyup', function () {
		var modal = $(this).closest('.modal');
		var searchText1 = modal.find('#search-nom-tech').length ? modal.find('#search-nom-tech').val().toLowerCase() : '';
		var searchText2 = modal.find('#search-prenom-tech').length ? modal.find('#search-prenom-tech').val().toLowerCase() : '';
		var searchText3 = modal.find('#search-email-tech').length ? modal.find('#search-email-tech').val().toLowerCase() : '';
		modal.find('tbody tr').filter(function () {
			var name = $(this).find('td:nth-child(1)').text().toLowerCase();
			var prenom = $(this).find('td:nth-child(2)').text().toLowerCase();
			var email = $(this).find('td:nth-child(3)').text().toLowerCase();
			$(this).toggle(name.indexOf(searchText1) > -1 && prenom.indexOf(searchText2) > -1 && email.indexOf(searchText3) > -1);
		});
	});
});




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