(function() {
    'use strict';

    var jinn = null;

    // On page loaded
    document.addEventListener('DOMContentLoaded', function() {
        jinn = new JinnApp();

        var authModel = new authModel();
        var authView = new AuthView(authModel);

        jinn._addElements(authView, authModel);

        jinn.router = new jinn.Router({

            map: {
                '': 'index'
            },

            index: function () {
                this.hideAllViews();
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