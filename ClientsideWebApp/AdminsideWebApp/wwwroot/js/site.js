document.addEventListener("DOMContentLoaded", function () {
    const toast = document.querySelector('.toast-message');

    if (toast) {
        setTimeout(() => {
            toast.style.opacity = '0';
            toast.style.transform = 'translateY(10px)';

            setTimeout(() => toast.remove(), 300);
        }, 3000);
    }
});