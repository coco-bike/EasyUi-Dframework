 //创建Frame
function createFramePage(url) {
    var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
    return s;
}

function addTab1(url,icon,titletext){
    if(!$('#tabs').tabs('exists',titletext)){  
        $('#tabs').tabs('add',{
            title:titletext,
            content:createFramePage(url),
            closable:true,
            icon:'',
        });
    }else{
        $('#tabs').tabs('select',titletext);
    }
    tabClose();
}

$(function(){
$('#_easyui_tree_1').click(function(){
    addTab1('../Admin/index/AddRole','','角色查询');
});
});