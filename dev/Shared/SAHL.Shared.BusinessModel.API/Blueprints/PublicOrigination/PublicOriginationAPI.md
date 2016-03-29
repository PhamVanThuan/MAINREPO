FORMAT: 1A
HOST: http://sahomeloans.com/api/

# SAHL Public API
Allows authorised SA Home Loans business partners to securely originate home loan applications.

Below is a basic (non-exhaustive) overview of the expected URL scheme for the API with some common examples.

## Global Lookups

	/lookups                                                       GET
	/lookups/salutations                                           GET
	/lookups/occupancytypes                                        GET
	/lookups/employmenttypes                                       GET

## Mortgage Loans

### Applications

	/applications/mortgageloans/1322345                            GET
	/applications/mortgageloans/lookups                            GET
	/applications/mortgageloans/lookups/products                   GET
	/applications/mortgageloans/lookups/applicationtypes           GET
	/applications/mortgageloans/lookups/applicantroletypes          GET

	/applications/mortgageloans/declarations                       GET

	/applications/mortgageloans/newpurchase/eligibility            GET / POST
	/applications/mortgageloans/newpurchase/qualification          GET / POST
	/applications/mortgageloans/newpurchase/submission             POST

	/applications/mortgageloans/switch/eligibility                 GET / POST
	/applications/mortgageloans/switch/qualification               GET / POST
	/applications/mortgageloans/switch/submission                  POST

	/applications/mortgageloans/refinance/eligibility              GET / POST
	/applications/mortgageloans/refinance/qualification            GET / POST
	/applications/mortgageloans/refinance/submission               POST

	/applications/mortgageloans/furtherloan/eligibility            GET / POST
	/applications/mortgageloans/furtherloan/qualification          GET / POST
	/applications/mortgageloans/furtherloan/submission             POST

	/applications/mortgageloans/readvance/eligibility              GET / POST
	/applications/mortgageloans/readvance/qualification            GET / POST
	/applications/mortgageloans/readvance/submission               POST
	/applications/mortgageloans/1234353/track                      GET

### Accounts
	/accounts/mortgageloans/1232143

## Life Policies
### Applications

	/applications/lifepolicies/
	/applications/lifepolicies/1322345

### Accounts
	/accounts/lifepolicies/34325345

## HOC Policies
### Applications

	/applications/hocpolicies/
	/applications/hocpolicies/1322345

### Accounts
	/accounts/hocpolicies/21343254

## Personal Loans
### Applications

	/applications/personalloans/
	/applications/personalloans/1322345
	/applications/personalloans/1322345/applicants

### Accounts

	/accounts/personaloans/12313256/transactions

## Legal Entities

	/legalentities/clients
	/legalentities/thirdparties
	/legalentities/leads

# group API

## Root [/]

### Retrieve the origination root entry point [GET]
+ Request With API Token

    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

        {
            "_links":{
                "self": {"href": "/"},
                "lookups": {"href": "/lookups"},
                "applications" : {"href": "/applications"}
            }
        }

+ Request Without API Token

    + Headers

            API-Token: Empty

+ Response 401 (application/json)

    + Body

            {
                "message" : "Unauthorised: An API token must be provided."
            }

# group Lookups
Lookup resources.

## Root [/lookups]
### Retrieve the lookups root information [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/lookups"},
                  "parent" : {"href": "/"},
                  "lookups": [{
                        "href": "/lookups/employmenttypes"
                      }, {
                        "href": "/lookups/occupancytypes"
                      }, {
                        "href": "/lookups/salutationtypes"
                      }],
                }
            }

## Employment Types [/lookups/employmenttypes]
### Retrieve the employment types lookup data [GET]
+ Request

    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "self" : {"href": "/lookups/employmenttypes"},
                  "parent" : {"href": "/lookups"},
                  "employmenttypes": [{
                        "href": "/lookups/employmenttypes/1"
                      }, {
                        "href": "/lookups/employmenttypes/2"
                    }, {
                      "href": "/lookups/employmenttypes/3"
                    }],
                },
                "_embedded": {
                    "employmenttypes": [{
                        "id": 1,
                        "description": "Salaried",
                        "_links": {
                          "self": {"href": "/lookups/employmenttypes/1"},
                          "parent": {"href": "/lookups/employmenttypes"}
                        }
                    }, {
                        "id": 2,
                        "description": "Self Employed",
                        "_links": {
                          "self": {"href": "/lookups/employmenttypes/2"},
                          "parent": {"href": "/lookups/employmenttypes"}
                        }
                    }, {
                        "id": 3,
                        "description": "Salaried with Housing Allowance",
                        "_links": {
                          "self": {"href": "/lookups/employmenttypes/3"},
                          "parent": {"href": "/lookups/employmenttypes"}
                      }
                    }]
                }
            }

