

const canvas = document.getElementById('drawing-board');
canvas.width = window.innerWidth - 10;
canvas.height = window.innerHeight - 10;

const ctx = canvas.getContext('2d');
ctx.fillStyle = "#222";
ctx.fillRect(0, 0, canvas.width, canvas.height);


let widthinput = document.getElementById("width");
let colorinput = document.getElementById("color");
let isdraw = false;
var x0 = 0;
var y0 = 0;
var connection = new signalR.HubConnectionBuilder()
                    .withUrl("/canvasHub")
                    .configureLogging(signalR.LogLevel.Information)
                    .build();

connection.start().then(
    function () {
        document.getElementById("connection").innerHTML = "connected";
    }).
    catch(function (err) {
        document.getElementById("connection").innerHTML = "connection error";
    });


window.onclose = function() {
    connection.invoke("Exit").catch(function (err) {
        return console.error(err.toString());
    });
}

connection.on("ReceiveDraw", function (draw) {
        Draw(draw);
    });

connection.on("Clear", function () {
       clear();
    })

connection.on("Count", function (count) {
       document.getElementById("count").innerHTML = count;
    })

function Draw(draw){
        ctx.moveTo(draw.x0, draw.y0);
        ctx.beginPath();
        ctx.strokeStyle = draw.color;
        ctx.lineWidth = draw.width;
        ctx.lineCap = 'round';
        ctx.lineTo(draw.x, draw.y);
        ctx.stroke();
    }

function clear(){
        ctx.clearRect(0, 0, canvas.width, canvas.height);
    }


document.getElementById("clear").onclick = function(){
        connection.invoke("Clear").catch(function (err) {
            return console.error(err.toString());
        });
    }

canvas.addEventListener('mousedown', (e) => {
        isdraw = true;
    });

canvas.addEventListener('mouseup', e => {
        isdraw = false;
    });

canvas.addEventListener('mousemove', (e) => {
    if (isdraw == false) return;
    //if (x0 === null) {
    //    x0 = e.clientX;
    //    y0 = y.clientY;
    //}

    let x = e.clientX;
    let y = e.clientY;
    let w = Number (widthinput.value);
    let c = colorinput.value;
    connection.invoke("SendDraw", x0, y0, x, y, w, c ).catch(function (err) {
        return console.error(err.toString());
    });
    x0 = e.clientX;
    y0 = e.clientY;
});

document.addEventListener('keydown', (e) => {
    if(e.key == "["){
        widthinput.value = parseInt( widthinput.value ) - 1;
    }
    if(e.key == "]"){
        widthinput.value = parseInt( widthinput.value ) + 1;
    }
})

document.getElementById('share_btn').onclick = function () {
    var dataURL = canvas.toDataURL('image/png').replace("image/png", "image/octet-stream");
    document.getElementById('imgurl').value = dataURL;
    document.getElementById('sendpic_form').submit();
};

function srcToFile(src, fileName, mimeType) {
    return (fetch(src)
        .then(function (res) { return res.arrayBuffer(); })
        .then(function (buf) { return new File([buf], fileName, { type: mimeType }); })
    );
}

