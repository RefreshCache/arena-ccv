﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PCOLogin.aspx.cs" Inherits="PCOLogin" EnableViewState="false" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<body>
    <form id="frmPCO" runat="server">
    <div>
        <input type="hidden" id="email" name="email" value="<% =_email %>" />
        <input type="hidden" id="password" name="password" value="<% =_password %>" />
    </div>
    </form>
</body>
</html>

<script type="text/javascript" language="javascript">
    document.getElementById('frmPCO').submit();
</script>
