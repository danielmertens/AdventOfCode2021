namespace AdventOfCode2021.Days;
internal class Day23 : AbstractDay
{
    protected override void Execute()
    {
        var nodes = CreatePuzzle();
        
        // limit the search initialy to a max score of 100_000 to eliminate some bad paths. 
        var cost = Rec(nodes, null, 100_000, 0); 

        Result(cost);
    }

    private int Rec(List<Node> nodes, Amphipod lastMoved, int costFound, int depth)
    {
        if (depth > 30) // Limit the search a bit
        {
            return costFound;
        }
        var currentCost = CalculateCost(nodes);
        if (currentCost >= costFound) return costFound;
        if (IsSolved(nodes))
        {
            Console.WriteLine($"We have a possible winner {currentCost} with depth {depth}");
            return currentCost;
        }

        foreach (var node in nodes)
        {
            if (!node.IsOccupied) continue;
            if (node.Amphipod == lastMoved) continue;
            if (!ShouldMove(node)) continue;
            var possibleDestinations = ReachableNodes2(node);

            foreach (var reachableDestination in possibleDestinations)
            {
                var destination = reachableDestination.node;
                if (!CanEnter(node, destination, node.Amphipod)) continue;
                destination.Amphipod = node.Amphipod;
                destination.Amphipod.StepsTaken += reachableDestination.steps;
                node.Amphipod = null;

                if (!(IsInRightRoom(destination.Type, destination.Amphipod.Type)
                    && ShouldMove(destination)))
                {
                    costFound = Rec(nodes, destination.Amphipod, costFound, depth + 1);
                }

                node.Amphipod = destination.Amphipod;
                node.Amphipod.StepsTaken -= reachableDestination.steps;
                destination.Amphipod = null;
            }
        }

        return costFound;
    }

    // Consider if the node should be moved.
    // Considers the node and the nodes this node is blocking in the room.
    private bool ShouldMove(Node node)
    {
        if (!node.IsOccupied) return true;
        if (!IsInRightRoom(node.Type, node.Amphipod.Type)) return true;
        if (node.Neighbours.Count == 1) return false;
        
        var nodesToEnd = NodesToEnd(node);

        foreach (var nto in nodesToEnd)
        {
            if (!nto.IsOccupied) return true;
            if (!IsInRightRoom(nto.Type, nto.Amphipod.Type)) return true;
        }

        return false;
    }

    // Get all nodes between the starting node and the backend of the room. (away from hallway)
    private List<Node> NodesToEnd(Node start)
    {
        var visited = new List<Node>() { start };
        var side1 = new List<Node>();
        var side2 = new List<Node>();

        var current = start.Neighbours[0];

        do
        {
            side1.Add(current);
            visited.Add(current);
            current = current.Neighbours.FirstOrDefault(n => !visited.Contains(n));
        } while (current != null && current.Type == start.Type);

        current = start.Neighbours[1];

        do
        {
            side2.Add(current);
            visited.Add(current);
            current = current.Neighbours.FirstOrDefault(n => !visited.Contains(n));
        } while (current != null && current.Type == start.Type);

        if (side1.Last().Neighbours.Count == 1)
        {
            return side1;
        }
        return side2;
    }

    // Check if a node type corresponds with a amphipodType.
    private bool IsInRightRoom(NodeTypes nodeType, AmphipodTypes amphipodType)
    {
        return (amphipodType == AmphipodTypes.Amber && nodeType == NodeTypes.RoomA)
            || (amphipodType == AmphipodTypes.Bronze && nodeType == NodeTypes.RoomB)
            || (amphipodType == AmphipodTypes.Copper && nodeType == NodeTypes.RoomC)
            || (amphipodType == AmphipodTypes.Desert && nodeType == NodeTypes.RoomD);
    }

    // Consider if the current node can enter the destination node.
    private bool CanEnter(Node current, Node destination, Amphipod amphipod)
    {
        return (destination.Type == NodeTypes.Hallway && current.Type != NodeTypes.Hallway)
            || (amphipod.Type == AmphipodTypes.Amber && destination.Type == NodeTypes.RoomA)
            || (amphipod.Type == AmphipodTypes.Bronze && destination.Type == NodeTypes.RoomB)
            || (amphipod.Type == AmphipodTypes.Copper && destination.Type == NodeTypes.RoomC)
            || (amphipod.Type == AmphipodTypes.Desert && destination.Type == NodeTypes.RoomD);

    }

    // Get all nodes that are considered to be valid next nodes for this node with the steps it requires.
    private List<ReachableNode> ReachableNodes2(Node start)
    {
        var destinations = new List<ReachableNode>();
        var visited = new List<Node>() { start };
        var openList = new List<Node>();
        openList.AddRange(start.Neighbours);
        var step = 1;

        while (openList.Count > 0)
        {
            var temp = new List<Node>();
            foreach (var consider in openList)
            {
                if (consider.IsOccupied || visited.Contains(consider)) continue;
                visited.Add(consider);
                if (consider.Type != NodeTypes.InFrontOfRoom)
                    destinations.Add(new ReachableNode { node = consider, steps = step });
                temp.AddRange(consider.Neighbours);
            }
            openList = temp;
            step++;
        }

        if (start.Type == NodeTypes.Hallway)
        {
            destinations = destinations.Where(d => d.node.Type != NodeTypes.Hallway).ToList();
        }

        return destinations;
    }

    private struct ReachableNode
    {
        public int steps { get; set; }
        public Node node { get; set; }
    }