## Occupancy Types [/lookups/occupancytypes]
### Retrieve the occupancy types lookup data [GET]
+ Request

    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "self" : {"href": "/lookups/occupancytypes"},
                  "parent" : {"href": "/lookups"},
                  "occupancytypes": [{
                        "href": "/lookups/occupancytypes/1"
                      }, {
                        "href": "/lookups/occupancytypes/5"
                      }],
                },
                "_embedded": {
                    "occupancytypes": [{
                        "id": 1,
                        "description": "Owner Occupied",
                        "_links": {
                          "self": {"href": "/lookups/occupancytypes/1"},
                          "parent": {"href": "/lookups/occupancytypes"}
                        }
                    }, {
                        "id": 5,
                        "description": "Investment Property",
                        "_links": {
                          "self": {"href": "/lookups/occupancytypes/5"},
                          "parent": {"href": "/lookups/occupancytypes"}
                        }
                    }]
                }
            }

## Salutation Types [/lookups/salutationtypes]
### Retrieve the salutation types lookup data [GET]
+ Request

    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "self" : {"href": "/lookups/salutationtypes"},
                  "parent" : {"href": "/lookups"},
                  "salutationtypes": [{
                        "href": "/lookups/salutationtypes/1"
                      }, {
                        "href": "/lookups/salutationtypes/2"
                      }, {
                        "href": "/lookups/salutationtypes/5"
                      }, {
                        "href": "/lookups/salutationtypes/6"
                      }, {
                        "href": "/lookups/salutationtypes/7"
                      }, {
                        "href": "/lookups/salutationtypes/8"
                      }, {
                        "href": "/lookups/salutationtypes/9"
                      }, {
                        "href": "/lookups/salutationtypes/11"
                      }, {
                        "href": "/lookups/salutationtypes/12"
                      }, {
                        "href": "/lookups/salutationtypes/13"
                      }, {
                        "href": "/lookups/salutationtypes/14"
                      }, {
                        "href": "/lookups/salutationtypes/15"
                      }],
                },
                "_embedded": {
                    "salutationtypes": [{
                        "id": 1,
                        "description": "Mr",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/1"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 2,
                        "description": "Mrs",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/2"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 5,
                        "description": "Prof",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/5"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 6,
                        "description": "Dr",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/6"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 7,
                        "description": "Capt",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/7"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 8,
                        "description": "Past",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/8"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 9,
                        "description": "Miss",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/9"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 11,
                        "description": "Sir",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/11"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 12,
                        "description": "Ms",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/12"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 13,
                        "description": "Lord",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/13"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 14,
                        "description": "Rev",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/14"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }, {
                        "id": 15,
                        "description": "Advocate",
                        "_links": {
                          "self": {"href": "/lookups/salutationtypes/15"},
                          "parent": {"href": "/lookups/salutationtypes"}
                        }
                    }]
                }
            }

# group Applications
Application resources.

## Root [/applications]
### Retrieve Application resources. [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications"},
                  "parent" : {"href": "/"},
                  "mortgageloans": {"href": "/applications/mortgageloans"},
                  "lifepolicies": {"href": "/applications/lifepolicies"},
                  "hocpolicies": {"href": "/applications/hocpolicies"},
                  "personalloans": {"href": "/applications/personalloans"}
                }
            }


# group Mortgage Loan Applications
Mortgage Loan Application resources.

## Root [/applications/mortgageloans]
### Retrieve mortgage loan applications root information [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans"},
                  "parent" : {"href": "/applications"},
                  "lookups": {"href": "/applications/mortgageloans/lookups"},
                  "declarations": {"href": "/applications/mortgageloans/declarations"},
                  "newpurchase": {"href": "/applications/mortgageloans/newpurchase"},
                  "switch": {"href": "/applications/mortgageloans/switch"},
                  "refinance": {"href": "/applications/mortgageloans/refinance"}
                  }
                }
            }

## Lookups [/applications/mortgageloans/lookups]
### Retrieve the mortgage loan application lookups root information [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/lookups"},
                  "parent" : {"href": "/applications/mortgageloans"},
                  "lookups": [{
                        "href": "/applications/mortgageloans/lookups/applicationtypes"
                      }, {
                        "href": "/applications/mortgageloans/lookups/products"
                      }, {
                        "href": "/applications/mortgageloans/lookups/applicantroletypes"
                      }]
                }
            }

