var capitec = {};

require('./elements/horizontal-menu.js')(capitec);
require('./elements/bread-crumbs-menu.js')(capitec);
require('./elements/notifications-popup.js')(capitec);
require('./elements/information-tooltip.js')(capitec);
require('./utils.js')(capitec);
require('./modules/login-module.js')(capitec);
require('./modules/data-queries.js')(capitec);
require('./queries/queries.js')(capitec);

Array.prototype.filterBy = function(by){
  for (var i = 0; i < this.length; i++) {
    if(this[i][by.name] == by.value){
      return this[i];
    }
  };
};

String.prototype.formatToCurrency = function(){
	var numeral = require('numeral');
	return numeral(this).format('R 0,0');
};

module.exports = capitec;