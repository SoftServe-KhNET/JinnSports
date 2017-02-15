'use sctrict';

function HeaderView(){	
	var headerComponent;	
	
	this.render = function(){
						
	}
	 
	
	this.init = function(){
	    this.headerComponent = document.getElementById('component-header');

		var ulElement = document.createElement('ul');
		
		ulElement.appendChild(this.buildHeaderElement('/', 'JinnSports'));
		ulElement.appendChild(this.buildHeaderElement('#', 'Главная'));
		ulElement.appendChild(this.buildHeaderElement('#teams', 'Команды'));
		ulElement.appendChild(this.buildHeaderElement('#results', 'Результаты'));
		
		
		this.headerComponent.appendChild(ulElement);
		
		return this;
	}
	
	this.show = function(){
		this.headerComponent.style.display = 'block';
	}
	
	this.hide = function(){		
		this.headerComponent.style.display = 'none';
	}
	
	this.buildHeaderElement = function(url, urlTitle){
		var liElement = document.createElement('li');
		var aElement = document.createElement('a');
		aElement.setAttribute('href', url);
		aElement.innerHTML = urlTitle;
		liElement.appendChild(aElement);
		return liElement;		
	}
}	