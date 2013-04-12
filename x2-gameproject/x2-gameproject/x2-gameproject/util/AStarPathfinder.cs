using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace X2Game
{
    class AStarPathfinder
    {
        /// <summary>
        /// A* Pathfinding algorithm using Manhattan heuristic for weights
        /// </summary>
        /// <param name="startX">start x coordinate</param>
        /// <param name="startY">starting y coordinate</param>
        /// <param name="goalX">the target x coordinate</param>
        /// <param name="goalY">the target y coordinate</param>
        /// <param name="tileMap">tilemap of the world</param>
        /// <returns>list of waypoints or null if no path was found</returns>
        public Stack<Node> FindPath(int startX, int startY, int goalX, int goalY, TileMap tileMap)
        {
            LinkedList<Node> openNodes = new LinkedList<Node>();
            HashSet<int> closedNodes = new HashSet<int>();

            //Add starting node (our current position)
            openNodes.AddFirst(new Node(startX, startY));

            //A* Pathfinding algorithm using Manhattan heuristic for weights
            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes.First.Value;
                openNodes.RemoveFirst();

                //Explore surrounding tiles
                for (int x = -1; x <= 1; ++x)
                {
                    for (int y = -1; y <= 1; ++y)
                    {
                        int dx = currentNode.X + x;
                        int dy = currentNode.Y + y;

                        //Don't check diagonals (seems like this results in better behaviour)
                        if (x != 0 && y != 0) continue;

                        //Already explored?
                        if (closedNodes.Contains(dx | (dy << 16))) continue;
                        closedNodes.Add(dx | (dy << 16));

                        //Reached our goal?
                        if (goalX == dx && goalY == dy)
                        {
                            Stack<Node> result = new Stack<Node>();
                            Node explore = new Node(dx, dy, 0, currentNode);
                            do
                            {
                                result.Push(explore);
                                explore = explore.Parent;
                            } while (explore != null);

                            return result;
                        }

                        //Impassable tile?
                        if (!tileMap.IsWalkable(dx, dy)) continue;

                        //Explore node!
                        int weight = Math.Abs(dx - goalX) + Math.Abs(dy - goalY) + currentNode.Weight;

                        bool foundPlace = false;
                        LinkedListNode<Node> node = openNodes.First;
                        while (node != null)
                        {
                            if (node.Value.Weight > weight)
                            {
                                openNodes.AddBefore(node, new Node(dx, dy, weight, currentNode));
                                foundPlace = true;
                                break;
                            }

                            node = node.Next;
                        }

                        if (!foundPlace) openNodes.AddLast(new Node(dx, dy, weight, currentNode));

                    }
                }
            }

            return new Stack<Node>();
        }

        public class Node : IComparable<Node>
        {
            public readonly int X;
            public readonly int Y;
            public readonly int Weight;
            public readonly Node Parent;

            public Node(int setX, int setY, int setWeight = 0, Node setParent = null)
            {
                X = setX;
                Y = setY;
                Weight = setWeight;
                Parent = setParent;
            }

            public int CompareTo(Node other)
            {
                return Weight.CompareTo(other.Weight);
            }

            public override string ToString()
            {
                return X + ", " + Y;
            }
        }
    }
}
