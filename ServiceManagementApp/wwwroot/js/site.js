$(document).ready(function () {
    // Bind the add company modal
    $('#addCompanyModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        var clientId = button.data('client-id'); // Extract info from data-* attributes
        var modal = $(this);
        modal.find('#clientId').val(clientId); // Insert clientId into hidden field
    });

    // Bind the edit company modal
    $('#editCompanyModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget); // Button that triggered the modal
        var companyId = button.data('company-id'); // Extract info from data-* attributes
        var modal = $(this);

        // Perform AJAX request to get company details
        $.ajax({
            url: '/Clients/GetCompanyDetails/' + companyId,
            type: 'GET',
            success: function (data) {
                modal.find('#editCompanyId').val(data.id);
                modal.find('#editCompanyName').val(data.companyName);
                modal.find('#editEIK').val(data.eik);
                // Add more fields as needed
            }
        });
    });

    // Save company
    $('#saveCompanyBtn').on('click', function () {
        $('#addCompanyForm').submit();
    });

    // Save edited company
    $('#saveEditCompanyBtn').on('click', function () {
        $('#editCompanyForm').submit();
    });
});
