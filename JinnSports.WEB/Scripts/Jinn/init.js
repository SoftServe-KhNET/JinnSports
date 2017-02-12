(function () {
    'use strict';

    var jinn = null;

    // On page loaded
    document.addEventListener('DOMContentLoaded', function () {

        jinn = new JinnApp();

        var teams = new tableModel();
        //var view1 = new teamsView(teams);

        //var tview = new tableView(teams);
        //var tview1 = new teamsView(teams);
        var eview1 = new eventsView(teams);

        jinn.Model = teams;
        jinn.View = eview1;

        //tview1.init();

        //console.log(tview);
        console.log(eview1);
    });
})();