<%@ Page Title="" Language="C#" MasterPageFile="~/Empleado/Empleado.Master" AutoEventWireup="true" CodeBehind="MiHorario.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.MiHorario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Fuente personalizada para el título "Horario de trabajo" */
        .empleado-title-font {
            font-family: 'ZCOOL XiaoWei', serif;
            font-weight: 400;
            font-size: 40px;
            line-height: 40px;
            color: #148C76;
        }

        /* Píldora de estado "Completado" */
        .badge-status {
            background-color: #148C76;
            color: #FFFFFF;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 14px;
            line-height: 21px;
            padding: 5px 16px; /* Ajuste para altura de 32px */
            border-radius: 16px; /* .rounded-pill de Bootstrap */
        }
        
        /* Contenedor de la tabla del horario */
        .schedule-container {
            border: 1px solid #78D5CD;
            border-radius: 12px;
            background: #FAFAFA;
            overflow: hidden; /* Para que la tabla respete los bordes redondeados */
            font-family: 'Plus Jakarta Sans', sans-serif;
        }

        /* Estilos de la tabla del horario */
        .schedule-table {
            border: none; /* Quitamos el borde por defecto */
            margin-bottom: 0; /* Quitamos el margen por defecto */
        }

        /* Estilos para todas las celdas (th y td) */
        .schedule-table th,
        .schedule-table td {
            border: 1px solid #78D5CD; /* Borde personalizado */
            text-align: center;
            vertical-align: middle;
            font-weight: 500;
            font-size: 14px;
            padding: 4px; /* Un padding más ajustado */
            height: 27px; /* Altura de fila de datos */
        }
        
        /* Cabecera de la tabla (Días de la semana) */
        .schedule-table thead th {
            background-color: #148C76;
            color: #FFFFFF;
            padding: 12px 16px;
            height: 46px; /* Altura de fila de cabecera */
        }

        /* Cabecera de Fila (Horas) */
        .schedule-table tbody th {
            background-color: #148C76;
            color: #FFFFFF;
            width: 64px;
            padding: 2px 0px;
        }

        /* Celdas de horario */
        .schedule-table td {
            width: 136px;
        }

        /* Celda de horario Ocupado */
        .cell-occupied {
            background-color: #78D5CD !important;
        }

        /* Celda de horario Libre */
        .cell-free {
            background-color: #FFFFFF;
        }
    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlAlert" runat="server" Visible="false" role="alert">
        <asp:Literal ID="litAlertMessage" runat="server" />
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <div class="main-content-wrapper bg-white p-3 rounded-3">

        <div class="d-flex flex-wrap justify-content-between align-items-center p-3">
            
            <div>
                <h1 class="empleado-title-font m-0">
                    Horario de trabajo
                </h1>
            </div>
            
            <div>
                <asp:HyperLink ID="hlAgregarExcepcion" runat="server" 
                    NavigateUrl="~/Empleado/AgregarExcepcion.aspx" 
                    CssClass="badge-status">
                    Agregar excepción
                </asp:HyperLink>
            </div>

        </div>
        <div class="p-3">
            
            <div class="schedule-container">
                
                <table class="table schedule-table">
                    
                    <thead>
                        <tr>
                            <th></th>
                            <th>Lunes</th>
                            <th>Martes</th>
                            <th>Miércoles</th>
                            <th>Jueves</th>
                            <th>Viernes</th>
                            <th>Sábado</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptHorario" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <%-- Columna de la Hora --%>
                                    <th scope="row">
                                        <%# Eval("Hora") %>
                                    </th>
                                    
                                    <%-- Celdas de Días (la clase se asigna dinámicamente) --%>
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
        </div>
        </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
