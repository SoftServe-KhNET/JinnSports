'use strict';

var Logger = function (DOMElement, eventInterval, sendUrl, sendInterval, sendCallback) {
	this.data = {events : []};
	this.view = DOMElement;
	this.eventInterval = eventInterval || 3000;
	this.sendUrl = sendUrl;
	this.sendInterval = sendInterval || 60000;
	this.sendCallback = sendCallback;
	this.eventsList = {
		UIEvent: "resize scroll select",
		MouseEvent: "click contextmenu dblclick mousedown mouseenter mouseleave mousemove mouseout mouseover mouseup",
		ClipboardEvent: "copy cut paste",
		DragEvent: "drag dragend dragenter dragleave dragover dragstart drop",
		KeyboardEvent: "keydown keypress keyup",
		WheelEvent: "wheel"
	}
	Object.freeze(this.eventsList);
}

Logger.prototype = {
	init : function() {
		this.initEventHandlers();
	},

	initEventHandlers : function() {
		var msgLog = function(message) {
			return this.getCurrentStringDate()+'\t'+message;
		}.bind(this);

		var getAttr = function(elem, attr) {
			return elem.hasAttribute(attr) ? elem.getAttribute(attr) : null;
		}

		var getVal = function(elem) {
			return elem.value || elem.innerText || 'empty';
		}

		var logObjEvent = function(event) {
			if(lastEvents[event.type]==true) return;
			lastEvents[event.type] = true;
			setTimeout(function(even){ lastEvents[even]=false; }, this.eventInterval, event.type)
			var targ = event.target;
			var eventObj = Object.create(null);
			eventObj.time=(new Date().getTime())/1000;
			eventObj.event = event.type;
			eventObj.tagName = targ.tagName;
			eventObj.id = getAttr(targ,"id");
			eventObj.value = getVal(targ);
			eventObj.CoordX = event.clientX;
			eventObj.CoordY = event.clientY;
			this.data.events.push(eventObj);
		}.bind(this);

		var lastEvents = {};

		for(var eventType in this.eventsList){
		    var events = this.eventsList[eventType].split(' ');
		    events.filter(function (val) {
		        this.view.addEventListener(val, logObjEvent);
		    }.bind(this));
		}
		this.sendDataToUrl = this.sendLoggedData.bind(this);

		if (this.sendUrl != void 0 && this.sendInterval != void 0) {
		    setInterval(this.sendLoggedData.bind(this), this.sendInterval, this.sendUrl, this.sendCallback);
		}
	},

	getData : function() {
		return this.data;
	},

	createXMLHttpRequest : function (){
		var xmlHttp = null;
		if(typeof XMLHttpRequest != "undefined"){
			xmlHttp = new XMLHttpRequest();
		}
		else if(typeof window.ActiveXObject != "undefined"){
			try {
				xmlHttp = new ActiveXObject("Msxml2.XMLHTTP.4.0");
			}
			catch(e){
				try {
					xmlHttp = new ActiveXObject("MSXML2.XMLHTTP");
				}
				catch(e){
					try {
						xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
					}
					catch(e){
						xmlHttp = null;
					}
				}
			}
		}
		return xmlHttp;
	},

	sendLoggedData: function (url, callback) {
	    var req = this.createXMLHttpRequest();
		if(req != null){
			var data = JSON.stringify(this.data);
			req.open('POST', url, true);
			req.setRequestHeader('Content-Type', 'application/json');
			req.onreadystatechange = function () {
			    if (req.readyState == 4 && (req.status == 200 || req.status == 204)) {
			        if (callback != undefined) {
			            callback();
			        }
			    }
			}.bind(this);
			req.send(data);
			this.data = { events: [] };
		}
	}
}