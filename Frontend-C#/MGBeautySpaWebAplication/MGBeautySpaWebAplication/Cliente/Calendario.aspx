<%@ Page Title="Calendario" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Calendario" EnableEventValidation="false"%>

<%-- 1. Contenido del Head (SOLO CSS y Fuentes) --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="Calendario.css">
</asp:Content>


<%-- 2. Contenido de Scripts (SOLO LAS FUNCIONES) --%>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsContent" runat="server">
    <%-- El JS de Bootstrap se queda aquí 
    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>
    --%>
    <script type="text/javascript">
        // Las variables se moverán al MainContent
        // Solo dejamos las funciones aquí

        function handleDayClick(buttonElement, dateString) {
            // IDs de C# que JavaScript necesita leer
            var ddlHorariosClientID = document.getElementById('ddlHorariosClientID_Value').value;
            var hdnSelectedDayClientID = document.getElementById('hdnSelectedDayClientID_Value').value;
            var hdnEmpleadoIdClientID = document.getElementById('hdnEmpleadoIdClientID_Value').value;
            var hdnDuracionServicioClientID = document.getElementById('hdnDuracionServicioClientID_Value').value;

            // 1. Actualizar el HiddenField
            document.getElementById(hdnSelectedDayClientID).value = dateString;

            // 2. Resaltar visualmente el día (CSS)
            var grid = document.querySelector('.calendar-grid');
            var selected = grid.querySelector('.calendar-day--selected');
            if (selected) {
                selected.classList.remove('calendar-day--selected');
            }
            buttonElement.classList.add('calendar-day--selected');

            // 3. Limpiar y deshabilitar el DropDownList mientras se carga
            var ddlHorarios = document.getElementById(ddlHorariosClientID);
            ddlHorarios.innerHTML = "";
            ddlHorarios.options.add(new Option("Cargando...", ""));
            ddlHorarios.disabled = true;

            // 4. Leer los valores desde los HiddenFields
            var empleadoId = document.getElementById(hdnEmpleadoIdClientID).value;
            var duracionServicio = document.getElementById(hdnDuracionServicioClientID).value;

            // 5. Llamar al WebMethod (AJAX)
            PageMethods.GetAvailableHours(dateString, empleadoId, duracionServicio, OnHoursSuccess, OnHoursError);
        }

        function OnHoursSuccess(result) {
            var ddlHorariosClientID = document.getElementById('ddlHorariosClientID_Value').value;
            var ddlHorarios = document.getElementById(ddlHorariosClientID);
            ddlHorarios.innerHTML = "";

            if (result && result.length > 0) {
                ddlHorarios.options.add(new Option("Seleccione un horario", ""));
                result.forEach(function (hora) {
                    ddlHorarios.options.add(new Option(hora, hora));
                });
                ddlHorarios.disabled = false;
            } else {
                ddlHorarios.options.add(new Option("No hay horas disponibles", ""));
                ddlHorarios.disabled = true;
            }
        }

        function OnHoursError(error) {
            var ddlHorariosClientID = document.getElementById('ddlHorariosClientID_Value').value;
            var ddlHorarios = document.getElementById(ddlHorariosClientID);
            ddlHorarios.innerHTML = "";
            ddlHorarios.options.add(new Option("Error al cargar horas", ""));
            ddlHorarios.disabled = true;
            console.error(error.get_message());
        }

        // Esta función SÍ puede estar aquí
        function updateSelectedHour() {
            var ddlHorariosClientID = document.getElementById('ddlHorariosClientID_Value').value;
            var hdnSelectedHourClientID = document.getElementById('hdnSelectedHourClientID_Value').value;

            var ddlHorarios = document.getElementById(ddlHorariosClientID);
            var hdnHour = document.getElementById(hdnSelectedHourClientID);
            if (hdnHour) {
                hdnHour.value = ddlHorarios.value;
            }
        }

</script>
</asp:Content>


