//后台登录验证
if (window != top) { top.location.href = "login.aspx" }
function CheckSafe(str) {
    var bads = "' &<>?%,;:()`~!#$^*{}[]|+-=\"";
    for (var i = 0; i < bads.length; i++) {
        if (str.indexOf(bads.substring(i, i + 1)) != -1) {
            return false;
            break;
        };
    };
    return true;
};
var login = function() {
    Cookie.write('r', String.uniqueID());
    if ($('user').value.trim() === '') {
        $('user').highlight('#FC9631');
        $('user').focus();
        return false;
    }
    if (!CheckSafe($('user').value) || $('user').value.length < 5) {
        $('user').focus();
        alert('用户名格式错误！');
        return false;
    }
    if ($('pwd').value.trim() === '') {
        $('pwd').focus();
        $('pwd').highlight('#FC9631');
        return false;
    }
    if ($('pwd').value.trim().length < 3) {
        $('pwd').focus();
        alert('密码格式错误！');
        return false;
    }
    if ($('num').value.trim().length < 5) {
        $('num').focus();
        alert('验证码错误！');
        return false;
    }
    $('pwd').store('text', $('pwd').value);
    $('pwd').value = MD5($('pwd').value);
    var _data = $("sys").toQueryString();
    new Request({ url: 'login.aspx?cmd=get',
        data: _data,
        onSuccess: function(txt) {
            switch (txt) {
                case "ok":
                    location.replace('/Developer');
                    break;
                case "yzm":
                    alert("验证码输入错误！");
                    $('pwd').value = $('pwd').retrieve('text');
                    break;
                case "err":
                    alert("用户名或密码输入错误！");
                    $('pwd').value = $('pwd').retrieve('text');
                    break;
                case "lock":
                    alert("当前账号无法登录，请与管理员联系！");
                    $('pwd').value = $('pwd').retrieve('text');
                    break;
                default:
                    break;
            }
            $$('input').each(function(i) {
                i.disabled = false;
            });
        }
    }).send();
    return false;
};