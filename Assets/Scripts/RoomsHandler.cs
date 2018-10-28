using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsHandler : MonoBehaviour
{

    public GameObject RectanglePrefab;
    public Transform RoomsTransform;

    public float SpawnTimer = 2f;
    public int NumberOfRooms = 15;
    public float Radius = 2f;
    public Color MainRoomColor;

    private int roomsCounter = 0;
	private Vector3 PrefabPosition;
    private float Timer = 0f;

	private GameObject Room;
    private List<GameObject> Rooms;
    private List<GameObject> MainRooms;



    private void Awake()
    {
        Rooms = new List<GameObject>();
        MainRooms = new List<GameObject>();
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
                DetectMainRooms();
                roomsCounter++;
            }
        }
    }

    private Vector2 GetRandomPointInCircle(float radius) { //(ellipse_width, ellipse_height)

        float t = 2 * Mathf.PI * Random.value;
        float u = Random.value + Random.value;
        float r = 0f;
        float x;
        float y;
        

        r = u > 1 ? 2 - u : r = u;

        x = Utility.RoundM(radius * r * Mathf.Cos(t));
        y = Utility.RoundM(radius * r * Mathf.Sin(t));

        return new Vector2(x,y);
        //return Mathf.Round(ellipse_width * r *Mathf.Cos(t)/2, tile_size),Mathf.Round(ellipse_height * r *Mathf.Sin(t)/2, tile_size)
    }

   
    private void ApplyPhysics()
   {
        Rooms.ForEach(room =>
        {
            room.AddComponent<PolygonCollider2D>();        
        });
   }

    //Prendo la media delle width e delle height, moltiplico per 1.25 la media, quella è la soglia
    private void DetectMainRooms()
    {
        float threshold;
        float averageW;
        float averageH;
        float sumW = 0;
        float sumH = 0;
        
        Rooms.ForEach(room => {
            sumW += room.GetComponent<Room>().Width;
            sumH += room.GetComponent<Room>().Height;
        });

        averageW = sumW / (Rooms.Count);
        averageH = sumH / (Rooms.Count);

        threshold = (averageH * averageW) * 1.2f;

        Rooms.ForEach(room => {
            if(room.GetComponent<Room>().Width * room.GetComponent<Room>().Height > threshold)
            { 
                MainRooms.Add(room);
                Debug.Log("Added room");
            }
        });

        MainRooms.ForEach(mainRoom =>
        {
            mainRoom.GetComponent<SpriteRenderer>().color = MainRoomColor;
        });
    }


}
