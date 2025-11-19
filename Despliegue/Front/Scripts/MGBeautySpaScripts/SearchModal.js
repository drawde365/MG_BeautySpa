// MGBeautySpaScripts/SearchModal.js
(function () {
    // Funciones genéricas para cualquier modal Bootstrap por id
    window.showModalById = function (id) {
        var el = document.getElementById(id);
        if (!el) return;
        var m = bootstrap.Modal.getOrCreateInstance(el);
        m.show();
    };
    window.hideModalById = function (id) {
        var el = document.getElementById(id);
        if (!el) return;
        var m = bootstrap.Modal.getOrCreateInstance(el);
        m.hide();
    };

    // Buscador (abre modal pequeño)
    window.openSearch = function () {
        showModalById('modalSearch');
        setTimeout(function () {
            var i = document.getElementById('txtSearchModal');
            if (i) i.focus();
        }, 200);
    };

    // Cierre programático (útil si lo llamas desde CodeBehind)
    window.closeSearch = function () {
        hideModalById('modalSearch');
    };
})();
