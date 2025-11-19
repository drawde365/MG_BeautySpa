using SoftInvBusiness;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class BuscarUsuarios : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private EmpleadoBO empleadoBO;
        private ServicioBO servicioBO; // BO de servicios (ajusta al nombre real si difiere)

        public BuscarUsuarios()
        {
            usuarioBO = new UsuarioBO();
            empleadoBO = new EmpleadoBO();
            servicioBO = new ServicioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarUsuarios();
            }
        }

        private void CargarUsuarios()
        {
            var lista = usuarioBO.ObtenerTodosUsuarios();
            if (lista == null)
            {
                rpUsuarios.DataSource = null;
            }
            else
            {
                rpUsuarios.DataSource = lista;
            }
            rpUsuarios.DataBind();
        }

        protected void btnReactivar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int userId = Convert.ToInt32(btn.CommandArgument);

            usuarioBO.actividadUsuario(userId, 1);

            CargarUsuarios();
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int userId = Convert.ToInt32(btn.CommandArgument);

            usuarioBO.actividadUsuario(userId, 0);

            CargarUsuarios();
        }

        protected void rpUsuarios_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                int activo = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "activo"));

                LinkButton btnEliminar = (LinkButton)e.Item.FindControl("btnEliminar");
                LinkButton btnReactivar = (LinkButton)e.Item.FindControl("btnReactivar");

                if (activo == 1)
                {
                    btnEliminar.Visible = true;   // usuario activo → mostrar eliminar
                    btnReactivar.Visible = false;
                }
                else
                {
                    btnEliminar.Visible = false;
                    btnReactivar.Visible = true;  // usuario inactivo → mostrar reactivar
                }
            }
        }

        // ---------------- MODAL SERVICIOS EMPLEADO ----------------

        /// <summary>
        /// Carga datos del empleado + servicios del empleado + servicios disponibles
        /// y deja el id del empleado en hfEmpleadoId.
        /// </summary>
        private void CargarServiciosEmpleado(int idEmpleado)
        {
            // Guardar empleado actual
            hfEmpleadoId.Value = idEmpleado.ToString();

            var empleado = empleadoBO.ObtenerEmpleadoPorId(idEmpleado);
            var serviciosEmpleado = empleadoBO.ListarServiciosDeEmpleado(idEmpleado);

            // Cabecera del modal
            litEmpNombre.Text = string.Format("{0} {1} {2}",
                empleado.nombre,
                empleado.primerapellido,
                empleado.segundoapellido);

            litEmpCorreo.Text = empleado.correoElectronico;

            // Tabla de servicios del empleado
            rpServiciosEmpleado.DataSource = serviciosEmpleado;
            rpServiciosEmpleado.DataBind();

            // Servicios disponibles para asignar (ajusta al método que tengas en tu BO)
            var disponibles = servicioBO.ListarTodoActivo(); // si tienes uno más específico, úsalo aquí
            ddlServiciosDisponibles.DataSource = disponibles;
            ddlServiciosDisponibles.DataTextField = "nombre";
            ddlServiciosDisponibles.DataValueField = "idServicio";
            ddlServiciosDisponibles.DataBind();

            ddlServiciosDisponibles.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        /// <summary>
        /// Abrir modal de servicios para un empleado.
        /// </summary>
        protected void btnServicios_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int idEmpleado = int.Parse(btn.CommandArgument);

            CargarServiciosEmpleado(idEmpleado);

            // Mostrar modal principal
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "abrirModalServicios",
                "showModalFormEmpleado();",
                true
            );
        }

        /// <summary>
        /// Comandos de la tabla de servicios (eliminar servicio).
        /// </summary>
        protected void rpServiciosEmpleado_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EliminarServicio")
            {
                int idServicio = int.Parse(e.CommandArgument.ToString());
                int idEmpleado = int.Parse(hfEmpleadoId.Value);

                // Eliminar relación servicio-empleado
                empleadoBO.EliminarServicioDeEmpleado(idEmpleado, idServicio);

                // Recargar datos del modal (servicios + combo)
                CargarServiciosEmpleado(idEmpleado);

                // Mantener el modal principal abierto
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "abrirModalServicios",
                    "showModalFormEmpleado();",
                    true
                );
            }
        }

        /// <summary>
        /// Guardar nuevo servicio para el empleado (modal Agregar servicio).
        /// </summary>
        protected void btnGuardarServicio_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfEmpleadoId.Value))
                return;

            int idEmpleado = int.Parse(hfEmpleadoId.Value);

            // Validar selección de servicio
            if (string.IsNullOrEmpty(ddlServiciosDisponibles.SelectedValue))
                return;

            int idServicio = int.Parse(ddlServiciosDisponibles.SelectedValue);


            // Registrar el servicio para el empleado
            // Ajusta el nombre del método a tu BO real si se llama distinto
            empleadoBO.AgregarServicioAEmpleado(idEmpleado, idServicio);

            // Recargar datos del modal
            CargarServiciosEmpleado(idEmpleado);

 
            // Cerrar modal Agregar y mantener abierto modal principal
            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "refrescarServicios",
                @"
                var mAgregar = bootstrap.Modal.getOrCreateInstance(document.getElementById('modalAgregarServicio'));
                mAgregar.hide();
                showModalFormEmpleado();",
                true
            );
        }
    }
}
