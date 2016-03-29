module.exports = function(applicationName){

	var queries = function(){
		this.getPrefixedIDNumber = "select top 2 id.IDNumber from [sahls03].[2am].test.IDNumbers id left join [sahls03].[2am].dbo.legalEntity le on id.IDNumber = le.IDNumber left join [capitec].dbo.Person p on id.idNumber = p.IdentityNumber where le.legalEntityKey is null and p.id is null and id.idNumber like '[IdPrefix]%'";
		this.getNextIDNumber =  "select top 2 id.IDNumber from [sahls03].[2am].test.IDNumbers id left join [sahls03].[2am].dbo.legalEntity le on id.IDNumber = le.IDNumber left join [capitec].dbo.Person p on id.idNumber = p.IdentityNumber where le.legalEntityKey is null and p.id is null";
		this.getPerson = "select * from dbo.Person where IdentityNumber = '[IdentityNumber]'";
		this.getApplicant = "select top 1 * from dbo.Applicant where PersonID = '[PersonID]' order by ID desc";
		this.getApplication = "select a.*, ap.Name as ApplicationType from dbo.Application a join dbo.ApplicationPurposeEnum ap on a.ApplicationPurposeEnumId = ap.Id where ApplicationNumber = '[ApplicationNumber]'";
		this.getApplicantApplication = "select * from dbo.ApplicationApplicant where ApplicationId = '[ApplicationID]' and ApplicantId = '[ApplicantID]'";
		this.getApplicationLoanDetail = 
		"select ald.id, applicationId, HouseholdIncome, Instalment, InterestRate, LoanAmount, LTV, Fees, et.Name as EmploymentType, " + 
		"ot.Name as OccupancyType from dbo.ApplicationLoanDetail ald " +
		"join EmploymentTypeEnum et on ald.EmploymentTypeID = et.Id join OccupancyTypeEnum ot on ald.OccupancyTypeEnumID = ot.Id " + 
		"where applicationId = '[ApplicationID]'";
		this.getSwitchApplicationLoanDetail = "select * from dbo.SwitchApplicationLoanDetail where id = '[ApplicationLoanDetailID]'";
		this.getApplicantEmployment = "select ae.*, et.Name from dbo.ApplicantEmployment ae join EmploymentTypeEnum et on ae.EmploymentTypeEnumId = et.Id where applicantId = '[ApplicantID]'";
		this.getApplicantAddress = 
		"select BoxNumber, UnitNumber, BuildingNumber, BuildingName, StreetNumber, StreetName, s.SuburbName, c.CityName, p.ProvinceName, co.CountryName " + 
		"from dbo.ApplicantAddress aa " + 
		"inner join dbo.Address a on aa.AddressId = a.Id " + 
		"inner join dbo.AddressTypeEnum at on aa.AddressTypeEnumId = at.Id " + 
		"inner join dbo.AddressFormatEnum af on a.AddressFormatEnumId = af.Id " + 
		"inner join geo.Suburb s on a.SuburbId = s.Id " + 
		"inner join geo.City c on s.CityId = c.Id " + 
		"inner join geo.Province p on c.ProvinceId = p.Id " + 
		"inner join geo.Country co on p.CountryId = co.Id " + 
		"where applicantId = '[ApplicantID]'";
		this.getEmailAddressContactDetail = "select emailAddress from dbo.ApplicantContactDetail acd inner join dbo.ContactDetail cd on acd.ContactDetailId = cd.Id " + 
		"inner join dbo.EmailAddressContactDetail e on cd.Id = e.Id " + 
		"where applicantId = '[ApplicantID]'";
		this.getPhoneContactDetailForApplicant = "select e.* from dbo.ApplicantContactDetail acd " + 
		"inner join dbo.ContactDetail cd on acd.ContactDetailId = cd.Id " + 
		"inner join dbo.PhoneNumberContactDetail e on cd.Id = e.Id " + 
		"inner join dbo.PhoneNumberContactDetailTypeEnum pn on e.PhoneNumberContactDetailTypeEnumId = pn.Id " + 
		"and pn.Name = '[ContactDetailType]' " + 
		"where applicantId = '[ApplicantID]' ";
		this.getApplicantDeclarations = "select DeclarationText, dte.Name as Answer from ApplicantDeclaration ad " +
		"inner join Declaration dd on ad.DeclarationId = dd.ID " +
		"inner join DeclarationDefinition def on dd.DeclarationDefinitionId = def.ID " +
		"inner join DeclarationTypeEnum dte on def.DeclarationTypeEnumId = dte.ID " +
		"where applicantId = '[ApplicantID]'";
		this.getNewPurchaseApplicationLoanDetail = "select * from dbo.NewPurchaseApplicationLoanDetail where id = '[ApplicationLoanDetailID]' ";
		this.getCurrentITCForApplicant = "select i.* from Capitec..Applicant a join Capitec..ApplicantITC i on a.Id = i.Id where a.Id = '[ApplicantID]'";
		this.getExistingIdNumberWithAppNumber = "select top 1 p.IdentityNumber, app.ApplicationNumber from Capitec..Person p " +
		"join Capitec.dbo.Applicant a on p.Id = a.PersonID " +
		"join Capitec.dbo.ApplicationApplicant aa on a.Id = aa.ApplicantId " +
		"join Capitec.dbo.Application app on aa.ApplicationId = app.Id " +
		"where ApplicationStatusEnumId = 'F14B51AE-D633-454B-8F3F-A2EE00F78B7B'";
		this.getApplicationNumberByStageAndStatus = "select top 1 a.ApplicationNumber from application a join dbo.ApplicationStageTypeEnum astg on a.ApplicationStageTypeEnumId = astg.Id " +
		"join ApplicationStatusEnum asta on a.ApplicationStatusEnumId = asta.Id where astg.Name = '[Stage]' and asta.Name = '[Status]' order by 1 desc";
		this.getBranchDetailsByBranchName = "select * from Capitec.security.Branch bra (nolock) " +
		"join Capitec.geo.Suburb sub (nolock) ON sub.Id = bra.SuburbId " +
		"left join Capitec.geo.City cit (nolock) ON cit.Id = sub.CityId " +
		"left join Capitec.geo.Province prov (nolock) ON prov.id = cit.ProvinceId " +
		"where bra.BranchName = '[Name]' ";
		this.getApplicantsForApplication = "select app.Id as ApplicantId, p.* " +
		"from capitec.dbo.Application a " +
		"join capitec.dbo.ApplicationApplicant aa on a.id = aa.applicationId " +
		"join capitec.dbo.Applicant app on aa.ApplicantId = app.Id " +
		"join capitec.dbo.Person p on app.PersonID = p.Id " +
		"where a.ApplicationNumber = '[ApplicationNumber]'";
		this.getPersonDetailsOfApplicantsOnPortalDeclinedApplication = "select top 2 p.* from Capitec.dbo.Application app " +
		"join Capitec.dbo.ApplicationApplicant aa on app.Id = aa.ApplicationId " +
		"join Capitec.dbo.Applicant a on aa.ApplicantId = a.Id " +
		"join Capitec.dbo.Person p on a.PersonID = p.Id " +
		"join Capitec.dbo.PersonITC ip on p.Id = ip.Id " +
		"where app.ApplicationStatusEnumId = 'DC304E67-5792-4F8C-8A25-B87C000DE96C' " +
		"order by NEWID()"
		this.updatePersonITCDate = "update ip set ITCdate = dateadd(dd, [DaysFromToday], GetDate()) " +
		"from Capitec.dbo.Person p " +
		"join Capitec.dbo.PersonITC ip on p.Id = ip.Id " +
		"join Capitec.dbo.ITC i on ip.CurrentITCId = i.Id " +
		"where p.IdentityNumber = '[IDNumber]'; " +
		"update i set ITCdate = dateadd(dd, [DaysFromToday], GetDate()) " +
		"from Capitec.dbo.Person p " +
		"join Capitec.dbo.PersonITC ip on p.Id = ip.Id " +
		"join Capitec.dbo.ITC i on ip.CurrentITCId = i.Id " +
		"where p.IdentityNumber = '[IDNumber]'"
		this.getPersonITCDetailsByIDNumber = "select p.* from Capitec.dbo.Person p " +
		"join Capitec.dbo.PersonITC ip on p.Id = ip.Id " +
		"join Capitec.dbo.ITC i on ip.CurrentITCId = i.Id " +
		"where p.IdentityNumber = '[IDNumber]'"
	};

	applicationName.queries = new queries();
};
