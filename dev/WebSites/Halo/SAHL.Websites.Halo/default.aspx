<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="SAHL.Websites.Halo._default" %>
<!DOCTYPE html>
<html lang="en" ng-app="halo.core" ng-controller="AppCtrl" ng-hint ng-strict-di>
<head>
    <base href="/halov3/">
    <title>Halo V3</title>
    <meta charset="utf-8"/>
    <link rel="stylesheet" href="css/metroui/metro-bootstrap.min.css" />
    <link rel="stylesheet" href="css/metroui/metro-bootstrap-responsive.min.css" />
    <link rel="stylesheet" href="css/angular-datepicker/angular-datepicker.css"  />
    <link rel="stylesheet" href="css/metroui/iconFont.min.css" />
    <link rel="stylesheet" href="css/angular-ui-grid/ui-grid-unstable.min.css"/>
    <link rel="stylesheet" href="css/angular-ui-grid/ui-grid-theme.css"/>
    <link rel="stylesheet" href="css/pnotify/pnotify.custom.min.css"/>
    <link rel="stylesheet" href="css/font-awesome/font-awesome.min.css"/>
    <link rel="stylesheet" href="css/flag-icon/flag-icon.min.css" />
    <link rel="stylesheet" href="less/app.css" />
    <link rel="stylesheet" href="css/font-awesome/font-awesome.min.css"/>
    <link href="assets/img/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    
    <script type="text/javascript">
    <%if (IsDebug){%>
        window.debug = true;
    <%}%>
        var authenticatedUser = {
            <%if (this.UserInfo != null) {%>                      
            fullAdName: "<%=MakeSafe(Page.User.Identity.Name) %>",
            domain: "<%=this.UserInfo.Domain %>",
            adName: "<%=this.UserInfo.UserName %>",
            displayName: '<%=this.UserInfo.DisplayName%>',
            emailAddress: '<%=this.UserInfo.EmailAddress%>',
            currentOrgRole: <%=this.UserRoles.FirstOrDefault()%>,
            orgRoles: [<%=this.UserRoles.Aggregate((f, l) => f + "," + l)%>],
            capabilities: [<%=this.UserCapabilities%>],
            state: '<%= this.UserInfo.Roles.Count() > 0 ? "ValidUser" : "NoOrgRole" %>'           
        <%} else if (this.Exception != null) {%>
            state: 'NoAccess',
            exception : {
            message:'<%=this.Exception.Message%>',
                stackTrace:'<%=this.Exception.StackTrace.Replace("\r\n","\\r\\n")%>',
            source:'<%=this.Exception.Source%>'
        }
        <% }else {%>
        state: 'NoAccess'
        <%}%>
        };
    </script>
</head>

