using System;
using System.Collections.Generic;

namespace Deikstra
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new Graph();

            //добавление вершин
            g.AddVertex("A");
            g.AddVertex("B");
            g.AddVertex("C");
            g.AddVertex("D");
            g.AddVertex("E");

            //добавление ребер
            g.AddEdge("A", "C", 3);
            g.AddEdge("A", "D", 1);
            g.AddEdge("A", "E", 1);
            g.AddEdge("B", "C", 4);
            g.AddEdge("B", "E", 1);



            var dijkstra = new Dijkstra(g);
            var path = dijkstra.FindShortestPath("A", "B");
            int len = path.Length;
            int i = 0;
            int pathLen = 0;
            string newpath = "";
            string prevChara = "69420";
            foreach (var chara in path) {
                i++;
                if (prevChara == "69420")
                {
                    prevChara = chara.ToString();
                    var vert = g.FindVertex(chara.ToString());
                    foreach (var edge in vert.Edges)
                    {
                        if (edge.ConnectedVertex.Name == prevChara.ToString())
                        {
                            pathLen += edge.EdgeWeight;
                            prevChara = chara.ToString();
                        }
                    }
                    
                }
                else
                {
                    var vert = g.FindVertex(chara.ToString());
                    foreach (var edge in vert.Edges)
                    {
                        if (edge.ConnectedVertex.Name == prevChara.ToString())
                        {
                            pathLen += edge.EdgeWeight;
                            prevChara = chara.ToString();
                        }
                    }
                }
                if (!(i == len))
                {
                    newpath += $"{chara}-";
                }
                else
                {
                    newpath += chara;
                }
            }

            Console.WriteLine($"Path: {newpath}");
            Console.WriteLine($"Length: {pathLen}");
            Console.ReadLine();
        }
    }
    public class GraphEdge
    {
        public GraphVertex ConnectedVertex { get; }

        public int EdgeWeight { get; }
        public GraphEdge(GraphVertex connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }
    public class GraphVertex
    {
        public string Name { get; }
        public List<GraphEdge> Edges { get; }
        public GraphVertex(string vertexName)
        {
            Name = vertexName;
            Edges = new List<GraphEdge>();
        }
        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }
        public void AddEdge(GraphVertex vertex, int edgeWeight)
        {
            AddEdge(new GraphEdge(vertex, edgeWeight));
        }
        public override string ToString() => Name;
    }
    public class Graph
    {
        public List<GraphVertex> Vertices { get; }

        public Graph()
        {
            Vertices = new List<GraphVertex>();
        }

        public void AddVertex(string vertexName)
        {
            Vertices.Add(new GraphVertex(vertexName));
        }

        public GraphVertex FindVertex(string vertexName)
        {
            foreach (var v in Vertices)
            {
                if (v.Name.Equals(vertexName))
                {
                    return v;
                }
            }

            return null;
        }

        public void AddEdge(string firstName, string secondName, int weight)
        {
            var v1 = FindVertex(firstName);
            var v2 = FindVertex(secondName);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }
    }

    public class GraphVertexInfo
    {
        public GraphVertex Vertex { get; set; }

        public bool IsUnvisited { get; set; }

        public int EdgesWeightSum { get; set; }

        public GraphVertex PreviousVertex { get; set; }

        public GraphVertexInfo(GraphVertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
        }
    }

public class Dijkstra
    {
        Graph graph;

        List<GraphVertexInfo> infos;
        public Dijkstra(Graph graph)
        {
            this.graph = graph;
        }
        void InitInfo()
        {
            infos = new List<GraphVertexInfo>();
            foreach (var v in graph.Vertices)
            {
                infos.Add(new GraphVertexInfo(v));
            }
        }

        GraphVertexInfo GetVertexInfo(GraphVertex v)
        {
            foreach (var i in infos)
            {
                if (i.Vertex.Equals(v))
                {
                    return i;
                }
            }

            return null;
        }

        public GraphVertexInfo FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            GraphVertexInfo minVertexInfo = null;
            foreach (var i in infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        public string FindShortestPath(string startName, string finishName)
        {
            return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName));
        }

        public string FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }

                SetSumToNextVertex(current);
            }
            
            return GetPath(startVertex, finishVertex);
        }

        void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Vertex.Edges)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        string GetPath(GraphVertex startVertex, GraphVertex endVertex)
        {
            var path = endVertex.ToString();
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;
                path = endVertex.ToString() + path;
            }

            return path;
        }
    }
}