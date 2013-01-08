<%@ Page Language="C#" MasterPageFile="~/squrl.master" AutoEventWireup="true" 
CodeFile="Default.aspx.cs" Inherits="_Default" Title="squrl - Short Quick Urls" 
EnableViewState="false"
%>

<asp:Content ID="headContent" ContentPlaceHolderID="head" runat="Server">
  <style type="text/css">
    .newStyle1
    {
      vertical-align: middle;
    }
    .newStyle2
    {
      vertical-align: middle;
    }
  </style>
</asp:Content>
<asp:Content ID="bodyContent" ContentPlaceHolderID="body" runat="Server">
	<img alt="squrl" src="images/squrl.gif" style="width: 300px;height: 225px" /><img src="images/squrl_logo.png" width="366" height="88" alt="squrl.us" /><br />
	<br />
	Create <span title="Short, Quick, Url">squrl</span>:
	<asp:TextBox ID="uxQuery" runat="server" Width="400px" CssClass="inputText" />
	<asp:Button ID="uxScurry" runat="server" Text="Scurry" CssClass="inputButton" OnClick="uxScurry_Click" />
	<div id="results">
		<asp:Literal ID="uxSqurlResult" runat="server" />
		<small title="Drag this link to your bookmark/link toolbar to easily create a squrl for any page">
			<a href="javascript:location.href='http://squrl.us/?b=1&q='+escape(location.href);">squrl this!</a> (bookmarklet)
		</small>
	</div>
</asp:Content>
