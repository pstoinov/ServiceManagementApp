$(document).ready(function () {
    // Fetch company data when the page is loaded
    function loadCompanyData(companyId) {
        $.ajax({
            url: `/api/EditCompanyApi/${companyId}`,
            type: 'GET',
            success: function (data) {
                $('#companyName').val(data.companyName);
                $('#eik').val(data.eik);
                $('#vatNumber').val(data.vatNumber);
                $('#phone').val(data.phone);
                $('#email').val(data.email);

                // Load associated clients
                let clientsList = $('#clientsList');
                clientsList.empty();
                data.clients.forEach(client => {
                    clientsList.append(`<li>${client.fullName}</li>`);
                });
            },
            error: function (error) {
                console.log("Error loading company data:", error);
            }
        });
    }

    // Save company data when the form is submitted
    $('#saveCompanyBtn').on('click', function () {
        let companyId = $('#companyId').val();

        let companyData = {
            id: companyId,
            companyName: $('#companyName').val(),
            eik: $('#eik').val(),
            vatNumber: $('#vatNumber').val(),
            phone: $('#phone').val(),
            email: $('#email').val()
        };

        $.ajax({
            url: `/api/EditCompanyApi/${companyId}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(companyData),
            success: function () {
                alert("Company details updated successfully.");
                window.location.href = "/Companies"; // Redirect to the companies list page
            },
            error: function (error) {
                console.log("Error updating company:", error);
                alert("Failed to update company. Please check the inputs.");
            }
        });
    });

    // Load the company data when the document is ready
    let companyId = $('#companyId').val();
    if (companyId) {
        loadCompanyData(companyId);
    }
});
