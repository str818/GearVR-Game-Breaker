var setX:float = 0;
var setY:float = 0;

function Start () {

this.gameObject.GetComponent.<Renderer>().material.SetTextureOffset("_MainTex", Vector2(setX,setY));
this.gameObject.GetComponent.<Renderer>().material.SetTextureOffset("_BumpMap", Vector2(setX,setY));

}