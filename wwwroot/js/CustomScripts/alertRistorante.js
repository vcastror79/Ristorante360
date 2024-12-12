document.addEventListener("DOMContentLoaded", function () {
    // Obtener los botones de eliminación
    const deleteButtons = document.querySelectorAll('.delete-button');

    // Agregar el evento de clic a cada botón de eliminación
    deleteButtons.forEach(function (button) {
        button.addEventListener('click', function () {
            // Obtener el ID del producto de suministro
            const supplyProductId = button.getAttribute('data-id');

            // Mostrar el SweetAlert
            Swal.fire({
                title: '¿Estás seguro?',
                text: 'Esta acción no se puede deshacer',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, eliminar',
                cancelButtonText: 'Cancelar'
            }).then((result) => {
                if (result.isConfirmed) {
                    // Si el usuario confirma la eliminación, enviar el formulario de eliminación
                    const form = document.querySelector(`form[data-id="${supplyProductId}"]`);
                    form.submit();
                }
            });
        });
    });
});
