update X2.Instance set ReturnActivityID = ANEW.ID
--from
--select AOLD.WorkflowID, WOLD.Name WorkflowName, AOLD.Name ActivityName, 
--AOLD.ID ReturnActivityID
--, WNEW.Name NewWorkflowName, WNEW.ID NewWorkflowID
--, ANew.ID NewActivityID, ANEW.Name NewActivityName
from
X2.Instance I
join x2.activity AOLD on I.ReturnActivityID=AOLD.ID
join x2.workflow WOLD on AOLD.WorkflowID=WOLD.ID
join x2.workflow WNew on WOLD.Name=WNew.NAme and WNew.ID > WOLD.ID
join 
(
	select max(id) ID from x2.workflow w1 group by w1.Name
) ARB on WNew.ID=ARB.ID
join x2.activity ANew on ANEW.Name=AOLD.Name and ANew.WorkflowID=WNEW.ID