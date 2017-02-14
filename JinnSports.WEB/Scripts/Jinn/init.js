(function () {
    'use strict';

    var jinn = null;

    // On page loaded
    document.addEventListener('DOMContentLoaded', function () {

        jinn = new JinnApp();

        var teams = new tableModel();
        var tview1 = new teamsView(teams);
        var eview1 = new eventsView(teams);

        jinn._addElements(teams, tview1, eview1);
        jinn.router = new jinn.Router({

            map: {
                '': 'index',
                '#teams': 'teams',
                '#results': 'results'
            },

            index: function () {
                this.hideAllViews();
            },

            teams: function () {
                this.hideAllViews();
                tview1.show();
            },

            results: function () {
                this.hideAllViews();
                eview1.show();
            },

            hideAllViews: function () {
                var views = jinn._views;
                for (var viewId in views) {
                    if (views.hasOwnProperty(viewId)) {
                        views[viewId].hide();
                    }
                }
            }

        });

        jinn._run('#');
    });
})();