using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Carrito : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // Este método se dispara al hacer clic en "Proceder al Pago".

            // Aquí debes implementar la lógica de negocio:
            // 1. Validar que el carrito no esté vacío.
            // 2. Calcular los totales finales.
            // 3. Redirigir al usuario a la página de pago/checkout.

            // Ejemplo de Redirección:
            Response.Redirect(ResolveUrl("~/Cliente/Checkout.aspx"));
        }
    }
}