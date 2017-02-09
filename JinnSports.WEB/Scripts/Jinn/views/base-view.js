'use strict';

var View = function () {
    alert('init view constr)');
    this.models = [];
    var argModels = arguments;
    attachModels.call(this, argModels);

    function attachModels(argModels) {
        for (var i = 0; i < argModels.length; i++) {
            if (argModels[i] instanceof Model) {
                this.models.push(argModels[i]);
            }
        }
    }

    this.events = new EventService();

    this.init();
}

View.prototype = {

    init: function () {
        alert('EventInit');
        this.createChildren()
            .setupHandlers()
            .enable();
    },

    createChildren: function () {
        this.$button = $('.button');

        return this;
    },

    setupHandlers: function () {
        this.modelUpdateHandler = this.render.bind(this);

        return this;
    },

    enable: function () {
        for (var i = 0; i < this.models.length; i++) {
            this.models[i].events.registerListener(EventService.messages.MODEL_HAS_BEEN_UPDATED,
                this.modelUpdateHandler,
                this);
        }
        this.$button.click(this.events.sendMessage(EventService.messages.MODEL_UPDATE_REQUEST, null));

        return this;
    },

    render: function () {
        console.log(this.models[0].get());
    }
};