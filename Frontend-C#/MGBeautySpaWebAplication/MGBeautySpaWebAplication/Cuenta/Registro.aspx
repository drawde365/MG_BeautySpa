<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="MGBeautySpaWebAplication.Cuenta.WebForm1" UnobtrusiveValidationMode="None"%>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Crear Cuenta | MG Beauty Spa</title>

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=ZCOOL+XiaoWei&family=Poppins:wght@300;400;500;600&family=Plus+Jakarta+Sans:wght@400;500;700&display=swap" rel="stylesheet" />
    
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">
    
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/icomoon/icomoon.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/css/vendor.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/style.css?v=3") %>" />

    <style>
        /* CSS Específico solo para el Upload de archivos (el resto usa Bootstrap) */
        .file-upload-wrapper {
            position: relative;
            width: 100%;
            height: 200px; /* Un poco más compacto */
            border: 2px dashed #dee2e6; /* Color borde Bootstrap */
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
            cursor: pointer;
            overflow: hidden;
            background-color: #f8f9fa;
            transition: all 0.3s ease;
        }
        .file-upload-wrapper:hover {
            background-color: #e9ecef;
            border-color: #1EC3B6;
        }
        .file-upload-wrapper.has-preview {
            border-style: solid;
            border-color: #1EC3B6;
            background-size: cover;
            background-position: center;
            background-repeat: no-repeat;
        }
        .file-upload-input {
            position: absolute; top: 0; left: 0; width: 100%; height: 100%;
            opacity: 0; cursor: pointer; z-index: 10;
        }
        .toggle-password {
            cursor: pointer;
            z-index: 10;
            color: #6c757d;
        }
    </style>
</head>

