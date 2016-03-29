module.exports = function(grunt) {

  // Load Grunt tasks declared in the package.json file
  require('matchdep').filterDev('grunt-*').forEach(grunt.loadNpmTasks);
  grunt.loadNpmTasks('grunt-contrib-watch');
  grunt.loadNpmTasks('grunt-exec');

  // Configure Grunt
  grunt.initConfig({

    // grunt-contrib-connect will serve the files of the project
    // on specified port and hostname
    connect: {
      all: {
        options:{
          port: 8080,
          hostname: "0.0.0.0",
          // Prevents Grunt to close just after the task (starting the server) completes
          // This will be removed later as `watch` will take care of that
          livereload: true
        }
      }
    },

    // grunt-open will open your browser at the project's URL
    open: {
      all: {
        // Gets the port from the connect configuration
        path: 'http://localhost:<%= connect.all.options.port%>/PublicOriginationAPI.html'
      }
    },
    watch: {
       files: ['*.md'],
       tasks: ['exec'],
       options: {
         livereload: true
       }
     },
    exec: {
        aglio: {
          command: 'aglio -i PublicOriginationAPI.md -o PublicOriginationAPI.html --full-width -t flatly-multi',
          stdout: true,
          stderr: true
        }
      }
  });

  // Creates the `server` task
  grunt.registerTask('server',[
    'open',
    'connect',
    'watch'
  ]);
};
