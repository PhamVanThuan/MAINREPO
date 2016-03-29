define('halo.refreshables', [], function () {
	var refreshables = (function () {
		var refreshedableFunctions = [];
		function addRefreshable(refreshable) {
			refreshedableFunctions.push(refreshable);
		}
		function refresh() {
			for (i = 0; i < refreshedableFunctions.length; i++) {
				try {
					refreshedableFunctions[i]();
				} catch (ex) {
				}
			}
		}
		return {
			addRefreshable: addRefreshable,
			refresh: refresh
		}
	}());
	return refreshables;
});