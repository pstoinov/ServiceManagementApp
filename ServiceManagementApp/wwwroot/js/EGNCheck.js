document.getElementById('IsCertifiedForCashRegisterRepair').addEventListener('change', function () {
    const isCertified = this.checked;
    const egnField = document.getElementById('EGN');
    const egnError = document.getElementById('egnError');

    if (isCertified) {
        egnField.setAttribute('required', 'required');
    } else {
        egnField.removeAttribute('required');
        egnError.style.display = 'none';
    }
});

document.querySelector('form').addEventListener('submit', function (event) {
    const isCertified = document.getElementById('IsCertifiedForCashRegisterRepair').checked;
    const egnField = document.getElementById('EGN');
    const egnError = document.getElementById('egnError');

    if (isCertified && !egnField.value) {
        egnError.style.display = 'block';
        event.preventDefault(); 
    } else {
        egnError.style.display = 'none';
    }
});
