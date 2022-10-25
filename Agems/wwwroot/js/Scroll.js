
document.addEventListener('scroll', function (e) {
    let documentHeight = document.body.scrollHeight;
    let currentScroll = window.scrollY + window.innerHeight;
    if (currentScroll > documentHeight) {
        addpage();
    }
})

window.onload = function () {
    addpage();
}

document.onscroll = function () {
    let documentHeight = document.body.scrollHeight;
    let currentScroll = window.scrollY + window.innerHeight;
    if (currentScroll > documentHeight) {
        addpage();
    }
};

function displayData(data) {
    const el = document.createElement('div');
    el.classList.add('page');
    data.forEach(sound => {
        let cont = document.createElement('div');
        cont.classList = "card border-warning mb-3";
        cont.style.width = "70%";
        el.appendChild(cont);
        let header = document.createElement('div');
        header.classList = "card-header";
        header.textContent = sound.name;
        cont.appendChild(header);
        let cardbody = document.createElement('div');
        cardbody.classList = "card-body";
        cont.appendChild(cardbody);
        let text = document.createElement('p');
        text.classList = "card-text";
        text.innerText = sound.about;
        cardbody.appendChild(text);
        let audio = document.createElement("audio");
        audio.controls = 'controls';
        let source = document.createElement("source");
        source.src = "/sounds/" + sound.soundPath;
        source.type = 'audio/mpeg';
        audio.appendChild(source);
        cardbody.appendChild(audio);
    });
    document.body.appendChild(el);
}

function display_error(error) {
    alert(error);
}

var uri = 'api/Spam';

function addpage() {
    fetch(uri)
        .then(response => response.json())
        .then(data => displayData(data))
        .catch(error => display_error(error));
}