var UserName = localStorage.getItem("UserName");
var Id = localStorage.getItem("Id");
var Role = localStorage.getItem("Role");
document.getElementById("Id").textContent = "Utilisateur : " + UserName;
document.getElementById("Role").textContent = "Designation : " + Role;

function UnAuth()
{
    $('#error-modal-text').text("Vous ete pas autoriser !");
    $('#error-modal').modal('show');
    setTimeout(function () {
        window.location.href = '/Account/HomePage';
    }, 1500);
}