// Scripts/MGBeautySpaScripts/ProductDetail.js
(function () {
    "use strict";

    // Abre el modal de ingredientes (el code-behind sólo llama openIng())
    window.openIng = function () {
        var el = document.getElementById("modalIng");
        if (!el) return;
        var m = bootstrap.Modal.getOrCreateInstance(el);
        m.show();
    };

    // Cambia la pestaña activa: 'det' | 'uso' | 'com'
    window.setTab = function (id) {
        var map = { det: "#tab-det", uso: "#tab-uso", com: "#tab-com" };
        var target = map[id];
        if (!target) return;
        var trigger = document.querySelector('[data-bs-target="' + target + '"]');
        if (trigger) new bootstrap.Tab(trigger).show();
    };

    /*
      Script para manejar los botones de cantidad (+/-)
      Adaptado para las clases de tu diseño de Figma.
    */
    document.addEventListener('click', function (event) {

        // 1. Revisa si el clic fue en un botón 'plus' o 'minus'
        const isPlusButton = event.target.classList.contains('qty-btn-plus');
        const isMinusButton = event.target.classList.contains('qty-btn-minus');

        // Si no fue en uno de esos botones, no hagas nada.
        if (!isPlusButton && !isMinusButton) {
            return;
        }

        // 2. Previene cualquier comportamiento por defecto (como un PostBack)
        event.preventDefault();

        // 3. Encuentra el contenedor padre '.quantity-picker'
        const picker = event.target.closest('.quantity-picker');
        if (!picker) {
            return;
        }

        // 4. Encuentra el TextBox '.qty-display' DENTRO de ese contenedor
        const quantityInput = picker.querySelector('.qty-display');
        if (!quantityInput) {
            return;
        }

        // 5. Obtiene el valor actual
        let currentValue = parseInt(quantityInput.value, 10);
        if (isNaN(currentValue)) {
            currentValue = 0;
        }

        // 6. Suma o resta
        if (isPlusButton) {
            quantityInput.value = currentValue + 1;
        }
        else if (isMinusButton) {
            if (currentValue > 0) { // No permite bajar de 0
                quantityInput.value = currentValue - 1;
            }
        }
    });
})();
