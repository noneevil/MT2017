// 用于快速生成plugin documentation
var Doc = new Class({

	initialize: function(view) {
		this.view = view;
	},
	
	execute: function() {
		var view = this.view,
			doc = new Element('div#doc');
		Array.from(view.panels).each(function(panel) {
			var panelEl = new Element('div.panel'),
				titleEl = new Element('h3.title').set('text', panel.title).inject(panelEl),
				props = panel.props,
				propsEl = new Element('div.props'),
				dlItems = [];
			
			if(panel.separator) panelEl.addClass('separator');
			
			for(var i = 0, l = props.length; i < l; i++) {
				var prop = props[i],
					type = prop.type ? prop.type : 'normal',
					dlEl = new Element('dl.prop-item'),
					dtEl = new Element('dt.prop-label'),
					ddEl = new Element('dd.prop-desc');
				
				var desc = null, label = null;
				switch (type) {
					case 'normal':
						dtEl.set('text', prop.label);
						ddEl.set('text', prop.desc);
						break;
					case 'require':
						dtEl.set('text', prop.label);
						
						desc = prop.desc;
						Array.from(desc).each(function(value) {
							new Element('span.sub.require').set('text', value).inject(ddEl);
						});
						break;
					case 'option':
						label = prop.label;
						dtEl.adopt([
							new Element('span.sub.name').set('text', label.name),
							new Element('span.sub.default').set('text','default: ' + label.def),
							new Element('span.sub.type').set('text', 'type: ' + label.type)
						]);
						
						ddEl.set('text', prop.desc);
						break;
					case 'author':
						dtEl.set('text', prop.label);
						
						desc = prop.desc;
						ddEl.adopt([
							new Element('span.sub').set('text', desc.author),
							new Element('span.sub').grab(
								new Element('a[href="mailto:' + desc.email + '"]').set('text', desc.email)
							),
							new Element('span.sub').grab(
								new Element('a[href="' + desc.blog + '"]').set('text', desc.blog)
							)
						]);	
						break;
				}
				// last item
				if(i == l - 1) dlEl.addClass('last-item');
				dlItems.push(dlEl.adopt([dtEl, ddEl]));
			}
			
			panelEl.grab(propsEl.adopt(dlItems));
			doc.grab(panelEl);
		});
		document.body.appendChild(doc);
	}
});