## Application Types [/applications/mortgageloans/lookups/applicationtypes]
### Retrieve the application types lookup data [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/lookups/applicationtypes"},
                  "parent" : {"href": "/applications/mortgageloans/lookups"},
                  "applicationtypes": [{
                        "href": "/applications/mortgageloans/lookups/applicationtypes/6"
                      }, {
                        "href": "/applications/mortgageloans/lookups/applicationtypes/7"
                      }, {
                        "href": "/applications/mortgageloans/lookups/applicationtypes/8"
                      }],
                },
                "_embedded": {
                    "applicationtypes": [{
                        "id": 6,
                        "description": "Switch Loan",
                        "_links": {
                          "self": {"href": "/applications/mortgageloans/lookups/applicationtypes/6"},
                          "parent": {"href": "/applications/mortgageloans/lookups/applicationtypes"}
                        }
                    }, {
                        "id": 7,
                        "description": "New Purchase Loan",
                        "_links": {
                          "self": {"href": "/applications/mortgageloans/lookups/applicationtypes/7"},
                          "parent": {"href": "/applications/mortgageloans/lookups/applicationtypes"}
                        }
                    }, {
                        "id": 8,
                        "description": "Refinance Loan",
                        "_links": {
                          "self": {"href": "/applications/mortgageloans/lookups/applicationtypes/8"},
                          "parent": {"href": "/applications/mortgageloans/lookups/applicationtypes"}
                        }
                    }]
                }
            }

## Products [/applications/mortgageloans/lookups/products]
### Retrieve the mortgage loan products lookup data [GET]
+ Request

    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/lookups/products"},
                  "parent" : {"href": "/applications/mortgageloans/lookups"},
                  "products": [{
                        "href": "/applications/mortgageloans/lookups/products/1"
                      }, {
                        "href": "/applications/mortgageloans/lookups/products/5"
                      }],
                },
                "_embedded": {
                    "products": [{
                        "id": 9,
                        "description": "New Variable Loan",
                        "_links": {
                          "self": {"href": "/applications/mortgageloans/lookups/products/9"},
                          "parent": {"href": "/applications/mortgageloans/lookups/products"}
                        }
                    }, {
                        "id": 11,
                        "description": "Edge",
                        "_links": {
                          "self": {"href": "/applications/mortgageloans/lookups/products/11"},
                          "parent": {"href": "/applications/mortgageloans/lookups/products"}
                        }
                    }]
                }
            }

## Applicant Role Types [/applications/mortgageloans/lookups/applicantroletypes]
### Retrieve the applicant role types lookup data [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/lookups/applicantroletypes"},
                  "parent" : {"href": "/applications/mortgageloans/lookups"},
                  "applicantroletypes": [{
                        "href": "/applications/mortgageloans/lookups/applicantroletypes/8"
                      }, {
                        "href": "/applications/mortgageloans/lookups/applicantroletypes/10"
                      }],
                },
                "_embedded": {
                    "applicantroletypes": [{
                        "id": 8,
                        "description": "Main Applicant",
                        "_links": {
                          "self": {"href": "/applications/mortgageloans/lookups/applicantroletypes/8"},
                          "parent": {"href": "/applications/mortgageloans/lookups/applicantroletypes"}
                        }
                    }, {
                        "id": 10,
                        "description": "Surety",
                        "_links": {
                          "self": {"href": "/applications/mortgageloans/lookups/applicantroletypes/10"},
                          "parent": {"href": "/applications/mortgageloans/lookups/applicantroletypes"}
                        }
                    }]
                }
            }

## Client Declarations  [/applications/mortgageloans/declarations]
### Retrieve mortgage loan application origination declarations [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/declarations"},
                  "parent" : {"href": "/applications/mortgageloans"},
                },
                "questions": [{
                    "id": 1,
                    "question": "Are you an income contributor?",
                    "answers": [{"Yes", "No"]
                }, {
                    "id": 2,
                    "question": "Do you give permission for SA Home Loans to conduct a credit bureau check?",
                    "answers": [{"Yes", "No"]
                }, {
                    "id": 3,
                    "question": "Are you married in community of property?",
                    "answers": [{"Yes", "No"]
                }]
            }

## group New Purchase Mortgage Loan Application Origination
New Purchase Mortgage Loan Application Origination resources.

### Root [/applications/mortgageloans/newpurchase]
#### Retrieve new purchase mortgage loan application origination root information [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/newpurchase"},
                  "parent" : {"href": "/applications/mortgageloans"},
                  "eligibility": {"href": "/applications/mortgageloans/newpurchase/eligibility"},
                  "qualification": {"href": "/applications/mortgageloans/newpurchase/qualification"},
                  "submission": {"href": "/applications/mortgageloans/newpurchase/submission"}
                }
            }

