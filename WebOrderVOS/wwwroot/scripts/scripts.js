$(document).ready(
    function(){
        $('.tableItems').waypoint(
            function(direction){
                if(direction == "down"){
                    $('.tableTitle').addClass('sticky');
                } else{
                    $('.tableTitle').removeClass('sticky');
                }
            }, {
                offset: '00px'
            }
        )
    }
)