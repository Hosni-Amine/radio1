
//Initialisation des donn�es d'utilisateur
var UserName = localStorage.getItem("UserName");
var Id = localStorage.getItem("Id");
var Role = localStorage.getItem("Role");
document.getElementById("Id").textContent = "Utilisateur : " + UserName;
document.getElementById("Role").textContent = "Designation : " + Role;
//if (Role = "Admin") {
//		$.ajax({
//			url: "/Admin/State",
//			type: 'GET',
//			success: function (response) {
//				var medcount = document.getElementById('tech_count');
//				medcount.textContent = response.tech_Count;
//				var techcount = document.getElementById('med_count');
//				techcount.textContent = response.doctor_Count;
//				var sallecount = document.getElementById('salle_count');
//				sallecount.textContent = response.salle_Count;
//			},
//			error: function (error) {
//				console.log(error);
//			}
//		});
//}



//Fonction principale pour la verification de l'autorisation 
function CheckAuth(location) {
	var url = '/' + location + '/' + location + 'List';
	console.log(url);
	$.ajax({
		type: "GET",
		url: url,
		success: function (result) {
			window.location.href = url;
		},
		error: function (xhr, status, error) {
			console.log(xhr);
			console.log(status);
			console.log(error);
			if (xhr.status == 403) {
				$('#error-modal-text').text("Tu n'a pas l'autorisation !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
				}, 2000);
			}
			else if (xhr.status == 401)
			{
				$('#error-modal-text').text("Session expirer !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
				}, 2000);
			}
		}
	});
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
				url: '/Account/AddTech_User',
				type: 'POST',
				data: { Technicien: Technicien, User: User },
				success: function (data) {
					console.log('Success:', data);
					if (data.success) {
						$('#add-tuser-modal #addontime').val('false');
						$('#add-tuser-modal').modal('hide');
						$('#success-modal-text').text(data.message);
						$('#success-modal').modal('show');
						setTimeout(function () {
							$('#success-modal').modal('hide');
						}, 1500);

					}
					else if (($('#add-tuser-modal #addontime').val() == 'true')) {
						console.log('Error in response:', data);
						$('#error-modal-text').text(data.message);
						$('#add-tuser-modal').modal('hide');
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
							$('#add-tuser-modal').modal('show');
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
						url: '/Account/AddDoctor_User',
						type: 'POST',
						data: { Doctor: doctor, User: User },
						success: function (data) {
							console.log('Success:', data);
							if (data.success) {
								$('#addontime').val('false');
								$('#add-duser-modal').modal('hide');
								$('#success-modal-text').text(data.message);
								$('#success-modal').modal('show');
								setTimeout(function () {
									$('#success-modal').modal('hide');
								}, 1500);

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
				url: '/Account/AddAdmin_User',
				type: 'POST',
				data: { User: User },
				success: function (data) {
					console.log('Success:', data);
					if (data.success) {
						$('#add-auser-modal #addontime').val('false');
						$('#add-auser-modal').modal('hide');
						$('#success-modal-text').text(data.message);
						$('#success-modal').modal('show');
						setTimeout(function () {
							$('#success-modal').modal('hide');
						}, 1500);

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