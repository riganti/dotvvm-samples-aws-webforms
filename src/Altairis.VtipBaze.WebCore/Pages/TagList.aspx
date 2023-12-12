<%@ Page Title="Categories" Language="C#" MasterPageFile="~/Pages/Site.Master" AutoEventWireup="true" CodeBehind="TagList.aspx.cs" Inherits="Altairis.VtipBaze.WebCore.Pages.TagList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainCPH" runat="server">
    <asp:ListView runat="server" ItemType="Altairis.VtipBaze.Data.Tag" SelectMethod="SelectTags">
        <LayoutTemplate>
            <article>
                <header>
                    List of categories
                </header>
                <ul class="taglist">
                    <asp:PlaceHolder ID="ItemPlaceHolder" runat="server" />
                </ul>
            </article>
        </LayoutTemplate>
        <ItemTemplate>
            <li><span class="ui-icon ui-icon-tag"></span>
                <asp:HyperLink runat="server" Text="<%# Item.TagName %>" NavigateUrl='<%# this.GetRouteUrl("TagSearch", new { TagName = Item.TagName }) %>' />
                <%# string.Format("({0})",  Item.Jokes.Count) %>
            </li>
        </ItemTemplate>
    </asp:ListView>
</asp:Content>
