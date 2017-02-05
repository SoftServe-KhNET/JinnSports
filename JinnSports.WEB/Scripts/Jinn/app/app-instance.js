'use strict';

function Application() {
    name = "AppObject";

    // Binding object to DOM element
    this.bindModel = function(obj, domElement) {
        // if (domElement.nodeType !== 1 && domElement.nodeType !== 8) {
        //     throw Error(bindModelNodeError);
        // }

        var binder = bindModelToNodeInternal(obj, domElement);
        return binder;
    };

    function getBindingContext(domElement) {

    };

    function bindModelToNodeInternal(obj, bindingContext) {
        var pubSub = {
            callbacks: {},

            on: function(msg, callback) {
                // Checking on undefined
                this.callbacks[msg] = this.callbacks[msg] || [];
                // Subscribing callback function
                this.callbacks[msg].push(callback);
            },

            publish: function(msg) {
                // Checking on undefined
                this.callbacks[msg] = this.callbacks[msg] || [];
                var i = 0;
                var length = this.callbacks[msg].length;
                for (i; i < length; i++) {
                    this.callbacks[msg][i].apply(this, arguments);
                }
            }
        };

        var data_attr = 'data-bind-' + bindingContext;
        var message = bindingContext + ':change';

        changeHandler = function(event) {
            var target = event.target || event.srcElement;
            var prop_name = target.getAttribute(data_attr);

            if (prop_name && prop_name !== '') {
                pubSub.publish(message, prop_name, target.value);
            }
        };

        document.addEventListener('change', changeHandler);

        pubSub.on(message, function(event, prop_name, new_val) {
            var elements = document.querySelectorAll('[' + data_attr +
                '=' + prop_name + ']');
            var tag_name;

            for (var i = 0, len = elements.length; i < len; i++) {
                tag_name = elements[i].tagName.toLowerCase();

                if (tag_name === "input" ||
                    tag_name === "textarea" ||
                    tag_name === "select") {
                    elements[i].value = new_val;
                } else {
                    elements[i].innerHTML = new_val;
                }
            }
        });

        return pubSub;
    };
};