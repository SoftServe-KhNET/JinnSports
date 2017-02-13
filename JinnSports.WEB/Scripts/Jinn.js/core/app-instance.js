'use strict';

function JinnApp() {
    this._models = {};
    this._views = {};

    // Global event bus
    this.events = new EventService();

    // Router
    this.router = null;

    // TODO: change func to add created obj to array
    this.Model = Model;
    this.View = View;
    this.Router = Router;

    // TODO: inheritance
    this.init.call(this);
};

_.extend(JinnApp.prototype, {
    init: function() {
        this._analizeDom()
            ._initializeModels()
            ._initializeViews();
    },

    _analizeDom: function() {
        // TODO: analize DOM of document and find elements with specifyied 'data-' attr

        return this;
    },

    _initializeModels: function() {
        // TODO: analize founded elements and create models for them

        return this;
    },

    _initializeViews: function() {
        // TODO: map elements and theirs setting to views using this._models

        return this;
    },

    _run: function(route) {
        if (this.router && this.router instanceof this.Router) {
            this.router._run(route);
        } else {
            _.l('Router not found');
        }

        return this;
    },

    _addElements: function() {
        var elements = arguments;
        var self = this;
        for (var i = 0; i < elements.length; i++) {
            if (elements[i] instanceof View) {
                self._views[elements[i]._id] = elements[i];
            } else if (elements[i] instanceof Model) {
                self._models[elements[i]._id] = elements[i];
            }
        }

        return this;
    }
});