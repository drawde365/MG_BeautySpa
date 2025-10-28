<%@ Page Title="Historial de Reservas"
Language="C#"
MasterPageFile="~/Cliente/Perfil/Perfil.Master"
AutoEventWireup="true"
CodeBehind="Reservas.aspx.cs"
Inherits="MGBeautySpaWebAplication.Cliente.Perfil.Reservas" %>

<asp:Content ID="ctBody" ContentPlaceHolderID="ProfileBodyContent" runat="server">

    <link rel="stylesheet" href="Reservas.css" />

    <div class="reservas-container">
        <h2 class="reservas-titulo">Historial de Reservas</h2>

        <asp:Repeater ID="rptReservas" runat="server">
            <ItemTemplate>
                <div class="reserva-item">
                    <div class="reserva-info">
                        <h4 class="reserva-numero">Reserva N.º <%# Eval("NumeroReserva") %></h4>
                        <p><strong>Servicio:</strong> <%# Eval("Servicio") %></p>
                        <p><strong>Fecha:</strong> <%# Eval("Fecha") %></p>
                        <p><strong>Empleado:</strong> <%# Eval("Empleado") %></p>
                    </div>
                    <div class="reserva-total">
                        <span class="total-label">Total:</span>
                        <strong>S/ <%# Eval("Total") %></strong>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>

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

</asp:Content>