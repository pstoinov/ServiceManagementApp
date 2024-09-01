$(document).ready(function () {
    // Функция за зареждане на данни за клиента
    function loadClientData(clientId) {
        $.ajax({
            url: `/api/EditClientApi/${clientId}`,
            type: 'GET',
            success: function (data) {
                $('#fullName').val(data.fullName);
                $('#phone').val(data.phone);
                $('#email').val(data.email);
            },
            error: function (error) {
                console.log("Error loading client data:", error);
            }
        });
    }

    // Запазване на данни за клиента при натискане на бутона за запазване
    $('#saveClientBtn').on('click', function () {
        let clientId = $('#clientId').val();

        let clientData = {
            id: clientId,
            fullName: $('#fullName').val(),
            phone: $('#phone').val(),
            email: $('#email').val()
        };

        $.ajax({
            url: `/api/EditClientApi/${clientId}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(clientData),
            success: function () {
                alert("Client details updated successfully.");
                window.location.href = "/Clients"; // Пренасочване към списъка с клиенти
            },
            error: function (error) {
                console.log("Error updating client:", error);
                alert("Failed to update client. Please check the inputs.");
            }
        });
    });

    // Зареждане на данни за клиента при готовност на документа
    let clientId = $('#clientId').val();
    if (clientId) {
        loadClientData(clientId);
    }
});
