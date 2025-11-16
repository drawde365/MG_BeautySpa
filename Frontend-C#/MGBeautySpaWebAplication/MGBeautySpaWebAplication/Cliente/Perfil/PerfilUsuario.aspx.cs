using SoftInvBusiness; // Para el BO de Usuario
using SoftInvBusiness.SoftInvWSUsuario;
using SoftInvBusiness.SoftInvWSCliente;// Para el usuarioDTO
using System;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class PerfilUsuario : Page
    {
        private UsuarioBO usuarioBO;
        private ClienteBO clienteBO;
        public PerfilUsuario()
        {
            usuarioBO = new UsuarioBO();
            clienteBO = new ClienteBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPerfil();
                SetModoEdicion(false); // Asegura que esté en modo solo lectura
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

            // --- Cargar AMBOS modos (Literals y TextBoxes) ---

            // Modo Lectura
            litUserNameGreeting.Text = usuario.nombre;
            litNombres.Text = usuario.nombre;
            litApellidos.Text = $"{usuario.primerapellido} {usuario.segundoapellido}";
            litEmail.Text = usuario.correoElectronico;
            litTelefono.Text = usuario.celular;

            // Modo Edición
            txtNombres.Text = usuario.nombre;
            txtPrimerApellido.Text = usuario.primerapellido;
            txtSegundoApellido.Text = usuario.segundoapellido;
            litEmailEdit.Text = usuario.correoElectronico; // Email no se edita
            txtTelefono.Text = usuario.celular;
        }

        /// <summary>
        /// Controla qué paneles y botones son visibles.
        /// </summary>
        private void SetModoEdicion(bool enEdicion)
        {
            // Paneles de datos
            pnlReadOnly.Visible = !enEdicion;
            pnlEdit.Visible = enEdicion;

            // Botones
            btnEditarPerfil.Visible = !enEdicion;
            btnGuardar.Visible = enEdicion;
            btnVolver.Visible = enEdicion; // 'Volver' (Cancelar) solo se ve en modo edición
        }

        protected void btnEditarPerfil_Click(object sender, EventArgs e)
        {
            SetModoEdicion(true);
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            // 1. Validar los campos (según los validadores del ASPX)
            Page.Validate();
            if (!Page.IsValid)
            {
                return;
            }

            // 2. Obtener el usuario de la sesión
            var usuario = Session["UsuarioActual"] as SoftInvBusiness.SoftInvWSUsuario.usuarioDTO;
            if (usuario == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            // 3. Actualizar el objeto DTO
            usuario.nombre = txtNombres.Text.Trim();
            usuario.primerapellido = txtPrimerApellido.Text.Trim();
            usuario.segundoapellido = txtSegundoApellido.Text.Trim();
            usuario.celular = txtTelefono.Text.Trim();


            SoftInvBusiness.SoftInvWSCliente.clienteDTO user = new SoftInvBusiness.SoftInvWSCliente.clienteDTO
            {
                nombre = usuario.nombre,
                primerapellido = usuario.primerapellido,
                segundoapellido = usuario.segundoapellido,
                correoElectronico = usuario.correoElectronico,
                contrasenha = usuario.contrasenha,
                celular = usuario.celular,
                urlFotoPerfil = usuario.urlFotoPerfil,
                rol = 1,
                idUsuario = usuario.idUsuario,
                activo = 1,
                rolSpecified = true,
                idUsuarioSpecified = true,
                activoSpecified = true
            };

            try
            {
                // 4. Llamar al BO para guardar en la BD
                // (Asumo que tu UsuarioBO tiene un método 'modificar')
                clienteBO.ModificarDatosCliente(user);

                // 5. Actualizar la Sesión (importante)
                Session["UsuarioActual"] = usuario;

                // 6. Volver al modo solo lectura y recargar datos
                SetModoEdicion(false);
                CargarPerfil();
            }
            catch (Exception ex)
            {
                // Manejar error de guardado
                // (Podrías usar un Literal para mostrar el error)
            }
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            // "Volver" ahora actúa como "Cancelar"
            SetModoEdicion(false);
            // Recargamos los datos originales por si el usuario cambió algo
            CargarPerfil();
        }
    }
}