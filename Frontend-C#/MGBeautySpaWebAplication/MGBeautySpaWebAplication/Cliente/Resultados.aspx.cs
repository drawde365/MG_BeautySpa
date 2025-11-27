using SoftInvBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Cliente
{
    public partial class Resultados : Page
    {
        private ProductoBO productoBO;
        private ServicioBO servicioBO;

        public Resultados()
        {
            productoBO = new ProductoBO();
            servicioBO = new ServicioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string q = (Request.QueryString["q"] ?? "").Trim();
            litQuery.Text = q;
            Literal1.Text = q;

            var productos = productoBO.ListarTodosActivos()
                .Select(p => new {
                    urlDestino = ResolveUrl("~/Cliente/DetalleProducto.aspx?id=" + p.idProducto),
                    urlImagen = ResolveUrl(p.urlImagen),
                    nombre = p.nombre,
                    precio = p.precio,
                    tipo = "Producto"
                });

            var servicios = servicioBO.ListarTodoActivo()
                .Select(s => new {
                    urlDestino = ResolveUrl("~/Cliente/DetalleServicio.aspx?id=" + s.idServicio),
                    urlImagen = ResolveUrl(s.urlImagen),
                    nombre = s.nombre,
                    precio = s.precio,
                    tipo = "Servicio"
                });

            var productosYServicios = productos.Concat(servicios).ToList();

            rptProductos.DataSource = productosYServicios;
            rptProductos.DataBind();

            litCount.Text = "";
        }
    }
}