#### Client Eligibility Questions [/applications/mortgageloans/newpurchase/eligibility]
##### Retrieve new purchase mortgage loan eligibility questions [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/newpurchase/eligibility"},
                  "parent" : {"href": "/applications/mortgageloans/newpurchase"},
                },
                "questions": [{
                    "id": 1,
                    "question": "How many people are applying for this loan?",
                    "answers": [{"One", "Two"],
                    "validations": []
                }, {
                    "id": 2,
                    "question": "Are all of the applicants present?",
                    "answers": [{"Yes", "No"],
                    "validations": [{"No": "All applicants need to be present in order to apply for a home loan."}]
                }, {
                    "id": 3,
                    "question": "Are any of the applicants currently under debt counselling?",
                    "answers": [{"Yes", "No"],
                    "validations": [{"Yes": "No applicants are allowed under debt counselling when applying for a home loan."}]
                }, {
                    "id": 4,
                    "question": "Are all of the applicants between the age of 18 and 60 years old?",
                    "answers": [{"Yes", "No"],
                    "validations": [{"No": "All applicants need to be between the ages of 18 and 60 years old in order to apply for a home loan."}]
                }, {
                    "id": 5,
                    "question": "Does the property have a title deed?",
                    "answers": [{"Yes", "No"],
                    "validations": [{"No": "A title deed is a requirement for applying for a home loan."}]
                }, {
                    "id": 6,
                    "question": "Have you signed (or are about to sign) an offer to purchase on the property that you require this loan for?",
                    "answers": [{"Yes", "No"],
                    "validations": [{"No": "SA Home Loans cannot provide a general 'pre-approval' - we can only process a bond application based on a specific offer to purchase, where the purchase price is known."}]
                }]
            }


#### Submit new purchase mortgage loan eligibility questions [POST]
+ Request
    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

    + Body

            {
                "answers": [{
                    "id": 1,
                    "answer": "One"
                }, {
                    "id":2,
                    "answer": "Yes"
                }, {
                    "id": 3,
                    "answer": "No"
                }, {
                    "id": 4,
                    "answer": "Yes"
                }, {
                    "id": 5,
                    "answer": "Yes"
                }, {
                    "id": 6,
                    "answer": "Yes"
                }]
            }

+ Response 200 (application/hal+json)

    + Body

            {
                eligibilityToken: "DF1678EA-C964-4DF1-BBDD-2C07C29E724C"
            }

#### Client Qualification Check [/applications/mortgageloans/newpurchase/qualification/{eligibility_token}]
##### Submit a clients financial details for qualification [POST]
+ Request Qualifying

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "productId":9,
                "purchasePrice": 1000000,
                "deposit": 100000,
                "occupancyType": 1,
                "employmentType": 2,
                "householdIncome": 55000,
                "loanTermInMonths": 240
            }
+ Parameters

      + eligibility_token: `DF1678EA-C964-4DF1-BBDD-2C07C29E724C` (string) - The eligibility token received when submitting the client eligibility questions


+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "parent" : {"href": "/applications/mortgageloans/newpurchase/qualification"},
                },
                "qualificationStatus": "successful",
                "qualificationMessage": "You may qualify for a home loan - subject to a full credit assessment",
                "loanAmount": 445000.00,
                "totalInterest": 509000.00,
                "totalRepayment": 954000.00,
                "ltvPercentage": 44.5,
                "ptiPercentage": 7.2,
                "interestRatePercentage": 8.9,
                "termInMonths":240,
                "monthlyInstalment": 4025.21,
                "fees": {
                    "originationFees": {
                        "registrationFee": 8219.42,
                        "initiationFee": 5700.0,
                        "totalFees": 13919.42
                    },
                    "monthlyServiceFee": 50.0
                },
                "qualificationToken": "95349862-6A96-4518-B564-0AC3D44FE440"
            }

+ Request Non Qualifying

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "productId":9,
                "purchasePrice": 1000000,
                "deposit": 0,
                "occupancyType": 1,
                "employmentType": 2,
                "householdIncome": 7999,
                "loanTermInMonths": 240
            }

+ Response 403 (application/hal+json)

      + Body

            {
							"_links":{
							"parent" : {"href": "/applications/mortgageloans/switch/qualification"},
							},
							"qualificationStatus": "Unsuccessful",
							"qualificationMessage": "Application Declined",
							"declineReasons": ["Total Income is below the required minimum for salaried applicants", "LTV exceeds maximum allowed."]
            }

#### Application Submission [/applications/mortgageloans/newpurchase/submission/{qualification_token}]
##### Submit a clients completed application [POST]
+ Request Successful Submission

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "applicationDate": "2015-06-02 T2 13:54:23",
                "applicationPartnerReference":"1223432543",
                "loanDetails" : {
                    "productId":9,
                    "purchasePrice": 1000000,
                    "deposit": 100000,
                    "occupancyType": 1,
                    "employmentType": 2,
                    "householdIncome": 55000,
                    "loanTermInMonths": 240
                },
                "applicants":[{
                    "id":1,
					"applicantRoleTypeId": 1,
                    "identityNumber": "7412204040081",
                    "dateOfBirth":"1974-12-20",
                    "salutationId":1,
                    "firstName": "John",
                    "lastName": "Locke",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "082",
                        "number": "5765755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "john@thelocke.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 38000
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                }],
                "primaryApplicantContactId": 1
            }
