module.exports = function(grunt) {
  'use strict';
  function getBasePath() {
    if(!grunt.option('basePath')) {
       return './';
     } else {
      return grunt.option('basePath');
     }
  }
  var globalConfig = {
    basePath: getBasePath(),
    appPath: getBasePath() + '/app',
    lessPath: getBasePath() + '/less',
    outputFileName: 'app.min.<%= grunt.template.today("yyyymmddHHMMss") %>.js'
  };

  // Project configuration.
  grunt.initConfig({
    pkg: grunt.file.readJSON('package.json'),
    globalConfig: globalConfig,
    clean: {
      build: ['dist'],
      jsFiles: ['<%= globalConfig.appPath %>/**/**.js', '<%= globalConfig.appPath %>/**/**.spec.js', '!<%= globalConfig.appPath %>/**/serviceConfig.js'],
      cssFiles: ['<%= globalConfig.lessPath %>/**/**.css*', '<%= globalConfig.lessPath %>/**/**.less'],
      htmlFiles: ['<%=globalConfig.appPath %>/**/**.tpl.html']
    },
    jshint: {
      options: {
        'curly': true,
        'quotmark': true,
        'eqnull': true,
        'eqeqeq': false,
        'undef': true,
        'unused': false,
        'jquery': true,
        'browser': true,
        'globals': {
        'jquery': true,
        'angular': true,
        'controller': true,
        },
        reporter: require('jshint-teamcity'),
        force: true
      },
      beforeconcat: ['<%= globalConfig.appPath %>/**/**.js', '!<%= globalConfig.appPath %>/**/**.spec.js'],
      afterconcat: ['<%= globalConfig.appPath %>/**/**.js'],
      local: {
        src: ['<%= globalConfig.appPath %>/**/**.js', '!<%= globalConfig.appPath %>/**/**.spec.js'],
        options: {
          reporter: require('jshint-stylish')
        }
      },
    },
    concat: {
      options: {
        separator: ';',
        stripBanners: {
          block: true,
          line: true
        },
        banner: '/*! <%= pkg.name %> - v<%= pkg.version %> - <%= grunt.template.today("yyyy-mm-dd") %> */'
      },
    },
    uglify: {
      options: {
        beautify: false,
        mangle: false
      },
    },
    useminPrepare: {
      options: {
        dest: '<%= globalConfig.appPath %>/../',
      },
      html: '<%= globalConfig.basePath %>/index.html',
    },
    usemin: {
      html: '<%= globalConfig.basePath %>/index.html',
    },
    'string-replace': {
      dist: {
        src: '<%= globalConfig.basePath %>/index.html',
        dest: '<%= globalConfig.basePath %>/index.html',
        options: {
          replacements: [{
            pattern: /\[timestamp\]/gi,
            replacement: '<%= grunt.template.today("yyyymmddHHMMss") %>'
          }]
        }
      },
      transformpath: {
        src: '<%= globalConfig.basePath %>/lib/pnotify/css/pnotify.bootstrap.css',
        dest: '<%= globalConfig.basePath %>/lib/pnotify/css/pnotify.bootstrap.css',
        options: {
          replacements: [{
            pattern: '../../../assets/img/glyphicons-halflings.png',
            replacement: '../assets/img/glyphicons-halflings.png'
          }]
        }
      },
      updateImages: {
        files: [{
          expand: true,
          src: ['<%= globalConfig.appPath %>/**/*.tpl.html','<%= globalConfig.lessPath %>/**/*.css', '<%= globalConfig.lessPath %>/**/*.min.css'],
          dest: '',
        }, ],
        options: {
          replacements: [{
            pattern: /\/assets\/img\/([^\/]*?\.png|[^\/]*?\.gif|[^\/]*?\.jpg|[^\/]*?\.jpeg)/gi,
            replacement: '/assets/img/$1?<%= grunt.template.today("yyyymmddHHMMss") %>',
          }]
        }
      },
      updateServiceConfigTimestamp: {
        files: [{
          expand: true,
          src: '<%= globalConfig.basePath %>/index.html',
          dest: '',
        }, ],
        options: {
          replacements: [{
            pattern: /serviceConfig\.js/gi,
            replacement: 'serviceConfig.js?<%= grunt.template.today("yyyymmddHHMMss") %>',
          }]
        }
      }
    },
    html2js: {
      options: {
        useStrict: true,
        htmlmin: {
          collapseBooleanAttributes: true,
          collapseWhitespace: true,
          removeAttributeQuotes: false,
          removeComments: true,
          removeEmptyAttributes: false,
          removeRedundantAttributes: false,
          removeScriptTypeAttributes: false,
          removeStyleLinkTypeAttributes: false
        },
        rename: function(moduleName) {
          return moduleName.substring(moduleName.lastIndexOf('/') + 1);
        }
      },
      main: {
        src: ['<%= globalConfig.basePath %>/**/*.tpl.html','!<%= globalConfig.basePath %>/**/obj/**/*.tpl.html'],
        dest: '<%= globalConfig.appPath %>/templates.js'
      },
    },
    cssmin: {

    },
    teamcity: {
      options: {
        suppressGruntLog: true,
        status: {
          warning: 'WARNING',
          failure: 'FAILURE',
          error: 'ERROR'
        }
      },
        all: {}
    }


  });

  // Load the plugins that provide the tasks.
  grunt.loadNpmTasks('grunt-contrib-uglify');
  grunt.loadNpmTasks('grunt-contrib-concat');
  grunt.loadNpmTasks('grunt-contrib-clean');
  grunt.loadNpmTasks('grunt-usemin');
  grunt.loadNpmTasks('grunt-string-replace');
  grunt.loadNpmTasks('grunt-html2js');
  grunt.loadNpmTasks('grunt-contrib-cssmin');
  grunt.loadNpmTasks('grunt-contrib-jshint');
  grunt.loadNpmTasks('grunt-teamcity');

  // Default task(s).
  grunt.registerTask('teamcity', ['teamcity', 'concat']);
  grunt.registerTask('concatClean', ['concat', 'clean:jsFiles', 'clean:cssFiles', 'clean:htmlFiles']);
  grunt.registerTask('forceClean', 'by default will force on a clean', function() {
    var task = ['concatClean'];
    grunt.option('force', true);
    grunt.task.run(task);
  });
  grunt.registerTask('publish', ['clean:build', 'string-replace', 'html2js', 'useminPrepare', 'forceClean', 'uglify', 'cssmin', 'usemin']);
  grunt.registerTask('default', ['jshint:local', 'html2js', 'copyDependencies']);
  
  grunt.registerTask('localMinify', ['clean:build', 'html2js', 'string-replace', 'useminPrepare', 'uglify', 'cssmin', 'usemin']);

  //grunt function to copy the dependencies in the package.json
  //and copy the dependencies from source to locl lib folder
  grunt.registerTask('copyDependencies', function() {
    
    //read depedencies list from the package.json
    var dependencies = grunt.config.get('pkg.dependencies');
    var desitnationFolder = grunt.config.get('globalConfig.basePath') + 'libs/';
    var sourceFolder = "..\\..\\..\\..\\Web\\External\\"

    for (var depend_it in dependencies){

      var versionNumber = dependencies[depend_it];

      //grunt.log.write(versionNumber);
      
      var toSearch = getToSearch(sourceFolder, depend_it, versionNumber, 'js');
      copyFiles(depend_it, toSearch, desitnationFolder, versionNumber);
      
      toSearch = getToSearch(sourceFolder, depend_it, versionNumber, 'map');
      copyFiles(depend_it, toSearch, desitnationFolder, versionNumber);

    }
   
  });

  function copyFiles(depend_it_file, to_Search, desitnationFolder, versionNumber){

      var dependencyName = depend_it_file;

      //get files in the folder source folder
      var files = grunt.file.expand(to_Search);
      grunt.file.mkdir(desitnationFolder + depend_it_file);

      for(var file in files){
        var fullFileName = files[file];
        var values = fullFileName.split('/');
        var fileName = values[values.length - 1];
        var destinationFile = desitnationFolder + '/' + depend_it_file + '/' + fileName;

        if (!doesDependencyExist(destinationFile)){
          grunt.file.copy(files[file], destinationFile);  
        }
        
      }

  }

  function doesDependencyExist(destinationFile){
      return grunt.file.exists(destinationFile)
  }

  function getToSearch(sourceFolder, depend_it, versionNumber, extention){
    
    if (versionNumber == '0.0.0'){
        return sourceFolder + depend_it + '\\lib\\*.' + extention;
    }
    else
    {
        return sourceFolder + depend_it + '\\lib\\*' + versionNumber + '*.' + extention;
    }
    
  }

};