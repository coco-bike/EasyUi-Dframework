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
$('#_easyui_tree_2').click(function(){
    addTab1('../Admin/RoleAuthority/AddRole','','角色');
});
$('#_easyui_tree_3').click(function(){
    addTab1('../Admin/RoleAuthority/addAuthority','','权限');
});
$('#_easyui_tree_4').click(function(){
    addTab1('../Admin/RoleAuthority/addRoleAuthority','','角色权限');
})
});