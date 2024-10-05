    $(document).ready(function () {
        // Validación del formulario
        $('form').submit(function (event) {
            if (!this.checkValidity()) {
                // Si el formulario no es válido, evitar que se envíe
                event.preventDefault();
                event.stopPropagation();
            }

            // Agregar clases CSS de validación (touched y was-validated) a los campos
            $(this).addClass('was-validated');
        });

    // Validación para el campo de Insumo
    $('#inputState').on('change', function () {
            var selectedValue = $(this).val().trim();
    var isValid = selectedValue !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });

    // Validación para el campo de Cantidad
    $('#cantidad').on('input', function () {
            var amount = $(this).val().trim();
    var isValid = amount !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });

    // Validación para el campo de Unidad
    $('#unidad').on('change', function () {
            var selectedValue = $(this).val().trim();
    var isValid = selectedValue !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });

    // Validación para el campo de Cantidad Minima
    $('#cantidadMinima').on('input', function () {
            var minimumAmount = $(this).val().trim();
    var isValid = minimumAmount !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });

    // Validación para el campo de Lote
    $('#lote').on('input', function () {
            var lote = $(this).val().trim();
    var isValid = lote !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });

    // Validación para el campo de Fecha de Ingreso
    $('#admissionDate').on('change', function () {
            var admissionDate = $(this).val().trim();
    var isValid = admissionDate !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });

    // Validación para el campo de Fecha de Expiracion
    $('#expirationDate').on('change', function () {
            var expirationDate = $(this).val().trim();
    var isValid = expirationDate !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });
    });

