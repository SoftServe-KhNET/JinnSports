'use strict';

function eventsView(model) {
    this.time = -1;

    this.sportTypeId = 1;

    this.table = function (sportTypeId, time) {
        return new $j_table({
            $j_tableId: "teamsTable",

            $j_container: "$j_table-component",

            $j_ajax: {
                url: '/api/Event/LoadEvents?sportTypeId=' + sportTypeId + '&time=' + time,
                modify: function (id, time) {
                    this.url = '/api/Event/LoadEvents?sportTypeId=' + id + '&time=' + time;
                },
            },

            $j_settings: {

                pagination:
                    {
                        next: "Следующая",
                        prev: "Предыдущая",
                        first: "",
                        last: "",
                        infoFormat: "Show __$j_START__ to __$j_FINISH__ from __$j_COUNT__",
                    },
                selector:
                    {
                        info: "Show __$j_Selector__ records",
                        options: ["10", "20", "30", "40"]
                    }
            },
            $j_data: {
                headers: ["Number", "Team Name"]
            }
        }, model);
    }
    this.sportTypeSelector = function () {
        return new $j_selector({
            $j_selectorId: "eventsSelector",

            $j_container: "settingsSelector",

            $j_data: {
                info: "Тип спорта",
                options: ["Все", "Футбол", "Баскетбол", "Хоккей"],
                values: ["0", "1", "2", "3"]
            },
        });
    }

    this.timeSelector = function () {
        return new $j_selector({
            $j_selectorId: "sportTypeSelector",

            $j_container: "settingsSelector",

            $j_data: {
                info: "Время",
                options: ["Все", "Прошедшие", "Будущие"],
                values: ["-1", "0", "1"]
            },
        });
    }
    this.init();
};
    _.extend(eventsView.prototype, {
        build: function () {
            var self = this;

            var $j_sportTypeSelector = new this.sportTypeSelector;
            var $j_timeSelector = new this.timeSelector;
          
            var table = new this.table(this.sportTypeId, this.time);

            var sportTypeSelector = document.getElementById($j_sportTypeSelector.$j_selectorId);
            sportTypeSelector.onchange = function () {
                self.changeSportType(sportTypeSelector);
                table.$j_ajax.modify(self.sportTypeId, self.time);
                table.update();
            }

            var timeSelector = document.getElementById($j_timeSelector.$j_selectorId);
            timeSelector.onchange = function () {
                self.changeTime(timeSelector);
                table.$j_ajax.modify(self.sportTypeId, self.time);
                table.update();
            }
        },

        changeSportType: function (sender) {
            var selected_index = document.getElementById(sender.id).selectedIndex;
            if (selected_index => 0) {
                var selected_option_value = document.getElementById(sender.id).options[selected_index].value;
                this.sportTypeId = selected_option_value;
            }
            else {
                alert('non selected');
            }
        },

        changeTime: function (sender) {
            var selected_index = document.getElementById(sender.id).selectedIndex;

            if (selected_index => 0) {
                var selected_option_value = document.getElementById(sender.id).options[selected_index].value;
                this.time = selected_option_value;
            }
            else {
                alert('non selected');
            }
        },

        init: function() {
            this.build();
        },

        render: function()
        {
            alert('render');
        }
    });
