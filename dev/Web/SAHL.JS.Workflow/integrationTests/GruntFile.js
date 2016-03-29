'use strict';

module.exports = function(grunt) {

    grunt.loadNpmTasks('grunt-protractor-runner');
    grunt.loadNpmTasks('grunt-protractor-webdriver');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-connect');
    var basePath = '../../Web/SAHL.JS.Workflow/integrationTests/';
    // require('load-grunt-tasks')(grunt);
    // Define the configuration for all the tasks
    grunt.initConfig({
        watch: {
            options: {
                liveReload: true
            },
            lib: {
                files: [basePath + './../lib/**/*.js', './index.html'],
                tasks: ['copy:lib'],
            }
        },
        protractor: {
            options: {
                configFile: basePath + 'conf.js', // Default config file 
                keepAlive: true, // If false, the grunt process stops when the test fails. 
                noColor: false, // If true, protractor will not use colors in its output. 
                args: {
                    seleniumServerJar: 'node_modules/protractor/selenium/selenium-server-standalone-2.45.0.jar',
                    // chromeDriver: 'node_modules/protractor/selenium/chromedriver.exe',
                    specs: [basePath + 'scenarioMaps/*.spec.js'],

                }
            },
            all: {}
        },
        connect: {
            server: {
                options: {
                    open:false,
                    keepalive: false,
                    port: 9001,
                    base:basePath,
                },
            },
            test: {

                options: {
                   open: 'http://localhost:9001/',
                    // keepalive:true,
                    port: 9001,
                    liveReload: true,
                    base:basePath,
                }
            }
        },
        copy: {
            lib: {
                expand: true,
                src: basePath + '/../lib/**/*.js',
                dest: basePath + '/lib/workflowManager',
                flatten:true
            }
        },


    });

    grunt.registerTask('serve', ['copy:lib', 'connect:server']);
    grunt.registerTask('test', ['copy:lib', 'connect:test', 'watch:lib']);
    grunt.registerTask('default', ['serve', 'protractor']);
};
