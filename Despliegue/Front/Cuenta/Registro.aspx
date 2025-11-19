<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="MGBeautySpaWebAplication.Cuenta.WebForm1" UnobtrusiveValidationMode="None"%>
<!DOCTYPE html>
<html lang="es">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Inicio de Sesión | MG Beauty Spa</title>

    <!-- Fuentes (prioridad para ZCOOL XiaoWei y Poppins) -->
    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=ZCOOL+XiaoWei&family=Poppins:wght@300;400;500;600&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.css">
    <!-- Estilos del sistema -->
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/icomoon/icomoon.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/css/vendor.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/style.css?v=3") %>" />
</head>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Crear una Cuenta</title>
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700&display=swap" rel="stylesheet">
    
    <script>
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
    </script>

    <style>
        html, body {
        height: 100%;
        }

        body, html {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Plus Jakarta Sans', sans-serif;
            background-color: #f0f0f0; 
        }

        .page-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 0px;
            width: 100%; 
            height: 100%;
            padding-top: 20px;
        }

        .content-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 0px;
            width: 960px;
            max-width: 960px;
            background-color: #FFFFFF;
            border-radius: 16px;
        }

        .title-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 16px 12px;
            width: 960px;
            box-sizing: border-box; 
        }

        .title-text {
            width: 100%;
            height: 35px;
            margin: 0; 
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 28px;
            line-height: 35px;
            text-align: center;
            color: #1C0D12;
        }
        
        .form-group {
            display: flex;
            flex-direction: column;
            align-items: flex-start;
            padding: 7px 16px;
            gap: 10px;
            width: 480px;
            box-sizing: border-box;
        }

        .form-label {
            width: 100%; 
            height: 24px;
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 500;
            font-size: 16px;
            line-height: 24px;
            color: #1C0D12;
        }

        .form-input {
            box-sizing: border-box;
            display: flex;
            flex-direction: row;
            align-items: center;
            padding: 15px;
            width: 100%; 
            height: 56px;
            background: #FFFFFF;
            border: 1px solid #E3D4D9;
            border-radius: 12px;
            

            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 16px;
            line-height: 24px;
            color: #757575;
        }

        .terms-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 4px 16px 12px;
            width: 960px;
            box-sizing: border-box;
            margin-top: 10px;
        }

        .terms-text {
            width: 100%;
            height: 21px;
            margin: 0;
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            text-align: center;
            color: #107369;
        }

        .button-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 12px 16px;
            width: 960px;
            box-sizing: border-box;
        }

        .submit-button {
            display: flex;
            flex-direction: row;
            justify-content: center;
            align-items: center;
            padding: 0px 16px;
            width: 480px;
            max-width: 480px;
            height: 40px;
            background: #1EC3B6;
            border-radius: 20px;
            border: none;
            cursor: pointer; 

            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 15px;
            line-height: 21px;
            text-align: center;
            color: #FCF7FA;
        }

        .modal-superpuesto {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
        
            background-color: rgba(0, 0, 0, 0.6); 

            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            z-index: 1000;
        }

        .success-box {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 0px;
            width: 960px;
            max-width: 960px;
            background-color: #FFFFFF;
            border-radius: 12px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }

        .success-title {
            width: 100%;
            padding: 20px 16px 12px;
            box-sizing: border-box;
            margin: 0;
            font-weight: 700;
            font-size: 28px;
            line-height: 35px;
            text-align: center;
            color: #1A0F12;
        }

        .success-message {
            width: 100%;
            padding: 4px 16px 12px;
            box-sizing: border-box;
            margin: 0;
            font-weight: 400;
            font-size: 16px;
            line-height: 24px;
            text-align: center;
            color: #1A0F12;
        }

        .button-wrapper {
            width: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 12px 16px;
        }

        .login-button {
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 0px 16px;
            width: 122px;
            height: 40px;
            background: #1EC3B6;
            border-radius: 20px;
            font-family: 'Plus Jakarta Sans';
            font-weight: 700;
            font-size: 14px;
            line-height: 21px;
            text-align: center;
            color: #FFFFFF;
            text-decoration: none; 
            cursor: pointer;
            border: none;
        }

        .password-wrapper {
            position: relative;
            width: 100%;
        }

        .password-wrapper .form-input {
            padding-right: 45px; /* espacio para el icono */
        }

        .toggle-password {
            position: absolute;
            right: 16px;
            top: 50%;
            transform: translateY(-50%);
            cursor: pointer;
            font-size: 20px;
            color: #666;
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <div id="formRegistro">

            <div class="page-container">
                <main class="content-wrapper">

                    <div class="title-container">
                        <h1 class="title-text">Crear una cuenta</h1>
                    </div>
                
                    <div class="form-group">
                        <label class="form-label">Nombres</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-input" placeholder="Ingrese su nombre" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator 
                            ID="revNombre" runat="server"
                            ControlToValidate="txtNombre"
                            ValidationExpression="^.{1,30}$"
                            ErrorMessage="El nombre no puede exceder los 30 caracteres."
                            ForeColor="Red" Display="Dynamic" />
                    </div>
                
                    <div class="form-group">
                        <label class="form-label">Apellido Paterno</label>
                        <asp:TextBox ID="txtApellidoP" runat="server" CssClass="form-input" placeholder="Ingrese su apellido paterno" />
                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellidoP"
                            ErrorMessage="El apellido paterno es obligatorio." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator 
                            ID="revApellidoP" runat="server"
                            ControlToValidate="txtApellidoP"
                            ValidationExpression="^.{1,30}$"
                            ErrorMessage="El apellido paterno no puede exceder los 30 caracteres."
                            ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Apellido Materno</label>
                        <asp:TextBox ID="txtApellidoM" runat="server" CssClass="form-input" placeholder="Ingrese su apellido materno" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtApellidoM"
                            ErrorMessage="El apellido materno es obligatorio." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator 
                            ID="revApellidoM" runat="server"
                            ControlToValidate="txtApellidoM"
                            ValidationExpression="^.{1,30}$"
                            ErrorMessage="El apellido materno no puede exceder los 30 caracteres."
                            ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Número de celular</label>
                        <asp:TextBox ID="txtCelular" runat="server" CssClass="form-input" placeholder="Ingrese su número de celular" />
                        <asp:RequiredFieldValidator ID="rfvCelular" runat="server" ControlToValidate="txtCelular"
                            ErrorMessage="El número de celular es obligatorio." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revCelular" runat="server" ControlToValidate="txtCelular"
                            ValidationExpression="^\d{9}$" ErrorMessage="Debe ingresar un número válido de 9 dígitos." ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Correo electrónico</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" placeholder="Ingrese su correo electrónico" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="El correo electrónico es obligatorio." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ErrorMessage="Ingrese un correo electrónico válido." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator 
                            ID="revEmailLength" runat="server"
                            ControlToValidate="txtEmail"
                            ValidationExpression="^.{1,100}$"
                            ErrorMessage="El correo no puede exceder los 100 caracteres."
                            ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Contraseña</label>
                            <div class="password-wrapper">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input"
                                             TextMode="Password" placeholder="Ingrese su contraseña" />
                                <i class="bi bi-eye-slash toggle-password"
                                   onclick="togglePassword('txtPassword', this)"></i>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="La contraseña es obligatoria." ForeColor="Red" Display="Dynamic" />
                            <asp:RegularExpressionValidator 
                                ID="revPasswordLength" runat="server"
                                ControlToValidate="txtPassword"
                                ValidationExpression="^.{1,30}$"
                                ErrorMessage="La contraseña no puede exceder los 30 caracteres."
                                ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="form-group">
                        <label class="form-label">Confirmar contraseña</label>

                            <div class="password-wrapper">
                                <asp:TextBox ID="txtConfirmar" runat="server" CssClass="form-input"
                                             TextMode="Password" placeholder="Confirme su contraseña" />
                                <i class="bi bi-eye-slash toggle-password"
                                   onclick="togglePassword('txtConfirmar', this)"></i>
                            </div>

                            <asp:RequiredFieldValidator ID="rfvConfirmar" runat="server" ControlToValidate="txtConfirmar"
                                ErrorMessage="Debe confirmar su contraseña." ForeColor="Red" Display="Dynamic" />
                            <asp:CompareValidator ID="cvPassword" runat="server" ControlToValidate="txtConfirmar" ControlToCompare="txtPassword"
                                ErrorMessage="Las contraseñas no coinciden." ForeColor="Red" Display="Dynamic" />
                    </div>

                    <div class="terms-container">
                        <p class="terms-text">
                            Al asociar una cuenta a MG Beatuy Spa, usted está aceptando nuestros Términos de Servicios y Políticas de Privacidad.
                        </p>
                    </div>

                    <div class="button-container">
                        <asp:Button ID="btnCrearCuenta" 
                                    runat="server" 
                                    Text="Crear Cuenta" 
                                    CssClass="submit-button"
                                    OnClick="btnCrearCuenta_Click" />
                    </div>
                    
                    <div class="text-center mt-3">
                         <small>
                             <a href="/Login.aspx" class="text-decoration-none">
                                 ¿Ya tienes una cuenta asociada? ¡Inicia Sesión!
                             </a>
                         </small>
                     </div>
                </main>
            </div>
        </div>

        <div id="modalExito" class="modal-superpuesto" style="display: none;">
            <div class="success-box">
                <h1 class="success-title">Cuenta creada</h1>
                <p class="success-message">
                    Su cuenta se ha creado correctamente. Ya puede empezar a explorar nuestros productos y servicios.
                </p>
                <div class="button-wrapper">
                    <a href="../Login.aspx" class="login-button">
                        Iniciar Sesión
                    </a>
                </div>
            </div>
        </div>

        <div id="modalError" class="modal-superpuesto" style="display: none;">
            <div class="success-box">
                <h1 class="success-title" style="color: #b91c1c;">Error</h1>
                <p class="success-message">
                    Ya existe una cuenta registrada con este correo electrónico. Por favor, utilice otro correo o inicie sesión.
                </p>
                <div class="button-wrapper">
                    <button class="login-button" onclick="document.getElementById('modalError').style.display='none';return false;">
                        Cerrar
                    </button>
                </div>
            </div>
        </div>
    </form>

</body>
</html>
