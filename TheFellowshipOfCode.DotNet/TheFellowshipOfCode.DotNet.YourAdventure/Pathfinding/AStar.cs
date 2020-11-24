using System;
using System.Collections.Generic;
using System.Linq;

public class Astar
{
    public List<List<Node>> Grid { get; set; }
    public int GridRows => Grid[0].Count;
    public int GridCols => Grid.Count;

    public Astar(List<List<Node>> grid)
    {
        Grid = grid;
    }

    public Stack<Node> FindPath(Node start, Node end)
    {
        Stack<Node> path = new Stack<Node>();
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();
        Node current = start;

        openList.Add(start);

        while (openList.Count != 0 && !closedList.Exists(x => x.Position == end.Position))
        {
            current = openList[0];
            openList.Remove(current);
            closedList.Add(current);

            foreach (Node n in GetAdjacentNodes(current))
            {
                if (!closedList.Contains(n) && n.Walkable)
                {
                    if (!openList.Contains(n))
                    {
                        n.Parent = current;
                        n.DistanceToTarget = Math.Abs(n.Position.X - end.Position.X) + Math.Abs(n.Position.Y - end.Position.Y);
                        n.Cost = n.Weight + n.Parent.Cost;
                        openList.Add(n);
                        openList = openList.OrderBy(node => node.F).ToList<Node>();
                    }
                }
            }
        }

        // construct path, if end was not closed return null
        if (!closedList.Exists(x => x.Position == end.Position))
        {
            return null;
        }

        // if all good, return path
        Node temp = closedList[closedList.IndexOf(current)];
        if (temp == null) return null;
        do
        {
            path.Push(temp);
            temp = temp.Parent;
        } while (temp != start && temp != null);
        return path;
    }

    private List<Node> GetAdjacentNodes(Node n)
    {
        List<Node> temp = new List<Node>();

        int row = (int)n.Position.X;
        int col = (int)n.Position.Y;

        if (row + 1 < GridRows)
        {
            temp.Add(Grid[col][row + 1]);
        }
        if (row - 1 >= 0)
        {
            temp.Add(Grid[col][row - 1]);
        }
        if (col - 1 >= 0)
        {
            temp.Add(Grid[col - 1][row]);
        }
        if (col + 1 < GridCols)
        {
            temp.Add(Grid[col + 1][row]);
        }

        return temp;
    }
}
