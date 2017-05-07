/**
 * MooBox: a mootools plugin for building dialog, inspired by jquery ui dialog
 * 
 * author: AndrewZhang (http://www.cnblogs.com/AndyWithPassion/)
 * version: 1.0
 * license: MIT-style license.
 * date: 2011-12-24 
 * 
 * require: mootools-core-1.4.2 
 *          mootools-more-1.4.0/Drag
 */
var MooBox = new Class({

	Implements: [Options, Events],
	
	options: {
		source: null,
		modal: false,
		autoOpen: false,
		showPos: 'center',
		draggable: true,
		dragEvents: {}, 
		resizable: false,
		resizeEvents: {},  
		width: 300,
		height: 'auto',
		minWidth: 150, 
		minHeight: 150,
		maxHeight: false,
		maxWidth: false,
		toggleBtn: false,
		title: '',
		zIndex: 999,
		buttons: false
		/* callbacks
		onBeforeClose: function(ui) {},
		onClose: function(ui)  {},
		onOpen: function(ui) {}
		*/
	},
	
	initialize: function(options) {
		this.setOptions(options);
		
		this.firstShow = true;
		this.destroyed = false;
		this.opened = false;
		
		// init base html and events
		this.initBaseHTML();
		
		// show box immediately
		if(this.options.autoOpen) this.open();
	}, 
	
	initBaseHTML: function() {
		var self = this,
			opts = self.options,
			// box content
			srcEl = document.id(self.options.source);
		// box container
		var container = new Element('div.moo-box-container').setStyle('z-index', opts.zIndex);
		
		// box title bar 
		var	titleBar = new Element('div.moo-box-titlebar'),
			titleTxt = opts.title ? opts.title : srcEl.get('title'),
			title = new Element('h3.moo-box-title').set('text', titleTxt),
			closeBtn = new Element('a.moo-box-close[href="#"]').set('text', 'x');
		/// hide box when click close	
		closeBtn.addEvent('click', function(e) { e.stop(); self.close(); })
			
		if(opts.toggleBtn) {
			var toggleBtn = new Element('a.moo-box-toggle[href="#"]').set('html', '▲').inject(titleBar);
			
			toggleBtn.addEvent('click', function() {
				var hideEls = [resizeEl, buttonBar, boxBody],
					conHeight = container.style.height;
			
				if(this.retrieve('expanded', true)) {
					// hide titleBar, buttonBar, resizeEl
					hideEls.each(function(el) {
						if(el) el.style.display = 'none';
					});
					container.style.height = 'auto';
					
					this.set('html', '▼').store('expanded', false);
				} else {
					// show titleBar, buttonBar, resizeEl
					hideEls.each(function(el) {
						if(el) el.style.display = 'block';
					});
					container.style.height = conHeight;
					
					this.set('html', '▲').store('expanded', true);
				}
			});
		}
		titleBar.adopt([title, closeBtn]).inject(container);
		
		// box body
		var boxBody = new Element('div.moo-box-body').grab(srcEl).inject(container);
			
		// box button bar
		var	btns = [], buttonBar = null, buttonSet = null;
		
		// init buttons in button bar
		var	buttons = opts.buttons;
		if(buttons) {
			for(var prop in buttons) {

				var btn = new Element('button.moo-box-btn').grab(new Element('span.moo-box-btn-text').set('text', prop));
				(function(key) {
					btn.addEvent('click', function(event) {
						// make scope chain less deeper
						var box = self; 
						if(typeOf(buttons[key]) == 'function')
							buttons[key].call(box, event);
					});
				})(prop);
				
				btns.push(btn);
			}
			
			if(btns.length !== 0) {
				buttonBar = new Element('div.moo-box-buttonbar').inject(container);
				buttonSet = new Element('div.moo-box-buttonset').adopt(btns).inject(buttonBar);
			}
		}
		
		if(opts.resizable) 
			var resizeEl = new Element('div.moo-box-resize').inject(container);
		
		// keep track of key elements
		self.container = container;
		self.titleBar = titleBar;
		self.title = title;
		self.boxBody = boxBody;
		self.buttonBar = buttonBar;
		self.resizeEl = resizeEl;
		self.source = srcEl; // aka source element
	}.protect(), 
	
	open: function() {
		// in open or destroyed state, do nothing
		if(this.isOpen() || this.destroyed) return;
		
		var self = this,
			opts = self.options,
			boxBody = self.boxBody, container = self.container;
		
		// TODO: modal box
		var docSize = document.getScrollSize();
		if(opts.modal) {
			// not first time, then just show modal level
			(self.modalDiv ? function() {
				self.modalDiv.setStyles({
					width: docSize.x,
					height: docSize.y,
					display: 'block'
				});
			// first time, then create a modal level	
			} : function() {
				self.modalDiv = new Element('div.moo-box-modal').setStyles({
					position: 'absolute',
					top: 0,
					left: 0,
					width: docSize.x,
					height: docSize.y,
					'z-index': opts.zIndex.toInt() - 1
				}).inject(document.body);
			})();
		}
		
		if(self.firstShow) {
			// temporarily remove box from viewport
			container.setStyles({
				'left': -10000,
				'width': opts.width,
				'height': opts.height
			}).inject(document.body);
				
			// calculate where box shows
			var showPos = opts.showPos, posStyles = {}, 
				docViewSize = document.getSize(), 
				docScroll = document.getScroll(),
				conSize = container.getSize(),
				toString = Object.prototype.toString, 
				calPos = function(val, objRef, dir) {
					var tag = !!!dir;
					if(typeof val == 'string') {
						switch (val) {
							case 'left':
								objRef.left = 0;
								if(tag) dir = 'y';
								break;
							case 'right':
								objRef.right = 0;
								if(tag) dir = 'y';
								break;
							case 'top':
								objRef.top = 0;
								if(tag) dir = 'x';
								break;
							case 'bottom':
								objRef.bottom = 0;
								if(tag) dir = 'x';
						}
						if(val == 'center' || tag) {
							var prop = '';
							if(dir == 'x') prop = 'left';
							if(dir == 'y') prop = 'top';
							objRef[prop] = (docViewSize[dir] - conSize[dir]) / 2 + docScroll[dir];
						}
					} else if(typeof val == 'number') {
						if(dir == 'x') objRef.left = val;
						if(dir == 'y') objRef.top = val;
					}
				};
			// default 'center'
			if(typeof showPos == 'string') {
				if(showPos == 'center') {
					calPos('center', posStyles, 'x');
					calPos('center', posStyles, 'y');
				} else {
					calPos(showPos, posStyles);
				}
			} else if(toString.call(showPos) == '[object Array]') {
				calPos(showPos[0], posStyles, 'x');
				calPos(showPos[1], posStyles, 'y');
			}
			
			// calculate element's vertical space
			var calElHeight = function(el) {
					if(!el) return 0;
					return el.getSize().y + el.getStyle('marginTop').toFloat() 
						+ el.getStyle('marginBottom').toFloat();
				},
				otherEls_h = calElHeight(self.titleBar) + calElHeight(self.buttonBar);
				
			// boxBody's vertical and horizontal space without height and width
			boxBody.store('extras', {
				ver: boxBody.getSize().y - boxBody.getStyle('height').toFloat(),
				hor: boxBody.getSize().x - boxBody.getStyle('width').toFloat()
			}).setStyles({
				height: container.getStyle('height').toFloat() - otherEls_h - boxBody.retrieve('extras').ver,
				width: container.getStyle('width').toFloat() - boxBody.retrieve('extras').hor
			});
			
			// configure resizable
			if(opts.resizable) {
				var	xlim = opts.maxWidth ? opts.maxWidth : null,
					ylim = opts.maxHeight ? opts.maxHeight : null;
				
				self.resize = container.makeResizable(Object.merge({
					handle: self.resizeEl,
					limit: { x: [opts.minWidth, xlim], y: [opts.minHeight, ylim] },
					onDrag: function(dragEl) {
						var conWidth = dragEl.getStyle('width').toFloat(),
							conHeight = dragEl.getStyle('height').toFloat(),
							extras = boxBody.retrieve('extras');
						
						boxBody.setStyles({
							width: conWidth - extras.hor,
							height: conHeight - extras.ver - otherEls_h
						});
					}
				}, opts.resizeEvents));
			}
			
			// show box
			if(posStyles.left === undefined) posStyles.left = 'auto';
			container.setStyles(posStyles);
			
			// configure draggable
			if(opts.draggable) {
				self.drag = container.makeDraggable(Object.merge({
					handle: self.title.setStyle('cursor', 'move'),
					limit: {x:[0, function() {
						return document.getScrollSize().x - container.getSize().x;
					}], y:[0, function() {
						return document.getScrollSize().y - container.getSize().y;
					}]} 
				}, opts.dragEvents));
			}
			
			// adjust styles, because there will be some issues if right or bottom is a certain value.
			// drag only applys to element's top and left
			var conPos = container.getPosition(),
				docScroll = document.getScroll();
			container.setStyles({
				left: conPos.x + docScroll.x,
				top: conPos.y + docScroll.y,
				right: 'auto',
				bottom: 'auto'
			});
			
			self.firstShow = false;
		}
		container.setStyle('display', 'block');
		// invoke open callback
		self.fireEvent('open', container);
		
		self.opened = true;
	},
	
	close: function() {
		// in hidden or destroyed state, do nothing
		if(!this.isOpen() || this.destroyed) return;
	
		var self = this,
			opts = self.options;
		
		// invoke beforeClose callback
		self.fireEvent('beforeClose', self.container);
		if(self.enableClose === false) return;
		
		if(opts.modal) self.modalDiv.setStyle('display', 'none');
		self.container.setStyle('display', 'none');
		
		self.opened = false;
		// invoke close callback
		self.fireEvent('close', self.container);
	},
	
	destroy: function() {
		var self = this,
			opts = this.options;
		// if has been destroyed, do nothing	
		if(self.destroyed) return;
		
		if(opts.draggable) {
			self.drag.detach();
			delete self.drag;
		}
		if(opts.resizable) {
			self.resize.detach();
			delete self.resize;
		}
		var source = self.boxBody.removeChild(self.source),
			container = self.container;
		/*
		self.container = container;
		self.titleBar = titleBar;
		self.title = title;
		self.boxBody = boxBody;
		self.buttonBar = buttonBar;
		self.resizeEl = resizeEl;
		self.source = srcEl; 
		*/
		['container', 'buttonBar', 'source', 'boxBody', 'resizeEl', 'titleBar', 'title'].each(function(prop) {
			if(self[prop]) delete self[prop];
		})
		
		container.destroy();
		
		self.destroyed = true;
		self.opened = false;
		// return back the source element
		return source; 
	},
	
	// settings is used to override original options
	restore: function(settings) {
		if(!this.destroyed) return;
		
		var opts = this.options;
		this.initialize(Object.merge(opts, settings));
	},
	
	isOpen: function() {
		return !!this.opened;
	}
});