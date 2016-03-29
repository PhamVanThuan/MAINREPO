<%

Const DOMAIN =  "SAHLNET"			' The domain under which the site falls.

Const LIGHTHOUSE_DB = "SAHLDB"		' The database that lighthouse must access.

Const DB_SERVER = "SAHLS03"			' The database server that lighthouse must use.

Const DB_USERID = "crystal"			' The database userID for non-user related operations.

Const DB_EWORK_USERID = "crystal"	' The ework database userID.

Const DB_EWORK = "e-work"           ' The ework database server that lighthouse must use.

Const FF_ShowYN = "Y"				' Display flexi fix product

const NETWORK_DOMAIN = "SAHL"		' RSM: 2006-02-03: Display the network domain name for logon

Dim MANAGE_LOAN_TRANSACTION_PLUGIN
MANAGE_LOAN_TRANSACTION_PLUGIN = "http://" & domain & "/base/plugins/accountmanagement/mortgageloan/ManageTransactions.aspx?param1=N&param0="

Const INTEREST_ONLY_MIN = 250000
Const INTEREST_ONLY_MAX = 3000000
Const INTEREST_ONLY_LTV = 91

%>