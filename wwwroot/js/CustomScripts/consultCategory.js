document.addEventListener("DOMContentLoaded", function () {
    var pizzaButton = document.getElementById("pizzaButton");
    var hamburguesaButton = document.getElementById("hamburguesaButton");
    var categoryIdInput = document.querySelector("#menuForm [name='categoryId']");
    var menuForm = document.getElementById("menuForm");
    var todoButton = document.getElementById("todoButton");

    todoButton.addEventListener("click", function () {
        categoryIdInput.value = 0;
        menuForm.submit();
    });

    pizzaButton.addEventListener("click", function () {
        categoryIdInput.value = 1;
        menuForm.submit();
    });

    hamburguesaButton.addEventListener("click", function () {
        categoryIdInput.value = 2;
        menuForm.submit();
    });

    var polloButton = document.getElementById("polloButton");
    var comidaRapidaButton = document.getElementById("comidaRapidaButton");
    var postreButton = document.getElementById("postreButton");

    polloButton.addEventListener("click", function () {
        categoryIdInput.value = 3;
        menuForm.submit();
    });

    comidaRapidaButton.addEventListener("click", function () {
        categoryIdInput.value = 4;
        menuForm.submit();
    });

    postreButton.addEventListener("click", function () {
        categoryIdInput.value = 5;
        menuForm.submit();
    });
});