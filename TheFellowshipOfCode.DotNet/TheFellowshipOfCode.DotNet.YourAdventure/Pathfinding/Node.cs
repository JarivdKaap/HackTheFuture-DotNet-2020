using System;
using System.Drawing;
using System.Numerics;

public class Node
{
    public Node Parent { get; set; }
    public Point Position { get; set; }
    public float DistanceToTarget { get; set; }
    public float Cost { get; set; }
    public float Weight { get; set; }

    public float F
    {
        get
        {
            if (Math.Abs(DistanceToTarget - (-1)) > 0.001 && Math.Abs(Cost - (-1)) > 0.001)
                return DistanceToTarget + Cost;
            return -1;
        }
    }

    public bool Walkable { get; set; }

    public Node(Point pos, bool walkable, float weight = 1)
    {
        Parent = null;
        Position = pos;
        DistanceToTarget = -1;
        Cost = 1;
        Weight = weight;
        Walkable = walkable;
    }
}
