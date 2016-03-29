module.exports = function(grunt){

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
         protractor: {
		    options: {
		      configFile: "node_modules/protractor/referenceConf.js", // Default config file
		      keepAlive: true, // If false, the grunt process stops when the test fails.
		      noColor: false, // If true, protractor will not use colors in its output.
		      args: {
		        // Arguments passed to the command
		      }
		    },
		    your_target: {
		      options: {
		        configFile: "myConf.js", // Target-specific config file
		        args: {
		        } // Target-specific arguments
		      }
		    },
		  },
		 watch: {
		    js: {
		        files: ['**/*.js','**/**/.js'],
		        tasks: ['protractor'],
		        options: {
		        	spawn:false
		        }
		    }
		}
    });
    grunt.loadNpmTasks('grunt-protractor-runner');
	grunt.loadNpmTasks('grunt-contrib-watch');
	grunt.registerTask('startSelenium', 'this is a task', function(){ 
	    grunt.util.spawn({ 
	      cmd: 'webdriver-manager', args: ['start', '--standalone']
	    }, function done(){ 
	      grunt.log.ok('Starting Selenium Server'); 
	    }); 
	}); 

	grunt.event.on('watch', function(action, filepath) {
		if(filepath.indexOf('spec.js') !== -1){
			grunt.config('protractor.your_target.options.args', {specs : [filepath]});
		}else{
			grunt.config('protractor.your_target.options.args', {specs : ['specs/*spec.js', 'specs/**/*spec.js']});
		}
	});

    grunt.registerTask('default', ['startSelenium', 'watch']);

};
