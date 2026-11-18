<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComentariosProducto.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMAD.Web.ComentariosProducto" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvComentarios" runat="server">
            </asp:GridView>
            <asp:Label ID="lblEstado" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
