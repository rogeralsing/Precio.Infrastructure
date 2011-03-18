<%@ Page Title="" Language="C#" MasterPageFile="~/CodeBaseBlogMaster.Master" AutoEventWireup="true" CodeBehind="ShowPost.aspx.cs" Inherits="CodeBaseBlog.ShowPost" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="BlogPost">
            <div class="post">
                <div class="heading">
                        <asp:Label ID="PublshDateLabel" runat="server" Text="Label"></asp:Label>
                        <asp:Label ID="SubjectLabel" runat="server" Text="Label"></asp:Label>    
                </div>
                <div class="content">
                    <asp:Label ID="BodyLabel" runat="server" Text="Label"></asp:Label>
                </div>
                <div class="categories">
                    Categories: <asp:Label ID="CategoryLabel" runat="server" Text="Label"></asp:Label>
                </div>
                <div class="heading">Comments</div>
                <div class="comments">
                    <asp:Repeater ID="CommentRepeater" runat="server" EnableViewState=false>
                    <ItemTemplate>
                        <div class="comment">
                            <%#Eval("Body") %><br />
                            <a href="<%#Eval("UserWebSite") %>"><%#Eval("UserName") %></a><br />
                            <hr />
                            <br />
                        </div>
                    </ItemTemplate>
                    </asp:Repeater>

                </div>
                <div class="addComment">
                    <div class="heading">Add a comment</div>
                    <br />
                    Name:<br /><asp:TextBox ID="NameTextbox" runat="server"></asp:TextBox>
                        <br />
                    Email:<br /><asp:TextBox ID="EmailTextbox" runat="server"></asp:TextBox>
                        <br />
                    Website:<br /><asp:TextBox ID="WebsiteTextbox" runat="server"></asp:TextBox>
                        <br />
                    Comment:<br />
                    &nbsp;<asp:TextBox ID="CommentTextbox" runat="server" TextMode="MultiLine"></asp:TextBox>
                        <br />
                        <asp:Button ID="ReplyButton" runat="server" Text="Reply" 
                            onclick="ReplyButton_Click" />
                </div>
            </div>
</div>

</asp:Content>
