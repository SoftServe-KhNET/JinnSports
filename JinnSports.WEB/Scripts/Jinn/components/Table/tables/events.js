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
            }
        },
        $j_settings: {
            pagination:
                {
                    next: "Следующая",
                    prev: "Предыдущая",
                    first: "",
                    last: "",
                    infoFormat: "Показать с __$j_START__ по __$j_FINISH__ из __$j_COUNT__",
                },
            selector:
                {
                    info: "Показать __$j_Selector__ записей",
                    options: ["10", "30", "60", "100"]
                }
        },

        $j_data: {
            headers: ["№", "Команда 1", "Счет", "Команда 2", "Дата"],
            records: ["$j_№", "TeamNames.0", "Score", "TeamNames.1", "Date"]
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
    var self = this;

    var viewProps = new View({
        build: function () {
            self.sportTypeSelector.show();
            self.timeSelector.show();

            var selfChild = this;
            var sportTypeSelector = document.getElementById(self.sportTypeSelector.$j_selectorId);
            sportTypeSelector.onchange = function () {
                selfChild.changeSportType(sportTypeSelector);
                selfChild.table.$j_ajax.modify(selfChild.sportTypeId, selfChild.time);
                selfChild.table.update();
            }

            var timeSelector = document.getElementById(self.timeSelector.$j_selectorId);
            timeSelector.onchange = function () {
                selfChild.changeTime(timeSelector);
                selfChild.table.$j_ajax.modify(selfChild.sportTypeId, selfChild.time);
                selfChild.table.update();
            }
        },

        changeSportType: function (sender) {
            var selected_index = document.getElementById(sender.id).selectedIndex;
            if (selected_index => 0) {
                var selected_option_value = document.getElementById(sender.id).options[selected_index].value;
                this.sportTypeId = selected_option_value;
            }
        },

        changeTime: function (sender) {
            var selected_index = document.getElementById(sender.id).selectedIndex;

            if (selected_index => 0) {
                var selected_option_value = document.getElementById(sender.id).options[selected_index].value;
                this.time = selected_option_value;
            }
        },

        init: function () {
            this.build();
            return this;
        },

        show: function () {
            this.build();
            this.table.show();
        },

        hide: function () {
            this.sportTypeSelector.remove();
            this.timeSelector.remove();
            this.table.hide();
        },

        render: function () {
            this.table.render();
        }
    }, model);

    _.extend(viewProps, this);
    return viewProps;
};