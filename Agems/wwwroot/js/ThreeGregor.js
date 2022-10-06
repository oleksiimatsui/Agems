import * as THREE from "https://cdn.jsdelivr.net/npm/three@0.121.1/build/three.module.js";
import { GLTFLoader } from "https://cdn.jsdelivr.net/npm/three@0.121.1/examples/jsm/loaders/GLTFLoader.js";
import { OrbitControls } from "https://cdn.jsdelivr.net/npm/three@0.121.1/examples/jsm/controls/OrbitControls.js"
var scene, renderer;
var camera;
var mesh;

var isMouseDown = false;
 
function init() {
    scene = new THREE.Scene(); 
    camera = new THREE.PerspectiveCamera( 75, window.innerWidth/window.innerHeight, 0.1, 1000 ); 
    camera.position.z = 40; 
    camera.position.y = 5; 
    renderer = new THREE.WebGLRenderer();  
    renderer.setSize( window.innerWidth, window.innerHeight ); 
    document.body.appendChild( renderer.domElement ); 
    renderer.outputEncoding = true;

    var light = new THREE.DirectionalLight("white", 1);
    var ambient = new THREE.AmbientLight("#85b2cd");
    light.position.set( 200, 800, 400 ).normalize();
    scene.add(light);
    scene.add(ambient);
    scene.background = new THREE.Color("grey");

    var texture = new THREE.Texture();
    var manager = new THREE.LoadingManager();
    manager.onProgress = function ( item, loaded, total ) {};
    var onProgress = function ( xhr ) {};
    var onError = function ( xhr ) {};

    var loader = new GLTFLoader();
    var mesh;

    loader.load(
        '../models/Greg.glb',
        function (gltf) {
            mesh = gltf.scene;
            mesh.scale.set(3, 3, 3);
            scene.add(mesh);

        },
        function (xhr) {
            $("#loading").hide();
        }
    );


    const controls = new OrbitControls(camera, renderer.domElement);
    controls.target.set(0, 10, 0);
    controls.update();
    controls.enablePan = false;
    controls.enableDamping = true;
    //camera.position.z = 5;

    render();
    //function animate() {
    //    requestAnimationFrame(animate);
    //    controls.update();
    //    stats.update();
    //    renderer.render(scene, camera);
    //}
    //animate();
}

function render() {
    requestAnimationFrame( render ); 
    renderer.render(scene, camera); 
}

window.addEventListener('DOMContentLoaded', init);
