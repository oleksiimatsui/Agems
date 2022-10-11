$.get('/api/Charts/JsonData', function (r) {

    var data = r;
    var el = document.createElement('canvas');
    $("#dvChart")[0].appendChild(el);

    //Fix for IE 8
    //if ($.browser.msie && $.browser.version == "8.0") {
    //    G_vmlCanvasManager.initElement(el);
    //}

    var ctx = el.getContext('2d');
    new Chart(ctx).Pie(data);

    for (var i = 0; i < data.length; i++) {
        var div = $("<div />");
        div.css("margin-bottom", "10px");
        div.html("<span style = 'display:inline-block;height:10px;width:10px;background-color:" + data[i].color + "'></span> " + data[i].text);
        $("#dvLegend").append(div);
    }


})