
let pressed = false;
let stars = [];
const mousePositions = [];
let sound;
let speed;
let weight = 10;

function setup() {
    sound = loadSound('assets/sound/sound.mp3');
    createCanvas(window.innerWidth, window.innerHeight);
    for (let i = 0; i < 800; i++) {
        stars[i] = new Star();
    }
}

$("#speed").change(function () {
    speed = $("#speed").val();
});

function RandomColorHex() {
    return "#" + ("00000" + Math.floor(Math.random() * Math.pow(16, 6)).toString(16)).slice(-6);
}

function draw() {
    speed = $("#speed").val();
    background("#222");
    translate(width / 2, height / 2);
    for (let i = 0; i < stars.length; i++) {
        stars[i].update();
        stars[i].show();
    }

    if (pressed == false) {
        mousePositions.shift();
        return;
    }
    mousePositions.push({ x: mouseX - width / 2, y: mouseY - height / 2 });
    if (mousePositions.length > 5) {
        mousePositions.shift();
    }
    for (let i = 0; i < mousePositions.length - 1; i++) {
        let x = mousePositions[i].x;
        let y = mousePositions[i].y;
        let xNext = mousePositions[i + 1].x;
        let yNext = mousePositions[i + 1].y;
        strokeWeight(map(i, 0, mousePositions.length - 2, 1, weight));
        stroke("yellow");
        line(x, y, xNext, yNext);
    }
    

}

function mousePressed() {
    pressed = true;
}
function mouseReleased() {
    pressed = false;
}