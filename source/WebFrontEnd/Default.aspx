<%@ Page Title="Skyline Operator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Fiti.WebFrontEnd.Default" %>

<asp:Content ContentPlaceHolderID="includes" runat="server">
    <script type="text/javascript" src="default.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Skyline Operator Demo</h1>
        <p class="lead">A demonstration of the Skyline Operator shown with the AutoScout24 database.</p>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h2>Database Size</h2>
            <div class="btn-group">
                <asp:Repeater runat="server" ID="QueryTargetRepeater" EnableViewState="False">
                    <ItemTemplate>
                        <button
                            class="btn btn-default <%# DataBinder.Eval(Container.DataItem, "classes") %>"
                            data-set="<%# DataBinder.Eval(Container.DataItem, "set") %>"
                            value="<%# DataBinder.Eval(Container.DataItem, "value") %>">
                            <%# DataBinder.Eval(Container.DataItem, "label") %>
                            <small><%# DataBinder.Eval(Container.DataItem, "description") %></small></button>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-12">
            <h2>Preferences</h2>
            <div class="row preferences">
                <asp:Repeater runat="server" ID="PreferenceSetsRepeater" EnableViewState="False">
                    <ItemTemplate>
                        <div class="col-md-<%# DataBinder.Eval(Container.DataItem, "cols") %>">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><%# DataBinder.Eval(Container.DataItem, "label") %></h3>
                                </div>
                                <div class="panel-body">
                                    <div class="btn-group-vertical">
                                        <asp:Repeater runat="server" ID="PreferenceOptionsRepeater" EnableViewState="False">
                                            <ItemTemplate>
                                                <button
                                                    class="btn btn-default <%# DataBinder.Eval(Container.DataItem, "classes") %>"
                                                    data-set="<%# DataBinder.Eval(Container.DataItem, "set") %>"
                                                    value="<%# DataBinder.Eval(Container.DataItem, "value") %>">
                                                    <%# DataBinder.Eval(Container.DataItem, "label") %></button>
                                            </ItemTemplate>
                                        </asp:Repeater>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>

                </asp:Repeater>
            </div>
        </div>
    </div>
    <div class="row well">
        <div class="col-md-12">
            <h2>Results</h2>
            <p><small>Query Execution Time: <strong><%=  QueryExecutionTime.HasValue? string.Format("{0} ms", QueryExecutionTime.Value.TotalMilliseconds) : string.Empty %></strong></small></p>
            <p>
                <asp:GridView ID="ResultsGrid" runat="server" AutoGenerateColumns="False" CssClass="table table-striped">

                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" />
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <img class="car-view" src='<%# ImageView(((System.Data.DataRowView) Container.DataItem).Row)%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:TemplateField HeaderText="Price">
                            <ItemTemplate>
                                <%# Eval("Price", "{0:c}") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Color" HeaderText="Color" />
                        <asp:BoundField DataField="Body" HeaderText="Body" />
                    </Columns>
                </asp:GridView>
                <asp:Label ID="MessageLabel" runat="server" Font-Bold="True" ForeColor="#FF3300" Visible="False"></asp:Label>
            </p>
        </div>
    </div>
    <asp:Panel runat="server" ID="DebugInfo">
        <div class="row">
            <div class="col-md-12">
                <p>
                    <label>Preference Query:</label>
                    <asp:TextBox ID="PreferenceQueryBox" runat="server" TextMode="MultiLine" ReadOnly="True" Rows="3" CssClass="log" />
                </p>
                <p>
                    <label>SQL Query:</label>
                    <asp:TextBox ID="SqlQueryBox" runat="server" TextMode="MultiLine" ReadOnly="True" CssClass="log" Rows="10" />
                </p>
            </div>
        </div>
    </asp:Panel>

</asp:Content>
