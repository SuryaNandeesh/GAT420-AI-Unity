using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class AINavDijkstra
{
    public static bool Generate(AINavNode startNode, AINavNode endNode, ref List<AINavNode> path)
    {
        var nodes = new SimplePriorityQueue<AINavNode>();

        //set the source node cost to 0
        startNode.Cost = 0;

        // queue source node with source cost as priority
        nodes.EnqueueWithoutDuplicates(startNode, startNode.Cost);

        bool found = false;
        while (!found && nodes.Count > 0)
        {
            //dequeue node
            var node = nodes.Dequeue();

            //is node at destination?
            if (node == endNode)
            {
                found = true;
                break;
            }

            foreach (var neighbor in node.neighbors)
            {
                // calculate cost to neighbor = node cost + distance to neighbor
                float cost = node.Cost + Vector3.Distance(node.transform.position, neighbor.transform.position);
                // if cost < neighbor cost, add to priority queue
                if (cost < neighbor.Cost)
                {
                    // set neighbor cost to cost
                    neighbor.Cost = cost;
                    // set neighbor parent to node
                    neighbor.Parent = node;
                    // enqueue without duplicates, neighbor with cost as priority
                    nodes.EnqueueWithoutDuplicates(neighbor, cost);
                }
            }
        }

        path.Clear();
        if (found)
        {
            // create path from destination to source using node parents
            CreatePathFromParents(endNode, ref path);
        }

        return found;
    }

    public static void CreatePathFromParents(AINavNode node, ref List<AINavNode> path)
    {
        // while node not null
        while (node != null)
        {
            // add node to list path
            path.Add(node);
            // set node to node parent
            node = node.Parent;
        }

        // reverse path
        path.Reverse();
    }
}