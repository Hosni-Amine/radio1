

//Fonction search pour la list des patient
$(document).ready(function () {
	$('#search-patient-input').on('keyup', function () {
		var searchText = $(this).val().toLowerCase();
		$('#patient-table tbody tr').filter(function () {
			$(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
		});
	});
});
function search_patient() {
	const tableContent = document.getElementById('patient-table').innerHTML;
	document.getElementById("table-copy").innerHTML = tableContent;
	$('#search-patient-modal').modal('show');
}
$(document).ready(function () {
	$('#search-patient-modal input').on('keyup', function () {
		var modal = $(this).closest('.modal'); // Get the closest modal
		var searchText1 = modal.find('#search-nom-pat').val().toLowerCase();
		var searchText2 = modal.find('#search-prenom-pat').val().toLowerCase();
		var searchText3 = modal.find('#search-telephone-pat').val().toLowerCase();
		modal.find('tbody tr').filter(function () {
			var nom = $(this).find('td:nth-child(1)').text().toLowerCase();
			var prenom = $(this).find('td:nth-child(2)').text().toLowerCase();
			var telephone = $(this).find('td:nth-child(3)').text().toLowerCase();
			$(this).toggle(nom.indexOf(searchText1) > -1 && prenom.indexOf(searchText2) > -1 && telephone.indexOf(searchText3) > -1 );
		});
	});
});


//Fonction API pour un patient
function delete_patient_btn(id) { 
	$('#delete-text').text("Voulez-vous vraiment supprimer ce Patient \n Les données relative sera automatiquement supprimer ?");
	var deletebtn = document.getElementById('delete-modal-btn');
	deletebtn.onclick = function () {
		Submit_Delete_Patient(id);
	};
	$('#delete_modal').modal('show');
}
function Submit_Delete_Patient(id) {
	$.ajax({
		url: "/Patient/DeletePatient/" + id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#profil-modal').modal('hide');
				$('#delete_modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					window.location.href = '/Patient/PatientList';
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

function add_patient_btn() {
	$.ajax({
		type: "GET",
		url: '/Account/AuthAdd',
		success: function (result) {
			$('#add-patient-modal').modal('show');
			$('#add-patient-modal #addontime').attr('value', 'true');
		},
		error: function (xhr, status, error) {
			if (xhr.status == 403) {
				$('#error-modal-text').text("Tu n'a pas l'autorisation !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
				}, 2000);
			}
			else if (xhr.status == 401) {
				$('#error-modal-text').text("Session expiré veuillez vous reconnecter !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					window.location.href = '/Account/Logout';
				}, 2000);
			}
		}
	});
}
function submit_add_patient() {
	event.preventDefault();
	let pattern1 = /^\+\d{11,}$/;
	if ($('#add-patient-modal #Telephonea').val() && pattern1.test($('#add-patient-modal #Telephonea').val())) {
		var Patient = {
			Prenom: $('#add-patient-modal #Prenom').val(),
			Nom: $('#add-patient-modal #Nom').val(),
			Telephone: $('#add-patient-modal #Telephonea').val(),
			DateN: $('#add-patient-modal #DateN').val(),
			LieuN: $('#add-patient-modal #LieuN').val(),
			Sexe: $('input[name="Sexe"]:checked').val(),
			SituationC: $('input[name="SituationCE"]:checked').val(),
			Adresse: $('#add-patient-modal #Adresse').val(),
			Ville: $('#add-patient-modal #Ville').val(),
		};
		console.log(Patient);
		if ($('#add-patient-modal #addontime').val() == 'true') {
			$.ajax({
				url: '/Patient/AddPatient',
				type: 'POST',
				data: { patient: Patient },
				success: function (data) {
					if (data.success) {
						$('#success-modal-text').text(data.message);
						$('#add-patient-modal').modal('hide');
						$('#success-modal').modal('show');
						if (window.location.href.includes('/Patient/PatientList')) {
							setTimeout(function () {
							location.reload();
							}, 1500);
						}
						else {
							Set_Patients_List(false);
							setTimeout(function () {
								$('#success-modal').modal('hide');
							}, 1500);
						}
						$('#success-modal').modal('hide');

					}
					else if (($('#add-patient-modal #addontime').val() == 'true')) {
						console.log('Error in response:', data);
						$('#error-modal-text').text(data.message);
						$('#add-patient-modal').modal('hide');
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
							$('#add-patient-modal').modal('show');
						}, 1500);
					}
				},
				error: function (xhr, status, error) {
					console.log('Error:', xhr, status, error);
					$('#error-modal-text').text('An error occurred: ' + error);
					$('#error-modal').modal('show');
					$('#add-patient-modal').modal('hide');
					setTimeout(function () {
						$('#error-modal').modal('hide');
						$('#add-patient-modal').modal('show');
					}, 1500);
				}
			});
		}
	}
	else {
		$('#error-modal-text').text("Choisir un numero de telephone internationale qui commance avec (+xxx) !");
		$('#add-patient-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#add-patient-modal').modal('show');
		}, 1500);
	}
}

