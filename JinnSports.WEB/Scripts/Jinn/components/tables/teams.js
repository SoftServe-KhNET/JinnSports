'use strict';

function teamsView(model) {
    this.table = new $j_table(
            {
                $j_tableId: "teamsTable",

                $j_container: "$j_table-component",

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
            },
            model);
}

_.extend(teamsView.prototype,
    {
        build: function () {
            this.table.update();
        },

        init: function () {
            this.build();
        },

        show: function () {
            this.table.show();
        },

        hide: function () {
            this.table.hide();
        },
    });