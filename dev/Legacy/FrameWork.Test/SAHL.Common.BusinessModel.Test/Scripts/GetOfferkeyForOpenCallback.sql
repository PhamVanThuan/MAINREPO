select top 1 o.offerkey from 
	offer o 
inner join
	callback cb 
on
	cb.GenericKey=o.offerKey
where cb.CompletedDate is null