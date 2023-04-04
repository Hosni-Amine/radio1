
//Fonctions API type d'operation
function operation_list() {
	$.ajax({
		url: '/TypeOperation/TypeOperationList',
		type: 'GET',
		dataType: 'json',
		success: function (data) {
			var operations = data.operations;
			if (operations.length !== 0) {
				var distinctNames = operations.filter((operation, index, self) =>
					index === self.findIndex((op) => (
						op.nom === operation.nom
					))
				);
				var tbody = $('#operation-table-body');
				var last_td = $('#Nom-Op');
				tbody.empty();
				$.each(distinctNames, function (index, operation) {
					var row = $('<tr id="' + operation.nom + '" style="background-color: #f4f5fa;">');
					row.append($('<td id="name" class="text-center" style="padding: 15px;">').text(operation.nom));
					row.append('<td id="action" class="text-center"></a><a href="#" onclick="" id=""><i class="fa-solid fa-pen-to-square m-r-5"></i> Salles associées</a></td>');
					tbody.append(row);
				});
				$('#operation-list-modal').modal('show');
			}
			else {
				$('#success-modal-text').text("pas d'opération trouvée !");
				$('#success-modal').modal('show');
				setTimeout(function () {
					$('#success-modal').modal('hide');
				}, 2500);
			}
		},
		error: function (xhr, status, error) {
			if (xhr.status == 403) {
				$('#error-modal-text').text("Tu n'a pas l'autorisation !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
				}, 1500);
			}
		}
	});		
}

//Fonctions API pour la gestion des operations associée a une salle 
function operation_associee(id) {
	var addbtn = document.getElementById("add-op-btn");
	addbtn.setAttribute("onclick", "add_op_btn('" + id + "')");
	$.ajax({
		url: '/TypeOperation/TypeOperationList',
		type: 'GET',
		data: { SalleId: id },
		dataType: 'json',
		success: function (data) {
				var tbody = $('#operation-salle-table-body');
				tbody.empty();
			$.each(data.operations, function (index, operation) {
				var row = $('<tr style="background-color: #f4f5fa;">');
				row.append($('<td class="text-center" style="padding: 15px;">').text(operation.nom));
				row.append('<td class="text-center"></a><a href="#" onclick="delete_op_btn(' + operation.id+","+ operation.salleId + ')"><i class="fa fa-trash-alt m-r-5"></i> Supprimer </a></td>');
					tbody.append(row);
				});
				$('#operation-salle-list-modal').modal('show');
		},
		error: function (xhr, status, error) {
			if (xhr.status == 403) {
				$('#error-modal-text').text("Tu n'a pas l'autorisation !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
				}, 1500);
			}
		}
	});
	
}

function add_op_btn(salle_id)
{
	var tbody = $('#operation-salle-table-body');
	var row = $('<tr style="background-color: #f4f5fa;">');
	row.append($('<td>').append($('<input>').attr('type', 'text').attr('id', 'for_operation_name')));
	row.append('<td class="text-center"><a href="#" onclick="submit_op_add(' + salle_id + ')"><i class="fa-solid fa-pen-to-square m-r-5"></i> Ajouter     |     </a><a href="#" onclick="operation_associee(' + salle_id + ')"><i class="fa fa-trash-alt m-r-5"></i>  Annuler </a></td>');
	tbody.append(row);
}
function submit_op_add(salle_id)
{
	var nom = $('#for_operation_name').val();
	var op = 
	{
		Nom: nom,
		Salleid: salle_id
	}
	$.ajax({
		url: '/TypeOperation/AddTypeOperation',
		type: 'POST',
		data: { operation: op },
		success: function (response) {
			if (response.success) {
				$('#check_edit').val() === 'yes';
				$('#success-modal-text').text("Type ajouter avec succées !");
				$('#success-modal').modal('show');
				setTimeout(function () {
					$('#success-modal').modal('hide');
					operation_associee(salle_id);
				}, 1500);
			}
			else
			{
				$('#error-modal-text').text(response.message);
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					operation_associee(id);
				}, 1500);
			}
		},
		error: function (xhr, status, error) {
				$('#error-modal-text').text(status+"//"+error);
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					operation_list();
				}, 1500);
			}
	});
}

function delete_op_btn(op_id,salle_id) {
	$('#delete_modal').modal('show');
	$('#delete-text').text("Voulez-vous vraiment supprimer ce type d'operation ?");
	var deletebtn = document.getElementById('delete-modal-btn');
	deletebtn.onclick = function () {
		Submit_Delete_Operation(op_id , salle_id);
	};
}
function Submit_Delete_Operation(op_id, salle_id) {
	$.ajax({
		url: "/TypeOperation/DeleteTypeOperation/" + op_id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#delete_modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					$('#success-modal').modal('hide');
					operation_associee(salle_id);
				}, 2000);
			} else {
				$('#error-modal-text').text(response.message);
				$('#error-modal').modal('show');
			}
		},
		error: function (error) {
			console.log(error);
		}
	});
}

//Fonction reload lorsqu'il y a un changement
$(document).ready(function () {
	$('#operation-salle-list-modal').on('hidden.bs.modal', function () {
		if ($('#check_edit').val() === 'yes')
		{
			location.reload();
		}
	});
});

//Recherche dans la liste des operations 
$(document).ready(function () {
	$('#operation-list-modal input').on('keyup', function () {
		var searchText1 = $('#operation-list-modal #search-input').val().toLowerCase();
		$('#operation-list-modal tbody tr').filter(function () {
			var name = $(this).find('td:nth-child(1)').text().toLowerCase();
			$(this).toggle(name.indexOf(searchText1) > -1);
		});
	});
});
//Recherche dans la list des operations de salle
$(document).ready(function () {
	$('#operation-salle-list-modal input').on('keyup', function () {
		var searchText1 = $('#operation-salle-list-modal #search-input').val().toLowerCase();
		$('#operation-salle-list-modal tbody tr').filter(function () {
			var name = $(this).find('td:nth-child(1)').text().toLowerCase();
			$(this).toggle(name.indexOf(searchText1) > -1);
		});
	});
});



