<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AgregarEmpleado.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.AgregarEmpleado" %>
<%--<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>--%>


<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <!-- Título de la pestaña (va en <title> dentro de la master) -->
    Añadir empleado -
</asp:Content>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
<!-- Extra en <head> si quieres estilos específicos -->
    <style>
        .time-input::placeholder { color: #999; }
        .section-title {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 24px;
            margin-bottom: 20px;
        }
    </style>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
<!-- CONTENIDO PRINCIPAL, aparece dentro de <main class="main-content ..."> -->

    <!-- Título de la pantalla -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h1 class="section-title mb-0">Añadir empleado</h1>
    </div>

    <!-- Resumen de validaciones -->
    <asp:ValidationSummary ID="vsErrores" runat="server"
        CssClass="alert alert-danger"
        HeaderText="Por favor corrige los siguientes errores:"
        EnableClientScript="true"
        DisplayMode="BulletList" />

    <!-- DATOS DEL EMPLEADO -->
    <div class="card mb-4 shadow-sm border-0">
        <div class="card-header bg-white border-0">
            <strong>Datos del empleado</strong>
        </div>
        <div class="card-body">

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtNombreCompleto" class="form-label">Nombre completo</label>
                    <asp:TextBox ID="txtNombreCompleto" runat="server"
                        CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                        ControlToValidate="txtNombreCompleto"
                        ErrorMessage="El nombre completo es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label for="txtCorreo" class="form-label">Correo electrónico</label>
                    <asp:TextBox ID="txtCorreo" runat="server"
                        CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                        ControlToValidate="txtCorreo"
                        ErrorMessage="El correo electrónico es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-4">
                    <label for="txtTelefono" class="form-label">Número de teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server"
                        CssClass="form-control" />
                </div>

                <div class="col-md-4">
                    <label for="txtContrasenha" class="form-label">Contraseña inicial</label>
                    <asp:TextBox ID="txtContrasenha" runat="server"
                        CssClass="form-control" TextMode="Password" />
                    <asp:RequiredFieldValidator ID="rfvContrasenha" runat="server"
                        ControlToValidate="txtContrasenha"
                        ErrorMessage="La contraseña inicial es obligatoria."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="col-md-4">
                    <label for="txtConfirmarContrasenha" class="form-label">Confirmar contraseña</label>
                    <asp:TextBox ID="txtConfirmarContrasenha" runat="server"
                        CssClass="form-control" TextMode="Password" />
                    <asp:CompareValidator ID="cvContrasenha" runat="server"
                        ControlToCompare="txtContrasenha"
                        ControlToValidate="txtConfirmarContrasenha"
                        ErrorMessage="Las contraseñas no coinciden."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>
            </div>

        </div>
    </div>

        <!-- HORARIO DE TRABAJO -->
    <div class="card mb-4 shadow-sm border-0">
        <div class="card-header bg-white border-0">
            <strong>Horario de trabajo (lunes a sábado)</strong>
        </div>
        <div class="card-body">
            <p class="text-muted mb-3">
                Haz clic en cada día para definir uno o varios rangos de horario.
            </p>

            <div class="table-responsive">
                <table class="table align-middle">
                    <thead>
                        <tr>
                            <th style="width: 20%;">Día</th>
                            <th style="width: 60%;">Horarios definidos</th>
                            <th style="width: 20%;">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        <!-- Lunes -->
                        <tr>
                            <td>Lunes</td>
                            <td>
                                <span id="spHorariosLunes" class="text-muted small">Sin horarios</span>
                                <asp:HiddenField ID="hfHorariosLunes" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <button type="button" class="btn btn-sm btn-outline-primary"
                                        onclick="abrirModalDia('Lunes')">
                                    Configurar
                                </button>
                            </td>
                        </tr>

                        <!-- Martes -->
                        <tr>
                            <td>Martes</td>
                            <td>
                                <span id="spHorariosMartes" class="text-muted small">Sin horarios</span>
                                <asp:HiddenField ID="hfHorariosMartes" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <button type="button" class="btn btn-sm btn-outline-primary"
                                        onclick="abrirModalDia('Martes')">
                                    Configurar
                                </button>
                            </td>
                        </tr>

                        <!-- Miércoles -->
                        <tr>
                            <td>Miércoles</td>
                            <td>
                                <span id="spHorariosMiercoles" class="text-muted small">Sin horarios</span>
                                <asp:HiddenField ID="hfHorariosMiercoles" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <button type="button" class="btn btn-sm btn-outline-primary"
                                        onclick="abrirModalDia('Miercoles')">
                                    Configurar
                                </button>
                            </td>
                        </tr>

                        <!-- Jueves -->
                        <tr>
                            <td>Jueves</td>
                            <td>
                                <span id="spHorariosJueves" class="text-muted small">Sin horarios</span>
                                <asp:HiddenField ID="hfHorariosJueves" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <button type="button" class="btn btn-sm btn-outline-primary"
                                        onclick="abrirModalDia('Jueves')">
                                    Configurar
                                </button>
                            </td>
                        </tr>

                        <!-- Viernes -->
                        <tr>
                            <td>Viernes</td>
                            <td>
                                <span id="spHorariosViernes" class="text-muted small">Sin horarios</span>
                                <asp:HiddenField ID="hfHorariosViernes" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <button type="button" class="btn btn-sm btn-outline-primary"
                                        onclick="abrirModalDia('Viernes')">
                                    Configurar
                                </button>
                            </td>
                        </tr>

                        <!-- Sábado -->
                        <tr>
                            <td>Sábado</td>
                            <td>
                                <span id="spHorariosSabado" class="text-muted small">Sin horarios</span>
                                <asp:HiddenField ID="hfHorariosSabado" runat="server" ClientIDMode="Static" />
                            </td>
                            <td>
                                <button type="button" class="btn btn-sm btn-outline-primary"
                                        onclick="abrirModalDia('Sabado')">
                                    Configurar
                                </button>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

        </div>
    </div>

    <!-- MODAL PARA CONFIGURAR HORARIOS DE UN DÍA -->
    <div class="modal fade" id="modalHorarioDia" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">
                        Horarios de trabajo - <span id="modalDiaNombre"></span>
                    </h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                </div>
                <div class="modal-body">

                    <div class="mb-3">
                        <label class="form-label">Agregar nuevo rango</label>
                        <div class="row g-2">
                            <div class="col-5">
                                <input type="time" id="txtModalInicio" class="form-control" />
                            </div>
                            <div class="col-5">
                                <input type="time" id="txtModalFin" class="form-control" />
                            </div>
                            <div class="col-2 d-grid">
                                <button type="button" class="btn btn-primary"
                                        onclick="agregarHorarioModal()">
                                    +
                                </button>
                            </div>
                        </div>
                        <small class="text-muted">Formato 24h. Ejemplo: 08:00 a 11:00.</small>
                    </div>

                    <hr />

                    <div>
                        <label class="form-label">Horarios actuales</label>
                        <ul id="listaHorariosDia" class="list-group small">
                            <!-- Se rellena por JS -->
                        </ul>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button"
                            class="btn btn-primary"
                            data-bs-dismiss="modal">
                        Aceptar
                    </button>
                </div>
            </div>
        </div>
    </div>


    <!-- SERVICIOS COSMETOLÓGICOS -->
    <div class="card mb-4 shadow-sm border-0">
        <div class="card-header bg-white border-0">
            <strong>Servicios cosmetológicos que realiza</strong>
        </div>
        <div class="card-body">
            <p class="text-muted mb-3">
                Selecciona los servicios que este empleado puede realizar.
            </p>

            <asp:CheckBoxList ID="cblServicios" runat="server"
                CssClass="form-check"
                RepeatDirection="Vertical"
                RepeatLayout="Flow">
            </asp:CheckBoxList>
        </div>
    </div>

    <!-- BOTONES -->
    <div class="d-flex justify-content-end mb-5">
        <asp:Button ID="btnCancelar" runat="server"
            CssClass="btn btn-outline-secondary me-2"
            Text="Cancelar"
            CausesValidation="false"
            PostBackUrl="~/Admin/PanelDeControl.aspx" />
        <asp:Button ID="btnGuardar" runat="server"
            CssClass="btn btn-primary"
            Text="Guardar"
            OnClick="btnGuardar_Click" />
    </div>

</asp:Content>

<asp:Content ID="ContentScripts" ContentPlaceHolderID="ScriptsContent" runat="server">

    <script type="text/javascript">
        let diaActual = null;

        function abrirModalDia(diaKey) {
            diaActual = diaKey;
            document.getElementById('modalDiaNombre').innerText = diaKey;
            document.getElementById('txtModalInicio').value = '';
            document.getElementById('txtModalFin').value = '';
            renderHorariosDia();

            var modalElement = document.getElementById('modalHorarioDia');
            var modal = new bootstrap.Modal(modalElement);
            modal.show();
        }

        function getHiddenFieldId() {
            return 'hfHorarios' + diaActual;
        }

        function getSpanId() {
            return 'spHorarios' + diaActual;
        }

        function renderHorariosDia() {
            const hf = document.getElementById(getHiddenFieldId());
            const lista = document.getElementById('listaHorariosDia');
            lista.innerHTML = '';

            const value = (hf.value || '').trim();
            if (!value) return;

            const segmentos = value.split(';').map(s => s.trim()).filter(s => s !== '');
            segmentos.forEach(seg => {
                const li = document.createElement('li');
                li.className = 'list-group-item d-flex justify-content-between align-items-center';
                li.textContent = seg;

                const btn = document.createElement('button');
                btn.type = 'button';
                btn.className = 'btn btn-sm btn-outline-danger';
                btn.textContent = 'Quitar';
                btn.onclick = function () { eliminarHorario(seg); };

                li.appendChild(btn);
                lista.appendChild(li);
            });
        }

        function agregarHorarioModal() {
            const ini = document.getElementById('txtModalInicio').value;
            const fin = document.getElementById('txtModalFin').value;

            if (!ini || !fin) {
                alert('Ingresa hora de inicio y fin.');
                return;
            }
            if (fin <= ini) {
                alert('La hora fin debe ser mayor que la hora inicio.');
                return;
            }

            const hf = document.getElementById(getHiddenFieldId());
            let partes = (hf.value || '').split(';').map(s => s.trim()).filter(s => s !== '');

            const nuevo = ini + '-' + fin;
            partes.push(nuevo);

            hf.value = partes.join(';');

            document.getElementById('txtModalInicio').value = '';
            document.getElementById('txtModalFin').value = '';

            renderHorariosDia();
            actualizarResumenDia();
        }

        function eliminarHorario(seg) {
            const hf = document.getElementById(getHiddenFieldId());
            let partes = (hf.value || '').split(';').map(s => s.trim()).filter(s => s !== '' && s !== seg);
            hf.value = partes.join(';');

            renderHorariosDia();
            actualizarResumenDia();
        }

        function actualizarResumenDia() {
            const hf = document.getElementById(getHiddenFieldId());
            const span = document.getElementById(getSpanId());
            const value = (hf.value || '').trim();

            if (!value) {
                span.textContent = 'Sin horarios';
            } else {
                span.textContent = value.replace(/;/g, ', ');
            }
        }
    </script>



</asp:Content>

<asp:Content ID="ContentExtra" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!-- ContentPlaceHolder1 (no lo uso, lo dejo vacío a menos que tú lo necesites) -->

</asp:Content>

