using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MapTile : MonoBehaviour {

	Rigidbody myRigidbody;
	public float speed;

	void Start(){
		myRigidbody = transform.GetComponent<Rigidbody>();
		myRigidbody.velocity = Vector3.back * speed;
	}

	void FixedUpdate(){
		if (transform.position.z <= MapTileSpawner.instance.bottomLimit){
			Destroy (gameObject);
		}
	}
}
