$(document).ready(function () {
    //console.log(model);
    var model = new Model();
    var view = new View(model);
       

    view.init(view);
    console.log(view.model.getData());
    //view.model.prototype.updateData('https://localhost:44300/api/Conformity')
    

    //controller.saveClickListener();
    //controller.getData('http://www.json-generator.com/api/json/get/copkCCozma?indent=2', controller.model.setData, controller.initialize, controller.tableClickListener, controller.classifyData);

}
)