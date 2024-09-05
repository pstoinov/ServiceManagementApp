$(document).ready(function() {
    // Fetch employee data when editing
    function loadEmployeeData(employeeId) {
        $.ajax({
            url: `/api/EmployeesApi/${employeeId}`,
            type: 'GET',
            success: function(data) {
                $('#FullName').val(data.fullName);
                $('#ServiceId').val(data.serviceId);
                $('#Position').val(data.position);
                $('#EmailAddress').val(data.emailAddress);
                $('#PhoneNumber').val(data.phoneNumber);
                $('#IsCertifiedForCashRegisterRepair').prop('checked', data.isCertifiedForCashRegisterRepair);
                $('#EGN').val(data.egn);
                $('#PictureUrl').val(data.pictureUrl);
            },
            error: function(error) {
                console.log("Error loading employee data:", error);
            }
        });
    }

    // Save employee data when the form is submitted
    $('#saveEmployeeBtn').on('click', function(e) {
        e.preventDefault(); // ѕредотврат€ва стандартното подаване на формата
        let employeeId = $('#Id').val();
        let employeeData = {
            id: employeeId,
            fullName: $('#FullName').val(),
            serviceId: $('#ServiceId').val(),
            position: $('#Position').val(),
            emailAddress: $('#EmailAddress').val(),
            phoneNumber: $('#PhoneNumber').val(),
            isCertifiedForCashRegisterRepair: $('#IsCertifiedForCashRegisterRepair').prop('checked'),
            egn: $('#EGN').val(),
            pictureUrl: $('#PictureUrl').val()
        };

        let ajaxType = employeeId ? 'PUT' : 'POST';
        let ajaxUrl = employeeId ? `/api/EmployeesApi/edit/${employeeId}` : "/api/EmployeesApi/create`;

        $.ajax({
            url: ajaxUrl,
            type: ajaxType,
            contentType: 'application/json',
            data: JSON.stringify(employeeData),
            success: function() {
                alert("Employee data saved successfully.");
                window.location.href = "/Employees";
            },
            error: function(error) {
                console.log("Error saving employee data:", error);
                alert("Failed to save employee data. Please check the inputs.");
            }
        });
    });

    // Load the employee data if we're editing an existing employee
    let employeeId = $('#Id').val();
    if (employeeId) {
        loadEmployeeData(employeeId);
    }
});
