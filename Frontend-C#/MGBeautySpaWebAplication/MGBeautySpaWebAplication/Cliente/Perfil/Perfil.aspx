<%@ Page Title="" Language="C#" MasterPageFile="~/Cliente/Perfil/Perfil.Master" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.Perfil" %>

<asp:Content ID="ctMain" ContentPlaceHolderID="ProfileBodyContent" runat="server">
    
    <style>
        /* Estilos generales de la página de perfil */
        .profile-info-wrapper {
            display: flex;
            flex-direction: column;
            width: 100%;
            max-width: 960px;
            padding: 0;
        }

        /* Saludo y Título */
        .profile-greeting-header {
            padding: 16px;
            width: 100%;
            box-sizing: border-box;
            margin-bottom: 20px;
        }

        .greeting-title {
            margin: 0;
            font-family: 'ZCOOL XiaoWei', serif;
            font-weight: 400;
            font-size: 40px;
            line-height: 40px;
            color: #148C76;
        }
        
        .greeting-subtitle {
            margin: 8px 0 0 0;
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            color: #1C0D12;
        }

        .data-grid {
            padding: 0 16px;
            width: 100%;
            display: flex;
            flex-wrap: wrap; 
            gap: 24px; /* Espacio entre filas/columnas */
            box-sizing: border-box;
            /* QUITAMOS el width: 438px; fijo de aquí */
        }

        /* Cada tarjeta de datos (Nombre, Email, etc.) */
        .data-card {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: flex-start;
            padding: 16px 8px 16px 0;
            gap: 4px;
            box-sizing: border-box;
            border-top: 1px solid #E6E8EB;

            /* REEMPLAZAMOS width: 438px; por la fórmula de cuadrícula flexible */
            width: calc(50% - 12px); /* 50% para dos columnas - (gap / 2) */
        }
        
        /* Contenedor especial para el campo de una sola columna */
        .data-card-full {
             width: 100%;
        }

        /* Estilo de la etiqueta (Nombres, Email, etc.) */
        .data-label {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 14px;
            font-weight: 500;
            color: #7DA3A1; /* Color gris suave */
            margin-bottom: 4px;
        }

        /* Estilo del valor (Sophia, Bennett, a2023...) */
        .data-value {
            font-family: 'Plus Jakarta Sans', sans-serif;
            font-size: 16px;
            font-weight: 400;
            color: #1C0D12;
            margin: 0;
        }
        
        /* Sección de Botón */
        .button-section {
            padding: 12px 16px;
            width: 100%;
            box-sizing: border-box;
        }

        .edit-button {
            display: flex;
            justify-content: center;
            align-items: center;
            padding: 0 16px;
            width: 89px;
            height: 40px;
            background: #1EC3B6;
            border-radius: 20px;
            font-weight: 700;
            font-size: 14px;
            color: #FCF7FA;
            border: none;
            cursor: pointer;
        }
    </style>

    <div class="profile-info-wrapper">

        <div class="profile-greeting-header">
            <h1 class="greeting-title">¡Hola, <asp:Literal ID="litUserNameGreeting" runat="server" />!</h1>
            <p class="greeting-subtitle">Aquí puedes revisar y actualizar tus datos personales.</p>
        </div>
        
        <div class="data-grid"> 

            <div class="data-card">
                <span class="data-label">Nombres</span>
                <p class="data-value"><asp:Literal ID="litNombres" runat="server">Sophia</asp:Literal></p>
            </div>
            
            <div class="data-card">
                <span class="data-label">Apellidos</span>
                <p class="data-value"><asp:Literal ID="litApellidos" runat="server">Bennett</asp:Literal></p>
            </div>

            <div class="data-card">
                <span class="data-label">Email</span>
                <p class="data-value"><asp:Literal ID="litEmail" runat="server">a20230636@pucp.edu.pe</asp:Literal></p>
            </div>
            
            <div class="data-card">
                <span class="data-label">Teléfono</span>
                <p class="data-value"><asp:Literal ID="litTelefono" runat="server">999888777</asp:Literal></p>
            </div>

            <div class="data-card">
                <span class="data-label">Fecha de nacimiento</span>
                <p class="data-value"><asp:Literal ID="litFechaNac" runat="server">12/02/2000</asp:Literal></p>
            </div>
            
            <div class="data-card">
                <span class="data-label">Sexo</span>
                <p class="data-value"><asp:Literal ID="litSexo" runat="server">Femenino</asp:Literal></p>
            </div>
            
            <div class="button-section">
                <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="edit-button" />
            </div>

        </div> </div>
</asp:Content>

