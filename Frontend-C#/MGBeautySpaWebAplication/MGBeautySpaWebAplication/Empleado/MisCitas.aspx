<%@ Page Title="" Language="C#" MasterPageFile="Empleado.Master" AutoEventWireup="true" CodeBehind="MisCitas.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.MisCitas" %>

<%-- 1. Título de la Página --%>
<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Citas
</asp:Content>

<%-- 2. ENLACE AL NUEVO CSS (Aquí está el cambio) --%>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- 
        Este 'ContentPlaceHolder' está en el <head> de tu MasterPage.
        Es el lugar correcto para enlazar hojas de estilo específicas de la página.
        Usamos ResolveUrl para asegurar que la ruta al archivo CSS sea correcta.
    --%>
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/CitasStyles.css") %>" />
</asp:Content>

<%-- 3. Contenido Principal de la Página (Este HTML no cambia) --%>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container-lg py-4">

        <h1 class="h2 mb-1 font-zcool" style="color: #148C76;">Citas</h1>
        <p class="fs-5" style="color: #555;">Revise las citas reservadas pendientes y completadas.</p>

        <ul class="nav citas-nav-tabs mt-4" id="citasTab" role="tablist">
            <li class="nav-item" role="presentation">
                <button class="nav-link active" id="proximas-tab" data-bs-toggle="tab" data-bs-target="#proximas-pane" type="button" role="tab" aria-controls="proximas-pane" aria-selected="true">Próximas</button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="pasadas-tab" data-bs-toggle="tab" data-bs-target="#pasadas-pane" type="button" role="tab" aria-controls="pasadas-pane" aria-selected="false">Pasadas</button>
            </li>
            <li class="nav-item" role="presentation">
                <button class="nav-link" id="canceladas-tab" data-bs-toggle="tab" data-bs-target="#canceladas-pane" type="button" role="tab" aria-controls="canceladas-pane" aria-selected="false">Canceladas</button>
            </li>
        </ul>

        <div class="tab-content" id="citasTabContent">
            
            <div class="tab-pane fade show active" id="proximas-pane" role="tabpanel" aria-labelledby="proximas-tab" tabindex="0">
                <div class="citas-tabla-container mt-4">

                    <div class="citas-fila citas-fila-header d-none d-md-flex">
                        <div class="citas-col">Cliente (Celular)</div>
                        <div class="citas-col">Servicio</div>
                        <div class="citas-col">Fecha</div>
                        <div class="citas-col">Hora</div>
                        <div class="citas-col-estado">Estado</div>
                    </div>

                    <div class="citas-fila">
                        <div class="citas-col">
                            <div class="fw-bold d-md-none">Cliente</div>
                            <div>Sophia Clark</div>
                            <div class="text-muted" style="font-size: 12px;">(987654321)</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Servicio</div>
                            <div>Tratamiento facial</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Fecha</div>
                            <div>15/03/2025</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Hora</div>
                            <div>10:00 AM</div>
                        </div>
                        <div class="citas-col-estado">
                            <div class="fw-bold d-md-none mt-2">Estado</div>
                            <span class="badge rounded-pill text-white estado-badge" style="background-color: #148C76;">Completado</span>
                        </div>
                    </div>

                    <div class="citas-fila">
                        <div class="citas-col">
                            <div class="fw-bold d-md-none">Cliente</div>
                            <div>Ethan Carter</div>
                            <div class="text-muted" style="font-size: 12px;">(974386432)</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Servicio</div>
                            <div>Terapia de masaje</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Fecha</div>
                            <div>16/03/2025</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Hora</div>
                            <div>2:00 PM</div>
                        </div>
                        <div class="citas-col-estado">
                            <div class="fw-bold d-md-none mt-2">Estado</div>
                            <span class="badge rounded-pill text-white estado-badge" style="background-color: #148C76;">Completado</span>
                        </div>
                    </div>
                    
                    <div class="citas-fila">
                        <div class="citas-col">
                            <div class="fw-bold d-md-none">Cliente</div>
                            <div>Olivia Bennett</div>
                            <div class="text-muted" style="font-size: 12px;">(978847342)</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Servicio</div>
                            <div>Acupuntura</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Fecha</div>
                            <div>20/03/2025</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Hora</div>
                            <div>11:30 AM</div>
                        </div>
                        <div class="citas-col-estado">
                            <div class="fw-bold d-md-none mt-2">Estado</div>
                            <span class="badge rounded-pill text-white estado-badge" style="background-color: #C31E1E;">Cancelado</span>
                        </div>
                    </div>

                    <div class="citas-fila">
                        <div class="citas-col">
                            <div class="fw-bold d-md-none">Cliente</div>
                            <div>Liam Foster</div>
                            <div class="text-muted" style="font-size: 12px;">(986887373)</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Servicio</div>
                            <div>Manicure</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Fecha</div>
                            <div>01/04/2025</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Hora</div>
                            <div>3:00 PM</div>
                        </div>
                        <div class="citas-col-estado">
                            <div class="fw-bold d-md-none mt-2">Estado</div>
                            <span class="badge rounded-pill text-white estado-badge" style="background-color: #A6A6A6;">Pendiente</span>
                        </div>
                    </div>

                    <div class="citas-fila">
                        <div class="citas-col">
                            <div class="fw-bold d-md-none">Cliente</div>
                            <div>Ava Reynolds</div>
                            <div class="text-muted" style="font-size: 12px;">(9748764673)</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Servicio</div>
                            <div>Pedicure</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Fecha</div>
                            <div>19/06/2025</div>
                        </div>
                        <div class="citas-col">
                            <div class="fw-bold d-md-none mt-2">Hora</div>
                            <div>1:00 PM</div>
                        </div>
                        <div class="citas-col-estado">
                            <div class="fw-bold d-md-none mt-2">Estado</div>
                            <span class="badge rounded-pill text-white estado-badge" style="background-color: #A6A6A6;">Pendiente</span>
                        </div>
                    </div>

                </div> <div class="text-center py-4">
                    <button class="btn text-white" style="background-color: #148C76; padding: 8px 24px; font-weight: 500;">Ver más</button>
                </div>

            </div>
            
            <div class="tab-pane fade" id="pasadas-pane" role="tabpanel" aria-labelledby="pasadas-tab" tabindex="0">
                <div class="p-4">
                    <p>Aquí se mostrará la lista de citas pasadas y completadas.</p>
                </div>
            </div>

            <div class="tab-pane fade" id="canceladas-pane" role="tabpanel" aria-labelledby="canceladas-tab" tabindex="0">
                <div class="p-4">
                    <p>Aquí se mostrará la lista de citas canceladas.</p>
                </div>
            </div>

        </div> </div> </asp:Content>