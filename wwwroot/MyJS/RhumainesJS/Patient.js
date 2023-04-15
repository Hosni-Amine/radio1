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

function submit_add_patient()
{
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
					data: { patient : Patient },
					success: function (data) {
						if (data.success) {
							Set_Patients_List(false);
							$('#success-modal-text').text(data.message);
							$('#add-patient-modal').modal('hide');
							$('#success-modal').modal('show');
							setTimeout(function () {
								$('#success-modal').modal('hide');
							}, 1500);
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