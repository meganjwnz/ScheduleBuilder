<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ScheduleBuilder.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="margin:auto;border:5px solid white">
                <tr>
                    <td>
                        <asp:Label ID="UsernameLabel" runat="server" Text="Username"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="UsernameTextBox" runat="server"></asp:TextBox>
                    </td> 
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="PasswordLabel" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="PasswordTextBox" runat="server"></asp:TextBox>
                    </td> 
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="LoginButton_Click" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="ErrorLabel" runat="server" Text="Incorrect User Credentials" ForeColor="Red"></asp:Label>
                    </td>   
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
