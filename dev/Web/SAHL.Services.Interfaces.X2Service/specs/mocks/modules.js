'use strict';
angular.module('halo.core.webservices', []).
	service('$haloWebService', ['$q', function($q){
		return {
			sendCommandAsync: function () {
			},
        	sendQueryAsync: function () {
        	}
		};
	}]);

var allApplicationsQueryResult =
{
	"$type": "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
	"ReturnData": {
		"$type": "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Halo.HaloApplicationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"QueryDurationInMilliseconds": 0,
		"NumberOfPages": 1,
		"ResultCountInAllPages": 2,
		"ResultCountInPage": 2,
		"Results": {
			"$id": "1",
			"$values": [{
				"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Name": "My Halo",
				"Sequence": 1,
				"Modules": null
			},
				{
					"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
					"Name": "Home",
					"Sequence": 2,
					"Modules": null
				}]
		}
	},
	"SystemMessages": {
		"$type": "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"systemMessages": {
			"$id": "2",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		},
		"HasErrors": false,
		"HasExceptions": false,
		"HasWarnings": false,
		"HasExceptionMessage": false,
		"AllMessages": {
			"$id": "3",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		}
	}
};

var homeApplicationConfigurationQueryResult =
{
	"$type": "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
	"ReturnData": {
		"$type": "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Halo.ApplicationConfigurationForRoleQueryResult, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"QueryDurationInMilliseconds": 0,
		"NumberOfPages": 1,
		"ResultCountInAllPages": 1,
		"ResultCountInPage": 1,
		"Results": {
			"$id": "1",
			"$values": [{
				"$type": "SAHL.Services.Interfaces.Halo.ApplicationConfigurationForRoleQueryResult, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Role": "Sales",
				"HaloApplicationModel": {
					"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
					"Name": "Home",
					"Sequence": 1,
					"Modules": {
						"$id": "2",
						"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloApplicationModuleModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
						"$values": [{
							"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModuleModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
							"Name": "Clients",
							"Sequence": 1,
							"IsTileBased": true,
							"NonTilePageState": "",
							"RootTileConfigurations": {
								"$id": "3",
								"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
								"$values": [{
									"$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
									"Name": "Legal Entity",
									"Template": "legalentityroot.tpl.html",
									"Sequence": 1,
									"startRow": 1,
									"startColumn": 6,
									"noOfRows": 5,
									"noOfColumns": 3
								}]
							}
						}]
					}
				}
			}]
		}
	},
	"SystemMessages": {
		"$type": "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"systemMessages": {
			"$id": "4",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		},
		"HasErrors": false,
		"HasExceptions": false,
		"HasWarnings": false,
		"HasExceptionMessage": false,
		"AllMessages": {
			"$id": "5",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		}
	}
};

var myHaloApplicationConfigurationQueryResult =
{
	"$type": "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
	"ReturnData": {
		"$type": "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Halo.ApplicationConfigurationForRoleQueryResult, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"QueryDurationInMilliseconds": 0,
		"NumberOfPages": 1,
		"ResultCountInAllPages": 1,
		"ResultCountInPage": 1,
		"Results": {
			"$id": "1",
			"$values": [{
				"$type": "SAHL.Services.Interfaces.Halo.ApplicationConfigurationForRoleQueryResult, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Role": "",
				"HaloApplicationModel": {
					"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
					"Name": "My Halo",
					"Sequence": 2,
					"Modules": {
						"$id": "2",
						"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloApplicationModuleModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
						"$values": [{
							"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModuleModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
							"Name": "Clients",
							"Sequence": 1,
							"IsTileBased": true,
							"NonTilePageState": "",
							"RootTileConfigurations": {
								"$id": "3",
								"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
								"$values": []
							}
						},
							{
								"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModuleModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
								"Name": "Home",
								"Sequence": 1,
								"IsTileBased": true,
								"NonTilePageState": "",
								"RootTileConfigurations": {
									"$id": "4",
									"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
									"$values": [{
										"$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
										"Name": "My Pipeline",
										"Template": "app/start/portalpages/myhalo/home/mypipeline/mypipeline.tpl.html",
										"Sequence": 1,
										"startRow": 0,
										"startColumn": 0,
										"noOfRows": 4,
										"noOfColumns": 4
									},
										{
											"$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
											"Name": "Total Sales",
											"Template": "app/start/portalpages/myhalo/home/totalsales/totalsales.tpl.html",
											"Sequence": 2,
											"startRow": 0,
											"startColumn": 4,
											"noOfRows": 4,
											"noOfColumns": 8
										},
										{
											"$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
											"Name": "My Commission",
											"Template": "app/start/portalpages/myhalo/home/mycommission/mycommission.tpl.html",
											"Sequence": 3,
											"startRow": 4,
											"startColumn": 0,
											"noOfRows": 4,
											"noOfColumns": 3
										},
										{
											"$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
											"Name": "Sales League",
											"Template": "app/start/portalpages/myhalo/home/salesleague/salesleague.tpl.html",
											"Sequence": 4,
											"startRow": 4,
											"startColumn": 3,
											"noOfRows": 5,
											"noOfColumns": 9
										}]
								}
							},
							{
								"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModuleModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
								"Name": "Tasks",
								"Sequence": 2,
								"IsTileBased": false,
								"NonTilePageState": "tasks",
								"RootTileConfigurations": {
									"$id": "5",
									"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
									"$values": []
								}
							},
							{
								"$type": "SAHL.Services.Interfaces.Halo.HaloApplicationModuleModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
								"Name": "Calendar",
								"Sequence": 3,
								"IsTileBased": true,
								"NonTilePageState": "",
								"RootTileConfigurations": {
									"$id": "6",
									"$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
									"$values": []
								}
							}]
					}
				}
			}]
		}
	},
	"SystemMessages": {
		"$type": "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"systemMessages": {
			"$id": "7",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		},
		"HasErrors": false,
		"HasExceptions": false,
		"HasWarnings": false,
		"HasExceptionMessage": false,
		"AllMessages": {
			"$id": "8",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		}
	}
};

