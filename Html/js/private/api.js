var domain="http://api.home-video.local";
var static_domain="http://static.home-video.local";

// 设备账号

function add_video(name,cover,description,publishYear,path,success_callback){
    let url=domain+"/api/Video/AddVideo"
    let data={"Name":name,"Cover":cover,"Description":description,"PublishYear":publishYear,"Path":path};
    post(url,data,success_callback);
}

function modify_video(id,name,cover,description,publishYear,path,success_callback){
    let url=domain+"/api/Video/ModifyVideo"
    let data={"Id":id,"Name":name,"Cover":cover,"Description":description,"PublishYear":publishYear,"Path":path};
    post(url,data,success_callback);
}

function delete_video(id,password,success_callback){
    let url=domain+"/api/Video/DeleteVideo"
    let data={"Id":id,"Password":password};
    post(url,data,success_callback);
}

function page_video(page_index,page_size,filters,success_callback){
    let url=domain+"/api/Video/PageVideo"
    let data={"PageIndex":page_index,"PageSize":page_size,"Data":filters};
    post(url,data,success_callback);
}

function post(url,param,success_callback){
    $("#hide").fadeTo(200,.5);
    $.ajax({
        type:"POST",
        url:url,
        data: param,
        dataType: "json",
        success:function(res){
            $("#hide").fadeOut(200);
            success_callback(res);
        },
        error:function(XMLHttpRequest, textStatus, errorThrown){
            if (XMLHttpRequest.status == 0){
                alert("请检查网络");
            }
        }
    });
}

function upload_picture(id,success_callback){
    let url=domain+"/api/Upload/UploadImage";
    $(`input[id=${id}]`).change(function(){
        let form_data = new FormData();
        form_data.append('file', $(`#${id}`)[0].files[0]);
         $.ajax({
            type:"POST",
            url:url,
            data:form_data,
            contentType:false,
            processData:false,
            dataType:"json",
            success:function(res){
                success_callback(res);
            }
         });
    });
}

function upload_video(id,onprogress_callback,success_callback){
    let url=domain+"/api/Upload/UploadVideo";
    $(`input[id=${id}]`).change(function(){
        let form_data = new FormData();
        form_data.append('file', $(`#${id}`)[0].files[0]);
        let xhr=new XMLHttpRequest();
        xhr.open('POST',url,true);

        xhr.onreadystatechange=function(){
            if(this.readyState==4){
                success_callback(JSON.parse(this.responseText));
            }
        }
        xhr.upload.onprogress=function(ev){
            if(ev.lengthComputable){
                let precent=parseFloat(100 * ev.loaded/ev.total).toFixed(2);
                onprogress_callback(precent);
            }
        }

        xhr.send(form_data);
    });
}

function get_pic_url(url){
    return static_domain+`/image/${url}`;
}

function get_video_url(url){
    return static_domain+`/video/${url}`;
}