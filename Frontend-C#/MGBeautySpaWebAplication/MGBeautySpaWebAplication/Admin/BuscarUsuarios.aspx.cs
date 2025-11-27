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
        private ServicioBO servicioBO;

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
                    btnEliminar.Visible = true;
                    btnReactivar.Visible = false;
                }
                else
                {
                    btnEliminar.Visible = false;
                    btnReactivar.Visible = true;
                }
            }
        }

        private void CargarServiciosEmpleado(int idEmpleado)
        {
            hfEmpleadoId.Value = idEmpleado.ToString();

            var empleado = empleadoBO.ObtenerEmpleadoPorId(idEmpleado);
            var serviciosEmpleado = empleadoBO.ListarServiciosDeEmpleado(idEmpleado);

            litEmpNombre.Text = string.Format("{0} {1} {2}",
                empleado.nombre,
                empleado.primerapellido,
                empleado.segundoapellido);

            litEmpCorreo.Text = empleado.correoElectronico;

            rpServiciosEmpleado.DataSource = serviciosEmpleado;
            rpServiciosEmpleado.DataBind();

            var disponibles = empleadoBO.ObtenerServiciosNoBrindadosDeEmpleado(idEmpleado);

            ddlServiciosDisponibles.DataSource = disponibles;
            ddlServiciosDisponibles.DataTextField = "nombre";
            ddlServiciosDisponibles.DataValueField = "idServicio";
            ddlServiciosDisponibles.DataBind();

            ddlServiciosDisponibles.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }

        protected void btnServicios_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            int idEmpleado = int.Parse(btn.CommandArgument);

            CargarServiciosEmpleado(idEmpleado);

            ScriptManager.RegisterStartupScript(
                this,
                GetType(),
                "abrirModalServicios",
                "showModalFormEmpleado();",
                true
            );
        }

        protected void rpServiciosEmpleado_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "EliminarServicio")
            {
                int idServicio = int.Parse(e.CommandArgument.ToString());
                int idEmpleado = int.Parse(hfEmpleadoId.Value);

                empleadoBO.EliminarServicioDeEmpleado(idEmpleado, idServicio);

                CargarServiciosEmpleado(idEmpleado);

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "abrirModalServicios",
                    "showModalFormEmpleado();",
                    true
                );
            }
        }

        protected void btnGuardarServicio_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfEmpleadoId.Value))
                return;

            int idEmpleado = int.Parse(hfEmpleadoId.Value);

            if (string.IsNullOrEmpty(ddlServiciosDisponibles.SelectedValue))
                return;

            int idServicio = int.Parse(ddlServiciosDisponibles.SelectedValue);


            empleadoBO.AgregarServicioAEmpleado(idEmpleado, idServicio);

            CargarServiciosEmpleado(idEmpleado);

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