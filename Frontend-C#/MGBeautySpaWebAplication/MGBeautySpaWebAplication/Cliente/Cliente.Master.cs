using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication
{
    public partial class ClienteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Debe existir sesión
            if (Session["Rol"] == null || Session["Nombre"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // Debe ser CLIENTE
            if (!string.Equals(Session["Rol"].ToString(), "Cliente", StringComparison.OrdinalIgnoreCase))
            {
                // Redirige según el rol real
                var rol = Session["Rol"].ToString();
                if (rol.Equals("Administrador", StringComparison.OrdinalIgnoreCase))
                    Response.Redirect("~/Admin/InicioAdmin.aspx");
                else if (rol.Equals("Empleado", StringComparison.OrdinalIgnoreCase))
                    Response.Redirect("~/Empleado/MisCitas.aspx");
                else
                    Response.Redirect("~/Login.aspx");
                return;
            }

            // Pinta header
            lblUsuario.Text = Session["Nombre"].ToString();
            lblRol.Text = Session["Rol"].ToString();
        }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}