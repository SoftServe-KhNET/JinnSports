'use strict';

function eventsView(model) {

    this.table = new $j_table({
            $j_tableId: "teamsTable",
            $j_ajax: {
                url: "/api/Team/LoadTeams"
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

    this.selector = new $j_selector(
        {
            $j_selectorId: "eventsSelector",

            $j_data: {
                info: "Please input time",
                options: ["101", "202", "303", "40"]
            },

            $j_events:{
                onclick: function b() { alert('event')}
                }
        });
    var self = this;

    (function init() {
        self.selector.init();
        self.table.init();
    })();
}