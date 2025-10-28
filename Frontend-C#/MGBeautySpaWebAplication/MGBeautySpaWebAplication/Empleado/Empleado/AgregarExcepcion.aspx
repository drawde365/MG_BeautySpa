<%@ Page Title="" Language="C#" MasterPageFile="~/Empleado/Empleado.Master" AutoEventWireup="true" CodeBehind="AgregarExcepcion.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.AgregarExcepcion" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        /* Título de la página */
        .exception-title {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 40px;
            line-height: 1.2;
            color: #1C0D12;
            text-align: center;
        }

        /* Subtítulo de la página */
        .exception-subtitle {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 28px;
            color: #1C0D12;
            text-align: center;
            line-height: 1.25;
        }

        /* Etiquetas del formulario (Fecha, Hora, etc.) */
        .form-label-lg {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 24px;
            color: #1C0D12;
            line-height: 1;
        }
        
        /* Estilo para "De" y "a" en la fila de Hora */
        .time-separator {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 400;
            font-size: 24px;
            color: #1C0D12;
            text-align: center;
        }

        /* Estilo para el botón de registro personalizado */
        .btn-custom-teal-lg {
            background-color: #1EC3B6;
            color: #FCF7FA;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 24px;
            line-height: 1.5;
            height: 69px;
            border-radius: 20px;
            border: none;
        }

        .btn-custom-teal-lg:hover {
            background-color: #19a599; /* Un poco más oscuro en hover */
            color: #FCF7FA;
        }

        /* Estilo para los inputs del formulario */
        .form-control-custom {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 20px;
            color: #757575;
            border: 1px solid #E3D4D9;
            border-radius: 12px;
            height: 56px;
        }
        
        .form-control-custom-area {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            color: #757575;
            border: 2px solid #F1E9EC;
            border-radius: 12px;
        }

    </style>
</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <%-- 
      Contenedor principal (Depth 3)
      Usamos container-lg para replicar el max-width: 960px
    --%>
    <div class="container-lg my-5">
        
        <%-- 
          Usamos un div como 'card' para agrupar el formulario.
          El 'gap: 15px' se traduce en los márgenes de los elementos.
        --%>
        <div class="bg-white p-4 p-md-5 rounded-3 shadow-sm">

            <div class="text-center mb-5">
                <h1 class="exception-title mb-3">
                    Agregar excepción en mi horario
                </h1>
                <p class="exception-subtitle">
                    Se aceptan excepciones de horario por motivos de salud y personales
                </p>
            </div>
            <div class="px-md-5">

                <%-- Fila 1: Fecha --%>
                <%-- Usamos el grid de Bootstrap para alinear label e input --%>
                <div class="row g-3 align-items-center mb-4">
                    <div class="col-md-2">
                        <asp:Label ID="lblFecha" runat="server" Text="Fecha:" CssClass="form-label-lg m-0" />
                    </div>
                    <div class="col-md-5">
                        <%-- Usar type="date" es mejor para la experiencia de usuario --%>
                        <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control form-control-custom" TextMode="Date" />
                    </div>
                </div>

                <%-- Fila 2: Hora --%>
                <%-- Usamos un grid más complejo para "De [input] a [input]" --%>
                <div class="row g-3 align-items-center mb-4">
                    <div class="col-md-2">
                        <asp:Label ID="lblHora" runat="server" Text="Hora:" CssClass="form-label-lg m-0" />
                    </div>
                    <div class="col-auto">
                        <span class="time-separator">De</span>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtHoraInicio" runat="server" CssClass="form-control form-control-custom" TextMode="Time" />
                    </div>
                    <div class="col-auto">
                        <span class="time-separator">a</span>
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtHoraFin" runat="server" CssClass="form-control form-control-custom" TextMode="Time" />
                    </div>
                </div>

                <%-- Fila 3: Motivo (Textarea) --%>
                <div class="mb-5">
                    <asp:Label ID="lblMotivo" runat="server" Text="Motivo de la excepción:" CssClass="form-label-lg mb-2" />
                    <asp:TextBox ID="txtMotivo" runat="server" 
                        CssClass="form-control form-control-custom-area" 
                        TextMode="MultiLine" 
                        Rows="5" 
                        placeholder="Explica el motivo de tu excepción..." />
                </div>

                <%-- Fila 4: Botón de Registrar (Depth 4, Frame 9) --%>
                <%-- 
                  Usamos d-grid y mx-auto para centrar el botón y 
                  darle el ancho (480px) del diseño (col-lg-7 en un 960px container)
                --%>
                <div class="d-grid col-12 col-md-10 col-lg-7 mx-auto">
                    <asp:Button ID="btnRegistrarExcepcion" runat="server" 
                        Text="Registrar excepción" 
                        CssClass="btn-custom-teal-lg" 
                        OnClick="btnRegistrarExcepcion_Click"/>
                </div>

            </div>
            </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