function edit_patient_btn(id) {
	$.ajax({
		url: "/Patient/GetPatientById/" + id,
		type: "GET",
		dataType: "json",
		success: function (data) {
			if (data != null) {
				$("#edit-patient-modal #Id").val(data.id);
				$("#edit-patient-modal #Prenom").val(data.prenom);
				$("#edit-patient-modal #Nom").val(data.nom);
				$("#edit-patient-modal #telephone").val(data.telephone);
				$("#edit-patient-modal #DateN").val(data.dateN);
				$("#edit-patient-modal #LieuN").val(data.lieuN);
				if (data.situationC === "Marie") {
					document.getElementById('Marie').checked = true;
				}
				else if (data.situationC === "Divorce") {
					document.getElementById('Divorce').checked = true;
				}
				else if (data.situationC === "Celibataire") {
					document.getElementById('Celibatairee').checked = true;
				}
				else {
					document.getElementById('Veuf').checked = true;
				}
				if (data.sexe === "Homme") {
					document.getElementById('HSexe').checked = true;
				} else if (data.sexe === "Femme") {
					document.getElementById('FSexe').checked = true;
				}
				$("#edit-patient-modal #Adresse").val(data.adresse);
				$("#edit-patient-modal #Ville").val(data.ville);
				$('#edit-patient-modal').modal('show');
			}
			else {
				console.log(data);
				$('#edit-patient-modal').modal('hide');
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
function Submit_edit_patient() {
	event.preventDefault();
	let pattern1 = /^\+\d{12,}$/;
	if ($('#edit-patient-modal #telephone').val() && pattern1.test($('#edit-patient-modal #telephone').val())) {
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
			var patient = {
				Id: $('#edit-patient-modal #Id').val(),
				Prenom: $('#edit-patient-modal #Prenom').val(),
				Nom: $('#edit-patient-modal #Nom').val(),
				Telephone: $('#edit-patient-modal #telephone').val(),
				Email: $('#edit-patient-modal #Email').val(),
				DateN: $('#edit-patient-modal #DateN').val(),
				LieuN: $('#edit-patient-modal #LieuN').val(),
				Sexe: sex,
				SituationC: Situation,
				Adresse: $('#edit-patient-modal #Adresse').val(),
				Ville: $('#edit-patient-modal #Ville').val(),
			};
			console.log(patient);
			$.ajax({
				url: '/Patient/SubmitEditPatient',
				type: 'POST',
				data: patient,
				success: function (data) {
					console.log('Success:', data);
					if (data.success) {
						$('#profil-modal').modal('hide');
						$('#edit-patient-modal').modal('hide');
						$('#success-modal-text').text(data.message);
						$('#success-modal').modal('show');
						setTimeout(function () {
							window.location.href = '/Patient/PatientList';
						}, 1500);
					} else {
						console.log('Error in response:', data);
						$('#error-modal-text').text(data.message);
						$('#edit-patient-modal').modal('hide');
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
							$('#edit-patient-modal').modal('show');
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
		$('#error-modal-text').text("Choisir un numero de telephone internationale commence avec un '+' !");
		$('#edit-patient-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#edit-patient-modal').modal('show');
		}, 1500);
	}
}
