var TableComponent = function () {
    alert('table constructor');
    var model = new tableModel();
    var view = new tableView(model);
}