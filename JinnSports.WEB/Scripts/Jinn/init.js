(function () {
    'use strict';

    var jinn = null;

    // On page loaded
    document.addEventListener('DOMContentLoaded', function () {

        jinn = new JinnApp();

        var model1 = new tableModel();
        var view1 = new tableView(model1);

        jinn.Model = model1;
        jinn.View = view1;

        view1.init();

        console.log(jinn);
    });
})();