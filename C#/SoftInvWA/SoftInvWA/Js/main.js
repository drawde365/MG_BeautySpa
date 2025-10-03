// Activar tooltips de Bootstrap
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

// Mensaje de añadido al carrito
function addToCart(productName) {
    const message = `<div class="alert alert-success alert-dismissible fade show" role="alert">
    <strong>${productName}</strong> ha sido añadido al carrito.
    <button type="button" class="close" data-dismiss="alert" aria-label="Cerrar">
      <span aria-hidden="true">&times;</span>
    </button>
  </div>`;

    $("#alerts").append(message);
}

// Modal de reserva
function reservarServicio(servicio) {
    $("#modalReserva .modal-body").html(
        `<p>Has seleccionado el servicio: <strong>${servicio}</strong></p>
     <p>Nuestro equipo te confirmará la cita.</p>`
    );
    $("#modalReserva").modal("show");
}

// Animaciones suaves
$(document).ready(function () {
    $(".depth-frame-18, .depth-frame-24").hover(
        function () {
            $(this).css("box-shadow", "0 6px 12px rgba(0,0,0,0.15)");
        },
        function () {
            $(this).css("box-shadow", "0 4px 8px rgba(0,0,0,0.08)");
        }
    );
});
