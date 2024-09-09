document.addEventListener('DOMContentLoaded', function () {
    jQuery('.datepicker').datepicker({
        format: 'dd-mm-yyyy',   // Desired date format
        todayHighlight: true,   // Highlights today's date
        autoclose: true,        // Automatically close after selection
        language: 'bg'          // Set language to Bulgarian (if necessary)
    });
});
