'use strict';

function teamsView(model) {
    this.table = new $j_table(
        {
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
                selector: ["10", "20", "30", "40"]
            },
            $j_data: {
                headers: ["Number", "Team Name"]
            }
        }, model);
    var self = this;

    (function init() {
        self.$j_table.init();
    })();
}