    // Check if the solution is found. All Amphipods are in the correct room.
    private bool IsSolved(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            if (node.Type is NodeTypes.Hallway or NodeTypes.InFrontOfRoom) continue;
            if (node.Type is NodeTypes.RoomA && node.Amphipod != null && node.Amphipod.Type == AmphipodTypes.Amber) continue;
            if (node.Type is NodeTypes.RoomB && node.Amphipod != null && node.Amphipod.Type == AmphipodTypes.Bronze) continue;
            if (node.Type is NodeTypes.RoomC && node.Amphipod != null && node.Amphipod.Type == AmphipodTypes.Copper) continue;
            if (node.Type is NodeTypes.RoomD && node.Amphipod != null && node.Amphipod.Type == AmphipodTypes.Desert) continue;
            return false;
        }
        return true;
    }

    // Calculate the current cost.
    private int CalculateCost(List<Node> nodes)
    {
        int cost = 0;
        foreach (var node in nodes)
        {
            if (node.Amphipod != null)
            {
                if (node.Amphipod.Type == AmphipodTypes.Amber) cost += node.Amphipod.StepsTaken * 1;
                if (node.Amphipod.Type == AmphipodTypes.Bronze) cost += node.Amphipod.StepsTaken * 10;
                if (node.Amphipod.Type == AmphipodTypes.Copper) cost += node.Amphipod.StepsTaken * 100;
                if (node.Amphipod.Type == AmphipodTypes.Desert) cost += node.Amphipod.StepsTaken * 1000;
            }
        }
        return cost;
    }

    // Create the puzzle input.
    private List<Node> CreatePuzzle()
    {
        var nodes = new List<Node>();
        Node previous = null;
        for (int i = 0; i < 11; i++)
        {
            var node = new Node();
            if (i == 2 || i == 4 || i == 6 || i == 8)
            {
                node.Type = NodeTypes.InFrontOfRoom;
            }
            else
            {
                node.Type = NodeTypes.Hallway;
            }
            if (previous != null)
            {
                node.AddNeighbour(previous);
            }
            nodes.Add(node);
            previous = node;
        }

        // Hallway A
        var a1 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Desert },
            Type = NodeTypes.RoomA
        };
        var a2 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Desert },
            Type = NodeTypes.RoomA
        };
        var a3 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Desert },
            Type = NodeTypes.RoomA
        };
        var a4 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Copper },
            Type = NodeTypes.RoomA
        };
        nodes[2].AddNeighbour(a1);
        a1.AddNeighbour(a2);
        a2.AddNeighbour(a3);
        a3.AddNeighbour(a4);
        nodes.Add(a1);
        nodes.Add(a2);
        nodes.Add(a3);
        nodes.Add(a4);

        // Hallway B
        var b1 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Amber },
            Type = NodeTypes.RoomB
        };
        var b2 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Copper },
            Type = NodeTypes.RoomB
        };
        var b3 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Bronze },
            Type = NodeTypes.RoomB
        };
        var b4 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Amber },
            Type = NodeTypes.RoomB
        };
        nodes[4].AddNeighbour(b1);
        b1.AddNeighbour(b2);
        b2.AddNeighbour(b3);
        b3.AddNeighbour(b4);
        nodes.Add(b1);
        nodes.Add(b2);
        nodes.Add(b3);
        nodes.Add(b4);

        // Hallway C
        var c1 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Desert },
            Type = NodeTypes.RoomC
        };
        var c2 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Bronze },
            Type = NodeTypes.RoomC
        };
        var c3 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Amber },
            Type = NodeTypes.RoomC
        };
        var c4 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Bronze },
            Type = NodeTypes.RoomC
        };
        nodes[6].AddNeighbour(c1);
        c1.AddNeighbour(c2);
        c2.AddNeighbour(c3);
        c3.AddNeighbour(c4);
        nodes.Add(c1);
        nodes.Add(c2);
        nodes.Add(c3);
        nodes.Add(c4);

        // Hallway D
        var d1 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Copper },
            Type = NodeTypes.RoomD
        };
        var d2 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Amber },
            Type = NodeTypes.RoomD
        };
        var d3 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Copper },
            Type = NodeTypes.RoomD
        };
        var d4 = new Node
        {
            Amphipod = new Amphipod { Type = AmphipodTypes.Bronze },
            Type = NodeTypes.RoomD
        };
        nodes[8].AddNeighbour(d1);
        d1.AddNeighbour(d2);
        d2.AddNeighbour(d3);
        d3.AddNeighbour(d4);
        nodes.Add(d1);
        nodes.Add(d2);
        nodes.Add(d3);
        nodes.Add(d4);

        return nodes;
    }

    private class Node
    {
        public Amphipod Amphipod { get; set; }
        public bool IsOccupied => Amphipod != null;
        public List<Node> Neighbours { get; set; } = new List<Node>();
        public NodeTypes Type { get; set; }

        internal void AddNeighbour(Node node)
        {
            if (!Neighbours.Contains(node))
            {
                Neighbours.Add(node);
                node.AddNeighbour(this);
            }
        }
    }

    private enum NodeTypes
    {
        Hallway,
        InFrontOfRoom,
        RoomA,
        RoomB,
        RoomC,
        RoomD
    }

    private class Amphipod
    {
        public int StepsTaken { get; set; }
        public AmphipodTypes Type { get; set; }
    }

    private enum AmphipodTypes
    {
        Amber,
        Bronze,
        Copper,
        Desert
    }
}
