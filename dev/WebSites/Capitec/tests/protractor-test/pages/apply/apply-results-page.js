var page = require('../../page');
var AsyncSpec = require('jasmine-async')(jasmine);
var async = require('async');

var applyResultsPage = new page();

applyResultsPage.url = "/application-result";

applyResultsPage.applicationNumber = element(by.binding("applicationNumber"));
applyResultsPage.headerText = "apply"; 
applyResultsPage.titleText = { accepted: "your application has been submitted", rejected: "your application has been declined" };

applyResultsPage.applicants = element.all(by.repeater('applicant in applicants'));
applyResultsPage.resultMessages = element.all(by.repeater('message in applicationResult.messages'));

applyResultsPage.appQualifiesMessage = element(by.id("appQualifiesMessage"));
applyResultsPage.appDeclinedMessage = element(by.id("appDeclinedMessage"));

applyResultsPage.applicant = function(index, callback){
    applyResultsPage.applicants.get(index).then(function(applicant){
    	applicant.getText().then(function(text){
    		callback(text);
    	});
    });
};

applyResultsPage.applicationResultMessages = function(callback){
    applyResultsPage.resultMessages.then(function(messages){
        var messagesArray = [];
            async.each(messages, function(message, done){
                if (message){
                        message.getText().then(function(text){
                        messagesArray.push(text);
                        done();
                    }); 
                }
            }, function(err){
                callback(messagesArray);
            });
    });
};

applyResultsPage.applicantMessages = function(index, callback){
	applyResultsPage.applicants.get(index).then(function(applicant){
        applicant.findElements(by.repeater('message in applicant.messages')).then(function(messages){
            var messagesArray = [];
            async.each(messages, function(message, done){
                if (message){
                        message.getText().then(function(text){
                        messagesArray.push(text);
                        done();
                    }); 
                }
            }, function(err){
                callback(messagesArray);
            });
        });
    });
};

applyResultsPage.isApplicantITCCheckFailedDisplayed = function(index, callback){
	applyResultsPage.applicants.get(index).then(function(applicant){
	    applicant.findElement(by.cssContainingText('li[ng-show="!applicant.passed"]','Credit Bureau Check Failed')).isDisplayed().then(function(result){
	    	callback(result);
	    });
	});
};

applyResultsPage.applicantMessagesHeader = function(index, callback){
    applyResultsPage.applicants.get(index).then(function(applicant){
        applicant.findElement(by.css('p')).then(function(element){
            callback(element);
        });
    });
};

applyResultsPage.firstApplicantMessages = function(callback){
	applyResultsPage.applicantMessages(0, callback);
};

applyResultsPage.secondApplicantMessages = function(callback){
	applyResultsPage.applicantMessages(1, callback);
};

applyResultsPage.firstApplicantMessagesHeader = function(callback){
    applyResultsPage.applicantMessagesHeader(0, callback);
};

applyResultsPage.secondApplicantMessagesHeader = function(callback){
    applyResultsPage.applicantMessagesHeader(1, callback);
};

applyResultsPage.assertAppQualifiesMessageIsDisplayed = function(){
    expect(applyResultsPage.appQualifiesMessage.getText()).toContain("The application provisionally qualifies and has been submitted to SA Home Loans for a full credit assessment");
    expect(applyResultsPage.appQualifiesMessage.getText()).toContain("An SA Home Loans consultant will contact the main contact person within 1 business day to complete the process.");
    expect(applyResultsPage.appQualifiesMessage.getText()).toContain("No further Capitec Credit can be offered at this stage, since it might affect the pending Home Loan application.");
};

applyResultsPage.assertAppDeclinedMessageIsDisplayed = function(){
    expect(applyResultsPage.appDeclinedMessage.getText()).toContain("The client might still qualify for a Capitec Credit product. " +
        "Please discuss the opportunity and use the Capitec Banking System for a Credit Application.");
};

module.exports = applyResultsPage;





