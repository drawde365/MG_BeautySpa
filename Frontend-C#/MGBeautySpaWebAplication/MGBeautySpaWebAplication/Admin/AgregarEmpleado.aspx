<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.Master" AutoEventWireup="true" CodeBehind="AgregarEmpleado.aspx.cs" Inherits="MGBeautySpaWebAplication.Admin.AgregarEmpleado" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Añadir empleado
</asp:Content>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .section-title {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-weight: 500;
            font-size: 24px;
            margin-bottom: 20px;
        }

        /* --- Servicios cosmetológicos --- */

        .servicios-card-header {
            font-weight: 600;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }

        .servicios-helper-text {
            font-size: 0.9rem;
            color: #6c757d;
            margin-bottom: 1rem;
        }

        .servicios-list {
            display: flex;
            flex-wrap: wrap;
            gap: 0.75rem;
            padding: 0;
            margin: 0;
            list-style: none;
        }

        /* Ocultamos el checkbox nativo */
        .servicios-list input[type="checkbox"] {
            display: none;
        }

        /* “Chips” de servicio */
        .servicios-list label {
            display: inline-flex;
            align-items: center;
            padding: 0.55rem 0.95rem;
            border-radius: 999px;
            border: 1px solid #d0d5dd;
            background-color: #ffffff;
            font-size: 0.92rem;
            font-family: 'Plus Jakarta Sans', sans-serif;
            color: #344054;
            cursor: pointer;
            box-shadow: 0 1px 2px rgba(16, 24, 40, 0.05);
            transition: all 0.15s ease-in-out;
            max-width: 100%;
        }

        .servicios-list label:hover {
            border-color: #12a594;
            box-shadow: 0 4px 10px rgba(18, 165, 148, 0.15);
        }

        /* Estado seleccionado */
        .servicios-list input[type="checkbox"]:checked + label {
            background-color: #0b8a7a;
            border-color: #0b8a7a;
            color: #ffffff;
            box-shadow: 0 6px 16px rgba(11, 138, 122, 0.35);
        }

        .file-upload-wrapper {
            position: relative;
            width: 100%;
            height: 300px;
            border: 2px dashed #148C76;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
            cursor: pointer;
            overflow: hidden;
            background-color: #fafcff;
        }
            .file-upload-wrapper:hover {
                background-color: #f7faff;
            }

        .file-upload-input {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            opacity: 0;
            cursor: pointer;
        }
        .file-upload-label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            color: #1C0D12;
            pointer-events: none;
        }
            .file-upload-label strong {
                font-size: 18px;
                font-weight: 700;
                display: block;
                margin-bottom: 8px;
            }
            .file-upload-label .file-upload-text {
                font-size: 14px;
                font-weight: 400;
            }

        .file-upload-wrapper.has-preview {
            border-style: solid;
            border-color: #148C76;
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
        }

            .file-upload-wrapper.has-preview .file-upload-label {
                display: none;
            }

        .validation-error {
            display: block;
            color: #C31E1E;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 14px;
            font-weight: 500;
            margin-top: 4px;
        }

        /* Para que en pantallas grandes se vea “grid” fluido */
        @media (min-width: 768px) {
            .servicios-list label {
                min-width: 280px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">

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
                <div class="col-md-4">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server"
                        CssClass="form-control"
                        MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="col-md-4">
                    <label for="txtPrimerApellido" class="form-label">Primer apellido</label>
                    <asp:TextBox ID="txtPrimerApellido" runat="server"
                        CssClass="form-control"
                        MaxLength="30" />
                    <asp:RequiredFieldValidator ID="rfvPrimerApellido" runat="server"
                        ControlToValidate="txtPrimerApellido"
                        ErrorMessage="El primer apellido es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="col-md-4">
                    <label for="txtSegundoApellido" class="form-label">Segundo apellido</label>
                    <asp:TextBox ID="txtSegundoApellido" runat="server"
                        CssClass="form-control"
                        MaxLength="30" />
                    <!-- Segundo apellido opcional, sin RequiredFieldValidator -->
                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-6">
                    <label for="txtCorreo" class="form-label">Correo electrónico</label>
                    <asp:TextBox ID="txtCorreo" runat="server"
                        CssClass="form-control"
                        TextMode="Email"
                        MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvCorreo" runat="server"
                        ControlToValidate="txtCorreo"
                        ErrorMessage="El correo electrónico es obligatorio."
                        CssClass="text-danger"
                        Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revCorreo" runat="server"
                        ControlToValidate="txtCorreo"
                        ValidationExpression="^.+@.+$"
                        ErrorMessage="El correo electrónico no es válido."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="col-md-3">
                    <label for="txtTelefono" class="form-label">Número de teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server"
                        CssClass="form-control"
                        MaxLength="12" />
                    <asp:RegularExpressionValidator ID="revTelefono" runat="server"
                    ControlToValidate="txtTelefono"
                    ValidationExpression="^\d{9}$"
                    ErrorMessage="El teléfono debe tener 9 dígitos."
                    CssClass="text-danger"
                    Display="Dynamic" />


                </div>
            </div>

            <div class="row mb-3">
                <div class="col-md-4">
                    <label for="txtContrasenha" class="form-label">Contraseña inicial</label>
                    <asp:TextBox ID="txtContrasenha" runat="server"
                        CssClass="form-control"
                        TextMode="Password"
                        MaxLength="100" />
                    <asp:RequiredFieldValidator ID="rfvContrasenha" runat="server"
                        ControlToValidate="txtContrasenha"
                        ErrorMessage="La contraseña inicial es obligatoria."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>

                <div class="col-md-4">
                    <label for="txtConfirmarContrasenha" class="form-label">Confirmar contraseña</label>
                    <asp:TextBox ID="txtConfirmarContrasenha" runat="server"
                        CssClass="form-control"
                        TextMode="Password"
                        MaxLength="100" />
                    <asp:CompareValidator ID="cvContrasenha" runat="server"
                        ControlToCompare="txtContrasenha"
                        ControlToValidate="txtConfirmarContrasenha"
                        ErrorMessage="Las contraseñas no coinciden."
                        CssClass="text-danger"
                        Display="Dynamic" />
                </div>
            </div>

            <div class="mb-4">
                <label class="form-label">Subir imagen</label>
                <div class="mb-4" style="width: 300px; max-width: 100%; margin: 0 auto;" >
                <div class="file-upload-wrapper" ID="fileUploadWrapper" runat="server">
                    <asp:FileUpload ID="fileUpload" runat="server" CssClass="file-upload-input" />
                        <div class="file-upload-label">
                            <strong>Subir imagen</strong>
                            <span class="file-upload-text">Arrastra y suelta o haz click para subir</span>
                        </div>
                    <asp:HiddenField ID="hdnImagenActual" runat="server" Value="" />
                </div>
                <span class="validation-error validation-error-js" style="display: none;"></span>
                <asp:CustomValidator ID="cvImagen" runat="server" 
                    ErrorMessage="Debe seleccionar una imagen para el empleado nuevo."
                    CssClass="validation-error" 
                    Display="Dynamic"
                    ValidationGroup="GuardarProducto"
                    ClientValidationFunction="validateImageUpload" />
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
                                <select id="selModalInicioHora" class="form-select">
                                    <option value="">--</option>
                                    <option value="00">00:00</option>
                                    <option value="01">01:00</option>
                                    <option value="02">02:00</option>
                                    <option value="03">03:00</option>
                                    <option value="04">04:00</option>
                                    <option value="05">05:00</option>
                                    <option value="06">06:00</option>
                                    <option value="07">07:00</option>
                                    <option value="08">08:00</option>
                                    <option value="09">09:00</option>
                                    <option value="10">10:00</option>
                                    <option value="11">11:00</option>
                                    <option value="12">12:00</option>
                                    <option value="13">13:00</option>
                                    <option value="14">14:00</option>
                                    <option value="15">15:00</option>
                                    <option value="16">16:00</option>
                                    <option value="17">17:00</option>
                                    <option value="18">18:00</option>
                                    <option value="19">19:00</option>
                                    <option value="20">20:00</option>
                                    <option value="21">21:00</option>
                                    <option value="22">22:00</option>
                                    <option value="23">23:00</option>
                                </select>
                            </div>
                            <div class="col-5">
                                <select id="selModalFinHora" class="form-select">
                                    <option value="">--</option>
                                    <option value="00">00:00</option>
                                    <option value="01">01:00</option>
                                    <option value="02">02:00</option>
                                    <option value="03">03:00</option>
                                    <option value="04">04:00</option>
                                    <option value="05">05:00</option>
                                    <option value="06">06:00</option>
                                    <option value="07">07:00</option>
                                    <option value="08">08:00</option>
                                    <option value="09">09:00</option>
                                    <option value="10">10:00</option>
                                    <option value="11">11:00</option>
                                    <option value="12">12:00</option>
                                    <option value="13">13:00</option>
                                    <option value="14">14:00</option>
                                    <option value="15">15:00</option>
                                    <option value="16">16:00</option>
                                    <option value="17">17:00</option>
                                    <option value="18">18:00</option>
                                    <option value="19">19:00</option>
                                    <option value="20">20:00</option>
                                    <option value="21">21:00</option>
                                    <option value="22">22:00</option>
                                    <option value="23">23:00</option>
                                </select>
                            </div>
                            <div class="col-2 d-grid">
                                <button type="button" class="btn btn-primary"
                                        onclick="agregarHorarioModal()">
                                    +
                                </button>
                            </div>
                                </div>
                        <small id="msgInfoHorario" class="d-block mt-2 text-muted">
                            Formato 24h. Ejemplo: 08:00 a 11:00.
                        </small>
                    </div>

                    <hr />

                    <div>
                        <label class="form-label">Horarios actuales</label>
                        <ul id="listaHorariosDia" class="list-group small">
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
        <div class="card-header bg-white border-0 d-flex align-items-center justify-content-between">
            <span class="servicios-card-header">Servicios cosmetológicos que realiza</span>
        </div>
        <div class="card-body">
            <p class="servicios-helper-text">
                Selecciona uno o varios servicios que este empleado puede realizar. 
                Estas opciones se usarán luego para mostrar qué tratamientos ofrece cada profesional.
            </p>

            <asp:CheckBoxList ID="cblServicios" runat="server"
                CssClass="servicios-list"
                RepeatLayout="Flow"
                RepeatDirection="Vertical">
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

        // --- FUNCIONES AUXILIARES DE TIEMPO ---
        function timeToMinutes(timeStr) {
            if (!timeStr) return 0;
            const parts = timeStr.split(':');
            return parseInt(parts[0]) * 60 + parseInt(parts[1]);
        }

        function minutesToTime(mins) {
            const h = Math.floor(mins / 60).toString().padStart(2, '0');
            const m = (mins % 60).toString().padStart(2, '0');
            return `${h}:${m}`;
        }

        // --- MANEJO DE MENSAJES DE ERROR (NUEVO) ---
        function mostrarError(mensaje) {
            const el = document.getElementById('msgInfoHorario');
            el.textContent = mensaje;
            el.classList.remove('text-muted');
            el.classList.add('text-danger', 'fw-bold'); // Rojo y negrita
        }

        function limpiarMensaje() {
            const el = document.getElementById('msgInfoHorario');
            el.textContent = 'Formato 24h. Ejemplo: 08:00 a 11:00.';
            el.classList.remove('text-danger', 'fw-bold');
            el.classList.add('text-muted');
        }

        // --- LÓGICA DE INTERFAZ ---

        function abrirModalDia(diaKey) {
            diaActual = diaKey;
            document.getElementById('modalDiaNombre').innerText = diaKey;
            document.getElementById('selModalInicioHora').value = '';
            document.getElementById('selModalFinHora').value = '';

            // Importante: Limpiamos cualquier error previo al abrir
            limpiarMensaje();
            renderHorariosDia();

            var modalElement = document.getElementById('modalHorarioDia');
            var modal = new bootstrap.Modal(modalElement);
            modal.show();
        }

        function getHiddenFieldId() { return 'hfHorarios' + diaActual; }
        function getSpanId() { return 'spHorarios' + diaActual; }

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

        // --- LÓGICA PRINCIPAL ---

        function agregarHorarioModal() {
            // 1. Limpiamos errores previos antes de validar
            limpiarMensaje();

            const iniHora = document.getElementById('selModalInicioHora').value;
            const finHora = document.getElementById('selModalFinHora').value;

            if (!iniHora || !finHora) {
                mostrarError('Por favor, selecciona tanto la hora de inicio como la de fin.');
                return;
            }

            const minutosInicio = timeToMinutes(iniHora + ':00');
            const minutosFin = timeToMinutes(finHora + ':00');

            if (minutosFin <= minutosInicio) {
                mostrarError('La hora de fin debe ser posterior a la hora de inicio.');
                return;
            }

            // 2. Obtener intervalos existentes
            const hf = document.getElementById(getHiddenFieldId());
            let intervalos = [];
            const rawValues = (hf.value || '').split(';').map(s => s.trim()).filter(s => s !== '');

            rawValues.forEach(val => {
                const parts = val.split('-');
                intervalos.push({
                    start: timeToMinutes(parts[0]),
                    end: timeToMinutes(parts[1])
                });
            });

            // 3. VALIDACIÓN DE CRUCE (SOLAPAMIENTO)
            for (let i = 0; i < intervalos.length; i++) {
                let existente = intervalos[i];
                // Si se cruzan estrictamente (sin contar bordes exactos)
                if (minutosInicio < existente.end && minutosFin > existente.start) {
                    mostrarError(`El horario ${iniHora}-${finHora} choca con uno existente (${minutesToTime(existente.start)}-${minutesToTime(existente.end)}).`);
                    return;
                }
            }

            // 4. Agregar y Procesar
            intervalos.push({ start: minutosInicio, end: minutosFin });
            intervalos.sort((a, b) => a.start - b.start);

            // Fusión de adyacentes
            let stack = [];
            if (intervalos.length > 0) {
                stack.push(intervalos[0]);
                for (let i = 1; i < intervalos.length; i++) {
                    let top = stack[stack.length - 1];
                    let current = intervalos[i];

                    if (top.end >= current.start) {
                        top.end = Math.max(top.end, current.end);
                    } else {
                        stack.push(current);
                    }
                }
            }

            // 5. Guardar
            const nuevosSegmentos = stack.map(inv => {
                return minutesToTime(inv.start) + '-' + minutesToTime(inv.end);
            });

            hf.value = nuevosSegmentos.join(';');

            // Éxito: Limpiamos inputs
            document.getElementById('selModalInicioHora').value = '';
            document.getElementById('selModalFinHora').value = '';

            renderHorariosDia();
            actualizarResumenDia();
        }

        function eliminarHorario(seg) {
            const hf = document.getElementById(getHiddenFieldId());
            let partes = (hf.value || '').split(';').map(s => s.trim()).filter(s => s !== '' && s !== seg);
            hf.value = partes.join(';');
            renderHorariosDia();
            actualizarResumenDia();
            limpiarMensaje(); // Limpiar error si había uno al borrar
        }

        function actualizarResumenDia() {
            const hf = document.getElementById(getHiddenFieldId());
            const span = document.getElementById(getSpanId());
            const value = (hf.value || '').trim();
            if (!value) span.textContent = 'Sin horarios';
            else span.textContent = value.replace(/;/g, ', ');
        }

        // --- SUBIDA DE IMAGEN (Sin cambios) ---
        function validateImageUpload(source, args) {
            const fileInput = document.getElementById('<%= fileUpload.ClientID %>');
            const hiddenImage = document.getElementById('<%= hdnImagenActual.ClientID %>');
            if (fileInput.files.length > 0 || hiddenImage.value !== '') { args.IsValid = true; return; }
            args.IsValid = false;
        }

        $(".file-upload-input").on("change", function () {
            const file = this.files[0];
            const $wrapper = $(this).closest(".file-upload-wrapper");

            // Lógica de visualización de imagen...
            if (file && file.type.startsWith("image/")) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    $wrapper.css("background-image", "url(" + e.target.result + ")").addClass("has-preview");
                    $wrapper.find(".file-upload-label").hide();
                    $wrapper.parent().find(".validation-error-js").hide();
                };
                reader.readAsDataURL(file);
            }
        });
    </script>
</asp:Content>

<asp:Content ID="ContentExtra" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
