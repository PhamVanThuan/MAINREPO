'use strict';

angular.module('SAHL.Services.Interfaces.Capitec.commands', []).
	factory('$capitecCommands', [function () {
		var shared = (function () {
						function AddNewUserCommand(username, emailAddress, firstName, lastName, rolesToAssign, branchId) {
				this.username = username || '';
				this.emailAddress = emailAddress || '';
				this.firstName = firstName || '';
				this.lastName = lastName || '';
				this.rolesToAssign = rolesToAssign || '';
				this.branchId = branchId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.AddNewUserCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.username!==0 && !this.username) {
							results.push({'username': 'Username is required'});
						}
					if(this.emailAddress!==0 && !this.emailAddress) {
							results.push({'emailAddress': 'Email Address is required'});
						}
					if(this.firstName!==0 && !this.firstName) {
							results.push({'firstName': 'First Name is required'});
						}
					if(this.lastName!==0 && !this.lastName) {
							results.push({'lastName': 'Last Name is required'});
						}
					if(this.rolesToAssign!==0 && !this.rolesToAssign) {
							results.push({'rolesToAssign': 'Roles To Assign is required'});
						}
					if(this.branchId!==0 && !this.branchId) {
							results.push({'branchId': 'Branch Id is required'});
						}
					return results;
			};
		}
			function ApplyForNewPurchaseCommand(applicationID, newPurchaseApplication) {
				this.applicationID = applicationID;
				this.newPurchaseApplication = newPurchaseApplication;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ApplyForNewPurchaseCommand,SAHL.Services.Interfaces.Capitec';
				}
			function ApplyForSwitchLoanCommand(applicationID, switchLoanApplication) {
				this.applicationID = applicationID;
				this.switchLoanApplication = switchLoanApplication;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ApplyForSwitchLoanCommand,SAHL.Services.Interfaces.Capitec';
				}
			function AutoLoginCommand(cp, branchCode, password) {
				this.cp = cp || '';
				this.branchCode = branchCode || '';
				this.password = password || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.AutoLoginCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.cP!==0 && !this.cP) {
							results.push({'cP': 'C P is required'});
						}
					if(this.branchCode!==0 && !this.branchCode) {
							results.push({'branchCode': 'Branch Code is required'});
						}
					if(this.password!==0 && !this.password) {
							results.push({'password': 'Password is required'});
						}
					return results;
			};
		}
			function ChangePasswordCommand(password, passwordconfirm) {
				this.password = password || '';
				this.passwordconfirm = passwordconfirm || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ChangePasswordCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.password!==0 && !this.password) {
							results.push({'password': 'Please provide a new password'});
						}
					if(this.passwordconfirm!==0 && !this.passwordconfirm) {
							results.push({'passwordconfirm': 'Please confirm your new password'});
						}
					if(this.password && this.password.length < 0) {
							results.push({'password': 'Password cannot be empty'});
						}
					if(this.passwordconfirm && this.passwordconfirm.length < 0) {
							results.push({'passwordconfirm': 'Confirm cannot be empty'});
						}
					return results;
			};
		}
			function AddNewBranchCommand(branchName, isActive, suburbId, branchCode) {
				this.branchName = branchName || '';
				this.isActive = isActive;
				this.suburbId = suburbId;
				this.branchCode = branchCode || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.AddNewBranchCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.branchName!==0 && !this.branchName) {
							results.push({'branchName': 'Branch Name is required'});
						}
					if(this.isActive!==0 && !this.isActive) {
							results.push({'isActive': 'Is Active is required'});
						}
					if(this.suburbId!==0 && !this.suburbId) {
							results.push({'suburbId': 'Suburb Id is required'});
						}
					if(this.branchCode!==0 && !this.branchCode) {
							results.push({'branchCode': 'Branch Code is required'});
						}
					return results;
			};
		}
			function AddNewCityCommand(cityName, sahlCityKey, provinceId) {
				this.cityName = cityName || '';
				this.sahlCityKey = sahlCityKey;
				this.provinceId = provinceId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.AddNewCityCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.cityName!==0 && !this.cityName) {
							results.push({'cityName': 'City Name is required'});
						}
					if(this.sahlCityKey!==0 && !this.sahlCityKey) {
							results.push({'sahlCityKey': 'Sahl City Key is required'});
						}
					if(this.provinceId!==0 && !this.provinceId) {
							results.push({'provinceId': 'Province Id is required'});
						}
					return results;
			};
		}
			function ChangeBranchDetailsCommand(id, branchName, isActive, suburbId) {
				this.id = id;
				this.branchName = branchName || '';
				this.isActive = isActive;
				this.suburbId = suburbId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ChangeBranchDetailsCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.branchName!==0 && !this.branchName) {
							results.push({'branchName': 'Branch Name is required'});
						}
					if(this.isActive!==0 && !this.isActive) {
							results.push({'isActive': 'Is Active is required'});
						}
					if(this.suburbId!==0 && !this.suburbId) {
							results.push({'suburbId': 'Suburb Id is required'});
						}
					return results;
			};
		}
			function ChangeProvinceDetailsCommand(id, provinceName, sahlProvinceKey, countryId) {
				this.id = id;
				this.provinceName = provinceName || '';
				this.sahlProvinceKey = sahlProvinceKey;
				this.countryId = countryId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ChangeProvinceDetailsCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.provinceName!==0 && !this.provinceName) {
							results.push({'provinceName': 'Province Name is required'});
						}
					if(this.sahlProvinceKey!==0 && !this.sahlProvinceKey) {
							results.push({'sahlProvinceKey': 'Sahl Province Key is required'});
						}
					if(this.countryId!==0 && !this.countryId) {
							results.push({'countryId': 'Country Id is required'});
						}
					return results;
			};
		}
			function AddNewProvinceCommand(provinceName, sahlProvinceKey, countryId) {
				this.provinceName = provinceName || '';
				this.sahlProvinceKey = sahlProvinceKey;
				this.countryId = countryId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.AddNewProvinceCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.provinceName!==0 && !this.provinceName) {
							results.push({'provinceName': 'Province Name is required'});
						}
					if(this.sahlProvinceKey!==0 && !this.sahlProvinceKey) {
							results.push({'sahlProvinceKey': 'Sahl Province Key is required'});
						}
					if(this.countryId!==0 && !this.countryId) {
							results.push({'countryId': 'Country Id is required'});
						}
					return results;
			};
		}
			function ChangeCityDetailsCommand(id, cityName, sahlCityKey, provinceId) {
				this.id = id;
				this.cityName = cityName || '';
				this.sahlCityKey = sahlCityKey;
				this.provinceId = provinceId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ChangeCityDetailsCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.cityName!==0 && !this.cityName) {
							results.push({'cityName': 'City Name is required'});
						}
					if(this.sahlCityKey!==0 && !this.sahlCityKey) {
							results.push({'sahlCityKey': 'Sahl City Key is required'});
						}
					if(this.provinceId!==0 && !this.provinceId) {
							results.push({'provinceId': 'Province Id is required'});
						}
					return results;
			};
		}
			function AddNewSuburbCommand(suburbName, sahlSuburbKey, postalCode, cityId) {
				this.suburbName = suburbName || '';
				this.sahlSuburbKey = sahlSuburbKey;
				this.postalCode = postalCode || '';
				this.cityId = cityId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.AddNewSuburbCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.suburbName!==0 && !this.suburbName) {
							results.push({'suburbName': 'Suburb Name is required'});
						}
					if(this.sahlSuburbKey!==0 && !this.sahlSuburbKey) {
							results.push({'sahlSuburbKey': 'Sahl Suburb Key is required'});
						}
					if(this.postalCode!==0 && !this.postalCode) {
							results.push({'postalCode': 'Postal Code is required'});
						}
					if(this.cityId!==0 && !this.cityId) {
							results.push({'cityId': 'City Id is required'});
						}
					return results;
			};
		}
			function ChangeSuburbDetailsCommand(id, suburbName, sahlSuburbKey, postalCode, cityId) {
				this.id = id;
				this.suburbName = suburbName || '';
				this.sahlSuburbKey = sahlSuburbKey;
				this.postalCode = postalCode || '';
				this.cityId = cityId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ChangeSuburbDetailsCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.suburbName!==0 && !this.suburbName) {
							results.push({'suburbName': 'Suburb Name is required'});
						}
					if(this.sahlSuburbKey!==0 && !this.sahlSuburbKey) {
							results.push({'sahlSuburbKey': 'Sahl Suburb Key is required'});
						}
					if(this.postalCode!==0 && !this.postalCode) {
							results.push({'postalCode': 'Postal Code is required'});
						}
					if(this.cityId!==0 && !this.cityId) {
							results.push({'cityId': 'City Id is required'});
						}
					return results;
			};
		}
			function ChangeUserDetailsCommand(id, emailAddress, firstName, lastName, status, rolesToAssign, rolesToRemove, branchId) {
				this.id = id;
				this.emailAddress = emailAddress || '';
				this.firstName = firstName || '';
				this.lastName = lastName || '';
				this.status = status;
				this.rolesToAssign = rolesToAssign || '';
				this.rolesToRemove = rolesToRemove || '';
				this.branchId = branchId;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.ChangeUserDetailsCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.emailAddress!==0 && !this.emailAddress) {
							results.push({'emailAddress': 'Email Address is required'});
						}
					if(this.firstName!==0 && !this.firstName) {
							results.push({'firstName': 'First Name is required'});
						}
					if(this.lastName!==0 && !this.lastName) {
							results.push({'lastName': 'Last Name is required'});
						}
					if(this.status!==0 && !this.status) {
							results.push({'status': 'Status is required'});
						}
					if(this.rolesToAssign!==0 && !this.rolesToAssign) {
							results.push({'rolesToAssign': 'Roles To Assign is required'});
						}
					if(this.branchId!==0 && !this.branchId) {
							results.push({'branchId': 'Branch Id is required'});
						}
					return results;
			};
		}
			function LoginCommand(username, password) {
				this.username = username || '';
				this.password = password || '';
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.LoginCommand,SAHL.Services.Interfaces.Capitec';
				this.Validate = function() {
					var results = [];
					if(this.username!==0 && !this.username) {
							results.push({'username': 'Username is required'});
						}
					if(this.password!==0 && !this.password) {
							results.push({'password': 'Password is required'});
						}
					if(this.username && this.username.length > 20) {
							results.push({'username': 'Username Max Length is 20'});
						}
					if(this.username && this.username.length < 4) {
							results.push({'username': 'Username Min Length is 4'});
						}
					if(this.password && this.password.length < 0) {
							results.push({'password': 'Password cannot be empty'});
						}
					return results;
			};
		}
			function NotifyThatApplicationReceivedCommand(applicants, applicationNumber) {
				this.applicants = applicants;
				this.applicationNumber = applicationNumber;
				this._name = 'SAHL.Services.Interfaces.Capitec.Commands.NotifyThatApplicationReceivedCommand,SAHL.Services.Interfaces.Capitec';
				}

			return {
				AddNewUserCommand: AddNewUserCommand,
				ApplyForNewPurchaseCommand: ApplyForNewPurchaseCommand,
				ApplyForSwitchLoanCommand: ApplyForSwitchLoanCommand,
				AutoLoginCommand: AutoLoginCommand,
				ChangePasswordCommand: ChangePasswordCommand,
				AddNewBranchCommand: AddNewBranchCommand,
				AddNewCityCommand: AddNewCityCommand,
				ChangeBranchDetailsCommand: ChangeBranchDetailsCommand,
				ChangeProvinceDetailsCommand: ChangeProvinceDetailsCommand,
				AddNewProvinceCommand: AddNewProvinceCommand,
				ChangeCityDetailsCommand: ChangeCityDetailsCommand,
				AddNewSuburbCommand: AddNewSuburbCommand,
				ChangeSuburbDetailsCommand: ChangeSuburbDetailsCommand,
				ChangeUserDetailsCommand: ChangeUserDetailsCommand,
				LoginCommand: LoginCommand,
				NotifyThatApplicationReceivedCommand: NotifyThatApplicationReceivedCommand
			};
		}());
		return shared;
	}]);