The structure of the TaskStatements folder is as follows:
Business Process Name -> Workflow Name -> (State name/optional) -> Statement.cs
i.e:
(folder) Business Process Name (x2.x2.Process.Name)
|	(folder) WorkFlow Name (x2.x2.WorkFlow.Name)
|	|	(folder-optional) State (x2.x2.State.Name)
|	|	[if there is no statement for the specific state, then the generic statement is used. There should be only 1 statement per sub-folder]
|	|	
|	|	GenericStatement.cs (generic statement for this WorkFlow (i.e. any state within))
|	|GenericStatement.cs (generic statement for any WorkFlow not catered for, but still within this business process (i.e. any workflow within)
|GenericStatement.cs (generic statement for any business process not catered for, i.e. no current business process exists)

see _template for structure

NB: Ensure your columns in the SQL statement are named as you would like them displayed on the UI
e.g. Use [Application Number] instead of ApplicationNumber

The column names [InstanceId], [TagIds] and [OriginationSource] are special column values that will not present themselves on the UI, but will instead be processed into the result object