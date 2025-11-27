using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication
{
    public partial class EmpleadoMaster : System.Web.UI.MasterPage
    {
        private usuarioDTO usuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserData();
            }
            VerificarSesion();
        }

        private void VerificarSesion()
        {
            var usuario = Session["UsuarioActual"] as usuarioDTO;

            if (usuario == null)
            {
                Response.Redirect(ResolveUrl("~/Login.aspx"));
            }
            else
            {
                if (usuario.rol != 2)
                {
                    Session.Clear();
                    Session.Abandon();
                    FormsAuthentication.SignOut();

                    Response.Redirect(ResolveUrl("~/Login.aspx"));
                    Session["UsuarioActual"] = null;
                }
                else
                {
                    return;
                }

            }
        }

        private void LoadUserData()
        {
            usuario = Session["UsuarioActual"] as usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect(ResolveUrl("~/Login.aspx"));
            }
            string nombre = usuario.nombre;

            litUserName.Text = nombre;
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            Response.Redirect(ResolveUrl("~/Login.aspx"));
            Session["UsuarioActual"] = null;
        }
    }
}