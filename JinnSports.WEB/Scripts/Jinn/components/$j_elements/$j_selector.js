'use strict';

function $j_selector(additionalProps)
{
    var props = {

        build: function () {
            var selectorContainer = document.getElementsByClassName(this.$j_container)[0];
            if (!selectorContainer) {
                if (this.$j_container) {
                    var selectorContainer = document.createElement('div');
                    selectorContainer.setAttribute('class', this.$j_container);
                }
            }
            var selectorLabel = document.createElement('label');
            selectorLabel.setAttribute('id', this.$j_selectorId + 'Label');

            var selector = document.createElement('select');
            selector.setAttribute('class', 'select-style');
            selector.setAttribute('id', this.$j_selectorId);

            function addOption(oList, optionName, optionValue) {
                var oOption = document.createElement("option");
                oOption.appendChild(document.createTextNode(optionName));
                oOption.setAttribute("value", optionValue);
                oList.appendChild(oOption);
            }

            for (var option = 0; option < this.$j_data.options.length; option++) {
                if (this.$j_data.values && this.$j_data.values[option]) {
                    addOption(selector, this.$j_data.options[option], this.$j_data.values[option]);
                } else {
                    addOption(selector, this.$j_data.options[option], this.$j_data.options[option]);
                }
            }

            if (this.$j_data.info.includes("__$j_Selector__")) {
                selectorLabel.innerHTML = this.$j_data.info.split("__$j_Selector__")[0];
                selectorLabel.appendChild(selector);
                selectorLabel.innerHTML += this.$j_data.info.split("__$j_Selector__")[1];
            } else {
                selectorLabel.innerHTML = this.$j_data.info;
                selectorLabel.appendChild(selector);
            }
            if (selectorContainer) {
                selectorContainer.appendChild(selectorLabel);
                document.body.appendChild(selectorContainer);
            } else {
                document.body.appendChild(selectorLabel);
            }
        },

        show: function(){
            this.build();
        },

        remove: function()
        {
            var selector = document.getElementById(this.$j_selectorId + 'Label');
            if (selector) {
                selector.remove();
            }
        }
       
    }
    _.extend(this, props);
    _.extend(this, additionalProps);
};