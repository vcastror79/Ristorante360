function previewImage(event) {
    const file = event.target.files[0];
    const reader = new FileReader();
    const imagePreview = document.getElementById('imagePreview');

    reader.onload = function () {
        imagePreview.src = reader.result;
        imagePreview.style.display = 'block';
    };

    if (file) {
        reader.readAsDataURL(file);
    }
}
