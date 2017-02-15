(function() {
    'use strict';

    var jinn = null;

    // On page loaded
    document.addEventListener('DOMContentLoaded', function () {

        jinn = new JinnApp();

        var mtable = new tableModel();
        //var authModel = new authModel();
        //var authView = new AuthView(authModel);
        var tview = new teamsView(mtable);
        var eview = new eventsView(mtable);

        jinn._addElements(mtable, tview, eview);
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
                tview.show();
            },

            results: function () {
                this.hideAllViews();
                eview.show();
                authView.show();
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