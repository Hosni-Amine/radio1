
//Fonction search pour les list des medecins
$(document).ready(function () {
	$('#search-doctor-input').on('keyup', function () {
		var searchText = $(this).val().toLowerCase();
		$('#doctor-table tbody tr').filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
		});
	});
});
function search_doctor() {
	const tableContent = document.getElementById('doctor-table').innerHTML;
	document.getElementById("table-copy").innerHTML = tableContent;
	$('#search-doctor-modal').modal('show');
}
$(document).ready(function () {

	$('#search-doctor-modal input').on('keyup', function () {
		var modal = $(this).closest('.modal'); // Get the closest modal
		var searchText1 = modal.find('#search-nom-doc').val().toLowerCase();
		var searchText2 = modal.find('#search-prenom-doc').val().toLowerCase();
		var searchText3 = modal.find('#search-matricule-doc').val().toLowerCase();
		var searchText4 = modal.find('#search-email-doc').val().toLowerCase();
		modal.find('tbody tr').filter(function () {
			var nom = $(this).find('td:nth-child(1)').text().toLowerCase();
			var prenom = $(this).find('td:nth-child(2)').text().toLowerCase();
			var matricule = $(this).find('td:nth-child(3)').text().toLowerCase();
			var email = $(this).find('td:nth-child(4)').text().toLowerCase();
			$(this).toggle(nom.indexOf(searchText1) > -1 && prenom.indexOf(searchText2) > -1 && matricule.indexOf(searchText3) > -1 && email.indexOf(searchText4) > -1);
		});
	});
});


