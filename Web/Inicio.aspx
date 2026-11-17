<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Inicio.aspx.cs" Inherits="Es.Udc.DotNet.PracticaMAD.Web.WebForm1" %>

<!DOCTYPE html><html xmlns="http://www.w3.org/1999/xhtml"><head runat="server"><meta http-equiv="Content-Type" content="text/html; charset=utf-8"/><title></title></head><body><form id="form1" runat="server">
        <div>
            <asp:Label ID="lblTitulo" runat="server" Text="MaD Marketplace"></asp:Label>
        </div>
        <asp:Button ID="btnPresentacion" runat="server" OnClick="Button1_Click" Text="Conocer la aplicación" />
        <br />
        <asp:Label ID="lblPresentacion" runat="server"></asp:Label>
    </form>
</body>
</html>
