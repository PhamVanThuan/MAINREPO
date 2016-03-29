Declare @LatestProcesses table(ID int)
INSERT INTO @LatestProcesses (ID) SELECT MAX(ID) as ID FROM X2.Process GROUP BY NAME ORDER BY NAME

UPDATE [X2].[X2].[State]
SET X2ID = newid()
FROM [X2].[X2].[State] (NOLOCK) 
INNER JOIN [X2].[X2].[Workflow](NOLOCK) ON [X2].[X2].[State].WorkflowID = [X2].[X2].[Workflow].ID
WHERE [X2].[X2].[Workflow].ProcessID IN (SELECT ID FROM @LatestProcesses)

--The devils code

UPDATE nst
SET nst.X2ID = st.X2ID
FROM [x2].[x2].[State] st
INNER JOIN [X2].[X2].[Workflow] wo (NOLOCK) ON st.WorkflowID = wo.ID
INNER JOIN [X2].[X2].[Process] pr (NOLOCK) ON wo.ProcessID = pr.ID
INNER JOIN [X2].[X2].[Process] npr (NOLOCK) ON npr.Name = pr.Name
INNER JOIN [X2].[X2].[Workflow] nwo (NOLOCK) ON nwo.ProcessID = npr.ID and nwo.Name = wo.Name
INNER JOIN [X2].[X2].[State] nst (NOLOCK) ON nst.WorkflowID = nwo.ID and nst.Name = st.Name
WHERE st.X2ID IS NOT NULL

UPDATE [X2].[X2].[Activity]
SET [X2ID] = newid()
FROM [X2].[X2].[Activity] (NOLOCK) 
INNER JOIN [X2].[X2].[Workflow](NOLOCK) ON [X2].[X2].[Activity].WorkflowID = [X2].[X2].[Workflow].ID
WHERE [X2].[X2].[Workflow].ProcessID IN (SELECT ID FROM @LatestProcesses)

--trogdor

UPDATE nst
SET nst.X2ID = st.X2ID
FROM [x2].[x2].[Activity] st
LEFT JOIN [x2].[State] fromState (NOLOCK) ON st.StateID = fromState.ID
LEFT JOIN [x2].[State] toState (NOLOCK) ON st.NextStateID = toState.ID
INNER JOIN [X2].[X2].[Workflow] wo (NOLOCK) ON st.WorkflowID = wo.ID
INNER JOIN [X2].[X2].[Process] pr (NOLOCK) ON wo.ProcessID = pr.ID
INNER JOIN [X2].[X2].[Process] npr (NOLOCK) ON npr.Name = pr.Name
INNER JOIN [X2].[X2].[Workflow] nwo (NOLOCK) ON nwo.ProcessID = npr.ID and nwo.Name = wo.Name
LEFT JOIN [X2].[X2].[Activity] nst (NOLOCK) ON nst.WorkflowID = nwo.ID and nst.Name = st.Name
LEFT JOIN [x2].[State] nfromState (NOLOCK) ON nst.StateID = nfromState.ID
JOIN [x2].[State] ntoState (NOLOCK) ON nst.NextStateID = ntoState.ID
WHERE st.X2ID IS NOT NULL and (nfromState.Name = fromState.Name or ntoState.Name = toState.Name)