+ Parameters

      + qualification_token: `DF1678EA-C964-4DF1-BBDD-2C07C29E724C` (string) - The qualification token received when performing the client qualification check


+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "parent" : {"href": "/applications/mortgageloans/newpurchase/submission"},
                },
                "submissionStatus": "successful",
                "submissionMessage": [{
                    "type": "application",
                    "severity": "info",
                    "message": "You may qualify for a home loan - subject to a full credit assessment"
                }],
                "applicationNumber": 158754545,
            }

+ Request Unsuccessful Submission

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "applicationDate": "2015-06-02 T2 13:54:23",
                "applicationPartnerReference":"1223432543",
                "loanDetails" : {
                    "productId":9,
                    "purchasePrice": 1000000,
                    "deposit": 100000,
                    "occupancyType": 1,
                    "employmentType": 2,
                    "householdIncome": 55000,
                    "loanTermInMonths": 240
                },
                "applicants":[{
                    "id":1,
					"applicantRoleTypeId": 1,
                    "identityNumber": "7412204040081",
                    "dateOfBirth":"1974-12-20",
                    "salutationId":1,
                    "firstName": "John",
                    "lastName": "Locke",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "082",
                        "number": "5765755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "john@thelocke.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 38000
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                },
				{
                    "id":2,
					"applicantRoleTypeId": 2,
                    "identityNumber": "8211045229080",
                    "dateOfBirth":"1982-11-04",
                    "salutationId":1,
                    "firstName": "Clinton",
                    "lastName": "Speed",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "084",
                        "number": "5722755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "clint@speed.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 11220
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                }],
                "primaryApplicantContactId": 1
            }

+ Response 403 (application/hal+json)

    + Body

            {
                "_links":{
                  "parent" : {"href": "/applications/mortgageloans/newpurchase/submission"},
                },
                "applicationNumber": 1234567,
                "applicationPartnerReference":"1223432543",
                "messages":["Your application was declined."],
                "loanDetails" : {
                    "messages":["The Household Income was below the minimum required for Salaried Applicants."]
                },
                "applicants":[{
                    "id":1,
                    "messages":["Applicant is over the age of 65."]
                },{
                    "id":2,
                    "messages":["Applicant is currently undergoing Debt Counselling."]
                }]
            }

## group Refinance Mortgage Loan Applications
Refinance Loan Application resources.

### Root [/applications/mortgageloans/refinance]
#### Retrieve refinance mortgage loan application origination root information [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/refinance"},
                  "parent" : {"href": "/applications/mortgageloans"},
                  "eligibility": {"href": "/applications/mortgageloans/refinance/eligibility"},
                  "qualification": {"href": "/applications/mortgageloans/refinance/qualification"},
                  "submission": {"href": "/applications/mortgageloans/refinance/submission"}
                }
            }

### Client Eligibility Questions [/applications/mortgageloans/refinance/eligibility]
#### Retrieve refinance mortgage loan eligibility questions [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                TODO
            }


#### Submit refinance mortgage loan eligibility questions [POST]
+ Request
    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

    + Body

            {
                TODO
            }

+ Response 200 (application/hal+json)

    + Body

            {
                eligibilityToken: "DF1678EA-C964-4DF1-BBDD-2C07C29E724C"
            }

### Client Qualification Check [/applications/mortgageloans/refinance/qualification/{eligibility_token}]
#### Submit a clients financial details for qualification [POST]

