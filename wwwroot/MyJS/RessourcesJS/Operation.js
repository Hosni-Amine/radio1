
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



