document.write("<script language=javascript src='js/private/api.js'></script>");
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
                let search_banner_name=$("#search_banner_name").val();
                let search_banner_customer=$("#search_banner_customer").val();

                filters={"Name":search_banner_name,"CustomerId":search_banner_customer};

            	page_banner((data.start / data.length)+1,data.length,filters,function(res){
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

            			$(".banner-delete").click(function(){
            				if(confirm("确定要删除吗")){
            					var id = $(this).parent().parent().find(".banner_id").text();
            					delete_banner(id,function(res){
            						if(res.code==0){
            							load_table();
            						}else{
            							alert(res.message);
            						}
            					});
            				}
            			});
            		}else{
            			alert(res.message);
            		}
            	});
            },
            columns:[
                { "data": "id",
                    "render": function ( data, type, full, meta ) {
                        return `<span><input type="checkbox" class="banner_select" value="${full.id}"></span>`;
                    }
                },
            	{ "data": "id",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="banner_id">${data}</span>`;
                    }
                },
                { "data": "name",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="banner_name">${data}</span>`;
                    }
                },
                { "data": "filePath",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="banner_pic"><img src="${get_pic_url(data)}" style="width:50px;height:50px;"></span>`;
                    }
                },
                { "data": "customerName",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="banner_customer">${data}</span>`;
                    }
                },
                { "data": "isDefault",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="banner_check">${data?"是":"否"}</span>`;
                    }
                },
                { "data": "isSelected",
                    "render": function ( data, type, full, meta ) {
                        return `<span class="banner_check">${data?"是":"否"}</span>`;
                    }
                },
                { "data": null,
                    "render": function ( data, type, full, meta ) {
                        return `<span class="btn btn-link banner-delete">删除</span>`
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

    //load_table();

    function load_publish_year(){
        let start_year=1972;
        let end_year=(new Date()).getFullYear();
        let options=`<option value="-1">--请选择--</option>`;;

        for(start_year;start_year<=end_year;start_year++){
            options+=`<option value="${start_year}">${start_year}</option>`;
        }

        $("#publish_time").html(options);
    }

    load_publish_year();

    upload_picture("cover_file",function(res){
        console.log(res);
        if(res.code==0){
            var url=res.data[0].filePaths[0];
            $("#photoCover").show();
            $("#photoCover_img").attr("src", get_pic_url(url));
            $("#img_url").val(url);
        }else{
            alert("文件上传失败！");
        }
    });

    $("#select_all").click(function(){
        let checked=$("#select_all").is(':checked');
        $('.banner_select').prop('checked',checked);
    });
    $("#sure").click(function(){
        let banner_name=$("#banner_name").val();
        let pic_url=$("#img_url").val();
        let customer_id=$("#banner_customer").val();
        let default_banner=$("input[name='banner_default']:checked").val()==1?true:false;
    	
        if(banner_name==""){
            $(".helpBlock").text("Banner名为空").show();
            return;
        }else{
            $(".helpBlock").hide();
        }
        if(pic_url==""){
            $(".helpBlock").text("Banner文件为空").show();
            return;
        }else{
            $(".helpBlock").hide();
        }
        if(customer_id<1){
            $(".helpBlock").text("商户名称为空").show();
            return;
        }else{
            $(".helpBlock").hide();
        }

    	add_banner(banner_name,pic_url,customer_id,default_banner,function(res){
    		if(res.code==0){
    			location.reload();
    		}else{
    			alert(res.message);
    		}
    	});
    })

    $("#search_button").click(function(){
        load_table();
    });
});