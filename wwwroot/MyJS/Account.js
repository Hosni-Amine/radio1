
//Fonction de Login pour le page d'accuill

function Signin() {
	$('#sign-in-modal').modal('show');	
}

function Submit_Login() {
	var user = {
		UserName: $('#Login-form #Email').val(),
		password: $('#Login-form #password').val()
	};
	$.ajax({
		url: "/Account/Login",
		type: 'POST',
		data: user,
		success: function (response) {
			console.log(response);
			if (response.user.role != null)
			{
				console.log(response.user);
				localStorage.setItem("Id", response.user.id);
				localStorage.setItem("Role", response.user.role);
				localStorage.setItem("UserName", response.user.userName);
				$('#success-modal-text').text("vous etes bien identifie");
				$('#sign-in-modal').modal('hide');
				$('#success-modal').modal('show');
				setTimeout(function () {
					$('#success-modal').modal('hide');
					window.location.href = '/Account/HomePage';
				}, 1500);
			}
			else
			{
				$('#error-modal-text').text("Mot de passe Incorrect !");
				$('#sign-in-modal').modal('hide');
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					$('#sign-in-modal').modal('show');
				}, 1500);
			}
		},
		error: function (error, xhr) {
			console.log(error);
			$('#error-modal-text').text("Nom d'utilisateur incorrect !");
			$('#sign-in-modal').modal('hide');
			$('#error-modal').modal('show');
			setTimeout(function () {
				$('#error-modal').modal('hide');
				$('#sign-in-modal').modal('show');
			}, 1500);
		}
	})
}

function hideall() {
	$('#add-duser-modal').modal('hide');
	$('#success-modal').modal('hide');
	$('#error-modal').modal('hide');
}

