(function () {
    'use strict';

    var jinn = null;

    // On page loaded
    document.addEventListener('DOMContentLoaded', function () {

        jinn = new JinnApp();

        var model1 = new adminModel();
        var view1 = new adminView(model1);

        jinn.Model = model1;
        jinn.View = view1;

        //view1.init();

        //console.log(jinn);
    });
})();