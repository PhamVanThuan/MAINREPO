angular.module('sahl.core.app.services', ['SAHL.Services.Interfaces.Capitec.sharedmodels']).
factory('$serialization', ['$capitecSharedModelsContainer', function ($capitecSharedModelsContainer) {
    var serialization = {
        deserialize: function (object) {
            var objectType = Object.prototype.toString.call(object);
            if (objectType === '[object Array]') {
                for (var i = 0, c = object.length; i < c; i++) {                                            
                    object[i] = serialization.deserialize(object[i]);              
                }
            }
            else if (objectType === '[object Object]') {
                if (object && object["_name"]) {
                    var newInstancedObject = serialization.createObjectFromContainer(object["_name"]);
                    for (var property in object) {
                        if (newInstancedObject[property] !== 'undefined') {                            
                            newInstancedObject[property] = serialization.deserialize(object[property]);                            
                        } else {
                            newInstancedObject[property] = object[property];
                        }
                    }
                    return newInstancedObject;
                }
            }
            return object;
        },
        createObjectFromContainer : function(objectTypeName){
            if ($capitecSharedModelsContainer.Container[objectTypeName] !== 'undefined') {
                var fn = $capitecSharedModelsContainer.Container[objectTypeName];
                if (typeof fn === 'function') {
                    return new fn();
                }
            }
            return null;
        }
    };

    return {
        deserialize: serialization.deserialize
    }
}]);