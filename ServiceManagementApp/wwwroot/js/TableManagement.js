$(function () {
    new DataTable('#dataTable', {
        pageLength: 20, // Брой записи на страница по подразбиране
        lengthMenu: [[20, 50, 100, -1], [20, 50, 100, "Всички"]],
        ordering: true, // Позволява сортиране
        language: {
            "sProcessing": "Обработка на резултатите...",
            "sLengthMenu": "Показвай _MENU_ резултата",
            "sZeroRecords": "Няма намерени резултати",
            "sInfo": "Показване на резултати от _START_ до _END_ от общо _TOTAL_",
            "sInfoEmpty": "Показване на резултати от 0 до 0 от общо 0",
            "sInfoFiltered": "(филтрирани от общо _MAX_ резултата)",
            "sInfoPostFix": "",
            "sSearch": "Търси:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Първа",
                "sPrevious": "Предишна",
                "sNext": "Следваща",
                "sLast": "Последна"
            },
            "sEmptyTable": "Няма данни в таблицата"
        }
    });
});
