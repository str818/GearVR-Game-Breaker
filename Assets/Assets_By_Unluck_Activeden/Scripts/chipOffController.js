var goArray = new Array(); //Contains chipped fracture fragments
var fakeArray = new Array(); //Contains chipped clones
var forceMax = 250; //Maximum force put on fracture fragments when chipped
var forceMin = 50; //Minimun force put on fracture fragments when chipped
var chipOffScale: float = 1; //Scales fracture fragments after chipped (clones randomize scale 0.2+random.value*chipOffScale)
var selfCollideDelay: float = 0.5; //Delays fracture fragments from colliding with other fracture fragments for, this will make fracture fragments nonsticky and always fall to the ground.
var disableDelay: float = 0; //Disables fracture fragments after chipped, 0 never disable. (seconds)
var disableRigidDelay: float = 4; //Disables rigid body after chipped, 0 never disable. (seconds)
var disableCollideDelay: float = 0; //Disables collider after chipped, 0 never disable. +Random.value(0~1) (seconds )
var fracturableDelay: float = 2; //Delays effect from start and reset. (seconds)
var rotateOnHit: int = 10; //Rotates randomly when hit (random degrees * fracture.hitPoints(0-1))
var collideChipMagnitude = 0;	//Chips fracture fragments on collision magnitude (0 disabled, 5 good, 25 max)
var chipAllOnOne = false; //True > chips all fracture fragments from base if one is chipped
var fractureShadows = false; //High performance may want to enable shadows
var fractureRecieveShadows = false; //High performance may want to enable shadows
var fractureMaterial: Material; //Easy change material on fracture fragments
var fractureMaterialChipped: Material; //Change material on chipped fracture fragments
var particleOneShot: ParticleEmitter; //Attach a particle emitter from project, this will spawn on ever chip
var particleDelay: float = 0.01; //Delays spawn of the particle emitter
var cloneFractures: int = 0; //Add clones of each fracture fragments when chipped (increased effect)
var chipMass: float = 0.01; //Each fracture fragments mass
var pMat: PhysicMaterial; //Adds physic material to each fragment, can be null
var softNormals: boolean = false; //Experimental alteration of normals
var mouseOverChips:boolean = true;	//True will trigger fracture fragments to chip on mouse over
var hitPointDecrease: float = .2;	//Amount decreased from fracture fragment hitPoints(1 =100%) on mouse over or *collide magnitude
var addBaseScript: boolean = true; //Adds "chipOffBase" script to the base making it clickable to reset fracture fragments

function Start() {
  if (disableCollideDelay > 0) {
    disableCollideDelay += 1;
  }
  for (var child: Transform in transform.FindChild("fractures")) {
    child.gameObject.AddComponent.<chipOffChild>();
    child.gameObject.AddComponent(MeshCollider);
    child.gameObject.GetComponent(MeshCollider).convex = true;
    if (pMat != null) {
      child.gameObject.GetComponent(MeshCollider).material = pMat;
    }
    if (!fractureShadows) child.gameObject.GetComponent.<Renderer>().castShadows = false;
    if (!fractureRecieveShadows) child.gameObject.GetComponent.<Renderer>().receiveShadows = false;
    if (fractureMaterial != null) {
      child.gameObject.GetComponent.<Renderer>().material = fractureMaterial;
    }
  }
  if (fractureMaterial == null) {
    fractureMaterial = transform.FindChild("fractures").GetChild(0).GetComponent.<Renderer>().material;
  }
  if (this.addBaseScript) {
    if (transform.FindChild("base") != null) {
      var chipBase: GameObject = transform.FindChild("base").gameObject;
      chipBase.AddComponent(BoxCollider);
      chipBase.AddComponent.<chipOffBase>();
    }
  }
}
function chipAll() {
  var bufferCloneFractures: int = cloneFractures;
  cloneFractures = 0;
  for (var i = transform.FindChild("fractures").childCount; i > 0; i--) {
    var child: chipOffChild = transform.FindChild("fractures").transform.GetChild(i - 1).GetComponent("chipOffChild");
    if (child != null) {
      child.chipMe(200);
    }
  }
  cloneFractures = bufferCloneFractures;
}
function resetAll() {
  //Destroys all fake fractures
  for (var f = 0; f < fakeArray.length; f++) {
    if (fakeArray[f] != null) GameObject.Destroy(fakeArray[f]);
  }
  //Resets all fractures
  for (var i = 0; i < goArray.length; i++) {
  	var gai : GameObject = goArray[i];
    gai.GetComponent.<chipOffChild>().resetMe();
  }
  //Resets gameObject Arrays
  fakeArray = new Array();
  goArray = new Array();
}