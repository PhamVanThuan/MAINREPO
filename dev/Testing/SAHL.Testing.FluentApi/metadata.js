function generateUUID(){
    var d = new Date().getTime();
    var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = (d + Math.random()*16)%16 | 0;
        d = Math.floor(d/16);
        return (c=='x' ? r : (r&0x3|0x8)).toString(16);
    });
    return uuid;
};
function traverseMetadataUI(metadataUI,callback){
  for (var i = 0; i < metadataUI.children.length; i++) {
    var child = metadataUI.children[i];
    callback(child);
    traverseMetadataUI(child,callback);
  };
}

function MetadataUI(metadata, parentMetadataUI, isVisible){

   var thisObj = this;
   this.image = "listItem.png";
   this.name = metadata.name;
   this.hide = function(){
     thisObj.isVisible = false;
   }
   this.show = function(){
     thisObj.isVisible = true;
     console.log('showing ',thisObj.name);
     if (thisObj.parent !== undefined && thisObj.parent !== null){
        thisObj.parent.show();
     }
   }
   this.enableDisable = function(value) {
       if (value !== undefined){
           thisObj.enabled = value;
           console.log('enabling ',thisObj.name);
           if (thisObj.parent !== undefined && thisObj.parent !== null){
                thisObj.parent.enableDisable(true);
           }
       }else{
           if (thisObj.enabled)
           {
             console.log('enabling ',thisObj.name);
             if (thisObj.parent !== undefined && thisObj.parent !== null){
                thisObj.parent.enableDisable(true);
             }
           }else{
              console.log('was disabled');
              for (var i = thisObj.children.length - 1; i >= 0; i--) {
                var child = thisObj.children[i];
                if (child.enabled){
                  thisObj.enabled = true;
                }
              };
           }
       }
     
   }
   this.show();
   this.user = { inputOutput:"", description:"" };
   this.metadata = metadata;
   this.children = new Array();
   this.parent = parentMetadataUI;

   if (metadata.isFunction){
      this.namePrefix = "Function: ";
      this.nameSuffix = "()"; 
      this.user.description = "Results:";
   }

   if (metadata.isObject){
      this.namePrefix = "Object: ";
      this.nameSuffix = "";
      this.user.description = "Filter:";
   }

   if (metadata.isFunctionParameter){
      this.namePrefix = "Parameter: ";
      this.nameSuffix = "";
      this.user.description = "Value:";
   }

   this.change = function(){
      if (thisObj.metadata.isObject){
        var filter = thisObj.user.inputOutput;
        for (var i = thisObj.children.length - 1; i >= 0; i--) {
           var child = thisObj.children[i];
           if (child.name.indexOf(filter) == -1){
              child.hide();
           }else{
            child.show();
           }
        };
      }
   }
   this.canAppend = function(results){
     if (typeof results === "string"){
        return false;
     }
     for (var i = thisObj.metadata.metadata.length - 1; i >= 0; i--) {
       var metadata = thisObj.metadata.metadata[i];
        if (metadata.name=="dynamic"){
          return false;
        }
     };
     return true;
   }
   
   this.append = function(results){
      var metadata = thisObj.metadata.describe(results);
      thisObj.metadata.append(metadata);
      var metadataUI = new MetadataUI(metadata,thisObj,true);
      metadataUI.enabled = true;
      thisObj.children.push(metadataUI);
      return metadataUI;
   }

   this.moveUp = function() {
      //find this element in the tree
      if (thisObj.parent === undefined){
        return;
      }
      for (var i = thisObj.parent.children.length - 1; i >= 0; i--) {
        var child = thisObj.parent.children[i];
        if (child.metadata.id == thisObj.metadata.id){
          console.log('found child');
          var previousChildPosition = (i-1);
          if (previousChildPosition > -1 ){
              var prevChild = thisObj.parent.children[i-1];
              thisObj.parent.children[previousChildPosition] = child;
              thisObj.parent.children[i] = prevChild;
              return;
          }
        }
      };
   };

   this.showHideChildren = function() {
      for (var i = thisObj.children.length - 1; i >= 0; i--) {
        var child = thisObj.children[i];
        if (child.isVisible){
          child.hide();
        }else{
           child.show();
        }
      };
   };
  
   var subMetdata =  this.metadata.metadata;
   for (var i = subMetdata.length - 1; i >= 0; i--) {
      this.children.push(new MetadataUI(subMetdata[i],this,true));
   };
}