var myHaloApplicationMenuItemsResult =
{
	"$type": "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
	"ReturnData": {
		"$type": "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Halo.ApplicationMenuItem, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"NumberOfPages": 1,
		"ResultCountInAllPages": 5,
		"ResultCountInPage": 5,
		"Results": {
			"$id": "1",
			"$values": [{
				"$type": "SAHL.Services.Interfaces.Halo.ApplicationMenuItem, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Name": "Home",
				"Sequence": 1,
				"ModuleName": "Home"
			},
			{
				"$type": "SAHL.Services.Interfaces.Halo.ApplicationMenuItem, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Name": "Team",
				"Sequence": 2,
				"ModuleName": "Team"
			},
			{
				"$type": "SAHL.Services.Interfaces.Halo.ApplicationMenuItem, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Name": "Calendar",
				"Sequence": 3,
				"ModuleName": "Calendar"
			},
			{
				"$type": "SAHL.Services.Interfaces.Halo.ApplicationMenuItem, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Name": "Mail",
				"Sequence": 4,
				"ModuleName": "Mail"
			},
			{
				"$type": "SAHL.Services.Interfaces.Halo.ApplicationMenuItem, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
				"Name": "My Clients",
				"Sequence": 5,
				"ModuleName": "My Clients"
			}]
		}
	},
	"SystemMessages": {
		"$type": "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
		"systemMessages": {
			"$id": "2",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		},
		"HasErrors": false,
		"HasExceptions": false,
		"HasWarnings": false,
		"HasExceptionMessage": false,
		"AllMessages": {
			"$id": "3",
			"$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
			"$values": []
		}
	}
};

