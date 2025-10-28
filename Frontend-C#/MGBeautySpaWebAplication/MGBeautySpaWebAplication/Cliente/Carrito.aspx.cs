using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web; // Necesario para Session

namespace MGBeautySpaWebAplication.Cliente
{
    // DTO para representar un ítem en el carrito
    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public string ImagenUrl { get; set; }
        public string Tamano { get; set; }
        public string TipoPiel { get; set; }
    }

    public partial class Carrito : Page
    {
        // Constantes para la lógica de negocio
        private const decimal TASA_IGV = 0.18m;
        // Declaraciones de controles (Visual Studio las genera, pero las listamos por referencia)
        // protected System.Web.UI.WebControls.Repeater rpCartItems;
        // protected System.Web.UI.WebControls.Literal litSubtotal, litEnvio, litImpuestos, litTotal;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<CartItemDTO> cartItems = GetCurrentCartItems();

                // La carga inicial se hace a través del método auxiliar
                RebindCartAndSummary(cartItems);
            }
        }

        // Obtiene o inicializa el carrito sin agregar productos por defecto (MOCK)
        private List<CartItemDTO> GetCurrentCartItems()
        {
            var cart = Session["Carrito"] as List<CartItemDTO>;

            if (cart == null)
            {
                cart = new List<CartItemDTO>();
                Session["Carrito"] = cart;
            }

            return cart;
        }

        // Lógica de cálculo y asignación de valores al resumen
        private void LoadOrderSummary(List<CartItemDTO> items)
        {
            // CÁLCULO 1: Subtotal
            decimal subtotal = items.Sum(item => item.PrecioUnitario * item.Cantidad);

            // CÁLCULO 3: Impuestos (Calculado sobre el subtotal base)
            decimal impuestos = Math.Round((subtotal * TASA_IGV), 2);

            // CÁLCULO 4: Total Final
            decimal total = subtotal + impuestos;

            // Asignar los valores a los Literales
            litSubtotal.Text = subtotal.ToString("N2");
            litImpuestos.Text = impuestos.ToString("N2");
            litTotal.Text = total.ToString("N2");
        }

        // Método auxiliar para recargar la vista y los totales después de cualquier cambio
        private void RebindCartAndSummary(List<CartItemDTO> cartItems)
        {
            // Re-enlazar el Repeater
            rpCartItems.DataSource = cartItems;
            rpCartItems.DataBind();

            // Re-ejecutar la lógica de resumen de pedido
            LoadOrderSummary(cartItems);

            Cliente masterPage = this.Master as Cliente;
            if (masterPage != null)
            {
                masterPage.UpdateCartDisplay();
            }
        }

        // ----------------------------------------------------
        // MANEJADORES DE EVENTOS
        // ----------------------------------------------------

        // Manejador para los botones + / -
        protected void Quantity_Click(object sender, EventArgs e)
        {
            LinkButton clickedButton = (LinkButton)sender;
            string argument = clickedButton.CommandArgument; // Obtiene "123|Sensible"

            // 1. Descomponer el CommandArgument en ID y Tipo
            string[] parts = argument.Split('|');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int productId))
                return; // Salir si el argumento es inválido

            string action = clickedButton.CommandName;
            string tipoPiel = parts[1]; // El valor de la variante

            List<CartItemDTO> cartItems = GetCurrentCartItems();

            // 2. 🌟 Búsqueda con CLAVE COMPUESTA 🌟
            CartItemDTO itemToUpdate = cartItems.FirstOrDefault(i =>
                i.ProductId == productId && i.TipoPiel == tipoPiel);

            if (itemToUpdate != null)
            {
                int delta = 0;

                if (action == "Increment")
                {
                    itemToUpdate.Cantidad++;
                    delta = 1;
                }
                else if (action == "Decrement")
                {
                    if (itemToUpdate.Cantidad > 1)
                    {
                        itemToUpdate.Cantidad--;
                        delta = -1;
                    }
                    else
                    {
                        // Eliminar el producto si la cantidad llega a 0
                        cartItems.Remove(itemToUpdate);
                        delta = -1;
                    }
                }

                // Actualizar el contador global de ítems
                UpdateCartCount(delta);

                // Actualizar el carrito en la Sesión
                Session["Carrito"] = cartItems;

                // Recargar la vista y los totales
                RebindCartAndSummary(cartItems);
                Cliente masterPage = this.Master as Cliente;
                masterPage.UpdateCartDisplay();
            }
        }

        // Actualiza el contador global de la cabecera
        private void UpdateCartCount(int delta)
        {
            int currentCount = (Session["CartCount"] as int?) ?? 0;
            Session["CartCount"] = Math.Max(0, currentCount + delta);
        }
        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            // 1. (Opcional: Verificar stock/finalizar lógica de carrito)
            //if (GetCurrentCartItems().Count > 0)
            // 2. Ejecutar JavaScript para mostrar el modal
            ScriptManager.RegisterStartupScript(
                this.Page, this.GetType(), 
                "ShowPaymentModal", 
                "var myModal = new bootstrap.Modal(document.getElementById('paymentModal')); myModal.show();", 
                true
            );
            // IMPORTANTE: NO uses Response.Redirect aquí, o el modal no se abrirá.
        }
        protected void btnProcessPayment_Click(object sender, EventArgs e)
        {
            // Limpiar mensajes de error previos (asumiendo que litPaymentError existe en el modal)
            // if (litPaymentError != null) litPaymentError.Text = "";

            // 1. Obtener y validar datos de entrada
            string cardNumber = txtCardNumber.Text.Trim().Replace(" ", "");
            string cvv = txtCVV.Text.Trim();
            string expiry = txtExpiryDate.Text.Trim();
            string name = txtNameOnCard.Text.Trim();

            if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 15 || 
                string.IsNullOrEmpty(cvv) || cvv.Length < 3 ||
                string.IsNullOrEmpty(expiry) || string.IsNullOrEmpty(name))
            {
                // Mostrar error si la validación falla
                // if (litPaymentError != null) litPaymentError.Text = "<span style='color: red;'>Por favor, complete todos los campos de la tarjeta correctamente.</span>";
                
                // Reabrir el modal después del PostBack
                ShowPaymentModal(); 
                return;
            }

            // 2. Simulación del procesamiento de pago (Lógica de Negocio/Integración de API)
            bool paymentSuccess = SimulatePaymentIntegration(cardNumber);

            if (paymentSuccess)
            {
                // 3. Si el pago es exitoso:
                // Limpiar el carrito de la sesión
                Session["Carrito"] = null;
                Session["CartCount"] = 0;

                // Redirigir a la página de confirmación de pedido
                Response.Redirect(ResolveUrl("~/Cliente/ConfirmacionPedido.aspx"));
            }
            else
            {
                // 4. Si el pago falla (simulación de rechazo)
                // if (litPaymentError != null) litPaymentError.Text = "<span style='color: red;'>El pago fue rechazado. Verifique los datos e intente de nuevo.</span>";
                
                // Reabrir el modal con el mensaje de error visible
                ShowPaymentModal(); 
            }
        }

        // Método auxiliar para simular la integración de pago (REEMPLAZAR CON TU LÓGICA REAL)
        private bool SimulatePaymentIntegration(string cardNumber)
        {
            // En un entorno real, aquí se llamaría a una API de pago (Stripe, PayPal, EzPay).
            // Simulación: Falla si el número termina en 0, es exitoso si termina en 1.
            if (cardNumber.EndsWith("0"))
            {
                return false;
            }
            return true;
        }
        
        // Método auxiliar para mostrar el modal (Se asume que la página no tiene UpdatePanel)
        private void ShowPaymentModal()
        {
            // El script para abrir el modal de Bootstrap
            string script = "var myModal = new bootstrap.Modal(document.getElementById('paymentModal')); myModal.show();";
            ScriptManager.RegisterStartupScript(this, GetType(), "ReopenModal", script, true);
        }
    }
}