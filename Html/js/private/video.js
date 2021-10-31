document.write("<script language=javascript src='js/private/api.js'></script>");
document.write("<script language=javascript src='js/DPlayer.min.js'></script>");
$(function(){
	if(!navigator.onLine){
        alert('网络未连接，请检查重试');
        e.stopPropagation();
        return;
    }

    var lang = {
        "sProcessing": "处理中...",
        "sLengthMenu": "每页 _MENU_ 项",
        "sZeroRecords": "没有匹配结果",
        "sInfo": "当前显示第 _START_ 至 _END_ 项，共 _TOTAL_ 项。",
        "sInfoEmpty": "当前显示第 0 至 0 项，共 0 项",
        "sInfoFiltered": "(由 _MAX_ 项结果过滤)",
        "sInfoPostFix": "",
        "sSearch": "搜索:",
        "sUrl": "",
        "sEmptyTable": "表中数据为空",
        "sLoadingRecords": "载入中...",
        "sInfoThousands": ",",
        "oPaginate": {
            "sFirst": "首页",
            "sPrevious": "上一页",
            "sNext": "下一页",
            "sLast": "末页",
            "sJump": "跳转"
        }
    };

    function load_table(){
    	$("#editable").dataTable({
    		language:lang,
            autoWidth: false,
            searching: false,
            ordering: false,
            processing: false,
            bPaginate:true,
            bInfo:true,
            destroy:true,
            iDisplayLength:10,
            bLengthChange:false,
            pagingType: "full_numbers",
            serverSide: true,
            ajax:function(data,callback,settings){
                let search_video_name=$("#search_video_name").val();

                filters={"Name":search_video_name};

            	page_video((data.start / data.length)+1,data.length,filters,function(res){
            		if(res.code==0){
            			if(res.data.data==null){
            				res.data.data="";
            			}

            			var show_data={};
            			show_data.draw = data.draw;
            			show_data.recordsTotal = res.data.totalCount;
            			show_data.recordsFiltered = res.data.totalCount;
            			show_data.data = res.data.data;
            			callback(show_data);

                        $(".video_cover").click(function(){
                            let video_cover=$(this).parent().parent().find(".video_cover").attr("value");
                            $("#show_cover_id").attr("src", get_pic_url(video_cover));
                        });

                        $(".video-modify").click(function(){
                            let video_id = $(this).parent().parent().find(".video_name").attr("value");
                            let video_name=$(this).parent().parent().find(".video_name").text();
                            let video_cover=$(this).parent().parent().find(".video_cover").attr("value");
                            let video_description=$(this).parent().parent().find(".video_description").text();
                            let publishYear=$(this).parent().parent().find(".video_publishYear").text();
                            let video_path=$(this).parent().parent().find(".video_publishYear").attr("value");

                            $("#add_video_model_titile").text("修改页面");

                            $("#video_id").val(video_id);
                            $("#video_name").val(video_name);
                            $("#photoCover_div").show();
                            $("#photoCover_img").attr("src", get_pic_url(video_cover));
                            $("#img_url").val(video_cover);
                            $("#img_url").val(video_cover);
                            $("#video_url").val(video_path);
                            $("#publish_time").val(publishYear);
                            $("#video_description").val(video_description);

                            $("#upload_video_id").hide();
                        });

            			$(".video-delete").click(function(){
            				let id = $(this).parent().parent().find(".video_name").attr("value");
                            $("#delete_video_id").val(id);
                            $("#delete_video_password").val("");
            			});

                        $(".video-play").click(function(){
                            let video_path=$(this).parent().parent().find(".video_publishYear").attr("value");
                            player.play(get_video_url(video_path));
                        });
            		}else{
            			alert(res.message);
            		}
            	});
            },
            columns:[
                { "data": "name",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="video_name" value="${full.id}">${data}</span>`;
                    }
                },
                { "data": "cover",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="video_cover" value="${full.cover}" data-toggle="modal" data-target="#show_cover_model">
                        <img src="${get_pic_url(data)}" style="width:50px;height:50px;">
                        </span>`;
                    }
                },
                { "data": "description",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="video_description">${data}</span>`;
                    }
                },
                { "data": "publishYear",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="video_publishYear" value="${full.path}">${data}</span>`;
                    }
                },
                { "data": "createTime",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="video_create">${convert_time(data)}</span>`;
                    }
                },
                { "data": null,
                    "render": function ( data, type, full, meta ) {
                        return `
                        <span class="btn btn-link video-play"  data-toggle="modal" data-target="#paly_video_model">播放</span>
                        <span class="btn btn-link video-modify" data-toggle="modal" data-target="#add_video_model">修改</span>
                        <span class="btn btn-link video-delete" data-toggle="modal" data-target="#delete_video_model">删除</span>`
                    }
                }
            ],
            fnDrawCallback:function(){
            	$("#editable_paginate").append("<p style='float:right'>  到第 <input style='height:25px;line-height:25px;width:35px;' class='margin text-center' id='changePage' type='text' maxlength='5'> 页  <a class='btn btn-default shiny' style='margin-bottom:5px' href='javascript:void(0);' id='dataTable-btn'>确认</a></p>");
                var oTable = $("#editable").dataTable();
                $('#dataTable-btn').click(function(e) {
                    if($("#changePage").val() && $("#changePage").val() > 0) {
                        var redirectpage = $("#changePage").val() - 1;
                    } else {
                        var redirectpage = 0;
                    }
                    oTable.fnPageChange(redirectpage);
                });
            }
    	}).api();
    }

    load_table();

    function show_cover(){

    }

    function load_publish_year(){
        let start_year=1972;
        let end_year=(new Date()).getFullYear();
        let options=`<option value="-1">--请选择--</option>`;;

        for(start_year;start_year<=end_year;start_year++){
            options+=`<option value="${start_year}">${start_year}</option>`;
        }

        $("#publish_time").html(options);

        $("#publish_time").val(end_year);
    }

    function convert_time(str){
        let date=new Date(str);
        return `${date.getFullYear()}-${date.getMonth()+1}-${date.getDate()} ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
    }

    load_publish_year();

    upload_picture("cover_file",function(res){
        console.log(res);
        if(res.code==0){
            var url=res.data.filename;
            $("#photoCover_div").show();
            $("#photoCover_img").attr("src", get_pic_url(url));
            $("#img_url").val(url);
        }else{
            alert("文件上传失败！");
        }
    });

    upload_video("video_file",function(percent) {
        $(".progress_bar_title").show();
        $("#progress_number_id").html(`${percent}%`);
        $("#progress_id").css("width",`${percent}%`);
    },function(res){
        console.log(res);
        if(res.code==0){
            $("#progress_title_id").text("上传完成");
            var url=res.data.file;
            $("#video_url").val(url);
        }else{
            alert("文件上传失败！");
        }
    });

    function clear_progress(){
        $("#progress_title_id").text("上传中...");
        $("#progress_number_id").html("0%");
        $("#progress_id").css("width","0%");
         $(".progress_bar_title").hide();
    }

    $("#sure").click(function(){
        clear_progress();

        let video_id=$("#video_id").val();
        let video_name=$("#video_name").val();
        let cover_url=$("#img_url").val();
        let publish_time=$("#publish_time").val();
        let video_url=$("#video_url").val();
        let video_description=$("#video_description").val();
    	
        if(video_name==""){
            $(".helpBlock").text("请填写视频名称").show();
            return;
        }else{
            $(".helpBlock").hide();
        }
        if(cover_url==""){
            $(".helpBlock").text("请上传封面文件").show();
            return;
        }else{
            $(".helpBlock").hide();
        }
        if(video_url==""){
            $(".helpBlock").text("请上传视频文件").show();
            return;
        }else{
            $(".helpBlock").hide();
        }
        if(video_description==""){
            $(".helpBlock").text("请填写视频简介").show();
            return;
        }else{
            $(".helpBlock").hide();
        }

        let callback=function(res){
            if(res.code==0){
                location.reload();
            }else{
                alert(res.message);
            }
        };

        if(video_id>0){
            modify_video(video_id,video_name,cover_url,video_description,publish_time,video_url,callback);
        }else{
            add_video(video_name,cover_url,video_description,publish_time,video_url,callback);
        }
    })

    $("#delete_video").click(function(){
        let id=$("#delete_video_id").val();
        let password=$("#delete_video_password").val();

        if(id<1){
            alert("ID 错误");
            return;
        }

        if(password==""){
            alert("密码为空");
            return;
        }

        delete_video(id,password,function(res){
            if(res.code==0){
                load_table();
            }else{
                alert(res.message);
            }
        });

        $("#delete_video_id").val("0");
        $("#delete_video_password").val("");
    });

    $("#search_button").click(function(){
        load_table();
    });

    $("#addTo").click(function(){
        clear();
    });

    function clear(){
        $("#add_video_model_titile").text("修改页面");
        let end_year=(new Date()).getFullYear();
        $("#video_id").val("0");
        $("#video_name").val("");
        $("#photoCover_div").hide();
        $("#photoCover_img").attr("src", "");
        $("#img_url").val("");
        $("#video_url").val("");
        $("#publish_time").val(end_year);
        $("#video_description").val("");

        $("#upload_video_id").show();
    }

    $(".close_video_modal").click(function(){
        player.destroy();
    });

    let player={
        __dplayer__:null,
        play:function(video_url){
            if(this.__dplayer__!=null)
                destroy();

            this.__dplayer__=new DPlayer({
                container:document.getElementById('player'),
                video:{
                    url:video_url
                }
            });
            
            this.__dplayer__.on("error",function(){
                window.location=`vlc://${video_url}`;
            });
        },
        destroy:function(){
            if(this.__dplayer__!=null){
                this.__dplayer__.destroy()
                this.__dplayer__=null;
            }
        }
    };
});