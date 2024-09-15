$(document).ready(function () {
    $("#clientSearch").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: '/Clients/FindClients', // Методът в контролера за търсене на клиенти
                data: { term: request.term },
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.name + " - " + item.phone + " - " + item.email,
                            value: item.id,
                            name: item.name,
                            phone: item.phone,
                            email: item.email
                        };
                    }));
                }
            });
        },
        select: function (event, ui) {
            $("#clientSearch").val(ui.item.label);
            $("#ClientId").val(ui.item.value);
            $("#ClientName").val(ui.item.name);
            $("#ClientPhone").val(ui.item.phone);
            $("#ClientEmail").val(ui.item.email);

            
            $.ajax({
                url: '/Requests/GetClientCompanies', 
                data: { clientId: ui.item.value },
                success: function (companies) {
                    var companyDropdown = $("#ClientCompanyId");
                    companyDropdown.empty(); 
                    companyDropdown.append('<option value="">Изберете фирма</option>');
                    $.each(companies, function (i, company) {
                        companyDropdown.append('<option value="' + company.companyId + '">' + company.companyName + '</option>');
                    });
                }
            });

            return false;
        }
    });
});
