using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente.Perfil
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
            }

            // 1️⃣ Validar contraseña actual
            if (usuarioBO.IniciarSesion(usuario.correoElectronico, antigua).idUsuario==0)
            {
                lblInfo.Text = "⚠️ La contraseña actual no es correcta.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            // 2️⃣ Validar que las nuevas contraseñas coincidan
            if (nueva != verificar)
            {
                lblInfo.Text = "⚠️ Las nuevas contraseñas no coinciden.";
                lblInfo.ForeColor = System.Drawing.Color.Red;
                return;
            }

            ClienteBO clienteBO = new ClienteBO();
            SoftInvBusiness.SoftInvWSCliente.clienteDTO user = new SoftInvBusiness.SoftInvWSCliente.clienteDTO
            {
                nombre = usuario.nombre,
                primerapellido = usuario.primerapellido,
                segundoapellido = usuario.segundoapellido,
                correoElectronico = usuario.correoElectronico,
                contrasenha = nueva,
                celular = usuario.celular,
                urlFotoPerfil = usuario.urlFotoPerfil,
                rol = 1,
                idUsuario = usuario.idUsuario,
                activo = 1,
                rolSpecified = true,
                idUsuarioSpecified = true,
                activoSpecified = true
            };

            clienteBO.ModificarDatosCliente(user);
            Session["UsuarioActual"] = user;
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            // Redirige de vuelta al perfil del usuario
            Response.Redirect("~/Cliente/Perfil/PerfilUsuario.aspx");
        }
    }
}