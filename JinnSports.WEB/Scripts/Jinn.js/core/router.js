'use strict';

function Router(additionalProps) {
    this.map = {};

    _.extend(this, additionalProps);

    this.init();
}

_.extend(Router.prototype, {

    init: function() {
        return this;
    },

    _navigate: function() {
        var self = this;
        if (this.map.hasOwnProperty(location.hash)) {
            var funcName = this.map[location.hash];
            if (this.hasOwnProperty(funcName)) {
                this[funcName].call(self);
            } else {
                _.l('No func for route: ' + location.hash);
            }
        } else {
            _.l('No such route: ' + location.hash);
        }
    },

    _run: function(route) {
        var self = this;
        window.onhashchange = function() {
            self._navigate();
            return false;
        };

        if (!route || typeof route !== 'string') {
            route = '';
        }
        location.hash = route;
        window.onhashchange();
    }
});