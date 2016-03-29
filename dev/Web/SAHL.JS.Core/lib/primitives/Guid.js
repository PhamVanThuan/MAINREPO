'use strict';
angular.module('sahl.js.core.primitives', [])
.service('$guidService', [
function () {
    var internal = {
        LongToByteArray: function (longvalue) {
            var byteArray = [0, 0, 0, 0, 0, 0, 0, 0];

            for (var index = 0; index < byteArray.length; index++) {
                var abyte = longvalue & 0xff;
                byteArray[index] = abyte;
                longvalue = (longvalue - abyte) / 256;
            }
            return byteArray;
        },
        BytesToHex : function(bytes){
            for (var hex = [], i = 0; i < bytes.length; i++) {
                hex.push((bytes[i] >>> 4).toString(16));
                hex.push((bytes[i] & 0xF).toString(16));
            }
            return hex.join("");
        },
        guidFromPattern: function (pattern) {
            return pattern.replace(/[xy]/g, function (c) {
                var r = Math.random() * 16 | 0,
                    v = c === 'x' ? r : (r & 0x3 | 0x8);
                return v.toString(16);
            });
        }
    };
    var operations = {
        newGuid : function(){
            var guidPattern = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx';
            return internal.guidFromPattern(guidPattern);
        },
        newComb: function () {
            var combPattern = 'xxxxxxxx-xxxx-4xxx-yxxx-';
            var comb = internal.guidFromPattern(combPattern);

            var days = moment().diff(moment([1900, 1, 1]), 'days');
            var daysBytes = internal.LongToByteArray(days).reverse();
            daysBytes = daysBytes.slice(daysBytes.length - 2);
            var daysHex = internal.BytesToHex(daysBytes);
            var msecs = Math.floor(moment().diff(moment().startOf('day')) / 3.333333);
            var msecsBytes = internal.LongToByteArray(msecs).reverse();
            msecsBytes = msecsBytes.slice(msecsBytes.length - 4);
            var msecsHex = internal.BytesToHex(msecsBytes);
            comb = comb + daysHex + msecsHex;
            return comb;
        }
    };
    return {
        newGuid: operations.newGuid,
        newComb : operations.newComb
    };
}]);
