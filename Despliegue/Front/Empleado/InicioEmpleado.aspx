<%@ Page Title="" Language="C#" MasterPageFile="~/Empleado/Empleado.Master" AutoEventWireup="true" CodeBehind="InicioEmpleado.aspx.cs" Inherits="MGBeautySpaWebAplication.Empleado.InicioEmpleado" %>
<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
     <%-- 
   Estilos específicos para esta página. 
   Idealmente, esto debería moverse a un archivo .css (como style.css o Empleado.css) 
   y cargarse en el HeadContent.
 --%>
 <style>
     .empleado-page-bg {
         /* Fondo de 'Depth 0, Frame 0' */
         min-height: calc(100vh - 120px); /* Ajustar altura menos header/footer */
     }

     .hero-card-empleado {
         /* Contenedor principal de 'Depth 3, Frame 1' */
         width: 960px;
         max-width: 960px;
         border: none;
         overflow: hidden; /* Para que la imagen respete los bordes */
     }

     .hero-image-container {
         /* Contenedor de 'Depth 5, Frame 0' */
         position: relative;
         width: 100%;
         height: 565px;
     }

     .hero-image {
         /* Estilo de 'image 7' */
         width: 100%;
         height: 100%;
         /* Asumimos que la imagen está en la carpeta de imágenes */
         /* background-image: url('<%: ResolveUrl("~/Content/images/empleado-hero-bg.png") %>'); */
         background-size: cover;
         background-position: center;
     }

     .hero-text-overlay {
         /* Posicionamiento de 'Depth 7, Frame 0' */
         position: absolute;
         width: 695px;
         height: 92px;
         left: 232px;
         top: 412px;
         z-index: 1;
         text-align: right;
     }

     .hero-title {
         /* Estilo de 'Bienvenido a...' */
         font-family: 'ZCOOL XiaoWei', serif;
         font-size: 48px;
         line-height: 60px;
         font-weight: 400;
         letter-spacing: -2px;
         color: #000000;
         margin: 0;
     }

     .hero-subtitle {
         /* Estilo de 'Aquí podrás ver...' */
         font-family: 'Plus Jakarta Sans', sans-serif;
         font-size: 16px;
         line-height: 24px;
         font-weight: 400;
         color: #000000;
         margin: 0;
     }
     
     .btn-custom-teal {
         /* Estilo de los botones 'Depth 5, Frame 1' */
         background-color: #1EC3B6;
         color: #FCF7FA;
         font-family: 'Plus Jakarta Sans', sans-serif;
         font-weight: 700;
         font-size: 14px;
         line-height: 21px;
         
         /* Dimensiones y alineación del botón */
         width: 143px; 
         height: 40px;
         display: inline-flex;
         align-items: center;
         justify-content: center;
         text-decoration: none;
     }

     .btn-custom-teal:hover {
         background-color: #19a599; /* Un poco más oscuro en hover */
         color: #FCF7FA;
     }
     
     .button-row {
         /* Contenedor de 'Depth 4, Frame 5/6' */
         padding: 12px 16px;
         gap: 20px;
     }
 </style>

</asp:Content>
<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
   
    <div class="empleado-page-bg d-flex align-items-center justify-content-center py-5">
        
        <div class="card hero-card-empleado">

            <div class="hero-image-container">
                
                <div class="hero-image">
                    <img src="<%: ResolveUrl("~/Content/images/Empleado/InicioEmpleado.png") %>" alt="MG Beauty Spa" style="height:100%">
                </div>

                
                <div class="hero-text-overlay">
                    <h1 class="hero-title">
                        Bienvenido a MG Beauty Spa
                    </h1>
                    <p class="hero-subtitle">
                        Aquí podrás ver tu horario, consultar tus citas y más
                    </p>
                </div>
            </div>
            
            <div class="card-body p-0">
                
                <div class="button-row">
                    <a href="<%: ResolveUrl("~/Empleado/MisCitas.aspx") %>" 
                       class="btn btn-custom-teal rounded-pill">
                        Ver mis citas
                    </a>
                </div>
                
                <div class="button-row">
                     <a href="<%: ResolveUrl("~/Empleado/MiHorario.aspx") %>" 
                        class="btn btn-custom-teal rounded-pill">
                        Ver mi horario
                    </a>
                </div>
            </div>
            
        </div>
    </div>
</asp:Content>
<asp:Content ID="ScriptsContent" ContentPlaceHolderID="ScriptsContent" runat="server">
</asp:Content>


