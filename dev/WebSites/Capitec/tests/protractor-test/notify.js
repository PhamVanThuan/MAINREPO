(function (module) {
    var grunt = require('grunt'),
        growl = require('growl'),
        Buffer = require('buffer').Buffer;

    var globalMessages = [];

    function notify(obj, title) {
        if (obj) {
            var message = Buffer.isBuffer(obj) ? obj.toString() : (obj.message || obj);
            var msg = grunt.log.uncolor(message);
            if (msg.length > 0) {
                growl(msg, {
                    title: title,
                    sticky: 'true'
                });
            }
        }
    }

    // add a hook to grunt.fail.warn(), grunt.fail.fatal()
    ['warn', 'fatal'].forEach(function (level) {
        grunt.util.hooker.hook(grunt.fail, level, function(obj) {
            notify(obj);
        });
    });

    // add a hook to grunt.log.warn(), grunt.log.error()
    ['warn', 'error'].forEach(function (level) {
        grunt.util.hooker.hook(grunt.log, level, function(obj) {
            notify(obj, level);
        });
    });

    // add a hook to grunt.warn()
    grunt.util.hooker.hook(grunt, 'warn', function(obj) {
        notify(obj, 'warn');
    });
    
    // add a hook to process.stderr.write()
    grunt.util.hooker.hook(process.stderr, 'write', function(obj) {
        var messages = grunt.log.uncolor((Buffer.isBuffer(obj) ? obj.toString() : (obj.message || obj))).split('\n');
        messages.forEach(function (message) {
            //notify(message, 'stderr');
        });
    });

    // add a hook to process.stdout.write() (only error lines)
    grunt.util.hooker.hook(process.stdout, 'write', function(obj) {
        var messages = grunt.log.uncolor((Buffer.isBuffer(obj) ? obj.toString() : (obj.message || obj))).split('\n');
        messages.forEach(function (message) {
            if (message && message.indexOf('failure ') > -1) {
                //notify(message, 'stdout');
            }
        });
    });

    grunt.util.hooker.hook(grunt.log, 'writeln', function(message) {
        //notify(message, 'Capitec Protractor Tests');
    });

    grunt.util.hooker.hook(grunt.log, 'oklns', function(message) {
        if(message.indexOf("keep the grunt process alive") != -1)
            Filter(globalMessages);
    });

    // add a hook to child process stdout/stderr write() (only error lines
    grunt.util.hooker.hook(grunt.util, 'spawn', {
        post: function(child) {
            if (child.stderr){
                child.stderr.on('data', function (data) {
                var messages = grunt.log.uncolor(data.toString()).split('\n');
                messages.forEach(function (message) {
                        //notify(message, 'stderr');
                    });
                });
            }
            if (child.stdout){
                child.stdout.on('data', function (data) {
                var messages = grunt.log.uncolor(data.toString()).split('\n');
                globalMessages.push(data.toString());
                });
            }
        }
    });

    
    function Filter(messages){
        var failures = [];
        var startReading = false;
        messages.forEach(function(message){
            if(message.indexOf('Failures:') != -1){
                startReading = true;
            }
            if(message.indexOf('Finished') != -1){
                startReading = false;
            }
            if(startReading){
                failures.push(message);
            }
        });
        FormatAndPrint(failures);
    };

    function FormatAndPrint(failures){       
        var concatenatedTestFailures = [];
        var compileTestFailure;
        var testFailure;
        failures.forEach(function(failureMessage){
                if(failureMessage.indexOf('should') != -1){
                    console.log(failureMessage);
                    testFailure = "";
                    compileTestFailure = true;
                }
                if(failureMessage.indexOf('Error:') != -1){
                        //We are done
                        compileTestFailure = false;
                        testFailure += (failureMessage + '\n');
                        concatenatedTestFailures.push(testFailure);
                }
                if(compileTestFailure){
                      testFailure += (failureMessage + '\n');    
                }                               
            });
        concatenatedTestFailures.forEach(function growlThem(failure){
            notify(failure, "Capitec Test Failure");  
        });
        concatenatedTestFailures = []; 
        globalMessages = [];
    }

}) (module);