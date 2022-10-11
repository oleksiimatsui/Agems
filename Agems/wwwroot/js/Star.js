

function Star() {

    this.k = 0;
    this.color = "white";
    this.x = random(-width, width);
    this.y = random(-height, height);
    this.z = random(width);
    this.pz = this.z;

    

    this.restart = function () {
        this.color = "white";
        this.z = width;
        this.x = random(-width, width);
        this.y = random(-height, height);
        this.pz = this.z;
    }

    this.update = function () {
        
        this.z = this.z - speed;
        if (this.z < 1) {
            this.restart();
        }
    };

    this.show = function () {
        fill(this.color);
        noStroke();
        var sx = map(this.x / this.z, 0, 1, 0, width);
        var sy = map(this.y / this.z, 0, 1, 0, height);
        var d = dist(mouseX - width / 2, mouseY - height / 2, sx, sy);
        var r = map(this.z, 0, width, 16, 0);

        
        if (d < r + weight/2) {
            this.color = "blue";
            if (pressed == true) {
                sound.play();
                this.restart();
            }
        } else {
            this.color = "white";
        }

        ellipse(sx, sy, r, r);


    //    var px = map(this.x / this.pz, 0, 1, 0, width);
    //    var py = map(this.y / this.pz, 0, 1, 0, height);

    //    this.pz = this.z;

     //   stroke(255);
     //   line(px, py, sx, sy);
    };
}