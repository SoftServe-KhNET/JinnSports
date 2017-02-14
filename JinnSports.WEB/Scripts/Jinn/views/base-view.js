'use strict';

var View = function (additionalProps) {
    var prefix = 'v';
    this._id = prefix + _.getUniqueId();

    this.events = new EventService();

    this.models = [];
    var argModels = arguments;
    attachModels.call(this, argModels);

    function attachModels(argModels) {
        for (var i = 1; i < argModels.length; i++) {
            if (argModels[i] instanceof Model) {
                this.models.push(argModels[i]);
            }
        }
    }

    _.extend(this, additionalProps);

    this.init();
}

_.extend(View.prototype, {

    init: function () {
        this.setupHandlers()
            .enable();
    },

    setupHandlers: function () {
        this.modelUpdateHandler = this.render.bind(this);

        return this;
    },

    enable: function () {
        this._subscribeToModels.apply(this, this.models);

        return this;
    },

    addModels: function () {
        for (var i = 0; i < arguments.length; i++) {
            if (arguments[i] instanceof Model) {
                this.models.push(arguments[i]);
            }
        }
        this._subscribeToModels.apply(this, arguments);
    },

    _subscribeToModels: function () {
        for (var i = 0; i < arguments.length; i++) {
            if (arguments[i] instanceof Model) {
                arguments[i].events.registerListener(
                    EventService.messages.MODEL_HAS_BEEN_UPDATED,
                    this.modelUpdateHandler,
                    this);
            }
        }
    },

    render: function () { },

    remove: function () { },

    show: function () { },

    hide: function () { }
});