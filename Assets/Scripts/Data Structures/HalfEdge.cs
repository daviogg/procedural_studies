using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfEdge {

    //The vertex the edge points to
    public Vertex v;

    //The face this edge is a part of
    public Triangle t;

    //The next edge
    public HalfEdge nextEdge;
    //The previous
    public HalfEdge prevEdge;
    //The edge going in the opposite direction
    public HalfEdge oppositeEdge;

    public HalfEdge(Vertex v)
    {
        this.v = v;
    }
}
