'use strict';

function EventService() {
    this._listeners = [];
};

EventService.prototype = {

    registerListener: function (messageType, listener, thisArg) {
        var existingListeners = this._listeners[messageType];

        if (!existingListeners) {
            existingListeners = this._listeners[messageType] = [];
        }

        existingListeners.push({
            listener: listener,
            thisArg: thisArg
        });
    },

    unregisterListener: function (messageType, listener) {
        var messageListeners = this._listeners[messageType];

        if (messageListeners !== void 0) {
            for (var i = 0; i < messageListeners.length; i++) {
                if (messageListeners[i].listener === listener) {
                    ArrayExtensions.remove(messageListeners, i);
                }
            }
        }
    },

    sendMessage: function (messageType, data) {
        var messageListeners = this._listeners[messageType];

        if (messageListeners !== void 0) {
            for (var i = 0; i < messageListeners; i++) {
                messageListeners[i].listener.call(messageListeners[i].thisArg, data);
            }
        }
    }
};

EventService.messages = {
    MODEL_UPDATE_REQUEST: 'ModelUpdateRequest',
    MODEL_HAS_BEEN_UPDATED: 'ModelHasBeenUpdated',
    VIEW_RENDER_REQUEST: 'ViewRenderRequest'
};