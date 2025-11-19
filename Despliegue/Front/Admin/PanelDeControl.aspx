<%@ Page Title="Panel de Control" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="PanelDeControl.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.PanelDeControl" %>

<%-- 1. CSS Específico de esta página (para la tabla) --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Título 'Reservas' */
        .h1-reservas {
            font-family: 'ZCOOL XiaoWei', serif;
            font-size: 48px;
            line-height: 40px;
            color: #1A0F12;
            margin-bottom: 24px;
        }
        
        /* Contenedor de la tabla */
        .table-container {
            background: #FAFAFA;
            border: 1px solid #E3D4D9;
            border-radius: 12px;
            overflow: hidden;
        }

        .table {
            margin-bottom: 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }

        /* Cabecera de la tabla */
        .table thead th {
            background: #FAFAFA;
            font-weight: 500;
            font-size: 14px;
            color: #1A0F12;
            padding: 12px 16px;
            border-bottom: 0;
            text-align: left;
        }
        .table thead th:first-child { font-size: 12px; line-height: 21px; padding: 2px 16px 12px; }
        .table thead th:last-child { text-align: center; }

        /* Filas de la tabla */
        .table tbody tr { border-top: 1px solid #E6E8EB; }
        .table tbody td {
            font-size: 14px;
            font-weight: 400;
            color: #1A0F12;
            padding: 8px 16px;
            vertical-align: middle;
            text-align: left;
        }
        .table tbody td:last-child { text-align: center; }

        .btn-custom-teal {
            /* Estilo de los botones 'Depth 5, Frame 1' */
            background-color: #1EC3B6;
            color: #FCF7FA;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 14px;
            line-height: 21px;
            /* Dimensiones y alineación del botón */
            width: 143px;
            height: 40px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            text-decoration: none;
        }

        /* Estilos para los botones de estado (badges) */
        .badge-estado {
            padding: 4px 16px;
            height: 32px;
            min-width: 84px;
            font-weight: 500;
            font-size: 14px;
            line-height: 24px;
            border-radius: 16px;
            display: inline-block;
        }
        .badge-completado { background-color: #148C76; color: #FFFFFF; }
        .badge-cancelado { background-color: #C31E1E; color: #FFFFFF; }
        .badge-pendiente { background-color: #A6A6A6; color: #FFFFFF; }
    </style>
</asp:Content>


<%-- 2. CONTENIDO PRINCIPAL (El título y la tabla) --%>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div>
        <h1 class="h1-reservas">Reservas</h1>
    </div>

    <div class="table-container">
        
        <table class="table">
            <thead>
                <tr>
                    <th scope="col">Cliente<br>(Celular)</th>
                    <th scope="col">Servicio</th>
                    <th scope="col">Fecha</th>
                    <th scope="col">Hora</th>
                    <th scope="col">Estado</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td>Sophia Clark<br>(987654321)</td>
                    <td>Tratamiento facial</td>
                    <td>15/03/2025</td>
                    <td>10:00 AM</td>
                    <td><span class="badge-estado badge-completado">Completado</span></td>
                </tr>
                <tr>
                    <td>Ethan Carter<br>(974386432)</td>
                    <td>Terapia de masaje</td>
                    <td>16/03/2025</td>
                    <td>2:00 PM</td>
                    <td><span class="badge-estado badge-completado">Completado</span></td>
                </tr>
                <tr>
                    <td>Olivia Bennett<br>(978847342)</td>
                    <td>Acupuntura</td>
                    <td>20/03/2025</td>
                    <td>11:30 AM</td>
                    <td><span class="badge-estado badge-cancelado">Cancelado</span></td>
                </tr>
                <tr>
                    <td>Liam Foster<br>(986887373)</td>
                    <td>Manicure</td>
                    <td>01/04/2025</td>
                    <td>3:00 PM</td>
                    <td><span class="badge-estado badge-pendiente">Pendiente</span></td>
                </tr>
            </tbody>
        </table>
        
    </div>
    <asp:LinkButton ID="btnVerMas" runat="server" CssClass="btn btn-custom-teal rounded-pill"
        OnClick="btnVerMas_Click">Ver más</asp:LinkButton>

</asp:Content>


<%-- 3. Scripts específicos de la página (si los necesitas) --%>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <%-- Tus scripts aquí --%>
</asp:Content>