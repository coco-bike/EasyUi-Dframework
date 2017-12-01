var page = {
    page_now: 0,
    page_max: 0,
    calculate_page: function (page_new) {
        var self = this;

        // 如果用户选择的页码和当前不一致，那么做重新计算页码的事情
        //if (page_new != self.page_now) {

            // 计算显示时先将原来的页码全部去掉
            $('#J_page_wrap').html('');

            // 当最大页码大于0的时候才需要显示页码
            if (self.page_max > 0) {
                var new_page_inner = '';

                // 如果当前页码为1，那么不需要首页、尾页和页码跳转功能
                if (self.page_max == 1) {
                    new_page_inner = '<a href="javascript:;" data="' + page_new + '" class="cur">' + page_new + '</a>';

                    // 不为1的时候，需要首页、尾页和页码跳转功能
                } else {
                    new_page_inner = '<a href="javascript:;" data="' + page_new + '" class="cur">' + page_new + '</a>';
                    var temp_page;

                    // 1-3 做循环，				
                    for (var i = 1; i <= 10; i++) {
                        // 计算当前页面之前三页，把真实存在的页码加在当前页码之前（即如果计算出来的页码大于0，则认为是真实存在）
                        temp_page = page_new - i;
                        if (temp_page > 0) {
                            new_page_inner = '<a href="javascript:;" data="' + temp_page + '">' + temp_page + '</a>' + new_page_inner;
                        }

                        // 计算当前页面之后三页，把真实存在的页码加在当前页码之后（即如果计算出来的页码小于等于最大页码，则认为是真实存在）
                        temp_page = page_new - -i;
                        if (temp_page <= self.page_max) {
                            new_page_inner = new_page_inner + '<a href="javascript:;" data="' + temp_page + '">' + temp_page + '</a>';
                        }
                    }

                    // 将页码，首页，尾页，跳转页码组装起来
                    new_page_inner =/* '<a href="javascript:;" data="1" >首页</a>' +*/ new_page_inner /*  '<a href="javascript:;" data="' + self.page_max + '">尾页</a>*/ + '<a href="javascript:;"  class="pageUpDown" data="0" type="previousPage">上一页</a>' + '<a href="javascript:;"  data="0" class="pageUpDown" type="lastPage">下一页</a>';
                }

                // 将组装好的页码代码块放到页码的容器中
                $('#J_page_wrap').html(new_page_inner);
            }

            self.page_now = page_new;
        }
}
//} 