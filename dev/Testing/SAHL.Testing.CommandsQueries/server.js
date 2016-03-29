var http = require("http"),
    path = require("path"),
    express = require("express"),
    config = require("./config.json"),
    servicesConfig = new Array(),
    fs = require("fs"),
    port = process.argv[2] || 3000,
    readline = require("readline"),
    bodyParser = require('body-parser'),
    request = require('request'),
    url =  require('url'),
    app = express(); 

app.use(bodyParser.json());


function configRouteForFile(route, filePath, contentType, beforeResponse) {
  configRoute(route, contentType, function(reqData,response,done){
     fs.readFile(filePath, function read(err, data) {
      if (err) {
          throw err;
      }
      if(beforeResponse != undefined){
        beforeResponse(function(){
          response(data);
          done();
        });
      } else {
        response(data);
        done();
      }
    });
  });
}

function configRoute(route, contentType, callback){
  app.use(route, function(req, res) {
    res.setHeader('Content-Type', contentType);
    res.setHeader("Access-Control-Allow-Origin", "*");
    res.writeHead(200);
    var resData = "";
    callback(req.body,function(data){
      resData += data;
    },function(){
      res.end(resData);
    });
  });
}

configRoute('/performCommandOrQuery','application/json',function(reqBody,response,done){

    var paramValues = new Array();
    paramValues.push(null);

    var commandQuery = reqBody;
    for (var j = commandQuery.params.length - 1; j >= 0; j--) {
      var param = commandQuery.params[j];
      paramValues.push(param.value);
    };
    var func = new Function("return "+commandQuery.func);
    var commandOrQuery = func();
    var instance = new (Function.prototype.bind.apply(commandOrQuery, paramValues));
    var uri = url.parse(commandQuery.postUrl);

    var data = JSON.stringify(instance);
    var headers = {
      'Content-Type': 'application/json',
      'Content-Length': Buffer.byteLength(data)
    };

    var options = {
      host: uri.host,
      port: 80,
      path: uri.path,
      method: 'POST',
      headers: headers
    };
    
    console.log('request made:',options);
    var req = http.request(options, function(res) {
      var result = '';

      res.on('data', function(chunk) {
        result += chunk;
      });

      res.on('end', function() {
        commandQuery.response = result;
        response(JSON.stringify(commandQuery));
        done();
      });
    });

    req.on('error', function(err) {
      commandQuery.response = err;
      response(JSON.stringify(commandQuery));
      done();
    });

    req.write(data);
    req.end();
});

for (var i = config.js.length - 1; i >= 0; i--) {
    var jsFilespec = config.js[i];
    configRouteForFile('/' + jsFilespec, jsFilespec,'text/javascript');
};
for (var i = config.css.length - 1; i >= 0; i--) {
    var cssFilespec = config.css[i];
    configRouteForFile('/' + cssFilespec, cssFilespec,'text/css');
};

configRouteForFile('/services','index.html','text/html');
configRouteForFile('/routeConfig', 'config.json',"application/json", function(done){
      console.log('saving config.json');
      config.services = new Array();
      for (var i = servicesConfig.length - 1; i >= 0; i--) {
          var serviceConfig = servicesConfig[i];
          config.services.push(serviceConfig);
          configRouteForFile('/'+ serviceConfig.route, serviceConfig.filespec,"text/javascript");
      };
      fs.writeFile('config.json', JSON.stringify(config,null,4), function (err) {
          if (err) {
            return console.log(err);
          }
          done();
      });
});

var rd = readline.createInterface({
    input: fs.createReadStream('filespecs.txt'),
    output: process.stdout,
    terminal: false
});

rd.on('line', function(line) {

  var pathSplit = line.split('/');
  var fileName = pathSplit[pathSplit.length-1];
  var fileNameSplit = fileName.split(".");
  var serviceName = fileNameSplit[fileNameSplit.length-3]+"Service";

  var serviceConf = {
    name: serviceName,
    route: "",
    filespec:line
  };

  //Get Commands
  if (line.indexOf(".Commands")> -1){
    serviceConf.route = serviceName + '/getCommands';
    servicesConfig.push(serviceConf);
  }
  
  //Get Queries
  if (line.indexOf(".Queries") > -1){
    serviceConf.route = serviceName + '/getQueries';
    servicesConfig.push(serviceConf);
  }

    //Get Queries
  if (line.indexOf(".SharedModels") > -1){
    serviceConf.route = serviceName + '/getModels';
    servicesConfig.push(serviceConf);
  }
});

http
  .createServer(app)
  .listen(port);