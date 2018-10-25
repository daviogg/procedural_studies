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

    public float Radius = 2f;

    private List<GameObject> Rooms;

    private List<GameObject> MainRooms;


    private void Awake()
    {
        Rooms = new List<GameObject>();
    }
    

    void Update()
    {
        Timer += Time.deltaTime;

        if (Timer >= SpawnTimer && roomsCounter < NumberOfRooms)
        {
            PrefabPosition = GetRandomPointInCircle(Radius);

            Room = Instantiate(RectanglePrefab, PrefabPosition, Quaternion.identity);
            Room.transform.SetParent(RoomsTransform);

            Room.AddComponent<Rigidbody2D>().gravityScale = 0;
            Room.GetComponent<Rigidbody2D>().freezeRotation = true;

            Rooms.Add(Room);
			Timer = 0;
			roomsCounter ++;
        }
        else
        {
            if (roomsCounter == NumberOfRooms)
            {
                ApplyPhysics();
                roomsCounter++;
            }
        }
    }



    private Vector2 GetRandomPointInCircle(float radius) { //(ellipse_width, ellipse_height)
        float t = 2 * Mathf.PI * Random.value;
        float u = Random.value + Random.value;
        float r = 0f;

        r = u > 1 ? 2 - u : r = u; 

        return new Vector2(radius * r * Mathf.Cos(t), radius*r*Mathf.Sin(t));

        //return Mathf.Round(ellipse_width * r *Mathf.Cos(t)/2, tile_size),Mathf.Round(ellipse_height * r *Mathf.Sin(t)/2, tile_size)
    }

    private void ApplyPhysics()
   {
        Rooms.ForEach(room =>
        {
            room.AddComponent<PolygonCollider2D>();        
        });
   }


}
