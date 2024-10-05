
        // Validación de formulario
    $(document).ready(function () {
        // Validación del formulario
        $('form.needs-validation').submit(function (event) {
            if (!this.checkValidity()) {
                // Si el formulario no es válido, evitar que se envíe
                event.preventDefault();
                event.stopPropagation();
            }

            // Agregar clases CSS de validación (touched y was-validated) a los campos
            $(this).addClass('was-validated');
        });

    // Validación para el campo de Nombre Completo
    $('#FullName').on('input', function () {
                var fullName = $(this).val().trim();
    var isValid = fullName !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
                } else {
        $(this).addClass('is-invalid');
                }
            });

    // Validación para el campo de Número de teléfono
    $('#PhoneNumber').on('input', function () {
                var phoneNumberRegex = /^\d{8}$/;
    var phoneNumber = $(this).val().trim();
    var isValid = phoneNumberRegex.test(phoneNumber);

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
                } else {
        $(this).addClass('is-invalid');
                }
            });

    // Validación para el campo de Dirección
    $('#Address').on('input', function () {
                var address = $(this).val().trim();
    var isValid = address !== '';

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
                } else {
        $(this).addClass('is-invalid');
                }
            });

    // Validación para el campo de Correo Electrónico
    $('#Email').on('input', function () {
                var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    var email = $(this).val().trim();
    var isValid = email === '' || emailRegex.test(email);

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
                } else {
        $(this).addClass('is-invalid');
                }
            });
        });
