'use strict';

function teamsView(model) {
    this.table = new $j_table({
                $j_tableId: "teamsTable",

                $j_container: "$j_table-component",

                $j_ajax: {
                    url: '/api/Team/LoadTeams',
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
                        info: "Показать __$j_Selector__ Записей",
                        options: ["10", "20", "30", "40"]
                    }
                },

                $j_data: {
                    headers: ["№", "Team Name"],
                    records: ["$j_№", "Name"]
                }
            }, model);

    var self = this;

    var viewProps = new View({

        build: function () {
            self.table.update();
        },

        init: function () {
            this.build();
            return this;
        },

        show: function () {
            self.table.show();
        },

        hide: function () {
            self.table.hide();
        },

        render: function () {
            self.table.render();
        }
    }, model);

    _.extend(viewProps, this);
    return viewProps;
};