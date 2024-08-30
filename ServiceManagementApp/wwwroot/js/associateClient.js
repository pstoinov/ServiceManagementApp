$(document).ready(function () {
    $('#associateClientBtn').on('click', function () {
        var selectedClientId = $('#SelectedClientId').val();
        var companyId = $('#CompanyId').val();

        $.ajax({
            url: '/api/ClientCompanyApi/associate',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({ SelectedClientId: selectedClientId, CompanyId: companyId }),
            success: function (response) {
                alert('Client associated successfully!');
                window.location.href = '/Companies';
            },
            error: function (xhr, status, error) {
                alert('An error occurred: ' + xhr.responseText);
            }
        });
    });
});
