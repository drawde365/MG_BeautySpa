<%@ Page Title="Resultados" Language="C#" MasterPageFile="~/Cliente/Cliente.Master"
    AutoEventWireup="true" CodeBehind="Resultados.aspx.cs"
    Inherits="MGBeautySpaWebAplication.Cliente.Resultados" %>

<asp:Content ID="ct2" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="ResultadosClienteStyle.css" />
    <style>
        /* Estilos básicos para la nueva etiqueta (puedes mover esto a tu CSS) */
        .product-type-label {
            font-size: 0.75rem;
            font-weight: bold;
            padding: 2px 6px;
            border-radius: 4px;
            margin-bottom: 8px;
            display: inline-block;
            text-transform: uppercase;
        }

        /* Diferenciar por tipo */
        .product-card[data-tipo='Producto'] .product-type-label {
            background-color: #e0f0ff;
            color: #00529b;
        }
        
        .product-card[data-tipo='Servicio'] .product-type-label {
            background-color: #dff0d8;
            color: #3c763d;
        }
    </style>

    <div class="results-header">
        <h1 class="results-title">Resultados para "<asp:Label ID="litQuery" runat="server" />"
            <small class="text-muted"><asp:Label ID="litCount" runat="server" /></small>
        </h1>
    </div>

    <%-- 
        CAMBIO: Usamos "Style" en lugar de "Visible" para que JS pueda controlarlo.
    --%>
    <asp:Panel runat="server" ID="pnlNoResults" CssClass="alert alert-light border" Style="display: none;">
        No hay resultados para "<asp:Literal ID="Literal1" runat="server" />". Intenta con otra palabra (p. ej., <em>crema</em>).
    </asp:Panel>

    <div class="products-grid-container">
        <div class="products-grid-row">

            <asp:Repeater ID="rptProductos" runat="server">
                <ItemTemplate>
                    <%-- 
                        CAMBIOS CLAVE:
                        1. El 'href' ahora usa Eval("urlDestino")
                        2. El 'data-tipo' usa Eval("tipo") (en minúscula)
                    --%>
                    <a href='<%# Eval("urlDestino") %>' 
                       class="product-card" 
                       data-tipo='<%# Eval("tipo") %>'>
            
                        <div class="product-image-container">
                            <img src='<%# Eval("urlImagen") %>' alt='<%# Eval("nombre") %>' style="width:100%;height:100%;object-fit:cover;" />
                        </div>

                        <div class="product-details">
                            <%-- La etiqueta también usa "tipo" en minúscula --%>
                            <span class="product-type-label"><%# Eval("tipo") %></span>
                
                            <h2 class="product-name"><%# Eval("nombre") %></h2>
                            <p class="product-description">S/ <%# String.Format("{0:0.00}", Eval("precio")) %></p>
                        </div>
                    </a>
                </ItemTemplate>
    
                <SeparatorTemplate>
                    <%-- Este Template se puede dejar vacío --%>
                </SeparatorTemplate>
            </asp:Repeater>
        </div>
    </div>

    <%-- ¡SCRIPT AÑADIDO! --%>
    <script>
        document.addEventListener('DOMContentLoaded', function () {
            // 1. Obtener los elementos del DOM
            // Usamos ClientID para que ASP.NET nos dé el ID correcto en el HTML
            const queryLiteral = document.getElementById('<%= litQuery.ClientID %>');
            const countLiteral = document.getElementById('<%= litCount.ClientID %>');
            const noResultsPanel = document.getElementById('<%= pnlNoResults.ClientID %>');
            
            // 2. Obtener la consulta y todos los items
            const searchQuery = queryLiteral.textContent.toLowerCase().trim();
            const allItems = document.querySelectorAll('.product-card');
            
            let visibleCount = 0;

            // 3. Recorrer cada item y filtrar
            allItems.forEach(function(item) {
                const itemName = item.querySelector('.product-name').textContent.toLowerCase().trim();
                
                // Comprobamos si el nombre incluye el texto buscado
                if (itemName.includes(searchQuery)) {
                    item.style.display = 'block'; // O 'flex', dependiendo de tu CSS
                    visibleCount++;
                } else {
                    item.style.display = 'none'; // Ocultamos el que no coincide
                }
            });

            // 4. Actualizar el contador de resultados
            countLiteral.textContent = visibleCount + ' resultado(s)';

            // 5. Mostrar el panel de "no resultados" si es necesario
            if (visibleCount === 0) {
                noResultsPanel.style.display = 'block';
            }
        });
    </script>

</asp:Content>