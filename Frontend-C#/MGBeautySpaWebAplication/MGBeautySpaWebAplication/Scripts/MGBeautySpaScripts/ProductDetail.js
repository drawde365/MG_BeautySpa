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

    // ++ / -- cantidades (delegado) — no requiere server
    document.addEventListener("click", function (ev) {
        var t = ev.target;
        if (!t.classList) return;
        if (t.classList.contains("plus") || t.classList.contains("minus")) {
            var group = t.closest(".input-group");
            if (!group) return;
            var input = group.querySelector(".qty");
            if (!input) return;
            var v = parseInt(input.value || "0", 10);
            if (isNaN(v)) v = 0;
            if (t.classList.contains("plus")) v++;
            if (t.classList.contains("minus")) v = Math.max(0, v - 1);
            input.value = v;
        }
    });
})();