+ Request Qualifying

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
							"productId": 9,
							"cashRequired": 250000,
							"estimatedPropertyValue": 1000000,
							"occupancyTypeId": 1,
							"employmentTypeId": 2,
							"householdIncome": 55000,
							"loanTermInMonths": 240,
							"capitaliseFees": false
            }

+ Parameters

      + eligibility_token: `DF1678EA-C964-4DF1-BBDD-2C07C29E724C` (string) - The eligibility token received when submitting the client eligibility questions


+ Response 200 (application/hal+json)

      + Body

            {
							"_links":{
								"parent" : {"href": "/applications/mortgageloans/refinance/qualification"},
							},
							"qualificationStatus": "successful",
							"qualificationMessage": "You may qualify for a home loan - subject to a full credit assessment",
							"loanAmount": 250000.00,
							"loanAmountInclFees": 250000.00,
							"totalInterest": 309000.00,
							"totalRepayment": 559000.00,
							"ltvPercentage": 50.0,
							"ptiPercentage": 7.2,
							"interestRatePercentage": 8.9,
							"termInMonths":240,
							"monthlyInstalment": 4025.21,
							"fees": {
									"originationFees": {
											"registrationFee": 8219.42,
											"initiationFee": 5700.0,
											"totalFees": 13919.42
									},
									"monthlyServiceFee": 50.0
							},
							"qualificationToken": "95349862-6A96-4518-B564-0AC3D44FE440"
            }

+ Request Non Qualifying

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
							"productId": 9,
							"cashRequired": 500000,
							"estimatedPropertyValue": 450000,
							"occupancyTypeId": 1,
							"employmentTypeId": 1,
							"householdIncome": 7999,
							"loanTermInMonths": 240,
							"capitaliseFees": false
            }

+ Response 403 (application/hal+json)

      + Body

						{
							"_links":{
								"parent" : {"href": "/applications/mortgageloans/refinance/qualification"},
					 		},
					 		"qualificationStatus": "Unsuccessful",
					 		"qualificationMessage": "Application Declined",
					 		"declineReasons": ["Total Income is below the required minimum for salaried applicants", "LTV exceeds maximum allowed."]
						}

#### Application Submission [/applications/mortgageloans/refinance/submission/{qualification_token}]
##### Submit a clients completed application [POST]
+ Request Successful Submission

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "applicationDate": "2015-06-02 T2 13:54:23",
                "applicationPartnerReference":"1223432543",
                "loanDetails" : {
                    "productId":9,
                    "cashRequired": 100000,
					"estimatedPropertyValue": 500000,
                    "occupancyType": 1,
                    "employmentType": 2,
                    "householdIncome": 55000,
                    "loanTermInMonths": 240,
					"capitaliseFees": true,
                },
                "applicants":[{
                    "id":1,
					"applicantRoleTypeId": 1,
                    "identityNumber": "7412204040081",
                    "dateOfBirth":"1974-12-20",
                    "salutationId":1,
                    "firstName": "John",
                    "lastName": "Locke",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "021",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "082",
                        "number": "5765755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "john@thelocke.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 38000
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                }],
                "primaryApplicantContactId": 1
            }

+ Parameters

      + qualification_token: `DF1678EA-C964-4DF1-BBDD-2C07C29E724C` (string) - The qualification token received when performing the client qualification check


+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "parent" : {"href": "/applications/mortgageloans/refinance/submission"},
                },
                "submissionStatus": "successful",
                "submissionMessage": [{
                    "type": "application",
                    "severity": "info",
                    "message": "You may qualify for a home loan - subject to a full credit assessment"
                }],
                "applicationNumber": 158754545,
            }
+ Request Unsuccessful Submission

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "applicationDate": "2015-06-02 T2 13:54:23",
                "applicationPartnerReference":"1223432543",
                "loanDetails" : {
                    "productId":9,
                    "cashRequired": 50000,
                    "estimatedPropertyValue": 1000000,
                    "occupancyType": 1,
                    "employmentType": 2,
                    "householdIncome": 55000,
                    "loanTermInMonths": 240,
					"capitaliseFees": true
                },
                "applicants":[{
                    "id":1,
					"applicantRoleTypeId": 1,
                    "identityNumber": "7412204040081",
                    "dateOfBirth":"1974-12-20",
                    "salutationId":1,
                    "firstName": "John",
                    "lastName": "Locke",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "082",
                        "number": "5765755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "john@thelocke.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 38000
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                },
				{
                    "id":2,
					"applicantRoleTypeId": 1,
                    "identityNumber": "8211045229080",
                    "dateOfBirth":"1982-11-04",
                    "salutationId":1,
                    "firstName": "Clinton",
                    "lastName": "Speed",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "084",
                        "number": "5722755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "clint@speed.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 11220
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                }],
                "primaryApplicantContactId": 1
            }

