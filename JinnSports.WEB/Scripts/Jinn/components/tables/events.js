'use strict';

function eventsView(model) {
    this.time = -1;

    this.sportTypeId = 1;

    this.table = new $j_table({
        $j_tableId: "eventsTable",

        $j_container: "$j_table-component",

        $j_ajax: {
            url: '/api/Event/LoadEvents?sportTypeId=' + this.sportTypeId + '&time=' + this.time,
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

    this.sportTypeSelector = new $j_selector({
        $j_selectorId: "eventsSelector",

        $j_container: "settingsSelector",

        $j_data: {
            info: "Тип спорта",
            options: ["Все", "Футбол", "Баскетбол", "Хоккей"],
            values: ["0", "1", "2", "3"]
        },
    });

    this.timeSelector = new $j_selector({
        $j_selectorId: "sportTypeSelector",

        $j_container: "settingsSelector",

        $j_data: {
            info: "Время",
            options: ["Все", "Прошедшие", "Будущие"],
            values: ["-1", "0", "1"]
        },
    });
}

_.extend(eventsView.prototype, {
    build: function () {
        var self = this;

        var sportTypeSelector = document.getElementById(this.sportTypeSelector.$j_selectorId);
        sportTypeSelector.onchange = function () {
            self.changeSportType(sportTypeSelector);
            this.table.$j_ajax.modify(self.sportTypeId, self.time);
            this.table.update();
        }

        var timeSelector = document.getElementById(this.timeSelector.$j_selectorId);
        timeSelector.onchange = function () {
            self.changeTime(timeSelector);
            this.table.$j_ajax.modify(self.sportTypeId, self.time);
            this.table.update();
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

    init: function () {
        this.build();
    },

    show: function () {
        this.table.show();
    },

    hide: function () {
        console.log('hide' + this);
        this.table.hide();
    },

    render: function () {
        alert('render');
    }
});