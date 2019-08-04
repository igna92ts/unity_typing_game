using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class FixedScroll : MonoBehaviour 
{
	public float scrollSpeed = 0.025f;
 
	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y - scrollSpeed, transform.position.z );
	}
}