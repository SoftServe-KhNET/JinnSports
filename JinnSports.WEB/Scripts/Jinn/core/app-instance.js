'use strict';

function JinnApp() {

    this.data = {};
    this.models = {};
    this.views = {};

    // Global event bus
    this.events = new EventService();

    this.Model = Model;
    this.View = View;

    //this.init();
};

_.extend(JinnApp.prototype, {

    init: function () {
        this._analizeDom()
            ._initializeModels()
            ._initializeViews();
    },

    _analizeDom: function () {
        // TODO: analize DOM of document and find elements with specifyied 'data-' attr

        return this;
    },

    _initializeModels: function () {
        // TODO: analize founded elements and create models for them

        return this;
    },

    _initializeViews: function () {
        // TODO: map elements and theirs setting to views using this._models

        return this;
    }
});