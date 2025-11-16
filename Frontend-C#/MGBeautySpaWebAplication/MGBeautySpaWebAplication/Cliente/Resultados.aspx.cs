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
            Literal1.Text = q; // Asigna la query al panel de "no resultados"

            // 1. OBTENER PRODUCTOS
            // Proyectamos a un tipo anónimo con propiedades comunes
            var productos = productoBO.ListarTodosActivos()
                .Select(p => new {
                    // Esta 'urlDestino' la usaremos en el href del ASPX
                    urlDestino = ResolveUrl("~/Cliente/DetalleProducto.aspx?id=" + p.idProducto),
                    urlImagen = ResolveUrl(p.urlImagen),
                    nombre = p.nombre,
                    precio = p.precio,
                    tipo = "Producto" // Etiqueta para JS y CSS
                });

            // 2. OBTENER SERVICIOS
            // Proyectamos al MISMO tipo anónimo
            var servicios = servicioBO.ListarTodoActivo()
                .Select(s => new {
                    // Asumimos una página de detalle diferente para servicios
                    urlDestino = ResolveUrl("~/Cliente/DetalleServicio.aspx?id=" + s.idServicio),
                    urlImagen = ResolveUrl(s.urlImagen), // Asumo que servicios también tienen urlImagen
                    nombre = s.nombre,
                    precio = s.precio,
                    tipo = "Servicio" // Etiqueta para JS y CSS
                });

            // 3. COMBINAR AMBAS LISTAS
            // Usamos Concat() para unir las dos listas y ToList() para ejecutar la consulta
            var productosYServicios = productos.Concat(servicios).ToList();

            // 4. ENLAZAR (BIND) LA LISTA COMBINADA
            rptProductos.DataSource = productosYServicios;
            rptProductos.DataBind();

            // El conteo de resultados lo hará el JS en el cliente.
            litCount.Text = "";
        }
    }
}