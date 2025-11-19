using SoftInvBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class BuscarUsuarios : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;
        private EmpleadoBO empleadoBO;
        public BuscarUsuarios()
        {
            usuarioBO = new UsuarioBO();
            empleadoBO = new EmpleadoBO();
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

            usuarioBO.actividadUsuario(userId,1);
            
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

        protected void btnServicios_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;
            int idEmpleado = int.Parse(btn.CommandArgument);

            // Guardar empleado seleccionado para futuras operaciones
            hfEmpleadoId.Value = idEmpleado.ToString();

            var empleado = empleadoBO.ObtenerEmpleadoPorId(idEmpleado);
            var servicios = empleadoBO.ListarServiciosDeEmpleado(idEmpleado);

            string nombre = $"{empleado.nombre} {empleado.primerapellido} {empleado.segundoapellido}";
            string correo = empleado.correoElectronico;

            JavaScriptSerializer js = new JavaScriptSerializer();
            string serviciosJson = js.Serialize(servicios);


            string eliminarTarget = btnEliminarServicio.UniqueID;

            string script = $@"
                llenarModalEmpleado('{nombre}', '{correo}', {serviciosJson}, '{eliminarTarget}');
                showModalFormEmpleado();
            ";

            ScriptManager.RegisterStartupScript(this, GetType(), "modalServicios", script, true);
        }

        protected void btnEliminarServicio_Click(object sender, EventArgs e)
        {
            string idServicioStr = Request["__EVENTARGUMENT"];
            if (string.IsNullOrEmpty(idServicioStr)) return;

            int idServicio = int.Parse(idServicioStr);
            int idEmpleado = int.Parse(hfEmpleadoId.Value);
            
            empleadoBO.EliminarServicioDeEmpleado(idEmpleado,idServicio);
            var servicios = empleadoBO.ListarServiciosDeEmpleado(idEmpleado);
            JavaScriptSerializer js = new JavaScriptSerializer();
            string serviciosJson = js.Serialize(servicios);

            string eliminarTarget = btnEliminarServicio.UniqueID;

            string script = $@"
                actualizarTablaServicios({serviciosJson}, '{eliminarTarget}');
                showModalFormEmpleado();
            ";

            ScriptManager.RegisterStartupScript(this, GetType(), "recargarServicios", script, true);
        }
    }
}