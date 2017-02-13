(function(global) {
    'use strict';

    var _ = global._ || {};

    var uniqueIdcounter = 0;

    // Removes element at index position from array
    function remove(array, index) {
        array.splice(index, 1);
    }

    _.remove = remove;

    // Returns unique Id in scope of whole window
    function getUniqueId() {
        uniqueIdcounter++;
        return uniqueIdcounter;
    }

    _.getUniqueId = getUniqueId;

    function l(obj) {
        console.log(obj);
    }

    _.l = l;

    // Extends obj from properties of other args
    function extend(obj) {
        var length = arguments.length;

        if (length < 2 || obj == null) {
            return obj;
        }

        var dst = obj;

        for (var i = 1; i < length; i++) {
            var arg = arguments[i];

            for (var key in arg) {
                if (arg.hasOwnProperty(key)) {
                    dst[key] = arg[key];
                }
            }
        }

        return dst;
    }

    _.extend = extend;

    global._ = _;
}(this));