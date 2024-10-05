//Vista AgregarInsumo.cshtml

    // Obtener la fecha actual
    var currentDate = new Date().toISOString().split('T')[0];

    // Establecer la fecha actual como valor predeterminado en los campos de fecha
    document.getElementById('admissionDate').value = currentDate;
    document.getElementById('expirationDate').value = currentDate;
//Vista AgregarInsumo.cshtml
