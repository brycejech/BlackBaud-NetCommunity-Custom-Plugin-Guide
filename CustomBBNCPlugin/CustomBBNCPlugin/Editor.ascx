<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="CustomBBNCPlugin.Editor" %>
<%@ Register TagPrefix="bbnc" Namespace="BBNCExtensions.ServerControls" Assembly="BBNCExtensions" %>

<p>
    <asp:Label ID="lbl_message" Text="Message" runat="server"></asp:Label><br />
    <asp:TextBox ID="ctrl_message" runat="server"></asp:TextBox>
</p>
