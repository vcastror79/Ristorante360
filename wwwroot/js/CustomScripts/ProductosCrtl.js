
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

    // Validación para el campo de Precio
    $('#oProduct_Price').on('input', function () {
            var price = $(this).val().trim();
            var isValid = price > 0;

    // Agregar o quitar la clase CSS 'is-invalid' según la validación
    if (isValid) {
        $(this).removeClass('is-invalid');
            } else {
        $(this).addClass('is-invalid');
            }
        });
    });



