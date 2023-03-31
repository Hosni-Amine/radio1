function Add_Salle()
{
    $('#add-salle-modal').modal('show');
}

function add_pdf(pdf)
{
    $.ajax({
        url: '/Salle/AddPDF',
        type: 'POST',
        data: pdf,
        processData: false,
        contentType: false,
        success: function (response) {
            console.log(response);
        },
        error: function (xhr, status, error) {
            console.log(status);
            $('#error-modal-text').text("Salle crée sans emplacement ! ");
            $('#error-modal').modal('show');
        }
    });  
}

function displayFileName() {
    const pdfFileInput = document.getElementById("file");
    const uploadLabel = document.querySelector('.upload');

    if (pdfFileInput.files.length > 0) {
        const pdfFileName = pdfFileInput.files[0].name;
        uploadLabel.innerHTML = pdfFileName;
    } else {
        uploadLabel.innerHTML = 'Choisir un fichier';
    }
}

function add_salle() {
    var pdf = new FormData();
    pdf.append("pdf", document.getElementById("file").files[0]);
    if ((pdf.get("pdf").name) != null && pdf != null) {
        var str = $('#add-salle-modal #Responsable').val();
        var op =
        {
            Nom : $('#add-salle-modal #Toperation').val()
        };         
        var salle =
        {
            Nom: $('#add-salle-modal #NomSalle').val(),
            Emplacement: $('#add-salle-modal #NomSalle').val(),
            Emplacement: pdf.get("pdf").name
        };
        console.log(salle);
        console.log(str);
        console.log(pdf.get("pdf").name);
        $.ajax({
            url: '/Salle/AddSalle',
            type: 'POST',
            data: { salle: salle, str: str, operation: op },
            success: function (response) {
                if (response.success) {
                    console.log(response);
                    add_pdf(pdf);
                }
                else {
                    console.log(response);
                    $('#add-salle-modal').modal('hide');
                    $('#error-modal-text').text(response.message);
                    $('#error-modal').modal('show');
                    setTimeout(function () {
                        $('#error-modal').modal('hide');
                        $('#add-salle-modal').modal('show');
                    }, 1500);
                }
            },
            error(xhr, status, error) {
                console.log(error);
            }
        });
    }
    else
    {
        $('#add-salle-modal').modal('hide');
        $('#error-modal-text').text("Pas de fichier choisis !");
        $('#error-modal').modal('show');
        setTimeout(function () {
            $('#error-modal').modal('hide');
            $('#add-salle-modal').modal('show');
        }, 1500);
    }
}


