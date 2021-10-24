var domain="http://tesa.eshitong.cn";
var static_domain="http://tess.eshitong.cn";

function login(username,password,success_callback){
    let url=domain+"/api/User/Login"
    let data={"loginName":username,"password":password};
    post(url,data,success_callback);
}

function logout(success_callback){
    let url=domain+"/api/User/Logout"
    let data={};
    post(url,data,success_callback);
}

function add_user(userName,loginName,loginPassword,roleId,success_callback){
	let url=domain+"/api/User/AddUser"
	let data={"UserName":userName,"LoginName":loginName,"LoginPassword":loginPassword,"RoleId":roleId};
    post(url,data,success_callback);
}

function modify_user(id,userName,loginName,loginPassword,roleId,success_callback){
	let url=domain+"/api/User/ModifyUser"
	let data={"Id":id,"UserName":userName,"LoginName":loginName,"LoginPassword":loginPassword,"RoleId":roleId};
    post(url,data,success_callback);
}

function delete_user(id,success_callback){
	let url=domain+"/api/User/DeleteUser"
	let data={"Id":id};
    post(url,data,success_callback);
}

function page_user(page_index,page_size,filters,success_callback){
    let url=domain+"/api/User/PageUser"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function add_role(roleName,success_callback){
	let url=domain+"/api/Role/AddRole"
	let data={"RoleName":roleName};
    post(url,data,success_callback);
}

function modify_role(id,roleName,success_callback){
	let url=domain+"/api/Role/ModifyRole"
	let data={"Id":id,"RoleName":roleName};
    post(url,data,success_callback);
}

function delete_role(id,success_callback){
	let url=domain+"/api/Role/DeleteRole"
	let data={"Id":id};
    post(url,data,success_callback);
}

function page_role(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Role/PageRole"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function query_role(success_callback){
    let url=domain+"/api/Role/QueryRole"
    let data={};
    post(url,data,success_callback);
}

function query_power(id,success_callback){
    let url=domain+"/api/Role/QueryPowerByRoleId"
    let data={"roleId":id};
    post(url,data,success_callback);
}

function modify_power(id,menuIds,success_callback){
    let url=domain+"/api/Role/ModifyPower"
    let data={"roleId":id,"menuIds":menuIds};
    post(url,data,success_callback);
}

function add_menu(menuName,pageUrl,orderIndex,success_callback){
	let url=domain+"/api/Menu/AddTopMenu"
	let data={"MenuName":menuName,"PageUrl":pageUrl,"OrderIndex":orderIndex};
    post(url,data,success_callback);
}

function modify_menu(id,menuName,pageUrl,orderIndex,success_callback){
	let url=domain+"/api/Menu/ModifyMenu"
	let data={"Id":id,"MenuName":menuName,"PageUrl":pageUrl,"OrderIndex":orderIndex};
    post(url,data,success_callback);
}

function delete_menu(id,success_callback){
	let url=domain+"/api/Menu/DeleteMenu"
	let data={"Id":id};
    post(url,data,success_callback);
}

function page_menu(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Menu/PageMenu"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function query_menu(success_callback){
    let url=domain+"/api/Menu/QueryPowerMenu"
    let data={};
    post(url,data,success_callback);
}

function query_menu_for_show(success_callback){
    let url=domain+"/api/Menu/QueryMenusForShow"
    let data={};
    post(url,data,success_callback);
}

// banner

function add_banner(name,path,customerId,isDefault,success_callback){
    let url=domain+"/api/Banner/AddBanner"
    let data={"Name":name,"FilePath":path,"CustomerId":customerId,"IsDefault":isDefault};
    post(url,data,success_callback);
}

function delete_banner(id,success_callback){
    let url=domain+"/api/Banner/DeleteBanner"
    let data={"Id":id};
    post(url,data,success_callback);
}

function page_banner(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Banner/PageBanner"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function select_banner(ids,success_callback){
    let url=domain+"/api/Banner/SelectBanner"
    let data={"ids":ids};
    post(url,data,success_callback);
}

// translator

function add_translator(username,loginname,password,addressCode,description,labelId,success_callback){
    let url=domain+"/api/Translator/AddTranslator"
    let data={"UserName":username,"LoginName":loginname,"Password":password,"AddressCode":addressCode,"Description":description,"LabelId":labelId};
    post(url,data,success_callback);
}

function modify_translator(id,username,loginname,password,addressCode,description,labelId,success_callback){
    let url=domain+"/api/Translator/ModifyTranslator"
    let data={"Id":id,"UserName":username,"LoginName":loginname,"Password":password,"AddressCode":addressCode,"Description":description,"LabelId":labelId};
    post(url,data,success_callback);
}

function delete_translator(id,success_callback){
    let url=domain+"/api/Translator/DeleteTranslator"
    let data={"Id":id};
    post(url,data,success_callback);
}

function page_translator(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Translator/PageTranslator"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function query_translator(success_callback){
    let url=domain+"/api/Translator/QueryTranslator"
    let data={};
    post(url,data,success_callback);
}

// video

function page_video(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Video/Page"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function page_device(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Device/PageDevice"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function page_label(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Label/PageLabel"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function query_label(success_callback){
    let url=domain+"/api/Label/QueryLabel"
    let data={};
    post(url,data,success_callback);
}

function add_label(label,success_callback){
    let url=domain+"/api/Label/AddLabel"
    let data={"Name":label};
    post(url,data,success_callback);
}

function delete_label(id,success_callback){
    let url=domain+"/api/Label/DeleteLabel"
    let data={"Id":id};
    post(url,data,success_callback);
}

// 业务员

function add_salesman(agentId,name,phone,remark,success_callback){
    let url=domain+"/api/Salesman/AddSalesman"
    let data={"AgentId":agentId,"Name":name,"Phone":phone,"Remark":remark};
    post(url,data,success_callback);
}

function modify_salesman(id,agentId,name,phone,remark,success_callback){
    let url=domain+"/api/Salesman/UpdateSalesman"
    let data={"Id":id,"AgentId":agentId,"Name":name,"Phone":phone,"Remark":remark};
    post(url,data,success_callback);
}

function delete_salesman(id,success_callback){
    let url=domain+"/api/Salesman/DeleteSalesman"
    let data={"Id":id};
    post(url,data,success_callback);
}

function page_salesman(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Salesman/PageSalesman"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function query_salesman(success_callback){
    let url=domain+"/api/Salesman/QuerySalesman"
    let data={};
    post(url,data,success_callback);
}

// 代理商

function add_agent(name,addressCode,address,contacts,phone,salesmanId,remark,success_callback){
    let url=domain+"/api/Agent/AddAgent"
    let data={"Name":name,"AddressCode":addressCode,"Address":address,"Contacts":contacts,"Phone":phone,"SalesmanId":salesmanId,"Remark":remark};
    post(url,data,success_callback);
}

function modify_agent(id,name,addressCode,address,contacts,phone,salesmanId,remark,success_callback){
    let url=domain+"/api/Agent/UpdateAgent"
    let data={"Id":id,"Name":name,"AddressCode":addressCode,"Address":address,"Contacts":contacts,"Phone":phone,"SalesmanId":salesmanId,"Remark":remark};
    post(url,data,success_callback);
}

function delete_agent(id,success_callback){
    let url=domain+"/api/Agent/DeleteAgent"
    let data={"Id":id};
    post(url,data,success_callback);
}

function page_agent(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Agent/PageAgent"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function query_agent(success_callback){
    let url=domain+"/api/Agent/QueryAgent"
    let data={};
    post(url,data,success_callback);
}

// 客户

function add_customer(name,addressCode,address,contacts,phone,agentId,salesmanId,remark,success_callback){
    let url=domain+"/api/Customer/AddCustomer"
    let data={"Name":name,"AddressCode":addressCode,"Address":address,"Contacts":contacts,"Phone":phone,"AgentId":agentId,"SalesmanId":salesmanId,"Remark":remark};
    post(url,data,success_callback);
}

function modify_customer(id,name,addressCode,address,contacts,phone,agentId,salesmanId,remark,success_callback){
    let url=domain+"/api/Customer/UpdateCustomer"
    let data={"Id":id,"Name":name,"AddressCode":addressCode,"Address":address,"Contacts":contacts,"Phone":phone,"AgentId":agentId,"SalesmanId":salesmanId,"Remark":remark};
    post(url,data,success_callback);
}

function delete_customer(id,success_callback){
    let url=domain+"/api/Customer/DeleteCustomer"
    let data={"Id":id};
    post(url,data,success_callback);
}

function page_customer(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Customer/PageCustomer"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function query_customer(agentId,success_callback){
    let url=domain+"/api/Customer/QueryCustomer"
    let data={"agentId":agentId};
    post(url,data,success_callback);
}

// 设备账号

function add_account(name,loginName,password,startTime,endTime,agentId,customerId,salesmanId,remark,success_callback){
    let url=domain+"/api/DeviceAccount/AddDeviceAccount"
    let data={"Name":name,"LoginName":loginName,"Password":password,"StartTime":startTime,"EndTime":endTime,"AgentId":agentId,"CustomerId":customerId,"SalesmanId":salesmanId,"Remark":remark};
    post(url,data,success_callback);
}

function modify_account(id,name,loginName,password,startTime,endTime,agentId,customerId,salesmanId,remark,success_callback){
    let url=domain+"/api/DeviceAccount/UpdateDeviceAccount"
    let data={"Id":id,"Name":name,"LoginName":loginName,"Password":password,"StartTime":startTime,"EndTime":endTime,"AgentId":agentId,"CustomerId":customerId,"SalesmanId":salesmanId,"Remark":remark};
    post(url,data,success_callback);
}

function examine_account(id,success_callback){
    let url=domain+"/api/DeviceAccount/ExamineDeviceAccount"
    let data={"Id":id};
    post(url,data,success_callback);
}

function reset_account_password(id,password,success_callback){
    let url=domain+"/api/DeviceAccount/ResetDeviceAccountPassword"
    let data={"Id":id,"Password":password};
    post(url,data,success_callback);
}

function lock_account(id,success_callback){
    let url=domain+"/api/DeviceAccount/LockDeviceAccount"
    let data={"Id":id};
    post(url,data,success_callback);
}

function unlock_account(id,success_callback){
    let url=domain+"/api/DeviceAccount/UnlockDeviceAccount"
    let data={"Id":id};
    post(url,data,success_callback);
}

function delete_account(id,success_callback){
    let url=domain+"/api/DeviceAccount/DeleteDeviceAccount"
    let data={"Id":id};
    post(url,data,success_callback);
}

function page_account(page_index,page_size,filters,success_callback){
    let url=domain+"/api/DeviceAccount/PageDeviceAccount"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

// 地址

function query_top_address(success_callback){
    let url=domain+"/api/AddressCode/QueryTopAddress"
    let data={};
    post(url,data,success_callback);
}

function query_sub_address(parentCode,success_callback){
    let url=domain+"/api/AddressCode/QuerySubAddress"
    let data={"parentCode":parentCode};
    post(url,data,success_callback);
}

function code_to_address(addressCode,success_callback){
    let url=domain+"/api/AddressCode/QueryAddressByCode"
    let data={"addressCode":addressCode};
    post(url,data,success_callback);
}

// other

function query_top_address(success_callback){
    let url=domain+"/api/AddressCode/QueryTopAddress"
    let data={};
    post(url,data,success_callback);
}

var flag=true;

function post(url,param,success_callback){
    $("#hide").fadeTo(200,.5);
    $.ajax({
        type:"POST",
        url:url,
        beforeSend:function(XHR){
            XHR.setRequestHeader('Authorization', get_token());
        },
        data: param,
        dataType: "json",
        success:function(res){
            $("#hide").fadeOut(200);
            if(res.code==-2){
                if(!flag) return;
                flag=false;
                window.location.href="login.html";
                alert(res.message);
                return;
            }
            flag=true;
            success_callback(res);
        },
        error:function(XMLHttpRequest, textStatus, errorThrown){
            if (XMLHttpRequest.status == 0){
                alert("请检查网络");
            }
        }
    });
}

function set_token(token){
    localStorage.setItem("token",token);
}

function get_token(){
    return localStorage.getItem("token");
}

function clear_token(){
    localStorage.clear();
}

function get_video_url(path){
    return static_domain+"/video/"+path;
}

function get_pic_url(path){
    return static_domain+"/image/"+path;
}

function upload_picture(id,success_callback){
    $(`input[id=${id}]`).change(function(){
        let form_data = new FormData();
        form_data.append('file', $(`#${id}`)[0].files[0]);
        let url=domain+"/api/FileUpload/UploadImage";
        $.ajax({
            type:"POST",
            url:url,
            data:form_data,
            contentType:false,
            processData:false,
            beforeSend:function(XHR){
                XHR.setRequestHeader('Authorization', get_token());
            },
            dataType:"json",
            success:function(res){
                success_callback(res);
            }
        });
    });
}