function operation_list() {
	$.ajax({
		url: '/TypeOperation/TypeOperationList',
		type: 'GET',
		dataType: 'json',
		success: function (data) {
			console.log('Success:', data.operations);
			var tbody = $('#operation-table-body');
			var dropdownMenu = $('#last-td');
			tbody.empty(); 
			$.each(data.operations, function (index, operation) {
				var dropdownMenu = $('#last-td');
				var row = $('<tr>');
				row.append($('<td style="padding: 15px;">').text(operation.nom));
				row.append(dropdownMenu);
				tbody.append(row);
			});
			$('#operation-list-modal').modal('show');

		},
		error: function (xhr, status, error) {
			if(xhr.status == 403)
			{
				$('#error-modal-text').text("Tu n'a pas l'autorisation !");
				$('#error-modal').modal('show');
				setTimeout(function () {
					$('#error-modal').modal('hide');
					}, 1500);
			}
		}
	});
}

function salle_list() {
	$.ajax({
		url: '/Salle/SalleList',
		type: 'GET',
		dataType: 'json',
		success: function (data) {
			console.log('Success:', data.operations);
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
