<%@ Page Language="C#" MasterPageFile="~/squrl.master" AutoEventWireup="true" CodeFile="scurry.aspx.cs" Inherits="Scurry" Title="Untitled Page" %>

<asp:Content ID="headContent" ContentPlaceHolderID="head" Runat="Server" />

<asp:Content ID="bodyContent" ContentPlaceHolderID="body" Runat="Server">
	<div id="results">
		<asp:Literal ID="uxSqurlResult" runat="server"></asp:Literal>
	</div>
</asp:Content>