function Metadata(srcObject, _name, metadataContext) {

   this.description = srcObject;
   this.id = generateUUID();
   this.name = _name;
   this.metadata = new Array();
   this.metadataContext = metadataContext;
   this.isFunction = false;
   this.isObject = false;
   this.isFunctionParameter = false;

   if (typeof this.description === 'function') {
      this.isFunction = true;
      var str = this.description.toString()
                  .replace("(","")
                  .replace("function","");
      var strSplit = str.split(')');
      var args = strSplit[0].split(',');
      if (args.length > 0)
      {
         for (var i = args.length - 1; i >= 0; i--) {
            var argName = args[i].trim();
            if (argName != ""){
               var metaData = new Metadata("", argName, this);
               this.metadata.push(metaData);
            }
         };
      }
   } else if (typeof this.description === 'string') {
      if (this.metadataContext.isFunction){
         this.isFunctionParameter = true;
      }
   } else {
       this.isObject = true;
      for (var propName in this.description) {
         var metaData = new Metadata(this.description[propName], propName, this);
         this.metadata.push(metaData);
      };
   }

   var thisObj = this;
   this.describe = function(results){  
      var metaData = new Metadata(results,'dynamic',thisObj);
      return metaData;
   }
   this.append = function(metadata){  
      thisObj.metadata.push(metadata);
   }
}

angular.module('metadata',[]).directive("metadataui", function(RecursionHelper) {
  return {
        restrict: "E",
        scope: {
            data: '='
        },
        template:'<div ng-show="data.isVisible"><img src="{{data.image}}" ng-click="data.moveUp()"><input ng-model="data.enabled" type="checkbox" ng-change="data.enableDisable()"/><a href ng-click="data.showHideChildren()"><b>{{data.namePrefix}}</b>{{data.name}}{{data.nameSuffix}}</a> <b>, {{data.user.description}} </b><input type="text" ng-model="data.user.inputOutput" ng-readonly="data.metadata.isFunction" ng-change="data.change()"/> <ul> <li ng-repeat="metadataUI in data.children"><metadataui data="metadataUI"></metadataui></li> </ul></div>',
        compile: function(element) {
            return RecursionHelper.compile(element, function(scope, iElement, iAttrs, controller, transcludeFn){
          });
        }
    };
}).factory('metadataUIElements',['$queryServiceRest',function($queryServiceRest){
   var metadata = new Metadata($queryServiceRest.api,'test');
   var metadataUI = new MetadataUI(metadata,null,true);
   return metadataUI;
}]).factory('RecursionHelper', ['$compile', function($compile){
    return {
        compile: function(element, link){
            // Normalize the link parameter
            if(angular.isFunction(link)){
                link = { post: link };
            }

            // Break the recursion loop by removing the contents
            var contents = element.contents().remove();
            var compiledContents;
            return {
                pre: (link && link.pre) ? link.pre : null,
                /**
                 * Compiles and re-adds the contents
                 */
                post: function(scope, element){
                    // Compile the contents
                    if(!compiledContents){
                        compiledContents = $compile(contents);
                    }
                    // Re-add the compiled contents to the element
                    compiledContents(scope, function(clone){
                        element.append(clone);
                    });

                    // Call the post-linking function, if any
                    if(link && link.post){
                        link.post.apply(null, arguments);
                    }
                }
            };
        }
    };
}]);