var clientsModuleConfigurationResult =
    {
        "$type": "SAHL.Core.Web.Services.ServiceQueryResult, SAHL.Core.Web, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
        "ReturnData": {
            "$type": "SAHL.Core.Services.ServiceQueryResult`1[[SAHL.Services.Interfaces.Halo.Models.ModuleConfigurationQueryResult, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "QueryDurationInMilliseconds": 0,
            "NumberOfPages": 1,
            "ResultCountInAllPages": 1,
            "ResultCountInPage": 1,
            "Results": {
                "$id": "1",
                "$values": [{
                    "$type": "SAHL.Services.Interfaces.Halo.Models.ModuleConfigurationQueryResult, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                    "ModuleConfiguration": {
                        "$type": "SAHL.Services.Interfaces.Halo.HaloModuleTileModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                        "Name": "Clients",
                        "Sequence": 1,
                        "IsTileBased": true,
                        "NonTilePageState": "",
                        "RootTileConfigurations": {
                            "$id": "2",
                            "$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                            "$values": [{
                                "$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                "name": "Legal Entity",
                                "sequence": 1,
                                "startRow": 0,
                                "startColumn": 0,
                                "noOfRows": 1,
                                "noOfColumns": 1,
                                "tileData": {
                                    "$type": "SAHL.UI.Halo.Home.Models.Clients.LegalEntityRootModel, SAHL.UI.Halo.Home, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                    "LegalName": "Mr Johan Jacobus Petrus Dercksen",
                                    "IDNumber": "6810205038082",
                                    "RegistrationNumber": null,
                                    "LegalEntityStatus": "Alive",
                                    "HomephoneNumber": null,
                                    "WorkphoneNumber": "() ",
                                    "CellPhoneNumber": "0832831692",
                                    "DateOfBirth": "1968-10-20T00:00:00",
                                    "PostalAddress": "",
                                    "DomiciliumAddress": "14 Honeysuckle Place\r\nGlen Hills\r\nAvoca\r\nKwazulu-Natal\r\nSouth Africa",
                                    "EmailAddress": "johan@draco.co.za",
                                    "BankingInstitute": "Nedbank"
                                },
                                "tileSubKeys": null,
                                "tileActions": {
                                    "$id": "3",
                                    "$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileActionModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                    "$values": []
                                }
                            }]
                        },
                        "ChildTileConfigurations": {
                            "$id": "4",
                            "$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                            "$values": [{
                                "$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                "name": "Legal Entity Address",
                                "sequence": 3,
                                "startRow": 1,
                                "startColumn": 0,
                                "noOfRows": 4,
                                "noOfColumns": 3,
                                "tileData": {
                                    "$type": "SAHL.UI.Halo.Home.Models.Clients.LegalEntityAddressChildModel, SAHL.UI.Halo.Home, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                    "AddressType": "Residential",
                                    "Address": "6 Itendele Road, Kloof, Kwazulu-Natal, 3610, South Africa",
                                    "EffectiveDate": "2014-07-23T00:00:00",
                                    "IsDomicilium": "No",
                                    "Notification": null
                                },
                                "tileSubKeys": {
                                    "$id": "5",
                                    "$type": "System.Linq.Enumerable+WhereSelectListIterator`2[[SAHL.UI.Halo.Shared.Configuration.HaloTileBusinessModel, SAHL.UI.Halo.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[SAHL.Core.BusinessModel.BusinessKey, SAHL.Core.BusinessModel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                    "$values": []
                                },
                                "tileActions": {
                                    "$id": "6",
                                    "$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileActionModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                    "$values": []
                                }
                            },
                                {
                                    "$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                    "name": "Legal Entity Address",
                                    "sequence": 3,
                                    "startRow": 1,
                                    "startColumn": 0,
                                    "noOfRows": 4,
                                    "noOfColumns": 3,
                                    "tileData": {
                                        "$type": "SAHL.UI.Halo.Home.Models.Clients.LegalEntityAddressChildModel, SAHL.UI.Halo.Home, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                        "AddressType": "Residential",
                                        "Address": "14 Honeysuckle Place, Glen Hills, Avoca, Kwazulu-Natal, South Africa",
                                        "EffectiveDate": "2014-07-24T14:54:44.64",
                                        "IsDomicilium": "Yes",
                                        "Notification": null
                                    },
                                    "tileSubKeys": {
                                        "$id": "7",
                                        "$type": "System.Linq.Enumerable+WhereSelectListIterator`2[[SAHL.UI.Halo.Shared.Configuration.HaloTileBusinessModel, SAHL.UI.Halo.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[SAHL.Core.BusinessModel.BusinessKey, SAHL.Core.BusinessModel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                        "$values": [{
                                            "$type": "SAHL.Core.BusinessModel.BusinessKey, SAHL.Core.BusinessModel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                            "KeyType": 0,
                                            "Key": 837034
                                        }]
                                    },
                                    "tileActions": {
                                        "$id": "8",
                                        "$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileActionModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                        "$values": []
                                    }
                                },
                                {
                                    "$type": "SAHL.Services.Interfaces.Halo.HaloTileConfigurationModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                    "name": "Mortgage Loan",
                                    "sequence": 2,
                                    "startRow": 0,
                                    "startColumn": 0,
                                    "noOfRows": 6,
                                    "noOfColumns": 4,
                                    "tileData": {
                                        "$type": "SAHL.UI.Halo.Home.Models.Clients.MortgageLoanChildModel, SAHL.UI.Halo.Home, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                        "AccountNumber": "3877597",
                                        "PropertyAddress": "14 Honeysuckle Place\r\nGlen Hills\r\nAvoca\r\nKwazulu-Natal\r\nSouth Africa",
                                        "LoanAgreementAmount": 1950000.0,
                                        "LoanCurrentBalance": 1941523.2245769033,
                                        "LoanArrearBalance": 0.0,
                                        "DebitOrderDay": 26,
                                        "InstallmentAmount": 15828.52,
                                        "RemainingInstalments": 238,
                                        "MonthsInArrears": 0
                                    },
                                    "tileSubKeys": {
                                        "$id": "9",
                                        "$type": "System.Linq.Enumerable+WhereSelectListIterator`2[[SAHL.UI.Halo.Shared.Configuration.HaloTileBusinessModel, SAHL.UI.Halo.Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null],[SAHL.Core.BusinessModel.BusinessKey, SAHL.Core.BusinessModel, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                        "$values": []
                                    },
                                    "tileActions": {
                                        "$id": "10",
                                        "$type": "System.Collections.Generic.List`1[[SAHL.Services.Interfaces.Halo.HaloTileActionModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                                        "$values": [{
                                            "$type": "SAHL.Services.Interfaces.Halo.HaloTileActionModel, SAHL.Services.Interfaces.Halo, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
                                            "name": "Mortgage Loan",
                                            "actionType": "DrillDown"
                                        }]
                                    }
                                }]
                        }
                    }
                }]
            }
        },
        "SystemMessages": {
            "$type": "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
            "systemMessages": {
                "$id": "11",
                "$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                "$values": []
            },
            "HasErrors": false,
            "HasExceptions": false,
            "HasWarnings": false,
            "HasExceptionMessage": false,
            "AllMessages": {
                "$id": "12",
                "$type": "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                "$values": []
            }
        }
    };