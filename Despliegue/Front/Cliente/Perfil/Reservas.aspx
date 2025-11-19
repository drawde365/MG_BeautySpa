<%@ Page Title="Historial de Reservas"
Language="C#"
MasterPageFile="~/Cliente/Perfil/Perfil.Master"
AutoEventWireup="true"
CodeBehind="Reservas.aspx.cs"
Inherits="MGBeautySpaWebAplication.Cliente.Perfil.Reservas" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">
    <link rel="stylesheet" href="Reservas.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">

    <div class="reservas-container">
        <h2 class="reservas-titulo">Historial de Reservas</h2>

        <asp:Repeater ID="rptReservas" runat="server"
            OnItemDataBound="rptReservas_ItemDataBound" 
            OnItemCommand="rptReservas_ItemCommand">

            <ItemTemplate>
                <div class="reserva-item">
                    <div class="reserva-info">
                        <h4 class="reserva-numero">Reserva N.º <%# Eval("NumeroReserva") %></h4>
                        <p><strong>Servicio:</strong> <%# Eval("Servicio") %></p>
                        <p><strong>Fecha:</strong> <%# Eval("Fecha") %></p>

                        <p><strong>Hora:</strong> <%# Eval("HoraInicio") %></p>
    
                        <p><strong>Empleado:</strong> <%# Eval("Empleado") %></p>

                        <div class="empleado-contacto">
                            <span class="contacto-item">
                                <i class="bi bi-envelope-fill"></i> <%# Eval("EmpleadoCorreo") %>
                            </span>

                            <span class="contacto-item">
                                <i class="bi bi-phone-fill"></i> <%# Eval("EmpleadoCelular") %>
                            </span>
                        </div>
                    </div>

                    <div class="reserva-estado-y-total">
                        <div class="reserva-total">
                            <span class="total-label">Total:</span>
                            <strong><%# Eval("Total") %></strong>
                        </div>

                        <div class="reserva-accion">
                            <asp:LinkButton 
                                ID="btnCancelarCita" 
                                runat="server" 
                                CssClass="btn-cancelar"
                                CommandName="Cancelar"
                                ToolTip="Cancelar Cita"
                                Visible="false">
                                <i class="bi bi-trash-fill"></i> Cancelar
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

        <asp:Panel ID="pnlNoReservas" runat="server" CssClass="no-pedidos-mensaje" Visible="false">
            Aún no tienes reservas en tu historial.
        </asp:Panel>

        <div class="reservas-footer">
            <asp:Button ID="btnVerMas" runat="server"
                        Text="Ver Más"
                        CssClass="btn-vermas"
                        OnClick="btnVerMas_Click" />

            <asp:Button ID="btnRegresar" runat="server"
                        Text="Regresar"
                        CssClass="btn-regresar"
                        OnClick="btnRegresar_Click" />
        </div>
    </div>

    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const ahora = new Date();

            document.querySelectorAll(".reserva-item").forEach(item => {
                const fecha = item.getAttribute("data-fecha");
                const hora = item.getAttribute("data-hora");

                if (fecha && hora) {
                    const fechaReserva = new Date(fecha + "T" + hora);

                    if (fechaReserva > ahora) {
                        const btn = item.querySelector(".btn-cancelar");
                        if (btn) btn.style.display = "inline-flex";
                    }
                }
            });
        });
    </script>

</asp:Content>
