function init_player(id){
    let video_html=`<div class="video_player">
    <video src=""></video>
    <div class="player_control">
    <input type="range" class="video_range" value="0" max="100" />
    <span class="player_play_pause" value="pause">
    ${play_svg()}
    </span>
    <span class="player_volume">
    ${volume_down()}
    </span>
    <span class="volume_span">
    <input type="range" class="volume_range" value="60" max="100" />
    </span>
    <span class="volume_text">
    60%
    </span>
    <span class="video_time">
    <span class="video_current_time">
    00:00
    </span>&nbsp;/
    <span class="video_end_time">
    10:00
    </span>
    </span>
    <span class="player_full_screen">
    ${full_screen()}
    </span>
    </div>
    </div>`;


    $(`#${id}`).html(video_html);
    init_event();
}

function init_event(){
    let video=$(".video_player>video")[0];
    video.oncanplay=function(){
        let hour=(this.duration/60).toFixed(0);
        let second=(this.duration%60).toFixed(0);
        $(".player_control>.video_range").attr("max",this.duration);
        $(".video_time>.video_end_time").text(`${hour}:${second}`);
    };

    video.ontimeupdate=function(){
        let hour=(this.currentTime/60).toFixed(0);
        let second=(this.currentTime%60).toFixed(0);
        $(".video_time>.video_current_time").text(`${hour}:${second}`);
        $(".player_control>.video_range").val(this.currentTime);
    };

    video.volume=0.6;

    let p=function(){
        if(video.paused){
            video.play();
            $(".player_control>.player_play_pause").html(pause_svg());
            $(".player_control>.player_play_pause").attr("value","play");
        }else{
            video.pause();
            $(".player_control>.player_play_pause").html(play_svg());
            $(".player_control>.player_play_pause").attr("value","pause");
        }
    };

    $(".video_player>video").click(p);

    $(".player_control>.player_play_pause").click(p);

    $(".volume_range").change(function(){
        video.volume=$(this).val()*0.01;
        $(".volume_text").text(`${$(this).val()}%`);
        $(".player_control>.player_volume").html(video.volume>0.8?volume_up():volume_down());
    });

    $(".player_volume").click(function(){
        video.muted=!video.muted;
        if(video.muted){
            $(this).html(volume_off());
            $(".volume_text").text("0%");
            $(".volume_range").val(0);
        }
        else{
            $(".volume_text").text(`${video.volume*100}%`);
            $(".player_control>.player_volume").html(video.volume>0.8?volume_up():volume_down());
            $(".volume_range").val(video.volume*100);
        }
    });

    $(".player_control>.video_range").mousedown(function(){
        video.pause();
    });

    $(".player_control>.video_range").change(function(){
        video.currentTime=$(this).val();
    });

    $(".player_control>.video_range").mouseup(function(){
        let status=$(".player_control>.player_play_pause").attr("value");
        if(status=="play")
            video.play();
    });

    $(".player_control>.player_full_screen").click(function(){
        if (video.webkitRequestFullScreen) {
            video.webkitRequestFullScreen();
        }
        else if (video.mozRequestFullScreen) {
            video.mozRequestFullScreen();
        }
        else if (video.msRequestFullScreen) {
            video.msRequestFullScreen();
        }
        else if (video.RequestFullScreen) {
            video.RequestFullScreen();
        }
    });
}

function play_video(url){
    $(".video_player>video").attr("src",url);
}

function clear_video(){
    let video=$(".video_player>video")[0];
    video.pause();
    $(".video_player>video").attr("src","");
}

function play_svg(){
    return `<svg xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 16 32">
    <path d="M15.552 15.168q0.448 0.32 0.448 0.832 0 0.448-0.448 0.768l-13.696 8.512q-0.768 0.512-1.312 0.192t-0.544-1.28v-16.448q0-0.96 0.544-1.28t1.312 0.192z"></path>
    </svg>`;
}

function pause_svg(){
    return `<svg xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 17 32">
    <path d="M14.080 4.8q2.88 0 2.88 2.048v18.24q0 2.112-2.88 2.112t-2.88-2.112v-18.24q0-2.048 2.88-2.048zM2.88 4.8q2.88 0 2.88 2.048v18.24q0 2.112-2.88 2.112t-2.88-2.112v-18.24q0-2.048 2.88-2.048z"></path>
</svg>`;
}

