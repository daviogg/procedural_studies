using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsHandler : MonoBehaviour {
    
	public GameObject RectanglePrefab;
	private float Timer = 0f;
	public float SpawnTimer = 2f;

	private Vector3 PrefabPosition;

	public float MinPosition = 0.5f;
	public float MaxPostion = 3f;
	
	void Update () {
		
		Timer += Time.deltaTime;
		
		if(Timer >= SpawnTimer){
			PrefabPosition = new Vector3(Random.Range(MinPosition, MaxPostion), //x 
							Random.Range(MinPosition, MaxPostion), //y 
								Random.Range(MinPosition, MaxPostion)); //z
			
			Instantiate(RectanglePrefab,PrefabPosition, Quaternion.identity);
			Timer = 0;
		}
	}

}
