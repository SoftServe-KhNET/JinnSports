(function (global) {
    'use strict';

    var _ = global._ || {};

    var uniqueIdcounter = 0;

    function remove(array, index) {
        array.splice(index, 1);
    }

    _.remove = remove;

    function getUniqueId() {
        uniqueIdcounter++;
        return uniqueIdcounter;
    }

    _.getUniqueId = getUniqueId;

    function l(obj) {
        console.log(obj);
    }

    _.l = l;

    global._ = _;
}(this));