function volume_off(){
    return `<svg xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 21 32">
    <path d="M13.728 6.272v19.456q0 0.448-0.352 0.8t-0.8 0.32-0.8-0.32l-5.952-5.952h-4.672q-0.48 0-0.8-0.352t-0.352-0.8v-6.848q0-0.48 0.352-0.8t0.8-0.352h4.672l5.952-5.952q0.32-0.32 0.8-0.32t0.8 0.32 0.352 0.8z"></path>
</svg>`;
}

function volume_up(){
    return `<svg xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 21 32">
    <path d="M13.728 6.272v19.456q0 0.448-0.352 0.8t-0.8 0.32-0.8-0.32l-5.952-5.952h-4.672q-0.48 0-0.8-0.352t-0.352-0.8v-6.848q0-0.48 0.352-0.8t0.8-0.352h4.672l5.952-5.952q0.32-0.32 0.8-0.32t0.8 0.32 0.352 0.8zM20.576 16q0 1.344-0.768 2.528t-2.016 1.664q-0.16 0.096-0.448 0.096-0.448 0-0.8-0.32t-0.32-0.832q0-0.384 0.192-0.64t0.544-0.448 0.608-0.384 0.512-0.64 0.192-1.024-0.192-1.024-0.512-0.64-0.608-0.384-0.544-0.448-0.192-0.64q0-0.48 0.32-0.832t0.8-0.32q0.288 0 0.448 0.096 1.248 0.48 2.016 1.664t0.768 2.528zM25.152 16q0 2.72-1.536 5.056t-4 3.36q-0.256 0.096-0.448 0.096-0.48 0-0.832-0.352t-0.32-0.8q0-0.704 0.672-1.056 1.024-0.512 1.376-0.8 1.312-0.96 2.048-2.4t0.736-3.104-0.736-3.104-2.048-2.4q-0.352-0.288-1.376-0.8-0.672-0.352-0.672-1.056 0-0.448 0.32-0.8t0.8-0.352q0.224 0 0.48 0.096 2.496 1.056 4 3.36t1.536 5.056z"></path>
</svg>`;
}

function volume_down(){
    return `<svg xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 21 32">
    <path d="M13.728 6.272v19.456q0 0.448-0.352 0.8t-0.8 0.32-0.8-0.32l-5.952-5.952h-4.672q-0.48 0-0.8-0.352t-0.352-0.8v-6.848q0-0.48 0.352-0.8t0.8-0.352h4.672l5.952-5.952q0.32-0.32 0.8-0.32t0.8 0.32 0.352 0.8zM20.576 16q0 1.344-0.768 2.528t-2.016 1.664q-0.16 0.096-0.448 0.096-0.448 0-0.8-0.32t-0.32-0.832q0-0.384 0.192-0.64t0.544-0.448 0.608-0.384 0.512-0.64 0.192-1.024-0.192-1.024-0.512-0.64-0.608-0.384-0.544-0.448-0.192-0.64q0-0.48 0.32-0.832t0.8-0.32q0.288 0 0.448 0.096 1.248 0.48 2.016 1.664t0.768 2.528z"></path>
</svg>`;
}

function full_screen(){
    return `<svg xmlns="http://www.w3.org/2000/svg" version="1.1" viewBox="0 0 32 33">
                            <path d="M6.667 28h-5.333c-0.8 0-1.333-0.533-1.333-1.333v-5.333c0-0.8 0.533-1.333 1.333-1.333s1.333 0.533 1.333 1.333v4h4c0.8 0 1.333 0.533 1.333 1.333s-0.533 1.333-1.333 1.333zM30.667 28h-5.333c-0.8 0-1.333-0.533-1.333-1.333s0.533-1.333 1.333-1.333h4v-4c0-0.8 0.533-1.333 1.333-1.333s1.333 0.533 1.333 1.333v5.333c0 0.8-0.533 1.333-1.333 1.333zM30.667 12c-0.8 0-1.333-0.533-1.333-1.333v-4h-4c-0.8 0-1.333-0.533-1.333-1.333s0.533-1.333 1.333-1.333h5.333c0.8 0 1.333 0.533 1.333 1.333v5.333c0 0.8-0.533 1.333-1.333 1.333zM1.333 12c-0.8 0-1.333-0.533-1.333-1.333v-5.333c0-0.8 0.533-1.333 1.333-1.333h5.333c0.8 0 1.333 0.533 1.333 1.333s-0.533 1.333-1.333 1.333h-4v4c0 0.8-0.533 1.333-1.333 1.333z"></path>
                        </svg>`;
}