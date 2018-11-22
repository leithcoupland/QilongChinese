using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Block : MonoBehaviour {

	public BlockFragment fragment;
	public float fragmentVelocity;

	Rigidbody myRigidbody;
	string answer = "";
	float lifetime = 10f;
	int speed = 6;
	TextMesh answerText;

	void Start(){
		myRigidbody = transform.GetComponent<Rigidbody>();
		myRigidbody.velocity = Vector3.back * speed;

		answer = QuestionManager.instance.RandomAnswer (true);
		answerText = GetComponentInChildren<TextMesh> ();

		Destroy (gameObject, lifetime);
	}

	void Update(){
		answerText.text = answer;
	}

	/*void OnTriggerEnter(Collider col){
		//Debug.Log (col.gameObject.name);
		if (col.gameObject.name == "Player" && col.gameObject.GetComponent<Player>().IsDashing()){
			if (QuestionManager.instance.TryAnswer(answer)){
				Destroy (gameObject);
			}
		}
	}*/

	public bool Hit(){
		if (QuestionManager.instance.TryAnswer(answer)){
			SpawnFragments ();
			AudioManager.instance.PlayHitSound ();
			Destroy (gameObject, 0.01f);
			return true;
		} else{
			return false;
		}
	}

	public void SetSpeed(int _speed){
		speed = _speed;
	}

	void SpawnFragments(){
		int fragmentsPerRow = 4;
		float fragmentSize = 1;
		Vector3 startPoint = new Vector3 (transform.position.x-1.5f, transform.position.y-1.5f, transform.position.z-1.5f);
		for (int x = 0; x < fragmentsPerRow; x++){
			for (int y = 0; y < fragmentsPerRow; y++){
				for (int z = 0; z < fragmentsPerRow; z++){
					if (Random.Range (0, 2) == 1) {
						continue;
					}
					float fragX = startPoint.x + x * fragmentSize;
					float fragY = startPoint.y + y * fragmentSize;
					float fragZ = startPoint.z + z * fragmentSize;
					BlockFragment newFrag = Instantiate (fragment, new Vector3 (fragX, fragY, fragZ), transform.rotation);
					newFrag.SetVelocity (Vector3.Normalize(newFrag.transform.position - transform.position) * fragmentVelocity);
				}
			}
		}
	}
}
