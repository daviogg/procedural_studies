using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsHandler : MonoBehaviour
{

    public GameObject RectanglePrefab;
    public float SpawnTimer = 2f;
    public float MinPosition = 0.5f;
    public float MaxPostion = 3f;
    public int NumberOfRooms = 15;

    private int roomsCounter = 0;
	private Vector3 PrefabPosition;
    private float Timer = 0f;

	public Transform RoomsTransform;

	private GameObject Room;
    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= SpawnTimer && roomsCounter < NumberOfRooms)
        {
            PrefabPosition = new Vector3(Random.Range(MinPosition, MaxPostion), //x 
                            Random.Range(MinPosition, MaxPostion), //y 
                                Random.Range(MinPosition, MaxPostion)); //z

			
            Room = Instantiate(RectanglePrefab, PrefabPosition, Quaternion.identity);
			Room.transform.parent = RoomsTransform;
			
			Timer = 0;
			roomsCounter ++;
        }
    }

}
