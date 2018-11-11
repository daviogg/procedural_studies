using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HullDelaunayVoronoi.Delaunay;
using HullDelaunayVoronoi.Primitives;

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
    private List<Vertex2> CentersOfRooms;
    private DelaunayTriangulation2 Delaunay;

    private Material lineMaterial;


    //TODO: rimuovere
    private Simplex<Vertex2> vertexForTest;

    private void Awake()
    {
        Rooms = new List<GameObject>();
        MainRooms = new List<GameObject>();
        CentersOfRooms = new List<Vertex2>();
    }

    private void Start()
    {
        Delaunay = new DelaunayTriangulation2();
        lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
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
            }
        });

        MainRooms.ForEach(mainRoom =>
        {
            mainRoom.GetComponent<SpriteRenderer>().color = MainRoomColor;  
        });

        StartCoroutine(GenerateDelaunayTriangulation());
       
    }

    IEnumerator GenerateDelaunayTriangulation()
    {
        yield return new WaitForSeconds(5);
        MainRooms.ForEach(mainRoom =>
        {
            Vector3 center = mainRoom.GetComponent<SpriteRenderer>().bounds.center;
            CentersOfRooms.Add(new Vertex2(center.x, center.y));
            Delaunay.Generate(CentersOfRooms);
        });
       
    }

    private void OnDrawGizmos()
    {
        if (MainRooms != null)
        {
            MainRooms.ForEach(mainRoom =>
            {
                Gizmos.DrawSphere(mainRoom.GetComponent<SpriteRenderer>().bounds.center, 0.3f);
            });
        }

        if (vertexForTest != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(new Vector3(vertexForTest.Vertices[0].X, vertexForTest.Vertices[0].Y, 0f), new Vector3(vertexForTest.Vertices[1].X, vertexForTest.Vertices[1].Y, 0.0f));
        }
    }

    private void OnPostRender()
    {
        if (Delaunay == null || Delaunay.Cells.Count == 0 || Delaunay.Vertices.Count == 0) return;

        GL.PushMatrix();

        GL.LoadIdentity();
        GL.MultMatrix(GetComponent<Camera>().worldToCameraMatrix);
        GL.LoadProjectionMatrix(GetComponent<Camera>().projectionMatrix);

        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);

        GL.Color(Color.red);

        foreach (DelaunayCell<Vertex2> cell in Delaunay.Cells)
        {
            DrawSimplex(cell.Simplex);
        }

        GL.End();
        GL.Begin(GL.QUADS);
        GL.Color(Color.yellow);

        foreach (Vertex2 v in Delaunay.Vertices)
        {
            DrawPoint(v);
        }

        GL.End();

        GL.PopMatrix();
    }

    private void DrawSimplex(Simplex<Vertex2> f)
    {
        GL.Vertex3(f.Vertices[0].X, f.Vertices[0].Y, 0.0f);
        GL.Vertex3(f.Vertices[1].X, f.Vertices[1].Y, 0.0f);

        GL.Vertex3(f.Vertices[0].X, f.Vertices[0].Y, 0.0f);
        GL.Vertex3(f.Vertices[2].X, f.Vertices[2].Y, 0.0f);

        GL.Vertex3(f.Vertices[1].X, f.Vertices[1].Y, 0.0f);
        GL.Vertex3(f.Vertices[2].X, f.Vertices[2].Y, 0.0f);

        vertexForTest = f;
    }

    

    private void DrawPoint(Vertex2 v)
    {
        float x = v.X;
        float y = v.Y;
        float s = 0.05f;

        GL.Vertex3(x + s, y + s, 0.0f);
        GL.Vertex3(x + s, y - s, 0.0f);
        GL.Vertex3(x - s, y - s, 0.0f);
        GL.Vertex3(x - s, y + s, 0.0f);
    }

    private void DrawCircle(Vertex2 v, float radius, int segments)
    {
        float ds = Mathf.PI * 2.0f / (float)segments;

        for (float i = -Mathf.PI; i < Mathf.PI; i += ds)
        {
            float dx0 = Mathf.Cos(i);
            float dy0 = Mathf.Sin(i);

            float x0 = v.X + dx0 * radius;
            float y0 = v.Y + dy0 * radius;

            float dx1 = Mathf.Cos(i + ds);
            float dy1 = Mathf.Sin(i + ds);

            float x1 = v.X + dx1 * radius;
            float y1 = v.Y + dy1 * radius;

            GL.Vertex3(x0, y0, 0.0f);
            GL.Vertex3(x1, y1, 0.0f);
        }

    }

}

