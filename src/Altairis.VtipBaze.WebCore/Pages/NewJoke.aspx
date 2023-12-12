<%@ Page Title="Add your joke" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="NewJoke.aspx.cs" Inherits="Altairis.VtipBaze.WebCore.Pages.NewJoke" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainCPH" runat="server">
    <article>
        <header>
            <asp:Localize runat="server" Text="Add your joke" />
        </header>
        <asp:MultiView ID="MultiViewPage" ActiveViewIndex="0" runat="server">
            <asp:View ID="ViewForm" runat="server">
                <p>
                    Here you can submit your own joke. It will appear on the site after it's approved by the editor.
                </p>
                <p>
                    <asp:TextBox ID="TextTextBox" runat="server" TextMode="MultiLine" Width="100%" Height="200px" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="TextTextBox" Display="None" ErrorMessage="Empty text is not very funny" />
                </p>
                <asp:ValidationSummary runat="server" />
                <p>
                    <asp:Button ID="ButtonSubmit" runat="server" Text="Submit" OnClick="ButtonSubmit_Click" />
                </p>
            </asp:View>
            <asp:View ID="ViewResult" runat="server">
                <p>
                    Your joke was added to the database. It will be published after it's approved by the editor. Thank you.
                </p>
            </asp:View>
        </asp:MultiView>
    </article>
</asp:Content>
