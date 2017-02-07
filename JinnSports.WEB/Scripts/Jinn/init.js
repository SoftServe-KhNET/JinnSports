(function () {
    'use strict';

    var Jinn = null;

    // On page loaded
    $(function () {
        // Mapping views to routes
        var routeMap = {
            '#': ['view1', 'view2'],

            '#news': ['view3']
        };

        Jinn = new JinnApp('body');
        _.l(Jinn);
        var model = new Jinn.Model({ lol: 'Lol' });
        var model1 = new Jinn.Model({ kek: 'kekdata' });
        _.extend(model1, {
            get: function () {

            }
        });
        var view = new View(model);
        _.l(model);
        _.l(model1);
    });
})();