//Fonction API pour le medecin
function delete_doctor_btn(id) {
	$('#delete-text').text("Voulez-vous vraiment supprimer ce Medecin ?");
	var deletebtn = document.getElementById('delete-modal-btn');
	deletebtn.onclick = function () {
		Submit_Delete_doctor(id);
	};
	$('#delete_modal').modal('show');
}
function Submit_Delete_doctor(id) {
	$.ajax({
		url: "/Doctor/DeleteDoctor/" + id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#profil-modal').modal('hide');
				$('#delete_modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/Doctor/DoctorList';
				}, 1500);
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

function add_doctor_btn() {
	$('#add-modal').modal('show');
	$('#addontime').attr('value', 'true');
}
$('#add-doctor-form').on('submit', function (event) {
	let pattern1 = /^\+\d{11,}$/;
	let pattern2 = /^\d{4,6}$/;
	event.preventDefault();
	if ($('#add-modal #Telephonea').val() && pattern1.test($('#add-modal #Telephonea').val()))
	{
		if ($('#add-modal #CodePostale').val() && pattern2.test($('#add-modal #CodePostale').val()))
		{
			var doctor = {
				Prenom: $('#add-modal #Prenom').val(),
				Nom: $('#add-modal #Nom').val(),
				Matricule: $('#add-modal #Matricule').val(),
				Telephone: $('#add-modal #Telephonea').val(),
				Email: $('#add-modal #Email').val(),
				DateN: $('#add-modal #DateN').val(),
				LieuN: $('#add-modal #LieuN').val(),
				Sexe: $('input[name="Sexe"]:checked').val(),
				SituationC: $('input[name="SituationCE"]:checked').val(),
				Adresse: $('#add-modal #Adresse').val(),
				Ville: $('#add-modal #Ville').val(),
				CodePostal: $('#add-modal #CodePostale').val()
			};
			console.log(doctor);
			if ($('#addontime').val() == 'true') {
				$.ajax({
					url: '/Doctor/AddDoctor',
					type: 'POST',
					data: doctor,
					success: function (data) {
						console.log('Success:', data);
						if (data.success) {
							$('#addontime').val('false');
							$('#add-modal').modal('hide');
							$('#success-modal-text').text(data.message);
							$('#success-modal').modal('show');
							setTimeout(function () {
								window.location.href = '/Doctor/DoctorList=';
							}, 1500);

						}
						else if (($('#addontime').val() == 'true')) {
							console.log('Error in response:', data);
							$('#error-modal-text').text(data.message);
							$('#add-modal').modal('hide');
							$('#error-modal').modal('show');
							setTimeout(function () {
								$('#error-modal').modal('hide');
								$('#add-modal').modal('show');
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
		}
		else
		{
			$('#error-modal-text').text("Choisir un code postale valide entre 4 et 6 chiffres !");
			$('#add-modal').modal('hide');
			$('#error-modal').modal('show');
			setTimeout(function () {
				$('#error-modal').modal('hide');
				$('#add-modal').modal('show');
			}, 1500);
		}
	}
	else { 
		$('#error-modal-text').text("Choisir un numero de telephone internationale qui commance avec (+xxx) !");
		$('#add-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#add-modal').modal('show');
		}, 1500);
	}
});

function edit_doctor_btn(id) {
	console.log(id);
	$.ajax({
		url: "/Doctor/GetDoctorById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				console.log(data);
				console.log(data.situationC);
				$("#edit-modal #Id").val(data.id);
				$("#edit-modal #Prenom").val(data.prenom);
				$("#edit-modal #Nom").val(data.nom);
				$("#edit-modal #Matricule").val(data.matricule);
				$("#edit-modal #telephone").val(data.telephone);
				$("#edit-modal #Email").val(data.email);
				$("#edit-modal #DateN").val(data.dateN);
				$("#edit-modal #LieuN").val(data.lieuN);
				if (data.situationC === "Marie") {
					document.getElementById('Marie').checked = true;
				}
				else if (data.situationC === "Divorce") {
					document.getElementById('Divorce').checked = true;
				}
				else if (data.situationC === "Celibataire") {
					document.getElementById('Celibatairee').checked = true;
				}
				else{
					document.getElementById('Veuf').checked = true;
				}
				if (data.sexe === "Homme") {
					document.getElementById('HSexe').checked = true;
				} else if (data.sexe === "Femme") {
					document.getElementById('FSexe').checked = true;
				}
				$("#edit-modal #Adresse").val(data.adresse);
				$("#edit-modal #Ville").val(data.ville);
				$("#edit-modal #CodePostal").val(data.codePostal);
				$('#edit-modal').modal('show');
			}
			else {
				console.log(data);
				$('#edit-modal').modal('hide');
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
$('#edit-doctor-form').on('submit', function (event) {
	event.preventDefault();
	let pattern1 = /^\+\d{12,}$/;
	let pattern2 = /^\d{4,6}$/;
	event.preventDefault();
	if ($('#edit-modal #telephone').val() && pattern1.test($('#edit-modal #telephone').val())) {
		if ($('#edit-modal #CodePostal').val() && pattern2.test($('#edit-modal #CodePostal').val())) {
			if (document.getElementById('HSexe').checked) {
				sex = "Homme";
			}
			else if (document.getElementById('FSexe').checked) {
				sex = "Femme";
			}
			if (document.getElementById('Celibatairee').checked) {
				Situation = "Celibataire";
			}
			else if (document.getElementById('Marie').checked) {
				Situation = "Marie";
			}
			else if (document.getElementById('Divorce').checked) {
				Situation = "Divorce";
			}
			else {
				Situation = "Veuf"
			}
			var user = {

			};
			var doctor = {
				Id: $('#edit-modal #Id').val(),
				Prenom: $('#edit-modal #Prenom').val(),
				Nom: $('#edit-modal #Nom').val(),
				Matricule: $('#edit-modal #Matricule').val(),
				Telephone: $('#edit-modal #telephone').val(),
				Email: $('#edit-modal #Email').val(),
				DateN: $('#edit-modal #DateN').val(),
				LieuN: $('#edit-modal #LieuN').val(),
				Sexe: sex,
				SituationC: Situation,
				Adresse: $('#edit-modal #Adresse').val(),
				Ville: $('#edit-modal #Ville').val(),
				CodePostal: $('#edit-modal #CodePostal').val()
			};
			console.log(doctor);
			$.ajax({
				url: '/Doctor/SubmitEditDoctor',
				type: 'POST',
				data: doctor,
				success: function (data) {
					console.log('Success:', data);
					if (data.success) {
						$('#profil-modal').modal('hide');
						$('#edit-modal').modal('hide');
						$('#success-modal-text').text(data.message);
						$('#success-modal').modal('show');
						setTimeout(function () {
							window.location.href = '/Doctor/DoctorList';
						}, 1500);
					} else {
						console.log('Error in response:', data);
						$('#error-modal-text').text(data.message);
						$('#edit-modal').modal('hide');
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
							$('#edit-modal').modal('show');
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
		else {
			$('#error-modal-text').text("Choisir un code postale valide entre 4 et 6 chiffres !");
			$('#edit-modal').modal('hide');
			$('#error-modal').modal('show');
			setTimeout(function () {
				$('#error-modal').modal('hide');
				$('#edit-modal').modal('show');
			}, 1500);
		}
	}
	else {
		$('#error-modal-text').text("Choisir un numero de telephone internationale commence avec un '+' !");
		$('#edit-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#edit-modal').modal('show');
		}, 1500);
	}
});

function profil_doctor_btn(id) {
	$.ajax({
		url: "/Doctor/GetDoctorById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				console.log(data);
				$("#profil-modal #Name").text(data.nom + data.prenom);
				$("#profil-modal #matricule").text(data.matricule);
				$("#profil-modal #telephone").text(data.telephone);
				$("#profil-modal #email").text(data.email);
				$("#profil-modal #dateN").text(data.dateN);
				$("#profil-modal #lieuN").text(data.lieuN);
				$("#profil-modal #situationC").text(data.situationC);
				$("#profil-modal #VilleAdresse").text(data.adresse+" / " + data.ville);
				$("#profil-modal #codePostal").text(data.codePostal);
				$("#profil-modal #sexe").text(data.sexe);
				$('#profil-modal').modal('show');
				var button = document.getElementById("delete-profil-btn");
				button.setAttribute("onclick", "delete_doctor_btn('" + data.id + "')");
				var button = document.getElementById("edit-profil-btn");
				button.setAttribute("onclick", "edit_doctor_btn('" + data.id + "')");
			}
			else {
				console.log(data);
				$('#profil-modal').modal('hide');
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

function cancel() {
	$('#add-modal').modal('hide');
	$('#edit-modal').modal('hide');
	$('#search-doctor-modal').modal('hide');
}
function hideall() {
	$('#add-modal').modal('hide');
	$('#edit-modal').modal('hide');
	$('#success-modal').modal('hide');
	$('#error-modal').modal('hide');
}




