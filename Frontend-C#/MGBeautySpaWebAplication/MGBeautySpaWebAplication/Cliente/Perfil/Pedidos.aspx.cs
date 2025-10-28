using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class Pedidos : Page
    {
        private static int limite = 3; // cuántos pedidos mostrar inicialmente

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarPedidos();
        }

        private void CargarPedidos()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("NumeroPedido", typeof(string));
            dt.Columns.Add("FechaCompra", typeof(string));
            dt.Columns.Add("Subtotal", typeof(string));

            // Datos en duro
            dt.Rows.Add("1001", "21/05/2025", "129.90");
            dt.Rows.Add("1002", "10/06/2025", "89.50");
            dt.Rows.Add("1003", "02/08/2025", "45.00");
            dt.Rows.Add("1004", "05/10/2025", "230.00");
            dt.Rows.Add("1005", "17/10/2025", "152.40");
            dt.Rows.Add("1006", "20/10/2025", "99.90");
            dt.Rows.Add("1007", "27/10/2025", "60.00");

            // Mostrar solo los primeros N pedidos
            DataTable dtLimitado = dt.Clone();
            for (int i = 0; i < Math.Min(limite, dt.Rows.Count); i++)
                dtLimitado.ImportRow(dt.Rows[i]);

            rptPedidos.DataSource = dtLimitado;
            rptPedidos.DataBind();

            // Si ya se mostraron todos, ocultamos el botón "Ver más"
            btnVerMas.Visible = (limite < dt.Rows.Count);
        }

        protected void btnDetalles_Command(object sender, CommandEventArgs e)
        {
            string numeroPedido = e.CommandArgument.ToString();
            Response.Redirect($"~/Cliente/Perfil/DetallePedido.aspx?pedido={numeroPedido}");
        }

        protected void btnVerMas_Click(object sender, EventArgs e)
        {
            // Aumenta el número de pedidos visibles
            limite += 3;
            CargarPedidos();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}
