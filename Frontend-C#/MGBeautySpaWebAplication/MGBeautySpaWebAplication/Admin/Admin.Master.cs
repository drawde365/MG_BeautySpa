using System;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.IO;
using SoftInvBusiness.SoftInvWSUsuario;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdminMaster : System.Web.UI.MasterPage
    {
        protected HyperLink navAdmPedidos;
        protected HyperLink navAdmProductos;
        protected HyperLink navAdmServicios;
        protected HyperLink navReportes;
        protected HyperLink navBuscarUsuarios;
        protected HyperLink navAgregarEmpleado;
        private usuarioDTO usuario;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserData();
                SetActiveNavigation();
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
                if (usuario.rol != 3)
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

        private void SetActiveNavigation()
        {
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            navAdmPedidos.CssClass = "nav-link";
            navAdmProductos.CssClass = "nav-link";
            navAdmServicios.CssClass = "nav-link";
            navReportes.CssClass = "nav-link";
            navBuscarUsuarios.CssClass = "nav-link";
            navAgregarEmpleado.CssClass = "nav-link";

            switch (currentPage)
            {
                case "admpedidos.aspx":
                    navAdmPedidos.CssClass = "nav-link active";
                    break;
                case "admproductos.aspx":
                case "insertarproducto.aspx":
                    navAdmProductos.CssClass = "nav-link active";
                    break;
                case "admservicios.aspx":
                case "insertarservicio.aspx":
                    navAdmServicios.CssClass = "nav-link active";
                    break;
                case "reportes.aspx":
                    navReportes.CssClass = "nav-link active";
                    break;
                case "buscarusuarios.aspx":
                    navBuscarUsuarios.CssClass = "nav-link active";
                    break;
                case "agregarempleado.aspx":
                    navAgregarEmpleado.CssClass = "nav-link active";
                    break;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
            Session["UsuarioActual"] = null;
        }
    }
}