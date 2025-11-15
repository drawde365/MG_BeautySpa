using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSComentario;
using SoftInvBusiness.SoftInvWSServicio;
using SoftInvBusiness.SoftInvWSUsuario;
using System.Globalization;

namespace MGBeautySpaWebAplication.Cliente
{
    // Las siguientes clases son DTOs locales de ayuda para el DataBinding
    public class Resena
    {
        public string NombreAutor { get; set; }
        public string Fecha { get; set; }
        public int Rating { get; set; }
        public string Comentario { get; set; }
        public string AvatarUrl { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }

    public class ReviewSummary
    {
        public double ScorePromedio { get; set; }
        public int TotalCount { get; set; }
        public List<RatingBar> Bars { get; set; }
    }

    public class RatingBar
    {
        public int Stars { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public partial class DetalleServicio : Page
    {
        private ServicioBO servicioBO;
        private ComentarioBO comentarioBO;
        private SoftInvBusiness.SoftInvWSServicio.servicioDTO servicioActual;

        public DetalleServicio()
        {
            servicioBO = new ServicioBO();
            comentarioBO = new ComentarioBO();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    if (int.TryParse(Request.QueryString["id"], out int servicioId))
                    {
                        CargarDatos(servicioId);
                    }
                }
                else
                {
                    Response.Redirect("~/Cliente/Servicios.aspx");
                }
            }
            else
            {
                servicioActual = Session["servicio_actual"] as SoftInvBusiness.SoftInvWSServicio.servicioDTO;
            }
        }

        private void CargarDatos(int servicioId)
        {
            try
            {
                servicioActual = servicioBO.obtenerPorId(servicioId);

                if (servicioActual != null)
                {
                    Session["servicio_actual"] = servicioActual;
                    PoblarDatos(servicioActual);
                }
                else
                {
                    Response.Redirect("~/Cliente/Servicios.aspx");
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
            }
        }

        private void PoblarDatos(SoftInvBusiness.SoftInvWSServicio.servicioDTO servicio)
        {
            // --- Encabezado y Breadcrumb ---
            Page.Title = "Servicio: " + servicio.nombre;
            litNombreBreadcrumb.Text = servicio.nombre;
            litNombreServicio.Text = servicio.nombre;
            //litDescripcionCorta.Text = servicio.descripcion;

            // --- Sección Principal ---
            imgServicio.ImageUrl = ResolveUrl(servicio.urlImagen);
            imgServicio.AlternateText = servicio.nombre;
            litDescripcionLarga.Text = servicio.descripcion;

            // Precio con formato de moneda local
            litPrecio.Text = servicio.precio.ToString("C", new CultureInfo("es-PE"));

            PintarResenas(servicio);
        }

        private void PintarResenas(SoftInvBusiness.SoftInvWSServicio.servicioDTO servicio)
        {
            // Asumo que tu BO tiene un método para obtener comentarios por servicio
            var listaComentarios = comentarioBO.ObtenerComentariosPorServicio(servicio.idServicio);
            var totalComentarios = listaComentarios != null ? listaComentarios.Count : 0;
            var scorePromedio = servicio.promedioValoracionSpecified ? servicio.promedioValoracion : 0.0;

            // --- Resumen de Reviews ---
            litReviewScore.Text = scorePromedio.ToString("F1");
            litReviewCount.Text = $"{totalComentarios} reseñas";

            // --- Barras de Calificación (MOCK DE DATOS) ---
            // En una aplicación real, esto se calcularía sobre 'listaComentarios'
            var bars = new List<RatingBar>
            {
                 new RatingBar { Stars = 5, Count = (int)(totalComentarios * 0.7), Percentage = 70 },
                 new RatingBar { Stars = 4, Count = (int)(totalComentarios * 0.2), Percentage = 20 },
                 new RatingBar { Stars = 3, Count = (int)(totalComentarios * 0.1), Percentage = 10 },
                 new RatingBar { Stars = 2, Count = 0, Percentage = 0 },
                 new RatingBar { Stars = 1, Count = 0, Percentage = 0 }
            };

            rpRatingBars.DataSource = bars;
            rpRatingBars.DataBind();

            // --- Lista de Reseñas ---
            // Mapeamos los DTOs del WS a los DTOs locales (Resena) para el Repeater
            var resenasMapeadas = listaComentarios?.Select(c => new Resena
            {
                NombreAutor = c.cliente.nombre,
                Comentario = c.comentario,
                Rating = c.valoracion,
                Fecha = "Hace 1 semana", // Mock de fecha
                AvatarUrl = c.cliente.urlFotoPerfil
            }).ToList();

            rpResenas.DataSource = resenasMapeadas;
            rpResenas.DataBind();
        }

        // --- Eventos de Botones ---

        protected void btnReservarCita_Click(object sender, EventArgs e)
        {
            // Lógica para reservar cita
            Response.Redirect("~/Cliente/SeleccionarEmpleado.aspx?servicioId=" + servicioActual.idServicio);
        }

        protected void btnEnviarResena_Click(object sender, EventArgs e)
        {
            // Lógica para guardar la reseña
            string comentario = txtNuevaResena.Text;

            // Lógica de inserción en comentarioBO...

            // Limpiar y recargar
            txtNuevaResena.Text = "";

            // Vuelves a cargar los datos para mostrar la nueva reseña
            PoblarDatos(servicioActual);
        }
    }
}