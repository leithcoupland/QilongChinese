using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTileSpawner : MonoBehaviour {

	public static MapTileSpawner instance = null;

	public MapTile mapTile;
	public float topLimit;
	public float bottomLimit;

	float tileLength;
	//bool scrolling;
	MapTile topTile;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		tileLength = mapTile.GetComponent<Renderer> ().bounds.size.z;
	}

	void Start(){
		//scrolling = true;
		PopulateInitialTiles ();
	}

	void Update(){
		if (topTile.transform.position.z <= topLimit - tileLength){
			SpawnNewTile (topTile.transform.position.z - (topLimit - tileLength));
		}
	}

	void PopulateInitialTiles(){
		Vector3 pos = new Vector3 (transform.position.x, transform.position.y, topLimit);
		SpawnNewTile ();
		while (pos.z > bottomLimit){
			pos += Vector3.back * tileLength;
			MapTile newTile = Instantiate (mapTile, pos, transform.rotation);
			int tileRotation = Random.Range (0, 3);
			//newTile.transform.Rotate (Vector3.up * tileRotation);
			Quaternion newRotation = Quaternion.Euler (0, tileRotation * 90, 0);
			newTile.transform.rotation = newRotation;

			//MapTile leftTile = Instantiate (mapTile, new Vector3 (pos.x - tileLength, pos.y, pos.z), Quaternion.Euler (0, tileRotation * 90, 0));
			//MapTile rightTile = Instantiate (mapTile, new Vector3 (pos.x + tileLength, pos.y, pos.z), Quaternion.Euler (0, tileRotation * 90, 0));
		}
	}

	public void SpawnNewTile(float diff = 0){
		topTile = Instantiate (mapTile, new Vector3 (transform.position.x, transform.position.y, topLimit + diff), transform.rotation);
		int tileRotation = Random.Range (0, 3);
		//topTile.transform.Rotate (Vector3.up * tileRotation);
		Quaternion newRotation = Quaternion.Euler (0, tileRotation * 90, 0);
		topTile.transform.rotation = newRotation;

		//MapTile leftTile = Instantiate (mapTile, new Vector3 (transform.position.x - tileLength, transform.position.y, topLimit + diff), Quaternion.Euler (0, tileRotation * 90, 0));
		//MapTile rightTile = Instantiate (mapTile, new Vector3 (transform.position.x + tileLength, transform.position.y, topLimit + diff), Quaternion.Euler (0, tileRotation * 90, 0));
	}
}
