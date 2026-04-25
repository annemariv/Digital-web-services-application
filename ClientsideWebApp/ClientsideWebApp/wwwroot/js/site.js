document.addEventListener("DOMContentLoaded", function () {
    const alert = document.querySelector(".alert-success-custom");
    if (alert) {
        setTimeout(() => {
            alert.classList.add("fade-out");
            setTimeout(() => alert.remove(), 600);
        }, 4000);
    }
});