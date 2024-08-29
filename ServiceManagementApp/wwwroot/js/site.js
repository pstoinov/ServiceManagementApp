// Премахни този код от JavaScript файла
//$('#editClientModal').on('show.bs.modal', function (event) {
//    var button = $(event.relatedTarget); // Button that triggered the modal
//    var clientId = button.data('client-id'); // Extract info from data-* attributes
//    var modal = $(this);

//    // Perform AJAX request to get client details
//    $.ajax({
//        url: '/Clients/GetClientDetails/' + clientId,
//        type: 'GET',
//        success: function (data) {
//            modal.find('#clientId').val(data.id);
//            modal.find('#clientName').val(data.fullName);
//            modal.find('#clientPhone').val(data.phone);
//            modal.find('#clientEmail').val(data.email);
//            // Add more fields as needed
//        }
//    });
//});
