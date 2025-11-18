<%@ Page Title="Mi Horario" Language="C#" MasterPageFile="~/Empleado/Empleado.Master" AutoEventWireup="true" CodeBehind="MiHorario.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.MiHorario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Mi Horario
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Fuente personalizada para el título */
        .empleado-title-font {
            font-family: 'ZCOOL XiaoWei', serif;
            font-weight: 400;
            font-size: 40px;
            line-height: 40px;
            color: #148C76;
        }

        /* Botón "Agregar excepción" */
        .badge-status {
            background-color: #148C76;
            color: #FFFFFF;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 14px;
            padding: 8px 16px;
            border-radius: 20px;
            text-decoration: none;
            transition: background 0.3s;
        }
        .badge-status:hover {
            background-color: #0e6b5a;
            color: #fff;
        }
        
        /* Contenedor de tablas */
        .schedule-container {
            border: 1px solid #78D5CD;
            border-radius: 12px;
            background: #FAFAFA;
            overflow: hidden;
            font-family: 'Plus Jakarta Sans', sans-serif;
            margin-bottom: 2rem;
        }

        /* Estilos tabla Horario */
        .schedule-table { border: none; margin-bottom: 0; width: 100%; }
        .schedule-table th, .schedule-table td {
            border: 1px solid #78D5CD;
            text-align: center; vertical-align: middle;
            font-size: 14px; padding: 4px; height: 27px;
        }
        .schedule-table thead th {
            background-color: #148C76; color: #FFFFFF; padding: 12px; height: 46px;
        }
        .schedule-table tbody th {
            background-color: #148C76; color: #FFFFFF; width: 64px;
        }
        .cell-occupied { background-color: #78D5CD !important; }
        .cell-free { background-color: #FFFFFF; }

        /* --- NUEVOS ESTILOS PARA EXCEPCIONES --- */
        .exceptions-section {
            margin-top: 30px;
            border-top: 2px dashed #E3D4D9;
            padding-top: 20px;
        }
        
        .exceptions-table th {
            background-color: #f8f9fa;
            color: #1C0D12;
            font-weight: 700;
            border-bottom: 2px solid #148C76;
            padding: 12px;
        }
        .exceptions-table td {
            padding: 12px;
            border-bottom: 1px solid #eee;
            vertical-align: middle;
        }
        
        .btn-rehabilitar {
            color: #d9534f;
            background: none;
            border: 1px solid #d9534f;
            padding: 4px 10px;
            border-radius: 6px;
            font-size: 0.85rem;
            transition: all 0.2s;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }
        .btn-rehabilitar:hover {
            background-color: #d9534f;
            color: #fff;
        }
        .text-muted-small { font-size: 0.85rem; color: #757575; }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" />

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" role="alert">
        <asp:Literal ID="litAlertMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <div class="main-content-wrapper bg-white p-4 rounded-3 shadow-sm">

        <%-- Cabecera --%>
        <div class="d-flex flex-wrap justify-content-between align-items-center mb-4">
            <h1 class="empleado-title-font m-0">Horario de trabajo</h1>
            <asp:HyperLink ID="hlAgregarExcepcion" runat="server" 
                NavigateUrl="~/Empleado/AgregarExcepcion.aspx" 
                CssClass="badge-status">
                <i class="bi bi-plus-circle me-1"></i> Agregar excepción
            </asp:HyperLink>
        </div>

        <%-- 1. Tabla de Horario Semanal --%>
        <div class="schedule-container">
            <table class="table schedule-table m-0">
                <thead>
                    <tr>
                        <th></th>
                        <th>Lunes</th><th>Martes</th><th>Miércoles</th><th>Jueves</th><th>Viernes</th><th>Sábado</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptHorario" runat="server">
                        <ItemTemplate>
                            <tr>
                                <th scope="row"><%# Eval("Hora") %></th>
                                <td class='<%# GetCellClass((bool)Eval("Lunes")) %>'></td>
                                <td class='<%# GetCellClass((bool)Eval("Martes")) %>'></td>
                                <td class='<%# GetCellClass((bool)Eval("Miercoles")) %>'></td>
                                <td class='<%# GetCellClass((bool)Eval("Jueves")) %>'></td>
                                <td class='<%# GetCellClass((bool)Eval("Viernes")) %>'></td>
                                <td class='<%# GetCellClass((bool)Eval("Sabado")) %>'></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <%-- 2. Sección de Excepciones Registradas --%>
        <div class="exceptions-section">
            <h3 class="h5 mb-3" style="color: #1C0D12; font-weight: 700;">
                Mis Excepciones
            </h3>

            <asp:UpdatePanel ID="upExcepciones" runat="server">
                <ContentTemplate>
                    <div class="table-responsive">
                        <table class="table exceptions-table">
                            <thead>
                                <tr>
                                    <th style="width: 20%;">Fecha</th>
                                    <th style="width: 60%;">Motivo</th>
                                    <th style="width: 20%; text-align: center;">Acción</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptExcepciones" runat="server" OnItemCommand="rptExcepciones_ItemCommand">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("Fecha", "{0:dd/MM/yyyy}") %> <br />
                                                <span class="text-muted-small"><%# Eval("Fecha", "{0:dddd}") %></span>
                                            </td>
                                            <td>
                                                <%# Eval("Motivo") %>
                                            </td>
                                            <td class="text-center">
                                                <%-- Botón Rehabilitar: Solo visible si es fecha futura --%>
                                                <asp:LinkButton ID="btnRehabilitar" runat="server" 
                                                    CssClass="btn-rehabilitar"
                                                    CommandName="Rehabilitar"
                                                    CommandArgument='<%# Eval("Fecha", "{0:yyyy-MM-dd}") %>'
                                                    Visible='<%# (DateTime)Eval("Fecha") > DateTime.Now %>'
                                                    OnClientClick="return confirm('¿Estás seguro de eliminar esta excepción y rehabilitar tu horario para este día?');">
                                                    <i class="bi bi-trash"></i> Rehabilitar
                                                </asp:LinkButton>

                                                <asp:Label ID="lblPasado" runat="server" 
                                                    Text="Finalizado" 
                                                    CssClass="text-muted small" 
                                                    Visible='<%# (DateTime)Eval("Fecha") <= DateTime.Now %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                
                                <%-- Mensaje si no hay excepciones --%>
                                <asp:Panel ID="pnlNoExcepciones" runat="server" Visible="false">
                                    <tr>
                                        <td colspan="3" class="text-center py-4 text-muted">
                                            No tienes excepciones registradas.
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </tbody>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

    </div>
</asp:Content>