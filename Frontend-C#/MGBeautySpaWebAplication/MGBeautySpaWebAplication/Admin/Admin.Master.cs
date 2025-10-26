using System;
using System.Web;
using System.Web.UI;

namespace MGBeautySpaWebAplication
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Rol"] == null || Session["Nombre"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!string.Equals(Session["Rol"].ToString(), "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                var rol = Session["Rol"].ToString();
                if (rol.Equals("Empleado", StringComparison.OrdinalIgnoreCase))
                    Response.Redirect("~/Empleado/MisCitas.aspx");
                else if (rol.Equals("Cliente", StringComparison.OrdinalIgnoreCase))
                    Response.Redirect("~/Cliente/InicioCliente.aspx");
                else
                    Response.Redirect("~/Login.aspx");
                return;
            }

            lblUsuario.Text = Session["Nombre"].ToString();
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}
