define("halo.notifications", ["jquery"], function ($) {
	var notifications = (function () {
		function show(options) {
			var locationClass = "stack-" + options.location;
			var width = '300px';
			if (!options.location) {
				width = '100%';
				locationClass = "stack-top";
			}
			var opts = {
				width: width,
				title: options.title,
				text: options.message,
				addclass: locationClass,
				type: options.notificationType,
				history:false
			};
			$.pnotify(opts);
		}
		function hideall() {
			$(".ui-pnotify-container").remove();
		}
		return {
			show: show,
			hideall : hideall
		}
	})();
	return notifications;
});