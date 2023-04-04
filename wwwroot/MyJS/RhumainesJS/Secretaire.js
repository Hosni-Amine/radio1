
//Fonctions API pour les secretaires
function add_secret_btn() {
	$('#add-secret-modal').modal('show');
	$('#add-secret-modal #addontime').attr('value', 'true');
}
$('#add-secret-form').on('submit', function (event) {
	event.preventDefault();
	console.log($('#add-secret-form').serialize());
	var secretaire = {
		Prenom: $('#add-secret-modal #Prenom').val(),
		Nom: $('#add-secret-modal #Nom').val(),
		Email: $('#add-secret-modal #Email').val(),
		Sexe: $('input[name="Sexe"]:checked').val(),
	};
	console.log(secretaire);
	if ($('#add-secret-modal #addontime').val() == 'true') {
		$.ajax({
			url: '/secretaire/Addsecretaire',
			type: 'POST',
			data: secretaire,
			success: function (data) {
				console.log('Success:', data);
				if (data.success) {
					$('#add-secret-modal #addontime').val('false');
					$('#add-secret-modal').modal('hide');
					$('#success-modal-text').text(data.message);
					$('#success-modal').modal('show');
					setTimeout(function () {
						window.location.href = '/secretaire/secretaireList';
					}, 1500);

				}
				else if (($('#add-secret-modal #addontime').val() == 'true')) {
					console.log('Error in response:', data);
					$('#error-modal-text').text(data.message);
					$('#add-secret-modal').modal('hide');
					$('#error-modal').modal('show');
					setTimeout(function () {
						$('#error-modal').modal('hide');
						$('#add-secret-modal').modal('show');
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

function delete_secret_btn(id) {
	var deletebtn = document.getElementById('delete-modal-btn');
	deletebtn.onclick = function () {
		Submit_Delete_secretaire(id);
	};
	$('#delete-text').text("Voulez-vous vraiment supprimer ce secretaire ?");
	$('#delete_modal').modal('show');
}
function Submit_Delete_secretaire(id) {
	$.ajax({
		url: "/secretaire/Deletesecretaire/" + id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#delete_modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/secretaire/secretaireList';
				}, 2000);
			} else {
				console.log('Error in response:', response);
				$('#search-secret-modal').modal('hide');
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

function edit_secret_btn(id) {
	console.log(id);
	$.ajax({
		url: "/secretaire/GetsecretaireById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				console.log(data);
				console.log(data.nom)
				$("#edit-secret-modal #Id").val(data.id);
				$("#edit-secret-modal #Prenom").val(data.prenom);
				$("#edit-secret-modal #Nom").val(data.nom);
				$("#edit-secret-modal #Email").val(data.email);
				if (data.sexe === "Homme") {
					document.getElementById('HSexet').checked = true;
				} else if (data.sexe === "Femme") {
					document.getElementById('FSexet').checked = true;
				}
				$('#edit-secret-modal').modal('show');
			}
			else {
				console.log(data);
				$('#edit-secret-modal').modal('hide');
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
$('#edit-secret-form').on('submit', function (event) {
	event.preventDefault();
	if (document.getElementById('HSexet').checked) {
		sex = "Homme";
	}
	else if (document.getElementById('FSexet').checked) {
		sex = "Femme";
	}
	var secretaire = {
		Id: $('#edit-secret-modal #Id').val(),
		Prenom: $('#edit-secret-modal #Prenom').val(),
		Nom: $('#edit-secret-modal #Nom').val(),
		Email: $('#edit-secret-modal #Email').val(),
		Sexe: sex,
	};
	$.ajax({
		url: '/secretaire/SubmitEditsecretaire',
		type: 'POST',
		data: secretaire,
		success: function (data) {
			console.log('Success:', data);
			if (data.success) {
				$('#edit-secret-modal').modal('hide');
				$('#success-modal-text').text(data.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/secretaire/secretaireList';
				}, 1500);
			} else {
				console.log('Error in response:', data);
				$('#error-modal-text').text(data.message);
				$('#edit-secret-modal').modal('hide');
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					$('#edit-secret-modal').modal('show');
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
function profil_secret_btn(id) {
	$.ajax({
		url: "/secretaire/GetsecretaireById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				console.log(data);
				$("#profil-secret-modal #Name").text(data.nom + data.prenom);
				$("#profil-secret-modal #Nom").text(data.nom);
				$("#profil-secret-modal #Prenom").text(data.prenom);
				$("#profil-secret-modal #Sexe").text(data.sexe);
				$('#profil-secret-modal').modal('show');
			}
			else {
				console.log(data);
				$('#profil-secret-modal').modal('hide');
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






//Rechercher dans la liste des secretaires
$(document).ready(function () {
	$('#search-secret-input').on('keyup', function () {
		var searchText = $(this).val().toLowerCase();
		$('#secret-table tbody tr').filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
		});
	});
});
//Rechercher avancée avec modal
function search_secret() {
	const tableContent = document.getElementById('secret-table').innerHTML;
	document.getElementById("table-secret-copy").innerHTML = tableContent;
	$('#search-secret-modal').modal('show');
}
$(document).ready(function () {
	$('#search-secret-modal input').on('keyup', function () {
		var searchText1 = $('#search-secret-modal #search-nom').val().toLowerCase(); 
		var searchText2 = $('#search-secret-modal #search-prenom').val().toLowerCase(); 
		var searchText3 = $('#search-secret-modal #search-email').val().toLowerCase(); 
		$('#search-secret-modal tbody tr').filter(function () {
			var name = $(this).find('td:nth-child(1)').text().toLowerCase(); 
			var prenom = $(this).find('td:nth-child(2)').text().toLowerCase(); 
			var email = $(this).find('td:nth-child(3)').text().toLowerCase(); 
			$(this).toggle(name.indexOf(searchText1) > -1 && prenom.indexOf(searchText2) > -1 && email.indexOf(searchText3) > -1 );
		});
	});
});



function cancelsecret() {
	$('#add-secret-modal').modal('hide');
	$('#search-secret-modal').modal('hide');
	$('#edit-secret-modal').modal('hide');
}
function hideall() {
	$('#add-modal').modal('hide');
	$('#edit-modal').modal('hide');
	$('#success-modal').modal('hide');
	$('#error-modal').modal('hide');
}