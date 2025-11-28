<%@ Page Title="Mis Citas" Async="true" Language="C#" MasterPageFile="Empleado.Master" AutoEventWireup="true" CodeBehind="MisCitas.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.MisCitas" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Mis Citas
</asp:Content>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/CitasStyles.css") %>" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
    
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <div class="container-lg py-4">

        <h1 class="h2 mb-1 font-zcool" style="color: #148C76;">Citas</h1>
        <p class="fs-5" style="color: #555;">Revise y gestione las citas reservadas.</p>

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
                        <div class="citas-col-accion">Acción</div>
                    </div>
                    <asp:Repeater ID="rptProximas" runat="server" OnItemDataBound="rptCitas_ItemDataBound" OnItemCommand="rptProximas_ItemCommand">
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
                                <div class="citas-col-accion">
                                    <asp:LinkButton ID="btnModificar" runat="server" CssClass="btn btn-sm btn-outline-primary" CommandName="Modificar" CommandArgument='<%# Eval("CitaId") %>' ToolTip="Modificar"><i class="bi bi-pencil-fill"></i></asp:LinkButton>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlNoProximas" runat="server" CssClass="citas-vacias-mensaje" Visible="false">No tiene próximas citas programadas.</asp:Panel>
                </div>
            </div>
            
            <div class="tab-pane fade" id="pasadas-pane" role="tabpanel" aria-labelledby="pasadas-tab" tabindex="0">
                <div class="citas-tabla-container mt-4">
                    <div class="citas-fila citas-fila-header d-none d-md-flex">
                        <div class="citas-col">Cliente (Celular)</div>
                        <div class="citas-col">Servicio</div>
                        <div class="citas-col">Fecha</div>
                        <div class="citas-col">Hora</div>
                        <div class="citas-col-estado">Estado</div>
                        <div class="citas-col-accion">Acción</div>
                    </div>
                   <asp:Repeater ID="rptPasadas" runat="server" OnItemDataBound="rptCitas_ItemDataBound" OnItemCommand="rptPasadas_ItemCommand">
                        <ItemTemplate>
                            <div class="citas-fila">
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none">Cliente</div>
                                    <div><asp:Literal ID="litClienteNombre" runat="server" Text='<%# Eval("ClienteNombre") %>'></asp:Literal></div>
                                    <div class="text-muted" style="font-size: 12px;">(<asp:Literal ID="litClienteCelular" runat="server" Text='<%# Eval("ClienteCelular") %>'></asp:Literal>)</div>
                                </div>
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none mt-2">Servicio</div>
                                    <div><asp:Literal ID="litServicioNombre" runat="server" Text='<%# Eval("ServicioNombre") %>'></asp:Literal></div>
                                </div>
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none mt-2">Fecha</div>
                                    <div><asp:Literal ID="litFecha" runat="server" Text='<%# Eval("Fecha") %>'></asp:Literal></div>
                                </div>
                                <div class="citas-col">
                                    <div class="fw-bold d-md-none mt-2">Hora</div>
                                    <div><asp:Literal ID="litHoraInicio" runat="server" Text='<%# Eval("HoraInicio") %>'></asp:Literal></div>
                                </div>
                                <div class="citas-col-estado">
                                    <div class="fw-bold d-md-none mt-2">Estado</div>
                                    <asp:Literal ID="litEstado" runat="server"></asp:Literal>
                                </div>
                                <div class="citas-col-accion">
                                    <div class="btn-action-group">
                                        <asp:LinkButton ID="btnAceptar" runat="server" CssClass="btn btn-sm btn-outline-success btn-accept" CommandName="Aceptar" CommandArgument='<%# Eval("CitaId") %>' ToolTip="Confirmar">
                                            <i class="bi bi-check-lg"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnCancelar" runat="server" CssClass="btn btn-sm btn-outline-danger btn-cancel" CommandName="Cancelar" CommandArgument='<%# Eval("CitaId") %>' ToolTip="Cancelar">
                                            <i class="bi bi-x-lg"></i>
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:Panel ID="pnlNoPasadas" runat="server" CssClass="citas-vacias-mensaje" Visible="false">No se encontraron citas pasadas.</asp:Panel>
                </div>
            </div>
            
            <div class="tab-pane fade" id="canceladas-pane" role="tabpanel" aria-labelledby="canceladas-tab" tabindex="0">
                <div class="citas-tabla-container mt-4">
                    <div class="citas-fila citas-fila-header d-none d-md-flex">
                        <div class="citas-col">Cliente (Celular)</div>
                        <div class="citas-col">Servicio</div>
                        <div class="citas-col">Fecha</div>
                        <div class="citas-col">Hora</div>
                        <div class="citas-col-estado">Estado</div>
                    </div>
                    <asp:Repeater ID="rptCanceladas" runat="server" OnItemDataBound="rptCitas_ItemDataBound">
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
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlNoCanceladas" runat="server" CssClass="citas-vacias-mensaje" Visible="false">No hay citas canceladas.</asp:Panel>
                </div>
            </div>

        </div>
    </div> 

    <%-- MODAL --%>
    <div class="modal fade" id="modificarCitaModal" tabindex="-1" aria-labelledby="modificarCitaModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <asp:UpdatePanel ID="upModal" runat="server">
                <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modificarCitaModalLabel">Modificar Cita</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"
                                onclick="window.location.href='/Empleado/MisCitas.aspx'">
                        </button>

                        </div>
                        <div class="modal-body">
                            <asp:HiddenField ID="hdnCitaIdModal" runat="server" />
                            <div class="mb-3"><label class="form-label">Nueva Fecha</label><asp:TextBox ID="txtNuevaFecha" runat="server" CssClass="form-control" TextMode="Date" /></div>
                            <div class="mb-3"><label class="form-label">Nueva Hora</label><asp:TextBox ID="txtNuevaHora" runat="server" CssClass="form-control" TextMode="Time" /></div>
                            <asp:Label ID="lblErrorModal" 
                               runat="server" 
                               CssClass="text-danger mt-2"
                               Visible="false"></asp:Label>
                        </div>
                        <div class="modal-footer">
                            
                            <asp:Button ID="btnCerrarModal" runat="server" Text="Cerrar" CssClass="btn btn-secondary" OnClick="btnCerrar_Click" />
                            <asp:Button ID="btnGuardarCambiosCita" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary" OnClick="btnGuardarCambiosCita_Click" />
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>