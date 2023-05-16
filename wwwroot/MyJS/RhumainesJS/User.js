
//Initialisation des données d'utilisateur
var UserName = localStorage.getItem("UserName");
var Id = localStorage.getItem("Id");
var Role = localStorage.getItem("Role");
document.getElementById("Id_user").textContent = "Utilisateur : " + UserName;
document.getElementById("Role").textContent = "Designation : " + Role;
if (Role = "Admin") {
	if (window.location.href.includes('/Account/HomePage')) {
		$.ajax({
			url: "/Admin/State",
			type: 'GET',
			success: function (response) {
				var medcount = document.getElementById('tech_count');
				medcount.textContent = response.technicienCount;
				var techcount = document.getElementById('med_count');
				techcount.textContent = response.doctorsCount;
				var sallecount = document.getElementById('salle_count');
				sallecount.textContent = response.sallesCount;
				var sallecount = document.getElementById('app_count');
				sallecount.textContent = response.appareilsCount;
				var sallecount = document.getElementById('secretaire_count');
				sallecount.textContent = response.secretaireCount;
				var sallecount = document.getElementById('patient_count');
				sallecount.textContent = response.patientCount;
			},
			error: function (error) {
				var medcount = document.getElementById('tech_count');
				medcount.textContent = "0";
				var techcount = document.getElementById('med_count');
				techcount.textContent = "0";
				var sallecount = document.getElementById('salle_count');
				sallecount.textContent = "0";
				var sallecount = document.getElementById('app_count');
				sallecount.textContent = "0";
			}
		});
	}
}
   

//Fonction principale pour la verification de l'autorisation 
function CheckAuth(location) {
	var url = '/' + location + '/' + location + 'List';
	if (location === 'Appointment') {
		$.ajax({
			type: "HEAD",
			url: url,
			success: function (result) {
				window.location.href = url;
			},
			error: function (xhr, status, error) {
				CheckError(xhr);
			}
		});
	}
	else if (location === 'AppareilRadio') {
		$.ajax({
			type: "GET",
			url: "/AppareilRadio/AppareilRadioJson",
			success: function (result) {
				if (Object.keys(result).length !== 0) {
					window.location.href = url;
				}
				else {
					$('#success-modal-text').text("Tu dois d'abord d'ajouter une salle");
					$('#success-modal').modal('show');
					setTimeout(function () {
						$('#success-modal').modal('hide');
						window.location.href = '/Salle/SalleList';
					}, 2000);
				}
			},
			error: function (xhr, status, error) {
				CheckError(xhr);
			}
		});
	}
	else if (location === 'Patient') {
		$.ajax({
			type: "GET",
			url: "/Patient/PatientListJson",
			success: function (result) {
					window.location.href = url;
			},
			error: function (xhr, status, error) {
				CheckError(xhr);
			}
		});
	}
	else {
		$.ajax({
			type: "GET",
			url: '/Account/Auth',
			success: function (result) {
				window.location.href = url;
			},
			error: function (xhr, status, error) {
				CheckError(xhr);
			}
		});
	}
}

function CheckAuthAdd(location) {
	var url = '/' + location + '/Add'+ location ;
	$.ajax({
		type: "GET",
		url: '/Account/AuthAdd',
		success: function (result) {
			window.location.href = url;
		},
		error: function (xhr, status, error) {
			CheckError(xhr);
		}
	});
}

function CheckAuthAdd_Depends(location) {
	var url = '/' + location + '/Add' + location;
	if (location === 'Appointment') {
		$.ajax({
			type: "GET",
			url: '/Account/AuthAdd',
			success: function (result) {
				$.ajax({
					type: "GET",
					url: '/TypeOperation/TypeOperationList',
					dataType: 'json',
					success: function (result) {
						if (Object.keys(result).length !== 0) {
							window.location.href = url;
						}
						else {
							$('#success-modal-text').text("Tu dois ajouter au moins un type d'operation pour planifié un rendez-vous");
							$('#success-modal').modal('show');
							window.location.href = '/Doctor/DoctorList';
						}
					},
					error: function (xhr, status, error) {
						CheckError(xhr);
					}
				});
			},
			error: function (xhr, status, error) {
				CheckError(xhr);
			}
		});
	}
	else if (location === 'Salle')
	{
		$.ajax({
			type: "GET",
			url: '/Account/AuthAdd',
			success: function (result) {
				$.ajax({
					type: "GET",
					url: '/Doctor/DoctorListJson',
					dataType: 'json',
					success: function (result) {
						if (Object.keys(result).length !== 0) {
							window.location.href = url;
						}
						else {
							$('#success-modal-text').text("Tu dois ajouter des Medecin pour l'affecter a la salle");
							$('#success-modal').modal('show');
							setTimeout(function () {
								$('#success-modal').modal('hide');
								window.location.href = '/Doctor/DoctorList';
							}, 2000);
						}
					},
					error: function (xhr, status, error) {
						CheckError(xhr);
					}
				});
			},
			error: function (xhr, status, error) {
				CheckError(xhr);
			}
		});
	}
	else {
		CheckAuthAdd(location);
	}
}


