use [2am]

if not exists (select 1 from [2am].dbo.category where categorykey=12)
	insert into [2am].dbo.category
		select 12,12,'Category 12' 

if not exists (select 1 from [2am].dbo.category where categorykey=13)
	insert into [2am].dbo.category
		select 13,13,'Category 13'

if not exists (select 1 from [2am].dbo.category where categorykey=14)
	insert into [2am].dbo.category
		select 14,14,'Category 14'

if not exists (select 1 from [2am].dbo.category where categorykey=15)
	insert into [2am].dbo.category
		select 15,15,'Category 15'

if not exists (select 1 from [2am].dbo.category where categorykey=16)
	insert into [2am].dbo.category
		select 16,16,'Category 16'

if not exists (select 1 from [2am].dbo.category where categorykey=17)
	insert into [2am].dbo.category
		select 17,17,'Category 17'

if not exists (select 1 from [2am].dbo.category where categorykey=18)
	insert into [2am].dbo.category
		select 18,18,'Category 18'

if not exists (select 1 from [2am].dbo.category where categorykey=19)
	insert into [2am].dbo.category
		select 19,19,'Category 19'

if not exists (select 1 from [2am].dbo.category where categorykey=20)
	insert into [2am].dbo.category
		select 20,20,'Category 20'

if not exists (select 1 from [2am].dbo.category where categorykey=21)
	insert into [2am].dbo.category
		select 21,21,'Category 21'

if not exists (select 1 from [2am].dbo.category where categorykey=22)
	insert into [2am].dbo.category
		select 22,22,'Category 22' 