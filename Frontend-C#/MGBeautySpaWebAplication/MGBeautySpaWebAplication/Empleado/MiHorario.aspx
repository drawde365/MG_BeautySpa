<%@ Page Title="Mis Citas" Language="C#" MasterPageFile="Empleado.Master" AutoEventWireup="true" CodeBehind="MisCitas.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.MisCitas" %>

<%-- 1. Título de la Página --%>
<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Mis Citas
</asp:Content>

<%-- 2. ENLACE AL CSS --%>
<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/CitasStyles.css") %>" />
    <style>
        /* Estilo simple para el mensaje de 'no hay citas' */
        .citas-vacias-mensaje {
            text-align: center; padding: 40px 20px; color: #757575;
            font-family: 'Plus Jakarta Sans', sans-serif; font-size: 1.1em;
            background-color: #f9f9f9; border-radius: 8px; margin-top: 1.5rem;
        }
        /* Estilos para el modal */
        .modal-body .form-label { font-weight: 600; margin-top: 10px; }
        .modal-body .form-control { border-radius: 6px; }
    </style>
</asp:Content>

<%-- 3. Contenido Principal de la Página --%>
<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container-lg py-4">

        <h1 class="h2 mb-1 font-zcool" style="color: #148C76;">Citas</h1>
        <p class="fs-5" style="color: #555;">Revise las citas reservadas pendientes y completadas.</p>

        <%-- Pestañas de navegación --%>
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
            
            <%-- ================== PESTAÑA PRÓXIMAS (MODIFICADA) ================== --%>
            <div class="tab-pane fade show active" id="proximas-pane" role="tabpanel" aria-labelledby="proximas-tab" tabindex="0">
                <div class="citas-tabla-container mt-4">

                    <div class="citas-fila citas-fila-header d-none d-md-flex">
                        <div class="citas-col">Cliente (Celular)</div>
                        <div class="citas-col">Servicio</div>
                        <div class="citas-col">Fecha</div>
                        <div class="citas-col">Hora</div>
                        <div class="citas-col-estado">Estado</div>
                        <div class="citas-col-accion">Acción</div>
                    </div>

                    <%-- ▼▼▼ MODIFICACIÓN: Añadido OnItemCommand ▼▼▼ --%>
                    <asp:Repeater ID="rptProximas" runat="server" 
                        OnItemDataBound="rptCitas_ItemDataBound" 
                        OnItemCommand="rptProximas_ItemCommand">
                        <ItemTemplate>
                            <div class="citas-fila">
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none">Cliente</div>
                                    <div><%# Eval("ClienteNombre") %></div>
                                    <div class="text-muted" style="font-size: 12px;">(<%# Eval("ClienteCelular") %>)</div>
                                </div>
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none mt-2">Servicio</div>
                                    <div><%# Eval("ServicioNombre") %></div>
                                </div>
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none mt-2">Fecha</div>
                                    <div><%# Eval("Fecha") %></div>
                                </div>
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none mt-2">Hora</div>
                                    <div><%# Eval("HoraInicio") %></div>
                                </div>
                                <div class="citas-col-estado">
                                    <div class="fw-bold d-md-none mt-2">Estado</div>
                                    <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                                </div>
                                
                                <%-- ▼▼▼ NUEVO BOTÓN DE MODIFICAR ▼▼▼ --%>
                                <div class="citas-col-accion">
                                    <asp:LinkButton ID="btnModificar" runat="server"
                                        CssClass="btn btn-sm btn-outline-primary"
                                        Text="Modificar"
                                        CommandName="Modificar"
                                        CommandArgument='<%# Eval("CitaId") %>' />
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Panel ID="pnlNoProximas" runat="server" CssClass="citas-vacias-mensaje" Visible="false">
                        No tienes próximas citas programadas.
                    </asp:Panel>
                </div>
            </div>
            
            <%-- ... (Pestañas 'pasadas' y 'canceladas' sin cambios) ... --%>
            
        </div>
    </div> 

    <%-- ================== MODAL PARA MODIFICAR CITA ================== --%>
    <div class="modal fade" id="modificarCitaModal" tabindex="-1" aria-labelledby="modificarCitaModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <%-- El UpdatePanel permite que el modal se actualice sin recargar la página --%>
            <asp:UpdatePanel ID="upModal" runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modificarCitaModalLabel">Modificar Cita</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <%-- Campo oculto para guardar el ID de la cita que estamos editando --%>
                            <asp:HiddenField ID="hdnCitaIdModal" runat="server" />
                            
                            <div class="mb-3">
                                <label for="<%= txtNuevaFecha.ClientID %>" class="form-label">Nueva Fecha</label>
                                <asp:TextBox ID="txtNuevaFecha" runat="server" CssClass="form-control" TextMode="Date" />
                            </div>
                            <div class="mb-3">
                                <label for="<%= txtNuevaHora.ClientID %>" class="form-label">Nueva Hora (HH:mm)</label>
                                <asp:TextBox ID="txtNuevaHora" runat="server" CssClass="form-control" TextMode="Time" />
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                            <asp:Button ID="btnGuardarCambiosCita" runat="server" Text="Guardar Cambios" 
                                CssClass="btn btn-primary" OnClick="btnGuardarCambiosCita_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>