function CheckError(xhr) {
	if (xhr.status == 403) {
		$('#error-modal-text').text("Tu n'a pas l'autorisation !");
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
		}, 2000);
	}
	else if (xhr.status == 401) {
		$('#error-modal-text').text("Session expire veuillez vous reconnecter !");
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			window.location.href = '/Account/Logout';
		}, 2000);
	}
}


//Ajout user avec role specifique 
function add_tuser_btn() {
	$('#add-tuser-modal').modal('show');
	$('#add-tuser-modal #addontime').attr('value', 'true');
}
$('#add-tuser-form').on('submit', function (event) {
	event.preventDefault();
	if ($('#add-tuser-modal #Password').val() == $('#add-tuser-modal #Password1').val()) {
		var Technicien = {
			Prenom: $('#add-tuser-modal #Prenom').val(),
			Nom: $('#add-tuser-modal #Nom').val(),
			Email: $('#add-tuser-modal #Email').val(),
			Sexe: $('input[name="Sexe"]:checked').val(),
		};
		var User = {
			UserName: $('#add-tuser-modal #UserName').val(),
			Password: $('#add-tuser-modal #Password').val(),
			Role: "Technicien",
		};
		console.log(Technicien);
		console.log(User);
		if ($('#add-tuser-modal #addontime').val() == 'true') {
			$.ajax({
				url: '/Admin/AddTech_User',
				type: 'POST',
				data: { Technicien: Technicien, User: User },
				success: function (data) {
					console.log('Success:', data);
					if (data.success) {
						$('#add-tuser-modal #addontime').val('false');
						$('#add-tuser-modal').modal('hide');
						$('#success-modal-text').text("" + data.message + " avec le UserName : <<" + User.UserName + ">>");
						$('#success-modal').modal('show');
						var sallecount = document.getElementById('tech_count');
						i = parseInt(sallecount.textContent);
						sallecount.textContent = i + 1;
					}
					else if (($('#add-tuser-modal #addontime').val() == 'true')) {
						console.log('Error in response:', data);
						$('#add-tuser-modal').modal('hide');
						$('#error-modal-text').text(data.message);
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
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
	else {
		$('#error-modal-text').text("Les deux mots de passe choisis ne sont pas identiques!");
		$('#add-tuser-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#add-tuser-modal').modal('show');
		}, 1500);
	}
});
function add_suser_btn() {
	$('#add-suser-modal').modal('show');
	$('#add-suser-modal #addontime').attr('value', 'true');
}
$('#add-suser-form').on('submit', function (event) {
	event.preventDefault();
	if ($('#add-suser-modal #Password').val() == $('#add-suser-modal #Password1').val()) {
		var Secretaire = {
			Prenom: $('#add-suser-modal #Prenom').val(),
			Nom: $('#add-suser-modal #Nom').val(),
			Email: $('#add-suser-modal #Email').val(),
			Sexe: $('input[name="Sexe"]:checked').val(),
		};
		var User = {
			UserName: $('#add-suser-modal #UserName').val(),
			Password: $('#add-suser-modal #Password').val(),
			Role: "Secretaire",
		};
		console.log(Secretaire);
		console.log(User);
		if ($('#add-suser-modal #addontime').val() == 'true') {
			$.ajax({
				url: '/Admin/AddSec_User',
				type: 'POST',
				data: { Secretaire: Secretaire, User: User },
				success: function (data) {
					console.log('Success:', data);
					if (data.success) {
						$('#add-suser-modal #addontime').val('false');
						$('#add-suser-modal').modal('hide');
						$('#success-modal-text').text("" + data.message + " avec le UserName : <<" + User.UserName + ">>");
						$('#success-modal').modal('show');
						var sallecount = document.getElementById('secretaire_count');
						i = parseInt(sallecount.textContent);
						sallecount.textContent = i+1;
					}
					else if (($('#add-suser-modal #addontime').val() == 'true')) {
						console.log('Error in response:', data);
						$('#add-suser-modal').modal('hide');
						$('#error-modal-text').text(data.message);
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
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
	else {
		$('#error-modal-text').text("Les deux mots de passe choisis ne sont pas identiques!");
		$('#add-suser-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#add-suser-modal').modal('show');
		}, 1500);
	}
});
function add_duser_btn() {
	$('#add-duser-modal').modal('show');
	$('#addontime').attr('value', 'true');
}
$('#add-duser-form').on('submit', function (event) {
	let pattern1 = /^\+\d{12,}$/;
	let pattern2 = /^\d{4,6}$/;
	event.preventDefault();
	if ($('#add-duser-modal #Telephonea').val() && pattern1.test($('#add-duser-modal #Telephonea').val())) {
		if ($('#add-duser-modal #CodePostale').val() && pattern2.test($('#add-duser-modal #CodePostale').val())) {
			if ($('#add-duser-modal #Password').val() == $('#add-duser-modal #Password1').val()) {
				var doctor = {
					Prenom: $('#add-duser-modal #Prenom').val(),
					Nom: $('#add-duser-modal #Nom').val(),
					Matricule: $('#add-duser-modal #Matricule').val(),
					Telephone: $('#add-duser-modal #Telephonea').val(),
					Email: $('#add-duser-modal #Email').val(),
					DateN: $('#add-duser-modal #DateN').val(),
					LieuN: $('#add-duser-modal #LieuN').val(),
					Sexe: $('input[name="Sexe"]:checked').val(),
					SituationC: $('input[name="SituationCE"]:checked').val(),
					Adresse: $('#add-duser-modal #Adresse').val(),
					Ville: $('#add-duser-modal #Ville').val(),
					CodePostal: $('#add-duser-modal #CodePostale').val()
				};
				var User = {
					UserName: $('#add-duser-modal #UserName').val(),
					Password: $('#add-duser-modal #Password').val(),
					Role: "Doctor",
				};
				console.log(doctor);
				if ($('#addontime').val() == 'true') {
					$.ajax({
						url: '/Admin/AddDoctor_User',
						type: 'POST',
						data: { Doctor: doctor, User: User },
						success: function (data) {
							console.log('Success:', data);
							if (data.success) {
								$('#addontime').val('false');
								$('#add-duser-modal').modal('hide');
								$('#success-modal-text').text("" + data.message + " avec le UserName : <<" + User.UserName + ">>");
								$('#success-modal').modal('show');
								var sallecount = document.getElementById('med_count');
								i = parseInt(sallecount.textContent);
								sallecount.textContent = i + 1;
							}
							else if (($('#addontime').val() == 'true')) {
								console.log('Error in response:', data);
								$('#error-modal-text').text(data.message);
								$('#add-duser-modal').modal('hide');
								$('#error-modal').modal('show');
								setTimeout(function () {
									$('#error-modal').modal('hide');
									$('#add-duser-modal').modal('show');
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
			else {
				$('#error-modal-text').text("Les deux mots de passe choisis ne sont pas identiques!");
				$('#add-duser-modal').modal('hide');
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					$('#add-duser-modal').modal('show');
				}, 1500);
			}
		}
		else {
			$('#error-modal-text').text("Choisir un code postale valide entre 4 et 6 chiffres !");
			$('#add-duser-modal').modal('hide');
			$('#error-modal').modal('show');
			setTimeout(function () {
				$('#error-modal').modal('hide');
				$('#add-duser-modal').modal('show');
			}, 1500);
		}
	}
	else {
		$('#error-modal-text').text("Choisir un numero de telephone internationale qui commance avec (+xxx) !");
		$('#add-duser-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#add-duser-modal').modal('show');
		}, 1500);
	}
});
function add_admin_btn() {
	$('#add-auser-modal').modal('show');
	$('#add-auser-modal #addontime').attr('value', 'true');
}
$('#add-auser-form').on('submit', function (event) {
	event.preventDefault();
	if ($('#add-auser-modal #Password').val() == $('#add-auser-modal #Password1').val()) {
		var User = {
			UserName: $('#add-auser-modal #UserName').val(),
			Password: $('#add-auser-modal #Password').val(),
			Role: "Admin",
		};
		console.log(User);
		if ($('#add-auser-modal #addontime').val() == 'true') {
			$.ajax({
				url: '/Admin/AddAdmin_User',
				type: 'POST',
				data: { User: User },
				success: function (data) {
					console.log('Success:', data);
					if (data.success) {
						$('#add-auser-modal #addontime').val('false');
						$('#add-auser-modal').modal('hide');
						$('#success-modal-text').text(""+data.message+" avec le UserName : <<"+User.UserName+">>");
						$('#success-modal').modal('show');
					}
					else if (($('#add-auser-modal #addontime').val() == 'true')) {
						console.log('Error in response:', data);
						$('#error-modal-text').text(data.message);
						$('#add-auser-modal').modal('hide');
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
							$('#add-auser-modal').modal('show');
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
	else {
		$('#error-modal-text').text("Les deux mots de passe choisis ne sont pas identiques!");
		$('#add-tuser-modal').modal('hide');
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
			$('#add-tuser-modal').modal('show');
		}, 1500);
	}
});
