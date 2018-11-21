var forceMax: int;
var forceMin: int;
var sParent: Transform;
var chipped:boolean;
var sPos: Vector3;
var sRot: Quaternion;
var sScale: Vector3;
var chipControl: chipOffController;
var isFake: boolean;
var hitPoints: float = 1;

function Start() {
	if (!isFake) {
	    sParent = gameObject.transform.parent;
	    chipControl = gameObject.transform.parent.parent.GetComponent("chipOffController");
	    forceMax = chipControl.forceMax * chipControl.chipMass;
	    forceMin = chipControl.forceMin * chipControl.chipMass;  
	    sRot = transform.rotation;
	    sScale = transform.localScale;
	    if (chipControl.softNormals) {
	        var mesh: Mesh = transform.GetComponent(MeshFilter).mesh;
	        var rotation: Quaternion;
	      	var normals: Vector3[] = mesh.vertices;
	      	mesh.normals = normals;
	    }
	    yield WaitForSeconds(this.chipControl.fracturableDelay);
	    transform.gameObject.GetComponent(MeshCollider).enabled = true;
    }
}

/*function OnMouseOver() {
  	if (!chipped && chipControl.mouseOverChips) {
  		chipMe(chipControl.hitPointDecrease);
  	}
}*/

function OnCollisionEnter(collision : Collision) {   
    if (this.chipControl.collideChipMagnitude >0&&collision.relativeVelocity.magnitude > this.chipControl.collideChipMagnitude&&!chipped){
    	chipMe(collision.relativeVelocity.magnitude*.2*chipControl.hitPointDecrease);
    }
}

function addForce(fMin: int, fMax: int) {
	var forceX: float = Random.Range(fMin, fMax);
	if (Random.value > 0.5) {
		forceX *= -1;
	}
	var forceY: float = Random.Range(fMin, fMax);
	if (Random.value > 0.5) {
	    forceY *= -1;
	}
	gameObject.GetComponent.<Rigidbody>().AddForce(forceX, Random.Range(fMin, fMax), forceY);
}

function chipMe(hitFor: float) {
	hitPoints -= hitFor;
    if (!chipped && hitPoints < 0) {
	    gameObject.AddComponent(Rigidbody);
	    gameObject.GetComponent(Rigidbody).mass = chipControl.chipMass;
	    chipControl.goArray.push(transform);
	    if (chipControl.fractureMaterialChipped != null) {
	      	gameObject.GetComponent.<Renderer>().material = chipControl.fractureMaterialChipped;
   	 	}
	    addForce(forceMin, forceMax);
	    gameObject.transform.parent = null;  //Releases fragment from parent
	    sPos = transform.position;
	    chipped = true;
	    if (gameObject.GetComponent(Rigidbody) != null && chipControl.disableRigidDelay > 0) {
	        removeRigid(chipControl.disableRigidDelay);
	    }
	    if (chipControl.disableDelay > 0) {
	        disableMe(chipControl.disableDelay);
	    }
	    if (chipControl.disableCollideDelay > 0) {
	        disableCollide(chipControl.disableCollideDelay + Random.value);
	    }
	    if (chipControl.selfCollideDelay > 0) {
	        selfCollideEnable(chipControl.selfCollideDelay);
	    } else {
	        gameObject.GetComponent(MeshCollider).convex = false;
	    }
	    if (chipControl.cloneFractures > 0) cloneFracture(chipControl.cloneFractures);
	    transform.localScale = transform.localScale * chipControl.chipOffScale;
	    if (chipControl.particleOneShot != null) {
	        addParticles(chipControl.particleDelay);
	    }
    	if (chipControl.chipAllOnOne&&hitPoints>-100) //chipAll() chips all fracture fragments by reducing hitPoints by 200, this ensures that chipAll() only runs once
    		chipControl.chipAll();
  		} else {
    	if (hitFor < 1) gameObject.transform.Rotate(Random.Range(-chipControl.rotateOnHit, chipControl.rotateOnHit+1) * (1-hitPoints), 0, Random.Range(-chipControl.rotateOnHit, chipControl.rotateOnHit+1) * (1-hitPoints));
    	if (chipControl.particleOneShot != null) addParticles(0);
    
  	}
}

function addParticles(uSec: float) {
	yield WaitForSeconds(uSec);
	GameObject.Instantiate(chipControl.particleOneShot, transform.position, transform.rotation);
}

function cloneFracture(uClones: int) {
  	for (var i: int = 0; i < uClones; i++) {
	    var fake: GameObject = GameObject.Instantiate(gameObject, transform.position + Vector3(Random.Range(-.2, .2), Random.Range(-.2, .2), Random.Range(-.2, .2)), transform.rotation);
	    fake.GetComponent.<chipOffChild>().isFake = true;
	    fake.GetComponent.<chipOffChild>().addForce(forceMin, forceMax);
	    fake.transform.localScale = transform.localScale * (0.2 + Random.value * chipControl.chipOffScale);
	    chipControl.fakeArray.push(fake.transform);
	    if (chipControl.disableRigidDelay > 0) {
	        fake.GetComponent.<chipOffChild>().removeRigid(chipControl.disableRigidDelay);
	    }
	    if (chipControl.disableDelay > 0) {
	        fake.GetComponent.<chipOffChild>().disableMe(chipControl.disableDelay);
	    }
	    if (chipControl.disableCollideDelay > 0) {
	        fake.GetComponent.<chipOffChild>().disableCollide(chipControl.disableCollideDelay - Random.value);
	    }
	    if (chipControl.selfCollideDelay > 0) {
	        fake.GetComponent.<chipOffChild>().selfCollideEnable(chipControl.selfCollideDelay);
	    } else {
	        fake.GetComponent(MeshCollider).convex = false;
	    }
  	}
}

function selfCollideEnable(uSec: float) {
	yield WaitForSeconds(uSec);
	gameObject.GetComponent(MeshCollider).convex = true;
}

function removeRigid(uSec: float) {
    yield WaitForSeconds(uSec);
    gameObject.Destroy(GetComponent(Rigidbody));
}

function disableCollide(uSec: float) {
    yield WaitForSeconds(uSec);
    gameObject.GetComponent(MeshCollider).enabled = false;
}

function disableMe(uSec: float) {
    yield WaitForSeconds(uSec);
    if (isFake) {
    	GameObject.Destroy(transform.gameObject);
    } else {
    	gameObject.active = false;
    }
}

function resetMe() {
    transform.gameObject.active = true;
    //Disable colliders and rigidbody
    transform.gameObject.GetComponent(MeshCollider).enabled = false;
    transform.gameObject.GetComponent(MeshCollider).convex = false;
    if (transform.gameObject.GetComponent(Rigidbody) != null) {
    	transform.gameObject.Destroy(GetComponent(Rigidbody));
    }
    //Resets material if needed
    if (chipControl.fractureMaterialChipped != null) {
    	transform.gameObject.GetComponent.<Renderer>().material = chipControl.fractureMaterial;
    }
    //Resets transforms
    transform.position = sPos;
    transform.rotation = sRot;
    transform.parent = sParent;
    transform.localScale = sScale;
    //Destroys then reapplies script
    transform.gameObject.Destroy(GetComponent("chipOffChild"));
    transform.gameObject.AddComponent.<chipOffChild>();
}