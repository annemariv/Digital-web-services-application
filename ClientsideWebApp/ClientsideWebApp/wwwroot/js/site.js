document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.select-service-btn').forEach(btn => {
        btn.addEventListener('click', function () {

            var serviceId = this.getAttribute('data-service');

            //auto select
            var select = document.querySelector('[name="Quote.Service"]');
            if (select) {
                select.value = serviceId;
            }

            //scroll to quote
            var quoteSection = document.getElementById('quote');
            if (quoteSection) {
                quoteSection.scrollIntoView({ behavior: 'smooth' });
            }

          
        });
    });
});