<body class="metro">
    <div class="bodyContent" ui-view=""></div>

    <script src="lib/jquery/jquery-2.1.1.min.js"></script>
    <script src="lib/jqueryui/jquery-ui-1.10.4.min.js"></script>
    
    <script src="lib/pnotify/pnotify.custom.min.js"></script>
    <script src="lib/metroui/metro.min.js"></script>
    <script src="lib/angular/angular.min.js"></script>
    <script src="lib/angular-animate/angular-animate.min.js"></script>
    <script src="lib/angular-ui-router/angular-ui-router.min.js"></script>
    <script src="lib/angular-ui-grid/ui-grid-unstable.min.js"></script>
    <script src="lib/moment/moment.min.js"></script>
    <script src="lib/angular-moment/angular-moment.min.js"></script>
    <script src="lib/jquery-throttle/jquery-throttle.min.js"></script>
    <script src="lib/angular-truncate/angular-truncate.min.js"></script>
    <script src="lib/raphaeljs/raphael-min.js"></script>
    <script src="lib/angular-elastic/elastic.js"></script>
    
    <script src="lib/angular-datepicker/angular-datepicker.js"></script>
    <script src="lib/angular-datepicker/angular-datepicker.min.js"></script>
    <script src="lib/angular-datepicker/index.js"></script>

    <script src="lib/underscore/underscore-min.js"></script>

    <script src="lib/jquery-signalr/jquery.signalR-2.1.0.min.js"></script>
    <script src="lib/angular-signalr/signalr-hub.min.js"></script>

    <script src="lib/spin/spin.min.js"></script>
    <script src="lib/sahl.js.core/SAHL.JS.Core.min.js"></script>
    <script src="lib/sahl.js.ui/SAHL.JS.UI.min.js"></script>
    <script src="lib/sahl.services.interfaces.userprofile/SAHL.Services.Interfaces.UserProfile.min.js"></script>
    <script src="lib/sahl.services.interfaces.x2service/sahl.services.interfaces.x2service.min.js"></script>
    <script src="lib/sahl.services.interfaces.halo/SAHL.Services.Interfaces.Halo.min.js"></script>
    <script src="lib/SAHL.Services.Interfaces.Logging/SAHL.Services.Interfaces.Logging.min.js"></script>
    <script src="lib/SAHL.Services.Interfaces.Search/SAHL.Services.Interfaces.Search.min.js"></script>
    <script src="lib/sahl.services.interfaces.workflowtask/sahl.services.interfaces.workflowtask.min.js"></script>
    <script src="lib/sahl.services.interfaces.jsonDocument/sahl.services.interfaces.jsonDocument.min.js"></script>
    <script src="lib/sahl.services.interfaces.workflowassignmentdomain/sahl.services.interfaces.workflowassignmentdomain.min.js"></script>
    <script src="lib/sahl.services.interfaces.query/sahl.services.interfaces.query.min.js"></script>
    <script src="lib/sahl.js.workflow/sahl.js.workflow.min.js"></script>

    <script src="lib/sahl.services.interfaces.documentmanager/sahl.services.interfaces.documentmanager.min.js"></script>
    <script src="lib/sahl.services.interfaces.domainprocessmanagerproxy/sahl.services.interfaces.domainprocessmanagerproxy.min.js"></script>
    <script src="lib/sahl.services.interfaces.financedomain/sahl.services.interfaces.financedomain.min.js"></script>
    <script src="lib/sahl.services.interfaces.cats/sahl.services.interfaces.cats.min.js"></script>
    <!-- views -->
    <script src="lib/sahl.ui.halo.views/sahl.ui.halo.views.min.js"></script>
    <script src="lib/sahl.ui.halo.myhalo.views/sahl.ui.halo.myhalo.views.min.js"></script>

    <script src="lib/gojs/go.js"></script>

    <script src="config/app.config.js"></script>
    <script src="app/sahl.webservices.config.js"></script>

    <!-- Events -->
    <script src="events/entityManagementEvents.js"></script>
    <script src="events/tileEvents.js"></script>
    <script src="events/events.js"></script>

    <!-- Services -->
    <script src="services/entityManager.js"></script>
    <script src="services/domainProcessManagerModels.js"></script>
    <script src="services/entityViewManager.js"></script>
    <script src="services/entityBreadcrumbManager.js"></script>
    <script src="services/searchManager.js"></script>
    <script src="services/haloservice.js"></script>
    <script src="services/paginationHelper.js"></script>
    <script src="services/tagService.js"></script>
    <script src="services/tileEditManager.js"></script>
    <script src="services/workflowTaskService.js"></script>
    <script src="services/lookupDataService.js"></script>
    <script src="services/transactionQueryService.js"></script>
    <script src="services/documentServiceDecoration.js"></script>
    <script src="services/treasuryManager.js"></script>
    <!-- Core Application modules -->
    <script src="services/workflowEngineManager.js"></script>
    <script src="services/wipBarService.js"></script>
        <!-- Core Application modules -->
    <script src="services/thirdPartyInvoiceManager.js"></script>
    <script src="services/accountsManager.js"></script>
    <script src="services/documentDownloadManager.js"></script>
    <script src="services/attorneysManager.js"></script>
    <script src="services/thirdPartyManager.js"></script>
    <script src="services/workflowAssignmentManager.js"></script>
    <script src="services/acronymManagerService.js"></script>

    <!-- Core Application modules -->
    <script src="app/error/error.js"></script>
    <script src="app/app.startables.js"></script>
    <script src="app/start/start.js"></script>
    <script src="app/start/portalPages/portalpages.js"></script>
    <script src="app/start/portalNavBar/portalNavBar.js"></script>
    <script src="services/entityActionBarServices.js"></script>
    <script src="app/start/entityActionBar/entityActionBar.js"></script>
    <script src="app/start/portalPages/settings/settings.js"></script>

    <!-- Portal Home modules -->
    <script src="app/start/portalPages/entity/entity.js"></script>
    <script src="app/start/portalPages/entity/common/common.js"></script>
    <script src="app/start/portalPages/entity/common/edit/edit.js"></script>
    <script src="app/start/portalPages/entity/common/wizard/wizard.js"></script>
    <script src="app/start/portalPages/entity/common/page/page.js"></script>
    <script src="app/start/portalPages/search/search.js"></script>

    <!-- Portal MyHalo modules -->
    <script src="app/start/portalPages/myhalo/myhalo.js"></script>
    <script src="app/start/portalPages/tasks/tasks.js"></script>
    <script src="app/start/portalPages/tasks/myTasks/myTasks.js"></script>
    <script src="app/start/portalPages/tasks/mytags/mytags.js"></script>
    <script src="app/start/portalPages/tasks/mytags/myTagManager.js"></script>
    
    <script src="app/start/charms/userCharms/userCharms.js"></script>

    <script src="app/start/portalPages/wipbar/wipbar.js"></script>

    <script src="apps/sahl.halo.app.variables/lib/sahl.halo.app.variables.min.js"></script>
    <script src="apps/sahl.halo.app.organisationstructure/lib/sahl.halo.app.organisationstructure.min.js"></script>
    <script src="apps/sahl.halo.app.domaindocumentation/lib/sahl.halo.app.domaindocumentation.min.js"></script>

    <script src="charms/sahl.halo.charm.mail/lib/sahl.halo.charm.mail.min.js"></script>

    <!-- app modules -->
    
    <script src="app/app.logging.js"></script>
    <script src="app/app.userProfile.js"></script>
    <script src="app/app.lookup.js"></script>
    <script src="app/app.js"></script>
</body>
</html>
