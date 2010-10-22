<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCOResult.aspx.cs" Inherits="PCOResult" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Could not get Access Token from Planning Center Online.
        <br />
        <asp:Literal ID="lError" runat="server"></asp:Literal>
        <br />
        <a id="aRetry" runat="server">Try Again...</a>
    </div>
    </form>
</body>
</html>
