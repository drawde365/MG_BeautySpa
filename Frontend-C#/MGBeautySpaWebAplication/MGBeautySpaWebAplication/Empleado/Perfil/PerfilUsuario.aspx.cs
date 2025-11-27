using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSUsuario;
using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Empleado
{
    public partial class PerfilUsuario : Page
    {
        private UsuarioBO usuarioBO;
        private EmpleadoBO empleadoBO;
        public PerfilUsuario()
        {
            usuarioBO = new UsuarioBO();
            empleadoBO = new EmpleadoBO();
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPerfil();
                SetModoEdicion(false);
            }
        }

        private void CargarPerfil()
        {
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;

            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx?ReturnUrl=" + Request.RawUrl);
                return;
            }

            litUserNameGreeting.Text = usuario.nombre;
            litNombres.Text = usuario.nombre;
            litApellidos.Text = $"{usuario.primerapellido} {usuario.segundoapellido}";
            litEmail.Text = usuario.correoElectronico;
            litTelefono.Text = usuario.celular;

            txtNombres.Text = usuario.nombre;
            txtPrimerApellido.Text = usuario.primerapellido;
            txtSegundoApellido.Text = usuario.segundoapellido;
            litEmailEdit.Text = usuario.correoElectronico;
            txtTelefono.Text = usuario.celular;
        }

        private void SetModoEdicion(bool enEdicion)
        {
            pnlReadOnly.Visible = !enEdicion;
            pnlEdit.Visible = enEdicion;

            btnEditarPerfil.Visible = !enEdicion;
            btnGuardar.Visible = enEdicion;
            btnVolver.Visible = enEdicion;
        }

        protected void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            SetModoEdicion(true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            usuario.nombre = txtNombres.Text.Trim();
            usuario.primerapellido = txtPrimerApellido.Text.Trim();
            usuario.segundoapellido = txtSegundoApellido.Text.Trim();
            usuario.celular = txtTelefono.Text.Trim();
            SoftInvBusiness.SoftInvWSEmpleado.empleadoDTO user = new SoftInvBusiness.SoftInvWSEmpleado.empleadoDTO
            {
                nombre = usuario.nombre,
                primerapellido = usuario.primerapellido,
                segundoapellido = usuario.segundoapellido,
                correoElectronico = usuario.correoElectronico,
                contrasenha = usuario.contrasenha,
                celular = usuario.celular,
                urlFotoPerfil = usuario.urlFotoPerfil,
                rol = 2,
                idUsuario = usuario.idUsuario,
                activo = 1,
                rolSpecified = true,
                idUsuarioSpecified = true,
                activoSpecified = true
            };

            try
            {
                empleadoBO.ModificarEmpleado(user);


                Session["UsuarioActual"] = usuario;

                SetModoEdicion(false);
                CargarPerfil();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            SetModoEdicion(false);
            CargarPerfil();
        }
    }
}