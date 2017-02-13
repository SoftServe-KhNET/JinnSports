'use strict';

function EventService() {
    this._listeners = [];
};

_.extend(EventService.prototype, {

    registerListener: function(messageType, listener, context) {
        var existingListeners = this._listeners[messageType];

        if (!existingListeners) {
            existingListeners = this._listeners[messageType] = [];
        }

        existingListeners.push({
            listener: listener,
            context: context
        });
    },

    unregisterListener: function(messageType, listener) {
        var messageListeners = this._listeners[messageType];

        if (messageListeners !== void 0) {
            for (var i = 0; i < messageListeners.length; i++) {
                if (messageListeners[i].listener === listener) {
                    _.remove(messageListeners, i);
                }
            }
        }
    },

    sendMessage: function(messageType, data) {
        var messageListeners = this._listeners[messageType];

        if (messageListeners !== void 0) {
            for (var i = 0; i < messageListeners.length; i++) {
                messageListeners[i].listener(messageListeners[i].context, data);
            }
        }
    }
});

EventService.on = EventService.registerListener;
EventService.off = EventService.unregisterListener;
EventService.notify = EventService.sendMessage;

EventService.messages = {
    MODEL_HAS_BEEN_UPDATED: 'ModelHasBeenUpdated',
    MODEL_HAS_BEEN_DESTROYED: 'ModelHasBeenDestroyed'
}