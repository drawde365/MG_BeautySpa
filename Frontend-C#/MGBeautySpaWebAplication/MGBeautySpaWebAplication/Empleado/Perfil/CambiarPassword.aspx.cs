using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Empleado.Perfil
{
    public partial class CambiarPassword : Page
    {
        UsuarioBO usuarioBO;
        protected void Page_Load(object sender, EventArgs e)
        {
            usuarioBO = new UsuarioBO();
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            string antigua = txtAntigua.Text.Trim();
            string nueva = txtNueva.Text.Trim();
            string verificar = txtVerificar.Text.Trim();

            usuarioDTO usuario = Session["UsuarioActual"] as usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect(ResolveUrl("~/Login.aspx"));
                return;
            }

            if (usuarioBO.IniciarSesion(usuario.correoElectronico, antigua).idUsuario == 0)
            {
                lblInfo.Text = "⚠️ La contraseña actual no es correcta.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (nueva != verificar)
            {
                lblInfo.Text = "⚠️ Las nuevas contraseñas no coinciden.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            if (string.IsNullOrEmpty(nueva))
            {
                lblInfo.Text = "⚠️ La nueva contraseña no puede estar vacía.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                usuario.contrasenha = nueva;

                UsuarioBO usuarioBO = new UsuarioBO();
                usuarioBO.actualizarContraseña(usuario.idUsuario, usuario.contrasenha);

                Session["UsuarioActual"] = usuario;

                Response.Redirect("~/Empleado/Perfil/PerfilUsuario.aspx");
            }
            catch (Exception ex)
            {
                lblInfo.Text = "Error al actualizar la contraseña: " + ex.Message;
                lblInfo.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Empleado/Perfil/PerfilUsuario.aspx");
        }
    }
}