+ Response 403 (application/hal+json)

    + Body

            {
                "_links":{
                  "parent" : {"href": "/applications/mortgageloans/refinance/submission"},
                },
                "applicationNumber": 1234567,
                "applicationPartnerReference":"1223432543",
                "messages":["Your application was declined."],
                "loanDetails" : {
                    "messages":["The total loan required of R50,000.00 does meet our minimum loan requirement."]
                },
                "applicants":[{
                    "id":1,
                    "messages":[""]
                },{
                    "id":2,
                    "messages":["Applicant is currently undergoing Debt Counselling."]
                }]
            }

## group Switch Mortgage Loan Applications
Switch Loan Application resources.

### Root [/applications/mortgageloans/switch]
#### Retrieve switch mortgage loan application origination root information [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)
    + Body

            {
                "_links":{
                  "self" : {"href": "/applications/mortgageloans/switch"},
                  "parent" : {"href": "/applications/mortgageloans"},
                  "eligibility": {"href": "/applications/mortgageloans/switch/eligibility"},
                  "qualification": {"href": "/applications/mortgageloans/switch/qualification"},
                  "submission": {"href": "/applications/mortgageloans/switch/submission"}
                }
            }

### Client Eligibility Questions [/applications/mortgageloans/switch/eligibility]
#### Retrieve switch mortgage loan eligibility questions [GET]
+ Request

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

+ Response 200 (application/hal+json)

    + Body

            {
                TODO
            }


#### Submit switch mortgage loan eligibility questions [POST]
+ Request
    + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)

    + Body

            {
                TODO
            }

+ Response 200 (application/hal+json)

    + Body

            {
                eligibilityToken: "DF1678EA-C964-4DF1-BBDD-2C07C29E724C"
            }

### Client Qualification Check [/applications/mortgageloans/switch/qualification/{eligibility_token}]
#### Submit a clients financial details for qualification [POST]
+ Request Qualifying

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "productId": 9,
                "existingLoan": 100000,
                "cashRequired": 200000,
                "estimatedPropertyValue": "1000000",
                "occupancyTypeId": 1,
                "employmentTypeId": 1,
                "loanTermInMonths": 240,
                "householdIncome": 29000,
                "capitaliseFees": true
            }

+ Parameters

      + eligibility_token: `DF1678EA-C964-4DF1-BBDD-2C07C29E724C` (string) - The eligibility token received when submitting the client eligibility questions


+ Response 200 (application/hal+json)

      + Body

            {
                "_links":{
                "parent" : {"href": "/applications/mortgageloans/switch/qualification"},
                },
                "qualificationStatus": "successful",
                "qualificationMessage": "You may qualify for a home loan - subject to a full credit assessment",
                "loanAmount": 300000.00,
                "loanAmountInclFees": 313074.00,
                "totalInterest": 360576.00,
                "totalRepayment": 660576.00,
                "ltvPercentage": 31.52,
                "ptiPercentage": 9.71,
                "interestRatePercentage": 8.9,
                "termInMonths":240,
                "monthlyInstalment": 2815.77,
                "fees": {
                       "originationFees": {
                       "registrationFee": 4826.00,
                       "initiationFee": 5700.00,
                       "cancellationFee": 2548.00,
                       "totalFees": 13074.00
                      },
                    "monthlyServiceFee": 50.0
                },
                "interimInterestProvision": 2134.62,
                "qualificationToken": "95349862-6A96-4518-B564-0AC3D44FE440"
            }

+ Request Non Qualifying

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "productId": 9,
                "existingLoan": 900000,
                "cashRequired": 200000,
                "estimatedPropertyValue": "1000000",
                "occupancyTypeId": 1,
                "employmentTypeId": 1,
                "loanTermInMonths": 240,
                "householdIncome": 7999,
                "capitaliseFees": true
            }

+ Response 403 (application/hal+json)

    + Body

            {
               "_links":{
                  "parent" : {"href": "/applications/mortgageloans/switch/qualification"},
               },
               "qualificationStatus": "Unsuccessful",
               "qualificationMessage": "Application Declined",
               "declineReasons": ["Total Income is below the required minimum for salaried applicants", "LTV exceeds maximum allowed."]
            }

