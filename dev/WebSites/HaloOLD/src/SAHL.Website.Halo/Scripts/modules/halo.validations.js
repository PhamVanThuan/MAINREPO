define("halo.validations", ["halo.notifications"], function (notifications) {
	var validations = (function () {
		function showErrors(data) {
			$.each(data.responseJSON, function (index, value) {
				if (!value.PropertyName) {
					var severityClass = '';
					switch (value.Severity) {
						case 0:
							severityClass = 'warning';
							break;
						case 1:
							severityClass = 'error';
						default:
					}
					notifications.show({
						message : value.Message, 
						notificationType : severityClass,
						location : "topleft"
					});
				} else {
					var $validatedElement = $('input[name*="' + value.PropertyName + '"]');
					//TODO : use a proper selector so that I don't have to check the length
					if ($validatedElement.length == 0) {
						$validatedElement = $('select[name*="' + value.PropertyName + '"]');
					}
					if ($validatedElement) {
						$validatedElement.popover({
							content: "<div><li class='icon-exclamation-sign icon-error'></li> <span>" + value.Message + "</span></div>",
							html: true
						});
						$validatedElement.popover("show");
					}
				}
			});
		}
		return {
			showErrors: showErrors
		}
	}());
	return validations;
});