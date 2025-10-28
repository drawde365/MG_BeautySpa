<%@ Page Title="Detalle Servicio" Language="C#" MasterPageFile="~/Cliente/Cliente.Master" AutoEventWireup="true" CodeBehind="DetalleServicio.aspx.cs" Inherits="MGBeautySpaWebAplication.Cliente.DetalleServicio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptsContent" runat="server">
    <link rel="stylesheet" href="DetalleServicio.css">
    
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link href="https://fonts.googleapis.com/css2?family=Plus+Jakarta+Sans:wght@400;500;700;800&display=swap" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="page-container">

        <nav class="breadcrumbs">
            <a href="Servicios.aspx">Servicios</a>
            <span>/</span>
            <%-- DATO DINÁMICO --%>
            <strong><asp:Literal ID="litNombreBreadcrumb" runat="server" /></strong>
        </nav>

        <header class="product-header">
            <div class="header-text">
                <%-- DATO DINÁMICO --%>
                <h1><asp:Literal ID="litNombreServicio" runat="server" /></h1>
                <%-- DATO DINÁMICO --%>
                <p><asp:Literal ID="litDescripcionCorta" runat="server" /></p>
            </div>
        </header>

        <section class="product-main-info">
            
            <div class="product-image-container">   
                <%-- DATO DINÁMICO --%>
                <asp:Image ID="imgServicio" runat="server" CssClass="product-image-css" />
            </div>

            <div class="product-details-container">
                <h2>Descripción</h2>
                <%-- DATO DINÁMICO --%>
                <p><asp:Literal ID="litDescripcionLarga" runat="server" /></p>
                
                <div class="price-section">
                    <%-- DATO DINÁMICO --%>
                    <span class="price">S/ <asp:Literal ID="litPrecio" runat="server" /></span>
                </div>
                
                <%-- CONTROL DE SERVIDOR --%>
                <asp:Button ID="btnRevisarCalendario" runat="server" 
                    Text="Reservar cita" 
                    CssClass="btn btn-primary" 
                    OnClick="btnRevisarCalendario_Click" />
            </div>
        </section>

        <section class="product-benefits">
            <h3>Beneficios</h3>
            
            <%-- LISTA DINÁMICA (REPEATER) --%>
            <asp:Repeater ID="rpBeneficios" runat="server">
                <HeaderTemplate>
                    <ul>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><%# Container.DataItem.ToString() %></li>
                </ItemTemplate>
                <FooterTemplate>
                    </ul>
                </FooterTemplate>
            </asp:Repeater>
            
        </section>

        <section class="product-reviews">
            <h3>Reseñas</h3>

            <div class="review-summary">
                <div class="summary-score">
                    <%-- DATO DINÁMICO --%>
                    <div class="score-number"><asp:Literal ID="litReviewScore" runat="server" /></div>
                    <div class="stars">
                        <%-- Podrías hacer esto dinámico también, pero lo dejamos estático por ahora --%>
                        <span>★</span><span>★</span><span>★</span><span>★</span><span>☆</span>
                    </div>
                    <%-- DATO DINÁMICO --%>
                    <div class="score-count"><asp:Literal ID="litReviewCount" runat="server" /></div>
                </div>
                
                <div class="summary-bars">
                    <%-- BARRAS DINÁMICAS (REPEATER) --%>
                    <asp:Repeater ID="rpRatingBars" runat="server">
                        <ItemTemplate>
                            <div class="bar-row">
                                <span><%# Eval("Stars") %></span>
                                <div class="bar-container">
                                    <div class="bar-fill" style="width: <%# Eval("Percentage") %>%;">
                                        
                                    </div>
                                </div> 
                                <span><%# Eval("Count") %></span>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <%-- LISTA DE RESEÑAS (REPEATER) --%>
            <div class="review-list">
                <asp:Repeater ID="rpResenas" runat="server">
                    <ItemTemplate>
                        <article class="review-item">
                            <div class="review-header">
                                <img class="avatar" src='<%# Eval("AvatarUrl") %>' alt='<%# Eval("NombreAutor") %>' />
                                <div class="reviewer-info">
                                    <span class="name"><%# Eval("NombreAutor") %></span>
                                    <span class="date"><%# Eval("Fecha") %></span>
                                </div>
                            </div>
                            <div class="stars">
                                <%-- Lógica para mostrar estrellas basadas en Eval("Rating") iría aquí --%>
                                <%-- Por simplicidad, se omite, pero podrías usar otro repeater anidado --%>
                                <span>★</span><span>★</span><span>★</span><span>★</span><span>★</span>
                            </div>
                            <p class="review-body">
                                <%# Eval("Comentario") %>
                            </p>
                            <div class="review-actions">
                                <%-- Los botones de Like/Dislike requerirían CommandName/CommandArgument en un LinkButton --%>
                                <button class="action-like">👍 <span><%# Eval("Likes") %></span></button>
                                <button class="action-dislike">👎 <span><%# Eval("Dislikes") %></span></button>
                            </div>
                        </article>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

            <%-- FORMULARIO DE NUEVA RESEÑA --%>
            <div class="add-review-form">
                <img class="avatar" src="/avatar-placeholder-user.png" alt="Tu avatar">
                <div class="form-content">
                    
                    <asp:TextBox ID="txtNuevaResena" runat="server" 
                        TextMode="MultiLine" 
                        placeholder="Escribe tu reseña..." 
                        CssClass="review-textarea-css" />
                        
                    <div class="form-footer">
                        <div class="rating-input">
                            <%-- Aquí necesitarías un control de rating (ej. de AjaxControlToolkit o JS) --%>
                            <span>☆</span><span>☆</span><span>☆</span><span>☆</span><span>☆</span>
                        </div>
                        
                        <asp:Button ID="btnEnviarResena" runat="server" 
                            Text="Enviar" 
                            CssClass="btn btn-submit" 
                            OnClick="btnEnviarResena_Click" />
                    </div>
                </div>
            </div>

        </section>

    </div>
</asp:Content>