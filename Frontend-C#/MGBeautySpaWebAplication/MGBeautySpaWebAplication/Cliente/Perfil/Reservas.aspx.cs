using System;
using System.Data;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente.Perfil
{
    public partial class Reservas : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarReservas();
        }

        private void CargarReservas()
        {
            // Cargar en duro las reservas
            DataTable dt = new DataTable();
            dt.Columns.Add("NumeroReserva", typeof(string));
            dt.Columns.Add("Servicio", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Empleado", typeof(string));
            dt.Columns.Add("Total", typeof(string));

            dt.Rows.Add("001", "Masaje relajante de 60 min", "12/08/2025", "Lucía Ramos", "90.00");
            dt.Rows.Add("002", "Tratamiento facial rejuvenecedor", "25/09/2025", "Carolina Pérez", "120.00");
            dt.Rows.Add("003", "Manicure y Pedicure", "05/10/2025", "Andrea Torres", "70.00");

            rptReservas.DataSource = dt;
            rptReservas.DataBind();
        }

        protected void btnVerMas_Click(object sender, EventArgs e)
        {
            // Cargar más reservas (todo en duro)
            DataTable dt = new DataTable();
            dt.Columns.Add("NumeroReserva", typeof(string));
            dt.Columns.Add("Servicio", typeof(string));
            dt.Columns.Add("Fecha", typeof(string));
            dt.Columns.Add("Empleado", typeof(string));
            dt.Columns.Add("Total", typeof(string));

            dt.Rows.Add("001", "Masaje relajante de 60 min", "12/08/2025", "Lucía Ramos", "90.00");
            dt.Rows.Add("002", "Tratamiento facial rejuvenecedor", "25/09/2025", "Carolina Pérez", "120.00");
            dt.Rows.Add("003", "Manicure y Pedicure", "05/10/2025", "Andrea Torres", "70.00");
            dt.Rows.Add("004", "Depilación con cera", "10/10/2025", "Mónica Vega", "60.00");
            dt.Rows.Add("005", "Masaje con piedras calientes", "15/10/2025", "Lucía Ramos", "150.00");

            rptReservas.DataSource = dt;
            rptReservas.DataBind();
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}