#### Application Submission [/applications/mortgageloans/switch/submission/{qualification_token}]
##### Submit a clients completed application [POST]
+ Request Successful Submission

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "applicationDate": "2015-06-02 T2 13:54:23",
                "applicationPartnerReference":"1223432543",
                "loanDetails" : {
                    "productId":9,
                    "cashRequired": 100000,
                    "existingLoan": 100000,
					"estimatedPropertyValue": 500000,
                    "occupancyType": 1,
                    "employmentType": 2,
                    "householdIncome": 55000,
                    "loanTermInMonths": 240,
					"capitaliseFees": true
                },
                "applicants":[{
                    "id":1,
					"applicantRoleTypeId": 1,
                    "identityNumber": "7412204040081",
                    "dateOfBirth":"1974-12-20",
                    "salutationId":1,
                    "firstName": "John",
                    "lastName": "Locke",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "082",
                        "number": "5765755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "john@thelocke.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 38000
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                }],
                "primaryApplicantContactId": 1
            }

+ Parameters

      + qualification_token: `DF1678EA-C964-4DF1-BBDD-2C07C29E724C` (string) - The qualification token received when performing the client qualification check


+ Response 200 (application/hal+json)

    + Body

            {
                "_links":{
                  "parent" : {"href": "/applications/mortgageloans/switch/submission"},
                },
                "submissionStatus": "successful",
                "submissionMessage": [{
                    "type": "application",
                    "severity": "info",
                    "message": "You may qualify for a home loan - subject to a full credit assessment"
                }],
                "applicationNumber": 158754545,
            }
+ Request Unsuccessful Submission

      + Headers

            API-Token: FCEA80C8-A446-47E8-B88B-B01F2F9273A4 (Replace with your private API-Token)
      + Body

            {
                "applicationDate": "2015-06-02 T2 13:54:23",
                "applicationPartnerReference":"1223432543",
                "loanDetails" : {
                    "productId":9,
					"cashRequired": 400000,
                    "existingLoan": 100000,
					"estimatedPropertyValue": 1500000,
                    "occupancyType": 1,
                    "employmentType": 2,
                    "householdIncome": 7999,
                    "loanTermInMonths": 240,
					"capitaliseFees": true
                },
                "applicants":[{
                    "id":1,
					"applicantRoleTypeId": 1,
                    "identityNumber": "7412204040081",
                    "dateOfBirth":"1974-12-20",
                    "salutationId":1,
                    "firstName": "John",
                    "lastName": "Locke",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "082",
                        "number": "5765755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "john@thelocke.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 38000
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                },
				{
                    "id":2,
					"applicantRoleTypeId": 1,
                    "identityNumber": "8211045229080",
                    "dateOfBirth":"1982-11-04",
                    "salutationId":1,
                    "firstName": "Clinton",
                    "lastName": "Speed",
                    "contactDetails": [{
                        "medium":"phone",
                        "type":"home",
                        "code": "031",
                        "number": "5765714"
                        },{
                        "medium":"phone",
                        "type":"work",
                        "code": "011",
                        "number": "5765712"
                        },{
                        "medium":"phone",
                        "type":"mobile",
                        "code": "084",
                        "number": "5722755"
                        },{
                        "medium":"email",
                        "type", "home",
                        "emailAddress": "clint@speed.co.za"
                        }
                    ],
                    "employmentDetails": {
                        "employmentTypeId": 1,
                        "grossMonthlyIncome": 11220
                    },
                    "residentialAddress" :{
                        "unitNumber": "",
                        "buildingNumber": "",
                        "buildingName": "",
                        "streetNumber": "12",
                        "streetName": "Broadway Street",
                        "suburbId": "34577"
                    },
                    "declarations": [{
                        "id": 1,
                        "answer": "Yes"
                    },{
                            "id":2,
                            "answer": "Yes"
                        },{
                            "id":3,
                            "answer": "Yes"
                    }]
                }],
                "primaryApplicantContactId": 1
            }

+ Response 403 (application/hal+json)

    + Body

            {
                "_links":{
                  "parent" : {"href": "/applications/mortgageloans/switch/submission"},
                },
                "applicationNumber": 1234567,
                "applicationPartnerReference":"1223432543",
                "messages":["Your application was declined."],
                "loanDetails" : {
                    "messages":["The Household Income was below the minimum required for Salaried Applicants."]
                },
                "applicants":[{
                    "id":1,
                    "messages":[""]
                },{
                    "id":2,
                    "messages":[""]
                }]
            }
