<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecuperarContraseña.aspx.cs" Inherits="MGBeautySpaWebAplication.Cuenta.RecuperarContraseña" %>
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

    <!-- Estilos del sistema -->
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/bootstrap.min.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/icomoon/icomoon.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/css/vendor.css") %>" />
    <link rel="stylesheet" href="<%: ResolveUrl("~/Content/style.css?v=3") %>" />
</head>
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Olvidé mi Contraseña</title>
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700&display=swap" rel="stylesheet">

    <style>
        body, html {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Plus Jakarta Sans', sans-serif;
        }

        .page-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 0px;
            width: 100%;
            min-height: 800px;
            background: #ECFFFD;
            justify-content: center;
        }

        .content-wrapper {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 20px 0px;
            width: 960px;
            max-width: 960px;
            background-color: #FFFFFF;
            border-radius: 12px;
            box-shadow: 0 4px 12px rgba(0, 0, 0, 0.05);
        }

        .title-container {
            padding: 20px 16px 12px;
            width: 960px;
            box-sizing: border-box;
        }

        .title-text {
            width: 100%;
            margin: 0;
            font-weight: 700;
            font-size: 28px;
            line-height: 35px;
            text-align: center;
            color: #1C0D12;
        }

        .subtitle-container {
            padding: 4px 16px 12px;
            width: 960px;
            box-sizing: border-box;
        }

        .subtitle-text {
            width: 100%;
            margin: 0;
            font-weight: 400;
            font-size: 16px;
            line-height: 24px;
            text-align: center;
            color: #1C0D12;
        }

        .input-wrapper {
            padding: 12px 16px;
            width: 480px;
            max-width: 480px;
            box-sizing: border-box;
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
            color: #1C0D12;
        }
        
        .form-input::placeholder {
            color: #757575;
        }

        .button-wrapper {
            display: flex;
            flex-direction: row;
            justify-content: center;
            align-items: flex-start;
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
            
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 700;
            font-size: 15px;
            line-height: 21px;
            text-align: center;
            color: #FCF7FA;
            
            border: none;
            cursor: pointer;
        }

        .link-container {
            display: flex;
            flex-direction: column;
            align-items: center;
            padding: 4px 16px 12px;
            width: 960px;
            box-sizing: border-box;
        }

        .form-link {
            width: 100%;
            font-family: 'Plus Jakarta Sans';
            font-style: normal;
            font-weight: 400;
            font-size: 14px;
            line-height: 21px;
            text-align: center;
            color: #107369;
            text-decoration: none;
        }

    </style>
</head>
<body>

    <div class="page-container">
        <main class="content-wrapper">
            
            <div class="title-container">
                <h1 class="title-text">¿Olvidaste tu contraseña?</h1>
            </div>

            <div class="subtitle-container">
                <p class="subtitle-text">
                    Ingresa el correo electrónico asociado a tu cuenta, y enviaremos un link para que establezcas una nueva contraseña.
                </p>
            </div>

            <form action="#" method="POST" style="display: contents;">
                <div class="input-wrapper">
                    <input type="email" class="form-input" placeholder="Ingrese su correo electrónico">
                </div>

                <div class="button-wrapper">
                    <a href="ModificarContraseña.aspx" class="submit-button" style="text-decoration: none;">
                        Enviar link al correo
                    </a>
                </div>
            </form>

            <div class="link-container">
                <a href="../Login.aspx" class="form-link">Volver a Iniciar Sesión</a>
            </div>

            <div class="link-container" style="padding-top: 0;">
                <a href="Registro.aspx" class="form-link">Crear cuenta</a>
            </div>

        </main>
    </div>

</body>
</html>