<body class="page-auth" style="background-color: #f0f2f5;">

    <form id="form1" runat="server" class="min-vh-100 d-flex align-items-center justify-content-center py-5">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div class="card shadow-sm p-4 p-md-5" style="max-width: 800px; width: 100%; border-radius: 16px;">
            
            <div class="text-center mb-4">
                <img src="<%: ResolveUrl("~/Content/images/MGLogo2.svg") %>" alt="MG Beauty Spa" style="height:60px;">
                <h3 class="mt-3 fw-bold" style="font-family: 'Plus Jakarta Sans', sans-serif;">Crear una cuenta</h3>
                <small class="text-muted">Únete a nosotros para reservar tus citas</small>
            </div>

            <div class="row g-3">
                
                <div class="col-12">
                    <label class="form-label fw-medium">Nombres</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ingrese su nombre" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="Obligatorio" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label class="form-label fw-medium">Apellido Paterno</label>
                    <asp:TextBox ID="txtApellidoP" runat="server" CssClass="form-control" placeholder="Apellido paterno" />
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellidoP" ErrorMessage="Obligatorio" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label class="form-label fw-medium">Apellido Materno</label>
                    <asp:TextBox ID="txtApellidoM" runat="server" CssClass="form-control" placeholder="Apellido materno" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtApellidoM" ErrorMessage="Obligatorio" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label class="form-label fw-medium">Celular</label>
                    <asp:TextBox ID="txtCelular" runat="server" CssClass="form-control" placeholder="999 999 999" />
                    <asp:RequiredFieldValidator ID="rfvCelular" runat="server" ControlToValidate="txtCelular" ErrorMessage="Obligatorio" CssClass="text-danger small" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revCelular" runat="server" ControlToValidate="txtCelular" ValidationExpression="^\d{9}$" ErrorMessage="9 dígitos req." CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label class="form-label fw-medium">Correo electrónico</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="correo@ejemplo.com" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Obligatorio" CssClass="text-danger small" Display="Dynamic" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Inválido" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label class="form-label fw-medium">Contraseña</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="******" />
                        <span class="input-group-text bg-white border-start-0">
                            <i class="bi bi-eye-slash toggle-password" onclick="togglePassword('<%= txtPassword.ClientID %>', this)"></i>
                        </span>
                    </div>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Obligatorio" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label class="form-label fw-medium">Confirmar contraseña</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-control" TextMode="Password" placeholder="******" />
                        <span class="input-group-text bg-white border-start-0">
                            <i class="bi bi-eye-slash toggle-password" onclick="togglePassword('<%= txtConfirmar.ClientID %>', this)"></i>
                        </span>
                    </div>
                    <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmar" ControlToCompare="txtPassword" ErrorMessage="No coinciden" CssClass="text-danger small" Display="Dynamic" />
                </div>

                <div class="col-12 mt-4">
                    <label class="form-label fw-medium mb-2">Foto de Perfil</label>
                    <div class="file-upload-wrapper" id="fileUploadWrapper" runat="server">
                        <asp:FileUpload ID="fileUpload" runat="server" CssClass="file-upload-input" />
                        <div class="file-upload-label px-3">
                            <i class="bi bi-cloud-arrow-up fs-1 text-muted mb-2"></i>
                            <strong class="d-block text-dark">Subir imagen</strong>
                            <span class="text-muted small">Arrastra y suelta o haz click para subir (JPG, PNG)</span>
                        </div>
                        <asp:HiddenField ID="hdnImagenActual" runat="server" Value="" />
                    </div>
                    <div class="validation-error-js text-danger small mt-1" style="display: none;"></div>
                </div>

                <div class="col-12 text-center mt-2">
                    <p class="text-muted small mb-0">
                        Al registrarte, aceptas nuestros <a href="#" class="text-decoration-none">Términos de Servicio</a> y <a href="#" class="text-decoration-none">Política de Privacidad</a>.
                    </p>
                </div>

                <div class="col-12 mt-3">
                    <asp:Button ID="btnCrearCuenta" runat="server" Text="Crear Cuenta" 
                        CssClass="btn btn-primary w-100 py-2 rounded-pill fw-bold" 
                        style="background: #1EC3B6; border: none;"
                        OnClick="btnCrearCuenta_Click" />
                </div>

                <div class="col-12 text-center mt-3">
                    <span class="small text-muted">¿Ya tienes cuenta?</span>
                    <a href="../Login.aspx" class="text-decoration-none fw-semibold" style="color: #1EC3B6;">Inicia Sesión</a>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalExito" tabindex="-1" aria-hidden="true" data-bs-backdrop="static">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content text-center p-4" style="border-radius: 16px;">
                    <div class="modal-body">
                        <div class="mb-3">
                            <i class="bi bi-check-circle-fill text-success" style="font-size: 3rem;"></i>
                        </div>
                        <h4 class="fw-bold mb-3">¡Cuenta creada!</h4>
                        <p class="text-muted mb-4">Tu registro ha sido exitoso. Ya puedes disfrutar de nuestros servicios.</p>
                        <a href="../Login.aspx" class="btn btn-primary w-100 rounded-pill" style="background: #1EC3B6; border: none;">Iniciar Sesión</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalError" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content text-center p-4" style="border-radius: 16px;">
                    <div class="modal-body">
                        <div class="mb-3">
                            <i class="bi bi-x-circle-fill text-danger" style="font-size: 3rem;"></i>
                        </div>
                        <h4 class="fw-bold mb-3 text-danger">Error</h4>
                        <p class="text-muted mb-4" id="modalErrorMessage">
                            Ya existe una cuenta registrada con este correo electrónico.
                        </p>
                        <button type="button" class="btn btn-secondary w-100 rounded-pill" data-bs-dismiss="modal">Cerrar</button>
                    </div>
                </div>
            </div>
        </div>

    </form>

    <script src="<%: ResolveUrl("~/Scripts/jquery.min.js") %>"></script>
    <script src="<%: ResolveUrl("~/Scripts/bootstrap.bundle.min.js") %>"></script>

    <script type="text/javascript">
        // Función para mostrar/ocultar contraseña
        function togglePassword(textBoxId, icon) {
            var input = document.getElementById(textBoxId);
            if (input.type === "password") {
                input.type = "text";
                icon.classList.remove("bi-eye-slash");
                icon.classList.add("bi-eye");
            } else {
                input.type = "password";
                icon.classList.remove("bi-eye");
                icon.classList.add("bi-eye-slash");
            }
        }

        // Script para previsualizar imagen (Drag & Drop)
        $(".file-upload-input").on("change", function () {
            const file = this.files[0];
            const $wrapper = $(this).closest(".file-upload-wrapper");
            const $label = $wrapper.find(".file-upload-label");
            const $errorDisplay = $(".validation-error-js");

            // Limpiar error previo
            if ($errorDisplay.length) $errorDisplay.text("").hide();

            if (file) {
                if (!file.type.startsWith("image/")) {
                    if ($errorDisplay.length) {
                        $errorDisplay.text("Solo se permiten archivos de imagen (JPG, PNG, JPEG)").show();
                    }
                    this.value = "";
                    $wrapper.css("background-image", "none").removeClass("has-preview");
                    $label.show();
                    return;
                }

                const reader = new FileReader();
                reader.onload = function (e) {
                    $wrapper.css("background-image", "url(" + e.target.result + ")");
                    $wrapper.addClass("has-preview");
                    $label.hide();
                };
                reader.readAsDataURL(file);
            } else {
                $wrapper.css("background-image", "none");
                $wrapper.removeClass("has-preview");
                $label.show();
            }
        });

        // Función auxiliar para llamar al Modal desde C#
        function showSuccessModal() {
            var myModal = new bootstrap.Modal(document.getElementById('modalExito'));
            myModal.show();
        }

        function showErrorModal(message) {
            if (message) { document.getElementById('modalErrorMessage').innerText = message; }
            var myModal = new bootstrap.Modal(document.getElementById('modalError'));
            myModal.show();
        }
    </script>
</body>
</html>