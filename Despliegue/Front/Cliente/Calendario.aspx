<%@ Page Title="Calendario" Async="true" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="Calendario.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Calendario" EnableEventValidation="false"%>

<%-- 1. Contenido del Head --%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700&family=ZCOOL+XiaoWei&display=swap" rel="stylesheet">
    <link rel="stylesheet" href="Calendario.css">
    
    <%-- ESTILOS PARA EL MODAL DE PAGO (Copiados/Adaptados de Carrito) --%>
    <style>
        /* Estilos base del modal */
        .payment-modal-content { border-radius: 16px; border: none; box-shadow: 0 10px 30px rgba(0,0,0,0.1); }
        .payment-modal-header { padding: 24px 24px 0; text-align: center; border-bottom: none; }
        .payment-modal-title { font-family: 'ZCOOL XiaoWei', serif; font-size: 1.5rem; color: #148C76; margin-bottom: 8px; }
        .payment-modal-subtitle { font-size: 0.9rem; color: #666; margin: 0; }
        .payment-modal-body { padding: 24px; }
        
        /* Inputs */
        .input-block { margin-bottom: 16px; }
        .input-label { display: block; font-weight: 600; font-size: 0.85rem; margin-bottom: 6px; color: #333; }
        .input-field { width: 100%; padding: 10px 14px; border: 1px solid #ddd; border-radius: 8px; font-size: 0.95rem; transition: border-color 0.2s; outline: none; }
        .input-field:focus { border-color: #148C76; box-shadow: 0 0 0 3px rgba(20, 140, 118, 0.1); }
        .row-2-inputs { display: flex; gap: 16px; }
        .input-half { flex: 1; }

        /* Footer y Botones */
        .payment-modal-footer { padding: 0 24px 24px; display: flex; gap: 12px; justify-content: flex-end; }
        .btn-cancel-payment { background: #f5f5f5; border: none; padding: 10px 20px; border-radius: 8px; color: #666; font-weight: 600; transition: 0.2s; }
        .btn-cancel-payment:hover { background: #e0e0e0; }
        .btn-confirm-payment { background: #148C76; border: none; padding: 10px 24px; border-radius: 8px; color: white; font-weight: 600; transition: 0.2s; }
        .btn-confirm-payment:hover { background: #0e6b5a; transform: translateY(-1px); }

        /* Modal Éxito */
        .success-modal-content { text-align: center; border-radius: 16px; border: none; }
        .success-modal-body { padding: 40px 24px; }
        .success-icon-wrapper { color: #148C76; margin-bottom: 16px; }
        .success-title { font-family: 'ZCOOL XiaoWei', serif; font-size: 1.8rem; color: #148C76; margin-bottom: 12px; }
        .success-message { color: #666; margin-bottom: 24px; line-height: 1.5; }
        .btn-success-action { background: #148C76; border: none; padding: 12px 32px; border-radius: 8px; color: white; font-weight: 600; width: 100%; max-width: 200px; }
    </style>
</asp:Content>

<%-- 2. Scripts (Funciones JS + Variables) --%>
<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        // --- Lógica del Calendario ---
        function handleDayClick(buttonElement, dateString) {
            var ddlHorariosClientID = document.getElementById('ddlHorariosClientID_Value').value;
            var hdnSelectedDayClientID = document.getElementById('hdnSelectedDayClientID_Value').value;
            var hdnEmpleadoIdClientID = document.getElementById('hdnEmpleadoIdClientID_Value').value;
            var hdnDuracionServicioClientID = document.getElementById('hdnDuracionServicioClientID_Value').value;

            document.getElementById(hdnSelectedDayClientID).value = dateString;

            var grid = document.querySelector('.calendar-grid');
            var selected = grid.querySelector('.calendar-day--selected');
            if (selected) selected.classList.remove('calendar-day--selected');
            buttonElement.classList.add('calendar-day--selected');

            var ddlHorarios = document.getElementById(ddlHorariosClientID);
            ddlHorarios.innerHTML = "";
            ddlHorarios.options.add(new Option("Cargando...", ""));
            ddlHorarios.disabled = true;

            var empleadoId = document.getElementById(hdnEmpleadoIdClientID).value;
            var duracionServicio = document.getElementById(hdnDuracionServicioClientID).value;

            PageMethods.GetAvailableHours(dateString, empleadoId, duracionServicio, OnHoursSuccess, OnHoursError);
        }

        function OnHoursSuccess(result) {
            var ddlHorariosClientID = document.getElementById('ddlHorariosClientID_Value').value;
            var hdnDuracionServicioClientID = document.getElementById('hdnDuracionServicioClientID_Value').value;

            var ddlHorarios = document.getElementById(ddlHorariosClientID);
            var duracionMinutos = parseInt(document.getElementById(hdnDuracionServicioClientID).value) || 0;

            ddlHorarios.innerHTML = "";

            if (result && result.length > 0) {
                ddlHorarios.options.add(new Option("Seleccione un horario", ""));

                result.forEach(function (horaInicio) {
                    var intervaloTexto = sumarMinutos(horaInicio, duracionMinutos*60);
                    ddlHorarios.options.add(new Option(intervaloTexto, horaInicio));
                });

                ddlHorarios.disabled = false;
            } else {
                ddlHorarios.options.add(new Option("No hay horas disponibles", ""));
                ddlHorarios.disabled = true;
            }
        }
        function sumarMinutos(horaStr, minutosASumar) {
            if (!horaStr) return "";

            var partes = horaStr.split(':');
            var hora = parseInt(partes[0]);
            var min = parseInt(partes[1]);

            var fecha = new Date();
            fecha.setHours(hora);
            fecha.setMinutes(min);
            fecha.setSeconds(0);

            var inicioFmt = formatearHora(fecha);

            fecha.setMinutes(fecha.getMinutes() + minutosASumar);

            var finFmt = formatearHora(fecha);

            return inicioFmt + " - " + finFmt;
        }

        function formatearHora(dateObj) {
            var hh = dateObj.getHours().toString().padStart(2, '0');
            var mm = dateObj.getMinutes().toString().padStart(2, '0');
            return hh + ':' + mm;
        }

        function OnHoursError(error) {
            console.error(error.get_message());
            var ddlHorariosClientID = document.getElementById('ddlHorariosClientID_Value').value;
            document.getElementById(ddlHorariosClientID).innerHTML = "<option>Error al cargar</option>";
        }

        function updateSelectedHour() {
            var ddlHorarios = document.getElementById(document.getElementById('ddlHorariosClientID_Value').value);
            var hdnHour = document.getElementById(document.getElementById('hdnSelectedHourClientID_Value').value);
            if (hdnHour) hdnHour.value = ddlHorarios.value;
        }

        function validatePaymentForm() {
            var cardNumber = document.getElementById('<%= txtCardNumber.ClientID %>').value.replace(/\s/g, '');
            var cardName = document.getElementById('<%= txtNameOnCard.ClientID %>').value.trim();
            var expiryDate = document.getElementById('<%= txtExpiryDate.ClientID %>').value.trim();
            var cvv = document.getElementById('<%= txtCVV.ClientID %>').value.trim();

            if (!cardNumber || !cardName || !expiryDate || !cvv) {
                showError('Todos los campos son obligatorios'); return false;
            }
            if (cardNumber.length !== 16 || !/^\d+$/.test(cardNumber)) {
                showError('El número de tarjeta debe tener 16 dígitos'); return false;
            }
            if (!/^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$/.test(cardName)) {
                showError('Nombre inválido'); return false;
            }
            if (cvv.length !== 3 || !/^\d+$/.test(cvv)) {
                showError('CVV inválido'); return false;
            }
            if (!/^\d{2}\/\d{2}$/.test(expiryDate)) {
                showError('Formato MM/AA requerido'); return false;
            }

            hideError();
            return true;
        }

        function showError(message) {
            var errorDiv = document.getElementById('paymentErrorMessage');
            document.getElementById('errorText').textContent = message;
            errorDiv.style.display = 'block';
        }

        function hideError() {
            document.getElementById('paymentErrorMessage').style.display = 'none';
        }

        function formatCardNumber(input) {
            var value = input.value.replace(/\s/g, '');
            input.value = value.match(/.{1,4}/g)?.join(' ') || value;
        }

        function formatExpiryDate(input) {
            var value = input.value.replace(/\D/g, '');
            if (value.length >= 2) input.value = value.substring(0, 2) + '/' + value.substring(2, 4);
            else input.value = value;
        }

        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            return !(charCode > 31 && (charCode < 48 || charCode > 57));
        }

        function isLetterKey(evt) {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            return ((charCode >= 65 && charCode <= 90) || (charCode >= 97 && charCode <= 122) || charCode === 32 || charCode > 192);
        }
    </script>
</asp:Content>

<%-- 3. Contenido Principal --%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
    
    <%-- Variables JS --%>
    <input type="hidden" id="ddlHorariosClientID_Value" value="<%= ddlHorarios.ClientID %>" />
    <input type="hidden" id="hdnSelectedDayClientID_Value" value="<%= hdnSelectedDay.ClientID %>" />
    <input type="hidden" id="hdnEmpleadoIdClientID_Value" value="<%= hdnEmpleadoId.ClientID %>" />
    <input type="hidden" id="hdnDuracionServicioClientID_Value" value="<%= hdnDuracionServicio.ClientID %>" />
    <input type="hidden" id="hdnSelectedHourClientID_Value" value="<%= hdnSelectedHour.ClientID %>" />
    
    <asp:UpdatePanel ID="upCalendario" runat="server">
        <ContentTemplate>
            <%-- Campos Ocultos --%>
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
                                <div class="weekday-cell">D</div><div class="weekday-cell">L</div><div class="weekday-cell">M</div>
                                <div class="weekday-cell">M</div><div class="weekday-cell">J</div><div class="weekday-cell">V</div>
                                <div class="weekday-cell">S</div>
                            </div>
                            
                            <asp:Repeater ID="rpCalendarDays" runat="server" OnItemDataBound="rpCalendarDays_ItemDataBound">
                                <HeaderTemplate><div class="calendar-grid"></HeaderTemplate>
                                <ItemTemplate><asp:LinkButton ID="btnDay" runat="server" /></ItemTemplate>
                                <FooterTemplate></div></FooterTemplate>
                            </asp:Repeater>
                        </div>
                    </section>

                    <section class="time-column">
                        <h2 class="time-title">Horarios Disponibles</h2>
                        <div class="time-select-wrapper">
                            <asp:DropDownList ID="ddlHorarios" runat="server" CssClass="time-select-dropdown" Enabled="false" onchange="updateSelectedHour();"/>
                        </div>
                        <div class="action-buttons">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn-action btn-cancel" OnClick="btnCancelar_Click" CausesValidation="false" />
                            <%-- NOTA: El OnClick ahora solo valida y abre el modal --%>
                            <asp:Button ID="btnReservar" runat="server" Text="Reservar Cita" CssClass="btn-action btn-submit" OnClick="btnReservar_Click" />
                        </div>
                    </section>
                </main>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%-- =====================================================
         MODAL DE PAGO - IGUAL QUE EN CARRITO
         ===================================================== --%>
    <div class="modal fade" id="paymentModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content payment-modal-content">
                <div class="payment-modal-header">
                    <h5 class="payment-modal-title">Confirmar Reserva</h5>
                    <p class="payment-modal-subtitle">Ingresa los datos de tu tarjeta para finalizar</p>
                </div>
                <div class="payment-modal-body">
                    <div id="paymentErrorMessage" class="alert alert-danger" style="display: none; margin-bottom: 15px; padding: 10px; border-radius: 8px; font-size: 0.9rem;">
                        <strong>Error:</strong> <span id="errorText"></span>
                    </div>

                    <div class="input-block">
                        <label class="input-label">Número de tarjeta <span style="color: #C31E1E;">*</span></label>
                        <asp:TextBox ID="txtCardNumber" runat="server" CssClass="input-field" placeholder="1234 5678 9012 3456" MaxLength="19" onkeyup="formatCardNumber(this)" onkeypress="return isNumberKey(event)" />
                    </div>
                    <div class="input-block">
                        <label class="input-label">Titular de la tarjeta <span style="color: #C31E1E;">*</span></label>
                        <asp:TextBox ID="txtNameOnCard" runat="server" CssClass="input-field" placeholder="Nombre completo" onkeypress="return isLetterKey(event)" />
                    </div>
                    <div class="row-2-inputs">
                        <div class="input-block input-half">
                            <label class="input-label">Vencimiento <span style="color: #C31E1E;">*</span></label>
                            <asp:TextBox ID="txtExpiryDate" runat="server" CssClass="input-field" placeholder="MM/AA" MaxLength="5" onkeyup="formatExpiryDate(this)" onkeypress="return isNumberKey(event)" />
                        </div>
                        <div class="input-block input-half">
                            <label class="input-label">CVV <span style="color: #C31E1E;">*</span></label>
                            <asp:TextBox ID="txtCVV" runat="server" CssClass="input-field" TextMode="Password" MaxLength="3" placeholder="123" onkeypress="return isNumberKey(event)" />
                        </div>
                    </div>
                </div>
                <div class="payment-modal-footer">
                    <button type="button" class="btn-cancel-payment" data-bs-dismiss="modal">Cancelar</button>
                    <%-- Este botón ejecuta el pago REAL --%>
                    <asp:Button ID="btnProcessPayment" runat="server" Text="Pagar y Reservar" CssClass="btn-confirm-payment" OnClientClick="return validatePaymentForm();" OnClick="btnProcessPayment_Click" />
                </div>
            </div>
        </div>
    </div>

    <%-- =====================================================
         MODAL DE ÉXITO
         ===================================================== --%>
    <div class="modal fade" id="paymentSuccessModal" tabindex="-1" aria-hidden="true" data-bs-backdrop="static" data-bs-keyboard="false">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content success-modal-content">
                <div class="modal-body success-modal-body">
                    <div class="success-icon-wrapper">
                        <svg xmlns="http://www.w3.org/2000/svg" width="64" height="64" fill="currentColor" viewBox="0 0 16 16">
                            <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/>
                        </svg>
                    </div>
                    <h3 class="success-title">¡Reserva Exitosa!</h3>
                    <p class="success-message">Tu cita ha sido reservada y pagada correctamente. Te hemos enviado un correo con los datos de la reserva</p>
                    <asp:Button ID="btnVolverInicio" runat="server" Text="Ir a Mis Reservas" CssClass="btn-success-action" OnClick="btnVolverInicio_Click" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>