<%-- 3. Contenido Principal (HTML + Variables JS) --%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    
    <%-- ▼▼▼ CORRECCIÓN: Ponemos las variables de JS aquí ▼▼▼ --%>
    <%-- Usamos HiddenFields para pasar los ClientID a JS de forma segura --%>
    <input type="hidden" id="ddlHorariosClientID_Value" value="<%= ddlHorarios.ClientID %>" />
    <input type="hidden" id="hdnSelectedDayClientID_Value" value="<%= hdnSelectedDay.ClientID %>" />
    <input type="hidden" id="hdnEmpleadoIdClientID_Value" value="<%= hdnEmpleadoId.ClientID %>" />
    <input type="hidden" id="hdnDuracionServicioClientID_Value" value="<%= hdnDuracionServicio.ClientID %>" />
    <input type="hidden" id="hdnSelectedHourClientID_Value" value="<%= hdnSelectedHour.ClientID %>" />
    
    <asp:UpdatePanel ID="upCalendario" runat="server">
        <ContentTemplate>
        
            <%-- ▼▼▼ CAMPOS OCULTOS AÑADIDOS ▼▼▼ --%>
            <asp:HiddenField ID="hdnEmpleadoId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnServicioId" runat="server" Value="0" />
            <asp:HiddenField ID="hdnDuracionServicio" runat="server" Value="0" />
            <asp:HiddenField ID="hdnSelectedDay" runat="server" Value="" />
            <asp:HiddenField ID="hdnSelectedHour" runat="server" Value="" />
            
            <div class="page-container">
                <nav class="breadcrumbs">
                    <a href="Servicios.aspx">Servicios</a>
                    <span>/</span>
                    <a href="SeleccionarEmpleado.aspx">Seleccionar Empleado</a>
                    <span>/</span>
                    <strong>Calendario</strong>
                </nav>

                <h1 class="main-title">Escoge la fecha y hora de tu reserva</h1>

                <main class="content-body">

                    <section class="calendar-column">
                        <div class="calendar-widget">
                            
                            <header class="calendar-header">
                                <asp:LinkButton ID="btnPrevMonth" runat="server" OnClick="btnPrevMonth_Click" CssClass="nav-arrow" Aria-Label="Mes anterior">&lt;</asp:LinkButton>
                                <h2><asp:Literal ID="litMonthYear" runat="server" /></h2>
                                <asp:LinkButton ID="btnNextMonth" runat="server" OnClick="btnNextMonth_Click" CssClass="nav-arrow" Aria-Label="Mes siguiente">&gt;</asp:LinkButton>
                            </header>
                            
                            <div class="calendar-weekdays">
                                <div class="weekday-cell">D</div>
                                <div class="weekday-cell">L</div>
                                <div class="weekday-cell">M</div>
                                <div class="weekday-cell">M</div>
                                <div class="weekday-cell">J</div>
                                <div class="weekday-cell">V</div>
                                <div class="weekday-cell">S</div>
                            </div>
                            
                            <%-- Quitado OnItemCommand --%>
                            <asp:Repeater ID="rpCalendarDays" runat="server" 
                                OnItemDataBound="rpCalendarDays_ItemDataBound">
                                <HeaderTemplate>
                                    <div class="calendar-grid">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnDay" runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </section>

                    <section class="time-column">
                        <h2 class="time-title">Horarios Disponibles</h2>
                        
                        <div class="time-select-wrapper">
                            <asp:DropDownList ID="ddlHorarios" runat="server" 
                                CssClass="time-select-dropdown" 
                                Enabled="false" 
                                onchange="updateSelectedHour();"/>
                        </div>
                        
                        <div class="action-buttons">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" 
                                CssClass="btn-action btn-cancel" 
                                OnClick="btnCancelar_Click" 
                                CausesValidation="false" />
                            <asp:Button ID="btnReservar" runat="server" Text="Reservar Cita" 
                                CssClass="btn-action btn-submit" 
                                OnClick="btnReservar_Click" />
                        </div>
                    </section>

                </main>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>