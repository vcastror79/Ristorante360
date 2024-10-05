// Tu JavaScript actualizado

$(document).ready(function () {
    // Variable para almacenar los productos seleccionados
    var selectedProducts = [];

    // Variable para verificar si se ha realizado algún cambio en la lista de productos seleccionados
    var productsChanged = false;

    // Función para agregar un producto a la lista de productos seleccionados
    function addProductToOrder(productId, productName, productPrice, productQuantity) {
        var existingProduct = selectedProducts.find(function (product) {
            return product.productId === productId;
        });

        if (existingProduct) {
            existingProduct.quantity += productQuantity;
        } else {
            selectedProducts.push({
                productId: productId,
                productName: productName,
                price: parseFloat(productPrice),
                quantity: productQuantity
            });
        }

        // Habilitar el botón de confirmar orden cuando hay al menos un producto seleccionado
        $('#confirmOrder').prop('disabled', false);

        // Indicar que ha habido cambios en la lista de productos seleccionados
        productsChanged = true;

        // Actualizar la lista de productos seleccionados en el modal
        updateSelectedProductsList();
    }

    // Función para mostrar la lista de productos seleccionados en el modal
    function updateSelectedProductsList() {
        var selectedProductsList = $('#selectedProductsList');
        selectedProductsList.empty();

        // Recorrer el arreglo de productos seleccionados y agregarlos a la lista
        selectedProducts.forEach(function (product) {
            var listItem = $('<li>').text(product.productName + ' : Precio: ₡' + product.price + ', Cantidad: ' + product.quantity+ '    ');

            // Agregar botón "Eliminar" con estilo de Bootstrap junto a cada producto en la lista
            var deleteButton = $('<button>').text('Eliminar').attr({
                'data-product-id': product.productId,
                'class': 'btn btn-outline-danger btn-xxs'
            });

            deleteButton.on('click', function () {
                removeProductFromOrder(product.productId);
            });

            listItem.append(deleteButton);
            selectedProductsList.append(listItem);
        });
    }

    // Función para eliminar un producto de la lista de productos seleccionados
    function removeProductFromOrder(productId) {
        selectedProducts = selectedProducts.filter(function (product) {
            return product.productId !== productId;
        });

        // Actualizar la lista de productos seleccionados en el modal
        updateSelectedProductsList();
    }

    $('#exampleModalCenter').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var productId = button.data('product-id');
        var productName = button.data('product-name');
        var productPrice = button.data('product-price');
        var productImage = button.data('product-image');

        var modal = $(this);
        modal.find('#productName').text(productName);
        modal.find('#productPrice').text('₡' + productPrice);
        modal.find('#productImage').attr('src', '/images/product/' + productImage);

        // Al abrir el modal, limpiamos los campos de cantidad y la confirmación
        $('#productQuantity').val(1);
        $('#confirmOrder').prop('disabled', true);

        // Establecer el ID del producto en el campo oculto
        modal.find('#productId').val(productId);
    });

    // Agregar el evento click al botón "Agregar a la orden"
    $('#addProductToOrder').on('click', function () {
        var productId = $('#productId').val(); // Obtener el ID del producto desde el campo oculto
        var productName = $('#productName').text();
        var productPrice = $('#productPrice').text().replace('₡', '');
        var productQuantity = parseInt($('#productQuantity').val());

        // Agregar el producto seleccionado a la lista de productos
        addProductToOrder(productId, productName, productPrice, productQuantity);
    });

    // Capturar el evento de salida (por ejemplo, al cerrar la pestaña o navegar a otra página)
    $(window).on('beforeunload', function (e) {
        if (productsChanged) {
            // Mostrar el mensaje de confirmación solo si hay productos seleccionados
            if (selectedProducts.length > 0) {
                var confirmationMessage = '¿Desea eliminar la orden? Los productos seleccionados se perderán.';
                (e || window.event).returnValue = confirmationMessage; // Soporte para navegadores más antiguos
                return confirmationMessage;
            }
        }
    });

    // Agregar el evento click al botón "Confirmar orden"
    $('#confirmOrder').on('click', function () {
        // Verificar si hay productos seleccionados
        if (selectedProducts.length > 0) {
            // Realizar la solicitud AJAX para enviar los productos al controlador
            $.ajax({
                type: 'POST',
                url: '/VentasPedidos/AgregarOrden', // Reemplazar "VentasPedidos" con el nombre de tu controlador
                data: JSON.stringify(selectedProducts),
                contentType: 'application/json',
                success: function (response) {
                    // Aquí puedes manejar la respuesta del servidor después de agregar la orden
                    console.log('Orden agregada exitosamente:', response);

                    // Redireccionar a la vista "ConfirmaPedido" con el orderId obtenido de la respuesta
                    redirectToConfirmaPedido(response.orderId);
                },
                error: function (error) {
                    // Manejar errores en caso de que ocurra algún problema con la solicitud
                    console.error('Error al agregar la orden:', error);
                }
            });
        }
    });

    // Función para redireccionar a la vista "ConfirmaPedido"
    function redirectToConfirmaPedido(orderId) {
        productsChanged = false; // Restablecer el estado de cambios en la lista de productos seleccionados
        selectedProducts = []; // Limpiar la lista de productos seleccionados
        window.location.href = '/VentasPedidos/ConfirmaPedido?orderId=' + orderId;
    }
});
