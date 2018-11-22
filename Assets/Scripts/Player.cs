using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

	public float speed;
	public float dashSpeed;
	public float dashLengthSecs;
	public float xMin, xMax, zMin, zMax;

	Rigidbody myRigidbody;
	bool dashing = false;
	Vector3 velocity;

	void Start(){
		myRigidbody = transform.GetComponent<Rigidbody>();
	}

	void FixedUpdate(){
		if (dashing) {
			HandleDashMovement ();
		} else {
			HandleMoveInput ();
			HandleActionInput ();
		}
	}

	void HandleMoveInput(){
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);
		myRigidbody.velocity = movement * speed;
		myRigidbody.position = new Vector3 (Mathf.Clamp (myRigidbody.position.x, xMin, xMax), 0f, Mathf.Clamp (myRigidbody.position.z, zMin, zMax));
	}

	void HandleActionInput(){
		if (Input.GetKey(KeyCode.Space)){
			StartDash ();
		}
		if (Input.GetKey(KeyCode.Escape)){
			MusicManager.instance.PlayMenuMusic ();
			SceneManager.LoadScene ("Menu");
		}
	}

	void OnTriggerEnter(Collider col){
		//Debug.Log (col.gameObject.name);
		if (col.gameObject.layer == 8) { // if obstacle
			Destroy (gameObject);
		} else if (col.gameObject.layer == 9 && dashing) { // if block
			bool correct = col.gameObject.GetComponent<Block>().Hit();
			if (!correct){
				TakeDamage ();
			}
		}
	}

	void HandleDashMovement(){
		myRigidbody.velocity = Vector3.forward * dashSpeed;
		myRigidbody.position = new Vector3 (Mathf.Clamp (myRigidbody.position.x, xMin, xMax), 0f, Mathf.Clamp (myRigidbody.position.z, zMin, zMax));
	}

	void StartDash(){
		if (!dashing) {
			dashing = true;
			Invoke ("EndDash", dashLengthSecs);
		}
	}

	void EndDash(){
		dashing = false;
	}

	public bool IsDashing(){
		return dashing;
	}

	void TakeDamage(){
		QuestionManager.instance.FlashAnswerOn ();
	}
}
