module.exports = function(applicationName){
	var breadCrumbs = function(){
	this.home = element(by.linkText("Home"));
	this.administration = element(by.xpath('//*[@id="breadcrumbs"]/ul/li[3]/a[1]'))
	this.goHome = function(){
		this.home.click();
	};	
	this.back = function(steps){
		var crumbs = element.all(by.repeater('state in $state.$current.path | removeHiddenCrumbs'));
		crumbs.then(function(arr){
			var index = arr.length-(steps+1);
			arr[index].click();
			})
		};
	};
	applicationName.breadcrumbs = new breadCrumbs();
};