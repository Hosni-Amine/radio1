
function toggleRows(button) {
	event.preventDefault();
    var table = button.parentNode.nextElementSibling.querySelector("table");
    var headerRow = table.querySelector("thead tr");
    var bodyRows = table.querySelectorAll("tbody tr.row-to-hide");

    if (headerRow.style.display == "none") {
        headerRow.style.display = "table-row";
        button.textContent = "Masquer";
    } else {
        headerRow.style.display = "none";
		button.textContent = "Afficher";
    }

    bodyRows.forEach(function (row) {
        row.style.display = (row.style.display == "none") ? "table-row" : "none";
    });
}


function delete_appareil_btn(id) {
	var deletebtn = document.getElementById('delete-modal-btn');
	deletebtn.onclick = function () {
		Submit_delete_appareil(id);
	};
	$('#delete-text').text("Voulez-vous vraiment supprimer cette Appareil ?");
	$('#delete_modal').modal('show');
}
function Submit_delete_appareil(id) {
	$.ajax({
		url: "/AppareilRadio/DeleteAppareilRadio/" + id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#delete_modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#search-tech-modal').modal('hide');
				$('#success-modal').modal('show');
				$('#' + id).remove();
				setTimeout(function () {
					$('#success-modal').modal('hide');
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

function edit_appareil_btn(id) {
	$.ajax({
		url: '/AppareilRadio/GetById',
		type: 'GET',
		data: { Id: id },
		success: function (response) {
			$.ajax({
				url: '/TypeOperation/TypeOperationList',
				type: 'GET',
				data: { ForApp: false , ForSalle: true, SalleId: response.salleId },
				success: function (list) {
					console.log(list);
					var selectops = $('#Toperations_edit_app');
					selectops.empty();
					$.each(list.operations, function (index, op) {
						var option = $('<option>').val(op.nom).text(op.nom);
						$.each(response.operations, function (index, op_selected) {
							if (op_selected.nom === op.nom)
							{
								option = $('<option selected>').val(op.nom).text(op.nom);
							}
						});
						selectops.append(option);
					});
					var selectM = $('#Maintenance_edit');
					selectM.empty();
					if (response.maintenance === 1) {
						var option1 = $('<option selected>').val('1').text('Mise en service');
						var option2 = $('<option>').val('0').text('Mise en maintenance ');
					}
					else {
						var option1 = $('<option>').val('1').text('Mise en service');
						var option2 = $('<option selected>').val('0').text('Mise en maintenance ');
					}
					selectM.append(option1);
					selectM.append(option2);
					$('#NumSerie_edit').val(response.numSerie);
					var editbtn = document.getElementById('submit-edit-app-btn');
					editbtn.onclick = function () {
						submit_edit_app(response.id, response.salleId);
					}
					$('#content-1').hide();
					$('#content-add').hide();
					$('#content-edit').show();
				},
				error: function (xhr, status, error) {
					$('#error-modal-text').text(status + xhr.status + error);
					$('#error-modal').modal('show');
					if (xhr.status == 401) {
						$('#error-modal-text').text("Session expirer !");
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
						}, 2000);
					}
				}
			});
		},
		error: function (xhr, status, error) {
			$('#error-modal-text').text(status + xhr.status + error);
			$('#error-modal').modal('show');
			if (xhr.status == 401) {
				$('#error-modal-text').text("Session expirer !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
				}, 2000);
			}
		}
	}); 
}
function submit_edit_app(app_id, salle_id) {
	let pattern1 = /^.{6,}$/;
	var Ops = $('#Toperations_edit_app').val();
	var NumSerie = $('#NumSerie_edit').val();
	if (NumSerie && pattern1.test(NumSerie)) {
		if (Ops.length !== 0) {
			var operations = [];
			Ops.forEach(function (op) {
				var object = { nom: op, AppareilRadioId: app_id };
				operations.push(object);
			});
			var app =
			{
				Id : app_id,
				NumSerie: NumSerie,
				Operations: operations,
				SalleId : salle_id,
				Maintenance: parseInt($('#Maintenance_edit').val()),
			};
			console.log(app);
			$.ajax({
				url: '/AppareilRadio/EditAppareilRadio',
				type: 'POST',
				data: { appareilradio: app },
				success: function (response) {
					if (response.success) {
						$('#success-modal-text').text(response.message);
						$('#success-modal').modal('show');
						setTimeout(function () {
							$('#success-modal').modal('hide');
						}, 1500);
						window.location.href = '/AppareilRadio/AppareilRadioList';
					}
					else {
						$('#error-modal-text').text(response.message);
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
						}, 1500);
					}
				},
				error: function (xhr, status, error) {
					$('#error-modal-text').text(status + "//" + error);
					$('#error-modal').modal('show');
					setTimeout(function () {
						$('#error-modal').modal('hide');
					}, 1500);
				}
			});

		}
		else {
			$('#error-modal-text').text("choisir au moin un type d'operation !");
			$('#error-modal').modal('show');
			setTimeout(function () {
				$('#error-modal').modal('hide');
			}, 1500);
		}
	}
	else {
		$('#error-modal-text').text("Choisir un nom qui contient au moin 6 caractere !");
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
		}, 1500);
	}
}
function Add_Appareil_Btn(id) {
	var addbtn = document.getElementById('submit-add-app-btn');
	var button = document.getElementById('Add-app-btn-' + id);
	var text = button.previousElementSibling;
	document.getElementById('Ajouter-Appareil-text').textContent = "Ajouter ( Salle : " + text.textContent + " ) ";
	$('#content-1').hide();
	$('#content-edit').hide();
	$('#content-add').show();
	$.ajax({
		url: '/TypeOperation/TypeOperationList',
		type: 'GET',
		data: { SalleId: id },
		success: function (response) {
			var selectops = $('#Toperations_add_app');
			selectops.empty();
			$.each(response.operations, function (index, op) {
				var option = $('<option>').val(op.nom).text(op.nom);
				selectops.append(option);
			});
			addbtn.onclick = function () {
				submit_add_app(id);
			}
		},
		error: function (xhr, status, error) {
			$('#error-modal-text').text(status + xhr.status + error);
			$('#error-modal').modal('show');
			if (xhr.status == 401) {
				$('#error-modal-text').text("Session expirer !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
				}, 2000);
			}
		}
	});
}
function submit_add_app(salle_id) {
	let pattern1 = /^.{6,}$/;
	var Ops = $('#Toperations_add_app').val();
	var NumSerie = $('#NumSerie_add').val();
	if (NumSerie && pattern1.test(NumSerie)) {
		if (Ops.length !== 0) {
			var operations = [];
			Ops.forEach(function (op) {
				var object = { nom: op };
				operations.push(object);
			});
			var app =
			{
				NumSerie: NumSerie,
				Operations: operations,
				Maintenance: parseInt($('#Maintenance_add').val()),
				SalleId: salle_id
			};
			console.log(app);
			$.ajax({
				url: '/AppareilRadio/AddAppareilRadio',
				type: 'POST',
				data: { appareilradio: app },
				success: function (response) {
					if (response.success) {
						$('#success-modal-text').text(response.message);
						$('#success-modal').modal('show');
						setTimeout(function () {
							window.location.href = '/AppareilRadio/AppareilRadioList';
							$('#success-modal').modal('hide');
						}, 5000);
					}
					else {
						$('#error-modal-text').text(response.message);
						$('#error-modal').modal('show');
						setTimeout(function () {
							$('#error-modal').modal('hide');
						}, 1500);
					}
				},
				error: function (xhr, status, error) {
					$('#error-modal-text').text(status + "//" + error);
					$('#error-modal').modal('show');
					setTimeout(function () {
						$('#error-modal').modal('hide');
					}, 1500);
				}
			});

		}
		else {
			$('#error-modal-text').text("choisir au moin un type d'operation !");
			$('#error-modal').modal('show');
			setTimeout(function () {
				$('#error-modal').modal('hide');
			}, 1500);
		}
	}
	else {
		$('#error-modal-text').text("Choisir un nom qui contient au moin 6 caractere !");
		$('#error-modal').modal('show');
		setTimeout(function () {
			$('#error-modal').modal('hide');
		}, 1500);
	}
}




function Cancel_Appareil_Btn() {
	$('#content-1').show();
	$('#content-add').hide();
	$('#content-edit').hide();

}

//recherche par salle 
$(document).ready(function () {
	$('#search-app-input').on('keyup', function () {
		var searchText = $(this).val().toLowerCase();
		$('[id^="col-salle"]').filter(function () {
			var cardTitle = $(this).find('h4[data-name]').data('name').toLowerCase();
			$(this).toggle(cardTitle.indexOf(searchText) > -1);
		});
	});
});