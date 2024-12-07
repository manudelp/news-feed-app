<%@ Page Language="C#" Async="true" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="news_feed_app._default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>News Feed</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="container mt-4">
        <img class="bbc" src="Assets/bbc_logo.svg" alt="BBC Logo" />
        <h1>Latest International News</h1>
        <div runat="server" id="newsFeed"></div>
    </form>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-3.7.0.min.js"></script>
    <script src="Scripts/modernizr-2.8.3.js"></script>
</body>
</html>
