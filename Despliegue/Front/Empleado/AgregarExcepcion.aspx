<%@ Page Title="Agregar Excepción" Language="C#" MasterPageFile="~/Empleado/Empleado.Master" AutoEventWireup="true" CodeBehind="AgregarExcepcion.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.AgregarExcepcion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Agregar Excepción
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

        /* Etiquetas del formulario */
        .form-label-lg {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 700;
            font-size: 24px;
            color: #1C0D12;
            line-height: 1;
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
            width: 100%;
            transition: background-color 0.3s ease;
        }

        .btn-custom-teal-lg:hover {
            background-color: #19a599;
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
            padding: 0.375rem 1.2rem;
        }
        
        .form-control-custom-area {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            color: #757575;
            border: 2px solid #F1E9EC;
            border-radius: 12px;
        }

        select.form-control-custom {
            appearance: none;
            background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23343a40' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M2 5l6 6 6-6'/%3e%3c/svg%3e");
            background-repeat: no-repeat;
            background-position: right 1.2rem center;
            background-size: 16px 12px;
        }
        
        /* ✅ NUEVO CSS: Estilo para las opciones deshabilitadas */
        .option-disabled {
            color: #d9534f !important; /* Rojo para indicar conflicto */
            background-color: #f8d7da; /* Fondo suave */
            font-style: italic;
        }
    </style>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-lg my-5">
        
        <div class="bg-white p-4 p-md-5 rounded-3 shadow-sm">

            <div class="text-center mb-5">
                <h1 class="exception-title mb-3">
                    Agregar excepción en mi horario
                </h1>
                <p class="exception-subtitle">
                    Se aceptan excepciones de horario por motivos de salud y personales.<br />
                    <small class="text-muted fs-6">(Seleccione un día disponible de su calendario)</small>
                </p>
            </div>

            <div class="px-md-5">

                <%-- Fila 1: Fecha (DropDownList) --%>
                <div class="row g-3 align-items-center mb-4">
                    <div class="col-md-3">
                        <asp:Label ID="lblFecha" runat="server" Text="Fecha:" CssClass="form-label-lg m-0" AssociatedControlID="ddlFecha" />
                    </div>
                    <div class="col-md-9">
                        <asp:DropDownList ID="ddlFecha" runat="server" CssClass="form-control form-control-custom" />
                    </div>
                </div>

                <%-- Fila 2: Motivo --%>
                <div class="mb-5">
                    <asp:Label ID="lblMotivo" runat="server" Text="Motivo de la excepción:" CssClass="form-label-lg mb-2 d-block" AssociatedControlID="txtMotivo" />
                    <asp:TextBox ID="txtMotivo" runat="server" 
                        CssClass="form-control form-control-custom-area" 
                        TextMode="MultiLine" 
                        Rows="5" 
                        placeholder="Explica el motivo de tu excepción..." />
                </div>

                <%-- Mensaje de Error --%>
                <asp:Label ID="lblError" runat="server" CssClass="text-danger fw-bold d-block text-center mb-3" Visible="false"></asp:Label>

                <%-- Fila 3: Botón --%>
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
    <script type="text/javascript">
        document.addEventListener('DOMContentLoaded', function () {
            var ddl = document.getElementById('<%= ddlFecha.ClientID %>');
            if (ddl) {
                // Iteramos sobre las opciones del DropDownList
                for (var i = 0; i < ddl.options.length; i++) {
                    var option = ddl.options[i];
                    
                    // Verificamos si la opción tiene la clase 'option-disabled'
                    // Esta clase se añade en el CodeBehind (C#)
                    if (option.classList.contains('option-disabled')) {
                        // Deshabilitamos la opción para que no pueda ser seleccionada
                        option.disabled = true;
                        
                        // Si la opción deshabilitada está seleccionada al cargar (raro, pero posible)
                        // forzamos la selección del primer ítem (el placeholder)
                        if (option.selected) {
                            ddl.selectedIndex = 0;
                        }
                    }
                }
            }
        });
    </script>
</asp:Content>