using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSServicio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class AdmServicios : System.Web.UI.Page
    {
        // Define cuántos ítems cargar por página (para "Ver más")
        private const int PageSize = 10;
        ServicioBO servicioBO;
        // Propiedad para rastrear la página actual

        public AdmServicios()
        {
            servicioBO = new ServicioBO();
        }
        private int CurrentPage
        {
            get { return (int)(ViewState["CurrentPage"] ?? 1); }
            set { ViewState["CurrentPage"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentPage = 1; // Empezar en la página 1
                CargarServicios();
            }
        }

        /// <summary>
        /// Carga los servicios desde la capa de negocio y los enlaza al Repeater.
        /// </summary>
        private void CargarServicios()
        {
            try
            {
                // 1. Llama a tu capa de 
                // Aquí deberías llamar a un método que liste tus servicios
                // Por ejemplo: bo.ListarServiciosPaginados(CurrentPage, PageSize);
                // *** INICIO: DATOS DE EJEMPLO (REEMPLAZAR) ***
                // Voy a simular una lista de servicios para este ejemplo
                var todosLosServicios = servicioBO.ListarTodoActivo();

                // Simular paginación
                var serviciosPaginados = todosLosServicios
                                            .Skip((CurrentPage - 1) * PageSize)
                                            .Take(PageSize)
                                            .ToList();
                // *** FIN: DATOS DE EJEMPLO ***

                // 2. Transforma los datos para el Repeater
                //    (Añade la URL de edición que falta en el DTO)
                var dataParaRepeater = serviciosPaginados.Select(s => new {
                    IDServicio = s.idServicio,
                    RutaImagen = s.urlImagen ?? "/Content/images/placeholder.png", // Imagen por defecto
                    NombreServicio = s.nombre,
                    Codigo = s.idServicio,
                    Tipo = s.tipo, // Asumiendo que 'tipo' es un string, si no, necesitarás un JOIN
                    Precio = s.precio,
                    Valoracion = s.promedioValoracion, // Lo usaremos para las estrellas
                    UrlEditar = $"~/Admin/EditarServicio.aspx?id={s.idServicio}" // URL dinámica
                }).ToList();

                // 3. Enlaza los datos al Repeater
                rptServicios.DataSource = dataParaRepeater;
                rptServicios.DataBind();

                // 4. Controla el botón "Ver más"
                // Si el total de servicios cargados es menor que el total esperado, oculta el botón
                // (Necesitarías un método bo.ContarTotalServicios() para esto)
                // bool hayMasDatos = (CurrentPage * PageSize) < bo.ContarTotalServicios();
                // btnVerMas.Visible = hayMasDatos;
                btnVerMas.Visible = (CurrentPage * PageSize) < todosLosServicios.Count; // Simulación
            }
            catch (Exception ex)
            {
                // Manejar el error, por ejemplo, mostrar un mensaje al usuario
                // lblError.Text = "Error al cargar servicios: " + ex.Message;
                // lblError.Visible = true;
                System.Diagnostics.Debug.WriteLine("Error al cargar servicios: " + ex.Message);
            }
        }

        /// <summary>
        /// Maneja los clics en los botones "Editar" y "Eliminar" dentro del Repeater.
        /// </summary>
        protected void rptServicios_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            // Obtener el ID del servicio desde CommandArgument
            int idServicio = Convert.ToInt32(e.CommandArgument);
            ServicioBO bo = new ServicioBO();

            switch (e.CommandName)
            {
                case "Editar":
                    // Redirige a la página de edición con el ID
                    Response.Redirect($"~/Admin/EditarServicio.aspx?id={idServicio}");
                    break;

                case "Eliminar":
                    // Lógica para eliminar (idealmente un borrado lógico)
                    try
                    {   
                        servicioDTO servicio = new servicioDTO();
                        servicio=servicioBO.obtenerPorId(idServicio);
                        servicioBO.eliminar(servicio);
                        // bo.EliminarServicio(idServicio); // Llama a tu método de negocio
                        //System.Diagnostics.Debug.WriteLine($"Simulando eliminación de servicio ID: {idServicio}");

                        // Recarga la lista para reflejar el cambio
                        CargarServicios();
                    }
                    catch (Exception ex)
                    {
                        // Manejar error de eliminación
                        System.Diagnostics.Debug.WriteLine("Error al eliminar servicio: " + ex.Message);
                    }
                    break;
            }
        }

        /// <summary>
        /// Carga la siguiente página de resultados.
        /// </summary>
        protected void btnVerMas_Click(object sender, EventArgs e)
        {
            CurrentPage++; // Incrementa el número de página
            CargarServicios(); // Vuelve a cargar los datos con la nueva página
        }

        /// <summary>
        /// Ayudante para renderizar las estrellas de valoración dinámicamente.
        /// </summary>
        protected string RenderStars(object valoracionObj)
        {
            int valoracion = 0;
            if (valoracionObj != null && valoracionObj != DBNull.Value)
            {
                valoracion = Convert.ToInt32(valoracionObj);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= 5; i++)
            {
                if (i <= valoracion)
                {
                    // Estrella llena
                    sb.Append("<i class=\"bi bi-star-fill\"></i> ");
                }
                else
                {
                    // Estrella vacía (usando la clase de tu CSS)
                    sb.Append("<i class=\"bi bi-star-fill star-empty\"></i> ");
                }
            }
            return sb.ToString();
        }
    }

    /*
    // --- Clases de simulación (BORRA ESTO y usa tus clases reales) ---
    public class ServicioBO
    {
        // Simulación de método
        public List<servicioDTO> ListarServicios() { return new List<servicioDTO>(); }
    }

    public class servicioDTO
    {
        public int idServicio { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string tipo { get; set; }
        public decimal precio { get; set; }
        public int valoracion { get; set; }
        public string rutaImagen { get; set; }
    }
    */
}