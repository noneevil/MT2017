var db = new Class({
    Implements: [Chain, Events, Options],
    PageIndex    :1,
    PageCmd      :0,
    PageObject   :[],
    PageReady    :false,//检查是否刚进入
    CookCach     :true,//是否使用cookies保存状态
    Cookies      :null,
    attachTo     :null,
    baseUrl      :null,
    postArgs     :null,
    Loading      :null,
    
    bodysize     :{}, 
    slider       : null, // slider that contains both oldContents and newContents, used to animate between 2 different views
    oldContents  : null, // used in animating from-view to new-view
    newContents  : null, // used in animating from-view to new-view
    sliderIndex  : 0,
    
    options: { 
        cookie    :null,
        baseurl   :null,
        cookcach  :true,
        animate   :false,
        selpage   :$('selpage'),
        selgroup  :$('seltype'),
        txtKey    :$('txtkey'),
        btnSerch  :$('btnserch'),
        btnAll    :$('btnall'),
        btnChkall :'chkall',
        command   :$empty
    },
    initialize: function(attachto,options) {    
        this.setOptions(options);
        this.attachTo = $(attachto);
        this.PageObject = $$('[type=text],select')
        this.baseUrl=this.options.baseurl;
        this.Cookies = this.options.cookie;
        this.CookCach = this.options.cookcach;
        
        
        window.Init=this.Init.bind(this);
        if(this.options.selpage) this.options.selpage.addEvent('change', function (){Init(this.PageCmd,1)}.bind(this));//绑定分页选择事件
        if(this.options.btnAll)  this.options.btnAll.addEvent('click',   Init.pass([0,1]));//绑定显示全部事件
        if(this.options.btnSerch)this.options.btnSerch.addEvent('click', Init.pass([1,1]));//绑定搜索事件
        if(this.options.selgroup)this.options.selgroup.addEvent('change',Init.pass([2,1]));//绑定分类选择事件

        window.addEvents({
            'resize'  : this.Create.bind(this),
            'scroll'  : this.Create.bind(this),
            'domready': this.Create.bind(this)
        });
        if(this.options.animate) this.Picker();
        this.Init(0,1);
    },
    Init:function(cmd,p) {
        this.PageCmd=cmd;
        this.PageIndex=p;
        //******************************初始检查********************************************
        if(this.CookCach){
            this.PageObject.each(function(item, index){
                if(!this.PageReady){
                    if($chk(Cookie.read(this.Cookies + index))) item.value = Cookie.read(this.Cookies+index);}
                else{
                    Cookie.write(this.Cookies + index, item.value);
                }
            }.bind(this));
            if(!this.PageReady){
                this.options.command();
                if($chk(Cookie.read(this.Cookies+'Page')))    this.PageIndex=Cookie.read(this.Cookies+'Page');
                if($chk(Cookie.read(this.Cookies+'PageCmd'))) this.PageCmd=Cookie.read(this.Cookies+'PageCmd');}
            else{
                Cookie.write(this.Cookies+'Page',    this.PageIndex);
                Cookie.write(this.Cookies+'PageCmd', this.PageCmd);
            }
        }
        //******************************初始检查********************************************
        window.PageCmd=this.PageCmd;
        window.PageIndex=this.PageIndex;
        this.postArgs=$(document.body).toQueryString();
        this.PageReady=true;
        this.btnstate(true);
//        this.Load();
        this.Load.bind(this).delay(500);
    },
    Load:function(){
        var _Url= this.baseUrl + '&cmd={0}&page='+this.PageIndex;
            _Url=_Url.replace('{0}',this.PageCmd)
        new Request({url:_Url,
                      data:this.postArgs,
                      onSuccess:function(txt){
                                var match = txt.match(/<body[^>]*>([\s\S]*?)<\/body>/i);
                                txt = (match) ? match[1] : txt; 
                                
                                if(this.options.animate){
                                    var o = this.oldContents;
                                    this.oldContents = this.newContents;
                                    this.newContents = o;
                                    this.newContents.empty();
                                    this.newContents.set('html',txt);
                                    this.sliderIndex > this.PageIndex ? this.fx('right') : this.fx('left');
                                    this.sliderIndex = this.PageIndex;
                                }else{
                                    this.attachTo.set('html',txt);
                                }
                                if($(this.options.btnChkall)) $(this.options.btnChkall).addEvent('click', this.AllChkboxs.bind(this).pass([$(this.options.btnChkall),'chk']));
                                $$('[name=chk]').addEvent('click', this.UnAllChkboxs.bind(this));
                                this.Loading.set({'class': 'line1px_3','text':'数据加载完成.'});
                                this.btnstate();
                                
                      }.bind(this),
//                      onRequest:this.btnstate(true),
                      onFailure:function(){
                                this.show({'class': 'line1px_2','text':'数据加载失败.'});
                                this.btnstate();
                      }.bind(this)}).send();
    },
    AllChkboxs:function(obj,name)
    {
        $$('[name='+name+']').each(function(item){item.checked =obj.checked;});//全选
        /*$$('[name=chk]').each(function(item){item.checked = item.checked ? false : true;})//反选*/
    },
    UnAllChkboxs:function(){
       if($(this.options.btnChkall).checked){$(this.options.btnChkall).checked=false;}
    },
    Create:function(){
        var size   = window.getSize();
        var scroll = window.getScroll();
        var width  = 150;
        var height = 28;
        var x = scroll.x + ((size.x - width) / 2);
        var y = scroll.y + ((size.y - height)/ 2)-100;
        
        if(!this.PageReady){
            this.hidden = true;
            this.MoveBox = $empty();
            this.Loading = new Element('div', {id: 'loading'+$time()}).inject(document.body);
            this.Loading.css({
                position: 'absolute', 
                'color': 'red',
                display:'none',
                top: y,
                left: x,
                width: width,
                height: height,
                overflow: 'hidden'
            });
        }
        this.Loading.css({
          'top'     : y,
          'left'    : x
        });
        /*
        if (this.MoveBox) this.MoveBox.cancel();
        this.MoveBox = new Fx.Morph(this.Loading, {
            duration   : 1000,
            transition : Fx.Transitions.Back.easeInOut
        }).start({
            'left': x,
            'top' : y
        });*/
    },
    show: function(txt) {
        this.Loading.setStyle('display', 'block');
        this.Loading.set(txt);
        this.Loading.fade('in');
    },
    hide: function() {
        this.Loading.fade('out');
    },
    btnstate:function(state){//提交数据时设置控件状态
        $$('input,select').each(function(item){item.disabled=state;});
        $$('[type=image]').each(function(item){
            if(state)
                item.src=item.src.replace(".gif","_.gif");
            else
                item.src=item.src.replace("_.",".");
        });
        if(state){
            this.show({'class': 'line1px_4','text':'数据加载中.请稍后...'});
            top.$('MaskLayer').removeClass('hide');
            window.status="数据加载中,请稍后...";
        }
        else{
            this.hide();
            top.$('MaskLayer').addClass('hide');
            window.status="完成";
        }
    },
    Picker: function() {
        this.bodysize = this.attachTo.getSize();
        var b = new Element('div', { styles: {position: 'relative', top: 0, left: 0,width: this.bodysize.x,height: this.bodysize.y,overflow: 'hidden'}}).inject(this.attachTo);
        this.slider = new Element('div', { styles: { position: 'absolute', top: 0, left: 0, width: 2 * this.bodysize.x, height: this.bodysize.y }})
                    .inject(b);
//				                    .set('tween', { duration: this.options.animationDuration, transition: Fx.Transitions.Quad.easeInOut }).inject(b);
        this.oldContents = new Element('div', { styles: { position: 'absolute', top: 0, left: this.bodysize.x, width: this.bodysize.x, height: this.bodysize.y }}).inject(this.slider);
        this.newContents = new Element('div', { styles: { position: 'absolute', top: 0, left: 0, width: this.bodysize.x, height: this.bodysize.y }}).inject(this.slider);
    },
    fx: function(fx) {
        if (fx == 'right') {
            this.oldContents.setStyles({ left: 0, opacity: 1 });
            this.newContents.setStyles({ left: this.bodysize.x, opacity: 1 });
            this.slider.setStyle('left', 0).tween('left', 0, -this.bodysize.x);
        } else if (fx == 'left') {
            this.oldContents.setStyles({ left: this.bodysize.x, opacity: 1 });
            this.newContents.setStyles({ left: 0, opacity: 1 });
            this.slider.setStyle('left', -this.bodysize.x).tween('left', -this.bodysize.x, 0);
        }
    }
});