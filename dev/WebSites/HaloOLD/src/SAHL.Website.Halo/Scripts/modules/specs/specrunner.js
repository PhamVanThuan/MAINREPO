require.config({
    baseUrl: "../../modules",
    paths: {
        'jquery': "../libs/jquery",
        'bootstrap': "../libs/bootstrap",
        'jasmine': 'Specs/jasmine/jasmine',
        'jasmine-html': 'Specs/jasmine/jasmine-html',
        'mockjax': 'Specs/mockjax/jquery.mockjax'
    },
    shim: {
        jasmine: {
            exports: 'jasmine'
        },
        'jasmine-html': {
            deps: ['jasmine'],
            exports: 'jasmine'
        },
        '../libs/noty/jquery.noty': ["jquery"],
        'notify': ['../libs/noty/jquery.noty'],
        'Specs/mockjax/jquery.mockjax' : ["jquery"]
    }
});

require(['jquery', 'jasmine-html'], function($, jasmine){
    var jasmineEnv = jasmine.getEnv();
    jasmineEnv.updateInterval = 1000;

    var htmlReporter = new jasmine.HtmlReporter();

    jasmineEnv.addReporter(htmlReporter);

    jasmineEnv.specFilter = function(spec) {
    return htmlReporter.specFilter(spec);
    };

    var specs = [];

    specs.push('Specs/halo.common.spec');
    specs.push('Specs/halo.actions.spec');

    $(function(){
        require(specs, function(){
            jasmineEnv.execute();
        });
    });
});