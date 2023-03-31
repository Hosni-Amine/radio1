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

//Crud type d'operation
function operation_list() {
	$.ajax({
		url: '/TypeOperation/TypeOperationList',
		type: 'GET',
		dataType: 'json',
		success: function (data) {
			var operations = data.operations;
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
				row.append($('<td class="text-center" style="padding: 15px;">').text(operation.nom));
				row.append('<td class="text-center"></a><a href="#" id="(' + operation.nom + ')"><i class="fa-solid fa-pen-to-square m-r-5"></i> Salles associeé </a></td>');
				tbody.append(row);
			});
			$('#operation-list-modal').modal('show');
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


function add_op_btn()
{
	var tbody = $('#operation-table-body');
	var row = $('<tr style="background-color: #f4f5fa;">');
	row.append($('<td>').append($('<input>').attr('type', 'text').attr('id', 'operation_id')));
	row.append('<td class="text-center"><a href="#" onclick="submit_op_add()"><i class="fa-solid fa-pen-to-square m-r-5"></i> Ajouter     |     </a><a href="#" onclick="operation_list()"><i class="fa fa-trash-alt m-r-5"></i>  Annuler </a></td>');
	tbody.append(row);
}
function submit_op_add()
{
	var nom = $('#operation_id').val();
	console.log(nom);
	$.ajax({
		url: '/TypeOperation/AddTypeOperation',
		type: 'POST',
		data: { nom: nom },
		success: function (response) {
			console.log(response);
			if (response.success) {
				$('#operation-list-modal').modal('hide');
				$('#success-modal-text').text("Type ajouter avec succées !");
				$('#success-modal').modal('show');
				setTimeout(function () {
					$('#success-modal').modal('hide');
					operation_list();
				}, 1500);
			}
			else
			{
				$('#operation-list-modal').modal('hide');
				$('#error-modal-text').text(response.message);
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					$('#operation-list-modal').modal('show');
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

function delete_op_btn(id) {
	$('#delete_modal').modal('show');
	$('#m-t-20').empty();
	var button = $('<button style="margin: 10px;">').attr('type', 'submit').addClass('btn btn-danger').attr('id', 'delete-modal-btn').text('Oui').on('click', Submit_Delete_Operation);
	var link = $('<a style="margin: 10px;">').attr('href', '#').addClass('btn btn-white').attr('data-bs-dismiss', 'modal').text('Non');
	$('#m-t-20').append(button);
	$('#m-t-20').append(link);
	$('#delete-text').text("Voulez-vous vraiment supprimer ce type d'operation ?");
	$('#delete-modal-btn').attr('data-id', id);
	console.log(id);
}
function Submit_Delete_Operation() {
	var record_id = $('#delete-modal-btn').attr('data-id');
	console.log(record_id);
	$.ajax({
		url: "/TypeOperation/DeleteTypeOperation/" + record_id,
		type: 'DELETE',
		success: function (response) {
			if (response.success) {
				$('#delete_modal').modal('hide');
				$('#operation-list-modal').modal('hide');
				$('#success-modal-text').text(response.message);
				$('#success-modal').modal('show');
				setTimeout(function () {
					$('#success-modal').modal('hide');
					operation_list()
				}, 2000);
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





