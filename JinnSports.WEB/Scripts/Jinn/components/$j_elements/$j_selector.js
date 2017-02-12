'use strict';

function $j_selector(additionalProps)
{        
    _.extend(this, additionalProps);
};

_.extend($j_selector.prototype, {
    init: function () {
        this.build()
    },

    build: function () {
            var selectContainer = document.getElementsByClassName('js-select-container')[0];
            var selectorLabel = document.createElement('label');
            console.log(this.$j_data.info);
            //if (this.$j_data.info)
            //selectorLabel.innerHTML = this.$j_data.options[option]
            var selector = document.createElement('select');
            selector.setAttribute('class', 'select-style');
            selector.setAttribute('id', this.$j_selectorId);
            var self = this;
            console.log(self);
            selector.onchange = function () {
                var a = self.$j_events.onclick;
                console.log(a);
                //this.$j_events.onclick();
            }

            function addOption(oList, optionName, optionValue) {
                var oOption = document.createElement("option");
                oOption.appendChild(document.createTextNode(optionName));
                oOption.setAttribute("value", optionValue);
                oList.appendChild(oOption);
            }

            for (var option = 0; option < this.$j_data.options.length; option++) {
                addOption(selector, this.$j_data.options[option], this.$j_data.options[option]);
            }
            selectContainer.appendChild(selector);
    }
});