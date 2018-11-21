function OnMouseDown () {

	if(gameObject.transform.parent.FindChild("fractures").transform.childCount == 0){
		gameObject.transform.parent.gameObject.GetComponent.<chipOffController>().resetAll();
	}else {
		gameObject.transform.parent.gameObject.GetComponent.<chipOffController>().chipAll();
	}

}