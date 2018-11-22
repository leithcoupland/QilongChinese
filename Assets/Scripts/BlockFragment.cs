using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BlockFragment : MonoBehaviour {

	Rigidbody myRigidbody;
	float lifetime = 5f;

	void Awake(){
		myRigidbody = transform.GetComponent<Rigidbody>();
	}

	void Start(){
		Destroy (gameObject, lifetime);
	}

	public void SetVelocity(Vector3 velocity){
		myRigidbody.velocity = velocity;
	}
}
