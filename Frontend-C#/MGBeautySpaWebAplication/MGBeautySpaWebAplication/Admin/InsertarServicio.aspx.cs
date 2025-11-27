using SoftInvBusiness;
using SoftInvBusiness.SoftInvWSServicio;
using System;
using System.IO;
using System.Web.UI;

namespace MGBeautySpaWebAplication.Admin
{
    public partial class InsertarServicio : System.Web.UI.Page
    {
        private ServicioBO servicioBO;

        protected void Page_Load(object sender, EventArgs e)
        {
            servicioBO = new ServicioBO();
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    int idServicio = Convert.ToInt32(Request.QueryString["id"]);
                    CargarDatosServicio(idServicio);
                }
                else
                {
                    h1Titulo.InnerText = "Añadir Servicio";
                    btnGuardar.Text = "Añadir Servicio";
                }
            }
        }

        private void CargarDatosServicio(int idServicio)
        {
            try
            {
                servicioDTO servicio = servicioBO.obtenerPorId(idServicio);

                if (servicio != null)
                {
                    h1Titulo.InnerText = "Editar Servicio";
                    btnGuardar.Text = "Actualizar Servicio";

                    txtTitulo.Text = servicio.nombre;
                    txtDescripcion.Text = servicio.descripcion;
                    txtPrecio.Text = servicio.precio.ToString("F2");
                    txtDuracion.Text = servicio.duracionHora.ToString();
                    ddlTipoServicio.SelectedValue = servicio.tipo;

                    if (!string.IsNullOrEmpty(servicio.urlImagen))
                    {
                        hdnImagenActual.Value = servicio.urlImagen;
                        fileUploadWrapper.Attributes["style"] = "background-image: url(" + ResolveUrl(servicio.urlImagen) + ")";
                        fileUploadWrapper.Attributes["class"] += " has-preview";
                    }
                }
            }
            catch (Exception ex)
            {
                litError.Text = "Error al cargar el servicio: " + ex.Message;
            }
        }

        protected void btnInsertarServicio_Click(object sender, EventArgs e)
        {
            try
            {
                servicioDTO servicio = new servicioDTO();
                string rutaImagenServidor = string.Empty;

                string carpetaGuardado = Server.MapPath("~/Content/images/Servicios/");
                if (!Directory.Exists(carpetaGuardado))
                {
                    Directory.CreateDirectory(carpetaGuardado);
                }

                if (fileUpload.HasFile)
                {
                    string nombreArchivo = Path.GetFileName(fileUpload.FileName);
                    string extension = Path.GetExtension(nombreArchivo).ToLower();

                    string[] extensionesValidas = { ".jpg", ".jpeg", ".png" };

                    if (Array.IndexOf(extensionesValidas, extension) < 0)
                    {
                        litError.Text = "Formato de archivo no válido. Solo se permiten imágenes (JPG, JPEG, PNG).";
                        return;
                    }

                    string nombreUnico = Guid.NewGuid().ToString() + extension;
                    string rutaCompleta = Path.Combine(carpetaGuardado, nombreUnico);
                    fileUpload.SaveAs(rutaCompleta);
                    rutaImagenServidor = "~/Content/images/Servicios/" + nombreUnico;
                }
                else if (Request.QueryString["id"] != null)
                {
                    rutaImagenServidor = hdnImagenActual.Value;
                }

                servicio.nombre = txtTitulo.Text.Trim();
                servicio.descripcion = txtDescripcion.Text.Trim();
                servicio.tipo = ddlTipoServicio.SelectedValue;
                servicio.urlImagen = rutaImagenServidor;

                double precio;
                if (double.TryParse(txtPrecio.Text, System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture, out precio))
                {
                    servicio.precio = precio;
                    servicio.precioSpecified = true;
                }

                int duracion;
                if (int.TryParse(txtDuracion.Text, out duracion))
                {
                    servicio.duracionHora = duracion;
                    servicio.duracionHoraSpecified = true;
                }

                servicio.promedioValoracion = 0;
                servicio.promedioValoracionSpecified = true;
                servicio.activo = 1;
                servicio.activoSpecified = true;

                if (Request.QueryString["id"] != null)
                {
                    servicio.idServicio = Convert.ToInt32(Request.QueryString["id"]);
                    servicio.idServicioSpecified = true;
                    servicioBO.modificar(servicio);
                }
                else
                {
                    servicioBO.insertar(servicio);
                }

                Response.Redirect("AdmServicios.aspx", false);
            }
            catch (Exception ex)
            {
                litError.Text = "Error al guardar el servicio: " + ex.Message;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdmServicios.aspx");
        }
    }
}