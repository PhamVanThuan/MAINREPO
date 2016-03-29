define("halo.utils", ["jquery"], function ($) {
	var utils = (function () {

		function delay(callback, ms, timer) {
			clearTimeout(timer);
			timer = setTimeout(callback, ms);
			return timer;
		};

		return {
			delay: delay
		}
	})();
	return utils;
});