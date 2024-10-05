$(document).ready(function () {
    $('.delete-user').on('click', function () {
        var button = $(this);
        var userName = button.data('name-modal');
        var userSurname = button.data('surname-modal');
        var userEmail = button.data('email-modal');
        var userId = button.data('userid-modal');
        var url = button.data('url-modal');

        var modal = $('#exampleModalCenter');
        modal.find('#name').text(userName + " " + userSurname);
        modal.find('#email').text(userEmail);
        modal.find('#userId').text(userId);

        modal.find('#confirmDelete').on('click', function () {
            var data = { userId: userId };

            $.post(url, data)
                .done(function () {
                    // Eliminación exitosa
                    // Aquí puedes agregar código para mostrar una notificación o realizar otras acciones
                    window.location.reload(); // Recargar la página después de eliminar
                })
                .fail(function () {
                    // Error en la eliminación
                    // Aquí puedes agregar código para mostrar una notificación o realizar otras acciones
                });
        });
    });
});
