/**
 * ����
 * 
 * @version 2009-8-26 16:02:54
 * @author Zerolone
 */
window.addEvent('domready',function(){
	//��ʼ��Сͼ
	var cycle=new Array();
	for(var i=0;i<4;i++){
		cycle[i]=$('cycle_img'+i);
		cycle[i].src=Switcher[i]['pic'];
	}

	//��ͼ
	var bigpic=$('bigpic');
	//��ʼ����ͼ
	bigpic.src=Switcher[0]['pic'];

	//ѭ��li
	var li=new Array();
	for(var i=0;i<4;i++){
		li[i]=$('li'+i);		
	}

	//������������¼�
	li[0].addEvent('mouseenter', function(e){lienter(0);});
	li[1].addEvent('mouseenter', function(e){lienter(1);});
	li[2].addEvent('mouseenter', function(e){lienter(2);});
	li[3].addEvent('mouseenter', function(e){lienter(3);});

	//��������Ƴ��¼�
	li[0].addEvent('mouseleave', function(e){cycleplay();});
	li[1].addEvent('mouseleave', function(e){cycleplay();});
	li[2].addEvent('mouseleave', function(e){cycleplay();});
	li[3].addEvent('mouseleave', function(e){cycleplay();});
	
	//���������¼�
	li[0].addEvent('click', function(e){window.open(Switcher[0]['url']);});
	
	//����
	$('cycle').addEvent('mouseenter', function(e){cyclestop();});	
	$('cycle').addEvent('mouseleave', function(e){cycleplay();});

	//��ͼ
	$('bigpic').addEvent('mouseenter', function(e){cyclestop();});	
	$('bigpic').addEvent('mouseleave', function(e){cycleplay();});
	
	//����
	$('bg').addEvent('mouseenter', function(e){cyclestop();});	
	$('bg').addEvent('mouseleave', function(e){cycleplay();});	

	//����
	$('SwitchTitle').addEvent('mouseenter', function(e){cyclestop();});	
	$('SwitchTitle').addEvent('mouseleave', function(e){cycleplay();});	

	//Ĭ��
	var slidint;
	Switchid=0;

	cycleplay();
	lienter(0);
	cycleplay();
});

/**
 * ���뻻����
 */
function lienter(id){
	//��������
	liclear();
	
	//ֹͣ����
	cyclestop();	
	
	//ѡ������
	$('li'+id).setStyle('background-image', 'url(images/bg.gif)');
	
	//����
	var str_title = '' ;
	str_title = '<h3><a href="'+Switcher[id]['url']+'" target="_blank">'+Switcher[id]['title']+'</a></h3><p>'+Switcher[id]['stitle']+'</p>' ;
	$('SwitchTitle').set('html',str_title);
	
	//��ͼ����
	$('bighref').href=Switcher[id]['url'];
//	$('bigpic').addEvent('click', function(e){window.open(Switcher[id]['url']);});
//	$('bigpic').removeEvent('click');
	
	//��ͼ
	//$('bigpic').src=Switcher[id]['pic'];
	
	//��ͼЧ��
	var bigpic = $('bigpic');
	var fx = new Fx.Tween(bigpic,{
		duration: 50,
		onComplete: function(){
			bigpic.src=Switcher[id]['pic'];
			bigpic.fade('in');
		}
	});
	fx.start('opacity',1,0);
}

/**
 * �������
 */
function liclear(){
	for(var i=0;i<4;i++){
		$('li'+i).setStyle('background-image', '');
	}	
}

/**
 * ѭ��������
 */
function cycleplay(){
	slidint = setTimeout(playnext,1000);
}

/**
 * ��ʾ��һ��
 */
function playnext(){
	if(Switchid==3){
		Switchid = 0;
	}
	else{
		Switchid++;
	};
	
	lienter(Switchid);
	cycleplay();
}

/**
 * ֹͣ
 */
function cyclestop(){
	clearTimeout(slidint);
}