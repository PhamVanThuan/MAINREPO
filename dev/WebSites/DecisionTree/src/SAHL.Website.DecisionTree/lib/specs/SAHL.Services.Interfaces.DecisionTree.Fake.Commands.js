var commandFakes = function(){
	function SystemMessages(){
		return {
			"$type" : "SAHL.Core.SystemMessages.SystemMessageCollection, SAHL.Core",
			"HasErrors" : false,
			"HasWarnings" : false,
			"ErrorMessages" : function(){
				var retObj = this.AllMessages;
				retObj.$values = $.grep(this.AllMessages.$values,function(msg){
					return msg.Severity === 1;
				});
				return retObj;
			},
			"WarningMessages" : function(){
				var retObj = this.AllMessages;
				retObj.$values = $.grep(this.AllMessages.$values,function(msg){
					return msg.Severity === 0;
				});
				return retObj;
			},
			"AllMessages" : {
				"$id" : "99",
				"$type" : "System.Collections.Generic.List`1[[SAHL.Core.SystemMessages.ISystemMessage, SAHL.Core",
				"$values" : []
			},
			"AddError": function(message){
				var message = {"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null","Severity" : 1,"Message" : message};
				this.AllMessages.$values.push(message);
				this.HasErrors = true;
			},
			"AddWarning": function(message){
				var message = {"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null","Severity" : 0,"Message" : message};
				this.AllMessages.$values.push(message);
				this.HasWarnings = true;
			},
			"AddInfo": function(message){
				var message = {
					"$type" : "SAHL.Core.SystemMessages.SystemMessage, SAHL.Core, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",
					"Severity" : 2,
					"Message" : message
				};
				this.AllMessages.$values.push(message);
			}
		}
	}

	return {
		SystemMessages : SystemMessages
		
	};
}();