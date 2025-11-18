using SoftInvBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class BuscarUsuarios : System.Web.UI.Page
    {
        private UsuarioBO usuarioBO;

        public BuscarUsuarios()
        {
            usuarioBO = new UsuarioBO();
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
            rpUsuarios.DataSource = usuarioBO.ObtenerTodosUsuarios();
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

    }
}