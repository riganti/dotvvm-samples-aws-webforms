<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Altairis.VtipBaze.WebCore.Pages.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainCPH" runat="server">
    <article>
        <header>
            Sign In
        </header>
        <asp:Login runat="server" RenderOuterTable="false" FailureText="<p><b>Error:</b> Sign in failed.</p>">
            <LayoutTemplate>
                <asp:Literal ID="FailureText" runat="server" />
                <div class="form">
                    <p>
                        <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="User name" />
                        <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="No user name provided" ValidationGroup="login" Display="None" />
                        <br />
                        <asp:TextBox ID="UserName" runat="server" TextMode="SingleLine" Width="100%" />
                    </p>
                    <p>
                        <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password" />
                        <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="No password provided" ValidationGroup="login" Display="None" />
                        <br />
                        <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="100%" />
                    </p>
                    <p>
                        <asp:CheckBox ID="RememberMe" runat="server" Text="remember me at this machine" Checked="true" />
                    </p>
                    <asp:ValidationSummary runat="server" ValidationGroup="login" />
                    <p class="buttons">
                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Sign In" ValidationGroup="login" />
                    </p>
                </div>
            </LayoutTemplate>
        </asp:Login>
    </article>
</asp:Content>
