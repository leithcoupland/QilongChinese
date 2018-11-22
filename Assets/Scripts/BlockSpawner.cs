using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour {

	public Block block;
	public float minSpawnRateSeconds;
	public float maxSpawnRateSeconds;
	public int horizontalSpawnRange;
	public int blockSpeed;

	//bool spawning;

	void Start(){
		//spawning = true;
		SpawnBlock ();
	}

	void SpawnBlock(){
		Vector3 pos = transform.position + Vector3.right * (Random.Range (0, horizontalSpawnRange) - horizontalSpawnRange/2);
		Block newBlock = Instantiate (block, pos, transform.rotation);
		newBlock.SetSpeed (blockSpeed);
		Invoke ("SpawnBlock", Random.Range(minSpawnRateSeconds, maxSpawnRateSeconds));
	}
}
