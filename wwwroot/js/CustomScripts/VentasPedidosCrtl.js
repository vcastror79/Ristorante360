//Función de buscar clientes
$(document).ready(function () {
    // Variable para almacenar la lista de clientes
    var clients = [];

    // Función para obtener la lista de clientes usando AJAX
    function getClients() {
        $.ajax({
            type: 'GET',
            url: '/VentasPedidos/GetAllClients', // Ruta al método GetAllClients en el controlador
            success: function (data) {
                // Almacenar la lista de clientes en la variable clients
                clients = data;
            },
            error: function (error) {
                console.error('Error al obtener la lista de clientes:', error);
            }
        });
    }

    // Llamar a la función para obtener la lista de clientes al cargar la página
    getClients();

    // Función para buscar clientes por nombre o número de teléfono
    function searchClients(query) {
        return clients.filter(function (client) {
            return client.fullName.toLowerCase().includes(query.toLowerCase()) ||
                client.phoneNumber.includes(query);
        });
    }

    // Función para actualizar la tabla de resultados de búsqueda de clientes
    function updateSearchResults(results) {
        var searchClientResults = $('#searchClientResults');
        searchClientResults.empty();

        results.forEach(function (client) {
            var row = '<tr>' +
                '<td>' + client.clientId + '</td>' +
                '<td>' + client.fullName + '</td>' +
                '<td>' + client.phoneNumber + '</td>' +
                '<td><button class="btn btn-primary btn-sm btn-select-client" data-dismiss="modal" data-client-id="' + client.clientId + '">Seleccionar</button></td>' +
                '</tr>';

            searchClientResults.append(row);
        });
    }

    // Manejar la entrada de texto en el campo de búsqueda de clientes
    $('#searchClientInput').on('input', function () {
        var query = $(this).val();
        var searchResults = searchClients(query);
        updateSearchResults(searchResults);
    });

    // Manejar el evento de clic en el botón "Seleccionar" dentro del modal de búsqueda de clientes
    $(document).on('click', '.btn-select-client', function () {
        var clientId = $(this).data('client-id');
        var selectedClient = clients.find(function (client) {
            return client.clientId === clientId;
        });

        if (selectedClient) {
            // Actualizar los campos de cliente con los datos seleccionados
            $('#firstName').val(selectedClient.fullName);
            $('#lastName').val(selectedClient.phoneNumber);
            $('#email').val(selectedClient.email);
            $('#address').val(selectedClient.address);
            // Agrega más campos si necesitas actualizar más información del cliente
        }
    });
});


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

    // Validación para el campo de Número de teléfono
    $('#lastName').on('input', function () {
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
    $('#address').on('input', function () {
        // Aquí podrías agregar cualquier validación adicional necesaria para la dirección
        // Por ejemplo, verificar que la dirección contenga ciertos caracteres válidos.
    });

    // Validación para la forma de pago (al menos una debe estar seleccionada)
    $('input[name="PaymentMethodId"]').on('change', function () {
        var isValid = $('input[name="PaymentMethodId"]:checked').length > 0;
        // Agregar o quitar la clase CSS 'is-invalid' según la validación
        $('input[name="PaymentMethodId"]').removeClass('is-invalid');
        if (!isValid) {
            $('input[name="PaymentMethodId"]').addClass('is-invalid');
        }
    });

    // Validación para el tipo de orden (al menos uno debe estar seleccionado)
    $('input[name="OrderTypeId"]').on('change', function () {
        var isValid = $('input[name="OrderTypeId"]:checked').length > 0;
        // Agregar o quitar la clase CSS 'is-invalid' según la validación
        $('input[name="OrderTypeId"]').removeClass('is-invalid');
        if (!isValid) {
            $('input[name="OrderTypeId"]').addClass('is-invalid');
        }
    });
});