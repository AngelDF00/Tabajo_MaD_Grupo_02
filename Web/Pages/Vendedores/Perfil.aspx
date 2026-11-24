<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Perfil.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMAD.Web.Pages.Vendedores.Perfil" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <title>Perfil del vendedor</title>
</head>
<body>
    <form id="form1" runat="server">
        <main>
            <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" />

            <asp:Panel ID="pnlPerfil" runat="server">
                <h1>
                    Perfil de <asp:Label ID="lblLoginVendedor" runat="server" />
                </h1>

                <section>
                    <h2>Resumen de valoraciones</h2>
                    <p>
                        <strong>Total de valoraciones:</strong>
                        <asp:Label ID="lblTotalValoraciones" runat="server" />
                    </p>
                    <p>
                        <strong>Media:</strong>
                        <asp:Label ID="lblMediaValoraciones" runat="server" />
                    </p>
                    <p>
                        <asp:HyperLink ID="lnkValorarProducto" runat="server" Visible="false">
                            Valorar este vendedor
                        </asp:HyperLink>
                    </p>
                </section>

                <section id="valoraciones">
                    <h2>Valoraciones recibidas</h2>

                    <asp:Label ID="lblSinValoraciones" runat="server" Visible="false"
                        Text="Este vendedor todavía no tiene valoraciones." />

                    <asp:Repeater ID="rptValoraciones" runat="server">
                        <HeaderTemplate>
                            <ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li>
                                <div>
                                    <strong>Autor:</strong> <%# Eval("Autor.Login") %> |
                                    <strong>Voto:</strong> <%# Eval("Voto") %> / 5 |
                                    <strong>Fecha:</strong> <%# Eval("FechaCreacion", "{0:dd/MM/yyyy HH:mm}") %>
                                </div>
                                <p>
                                    <%# Eval("Comentario") %>
                                </p>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate>
                            </ul>
                        </FooterTemplate>
                    </asp:Repeater>

                    <div>
                        <asp:HyperLink ID="lnkPaginaAnterior" runat="server" Visible="false">
                            &laquo; Anterior
                        </asp:HyperLink>
                        &nbsp;
                        <asp:HyperLink ID="lnkPaginaSiguiente" runat="server" Visible="false">
                            Siguiente &raquo;
                        </asp:HyperLink>
                    </div>
                </section>
            </asp:Panel>
        </main>
    </form>
</body>
</html>
