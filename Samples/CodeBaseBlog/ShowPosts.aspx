<%@ Page Title="" Language="C#" MasterPageFile="~/CodeBaseBlogMaster.Master" AutoEventWireup="true"
    CodeBehind="ShowPosts.aspx.cs" Inherits="CodeBaseBlog.ShowPosts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Repeater ID="PostRepeater" runat="server" EnableViewState=false>
        <HeaderTemplate>
            <div id="Posts">
        </HeaderTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
        <ItemTemplate>
            <div class="post">
                <div class="heading">
                    <a href ="ShowPost.aspx?postId=<%#Eval("Id") %>">
                        <%#Eval("PublishDateString") %>
                    
                        <%#Eval("Subject") %>
                    </a> 
                </div>
                <div class="content">
                    <%#Eval("Body") %>
                </div>
                <div class="categories">
                    Categories: <%#Eval("CategoryString") %>
                </div>
                <div class="comments">
                    Comments: <%#Eval("CommentCount") %>
                </div>
            </div>
            
            </ItemTemplate>
    </asp:Repeater> 
</asp:Content>
