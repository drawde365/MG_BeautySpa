using System;
using System.Data;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class DetallePedido : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarDetalle();
        }

        private void CargarDetalle()
        {
            string numeroPedido = Request.QueryString["pedido"];
            if (string.IsNullOrEmpty(numeroPedido))
                numeroPedido = "1001"; // valor por defecto si entra directo

            // Asignar valores fijos según el pedido
            litNumeroPedido.Text = numeroPedido;

            string fecha = "21/05/2025";
            string total = "129.90";

            if (numeroPedido == "1002")
            {
                fecha = "10/06/2025";
                total = "89.50";
            }
            else if (numeroPedido == "1003")
            {
                fecha = "02/08/2025";
                total = "45.00";
            }

            litFecha.Text = fecha;
            litTotal.Text = total;

            // Productos en duro
            DataTable dt = new DataTable();
            dt.Columns.Add("Imagen", typeof(string));
            dt.Columns.Add("Nombre", typeof(string));
            dt.Columns.Add("Descripcion", typeof(string));
            dt.Columns.Add("Cantidad", typeof(int));
            dt.Columns.Add("Subtotal", typeof(string));

            if (numeroPedido == "1001")
            {
                dt.Rows.Add("~/img/producto1.jpg", "Crema facial hidratante", "Hidrata y suaviza la piel.", 1, "59.90");
                dt.Rows.Add("~/img/producto2.jpg", "Tónico de rosas", "Refresca y equilibra el rostro.", 2, "70.00");
            }
            else if (numeroPedido == "1002")
            {
                dt.Rows.Add("~/img/producto3.jpg", "Mascarilla nutritiva", "Repara el cabello dañado.", 1, "49.90");
                dt.Rows.Add("~/img/producto4.jpg", "Aceite de argán", "Nutre el cabello y da brillo.", 1, "39.60");
            }
            else
            {
                dt.Rows.Add("~/img/producto5.jpg", "Exfoliante corporal", "Elimina impurezas suavemente.", 1, "45.00");
            }

            rptProductos.DataSource = dt;
            rptProductos.DataBind();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/Pedidos.aspx");
        }
    }
}
