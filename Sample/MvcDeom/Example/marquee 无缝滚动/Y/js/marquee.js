/**
 * �޷����
 * 
 * @version 2011-3-21 15:55:01
 * @author Zerolone
 */
//�����ٶȣ� Ψһ��Ҫ��JS�е���������
marquees_speed=20;
marquees_stop=false;
window.addEvent('domready',function(){
	//��ȡ����
	var marquees      = $('marquees');
	
	//��ȡ�������
	marqueesHeight = marquees.clientHeight;
	
	//�����������ֹͣ�� �Ƴ�����
	marquees.addEvent('mouseenter', function(e){marquees_stop=true});
	marquees.addEvent('mouseleave', function(e){marquees_stop=false});
	
	//����2��
	marquees.set('html', marquees.get('html'), marquees.get('html'));
	
	//��ʱ��
	var marquees_timerCount = new Hash({counter: 0});
	var marquees_timer = marquees_timerFunc.periodical(20, marquees_timerCount);
});

var marquees_timerFunc = function(){
	var marquees     = $('marquees');
	var halfmarquees = marquees.getScrollSize().x / 2;
	
	//�ƶ���
	marquees.scrollTo(this.counter * 1, 0);
	
	//�������
	if(!marquees_stop) this.counter++;
	
	//���ѭ����ͷ
	if(halfmarquees.round() == marquees.getScroll().x) this.counter=0;
	
	//������Ϣ
	$('aaa').value= halfmarquees;$('bbb').value= marquees.getScroll().x;
}