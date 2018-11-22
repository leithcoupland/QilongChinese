using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

	void Start () {
		transform.rotation = Quaternion.Euler(transform.rotation.x, Random.Range (1, 360), transform.rotation.z);
	}
}
