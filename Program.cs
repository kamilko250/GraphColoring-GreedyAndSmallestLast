using System;
using System.Collections.Generic;
using System.IO;

namespace ASD_PS6
{
    public class Graph
    {
        public int V;
        public List<List<int>> IncidentList;
        public Graph(string path)
        {
            Read(path);
        }
        private void AddEdge(int v, int w)
        {
            IncidentList[v - 1].Add(w - 1);
            IncidentList[w - 1].Add(v - 1);
        }
        private int[] Greedy(List<int> orderList,bool[] colors,int[] result)
        {
            foreach (var u in orderList)
            {
                int freeColor;

                for (int i = 0; i < colors.Length; i++)
                    colors[i] = true;

                foreach (var i in IncidentList[u])
                    if (result[i] != -1)
                        colors[result[i]] = false;

                for (freeColor = 0; freeColor < V; freeColor++)
                    if (colors[freeColor])
                        break;

                result[u] = freeColor;
            }
            return result;
        }
        public void GreedyColoring()
        {
            List<int> orderList = new List<int>();
            for (int i = 0; i < V; i++)
                orderList.Add(i);
            int[] result = new int[V];
            bool[] colors = new bool[V];

            for (int i = 0; i < result.Length; i++)
                result[i] = -1;
            result[orderList[0]] = 0;

            Greedy(orderList, colors, result);
            Write(result,"Greedydata.txt");
        }

        private List<int> MakeOrder()
        {
            List<List<int>> newIncidentList = new List<List<int>>();
            for (int i = 0; i < IncidentList.Count; i++)
            {
                newIncidentList.Add(new List<int>());
            }
            int k = 0;
            foreach (var node in IncidentList)
            {
                foreach (var edge in node)
                    newIncidentList[k].Add(edge);
                k++;
            }

            List<int> orderList = new List<int>();
            int count = newIncidentList.Count;

            for (int i = 0; i < count; i++)
            {
                int minNodeValue = Search(newIncidentList, orderList);
                DeleteEdge(newIncidentList, minNodeValue);
                orderList.Add(minNodeValue);
            }
            return orderList;
        }
        public void SLColoring()
        {
            List<int> orderList = MakeOrder();
            int[] result = new int[V];
            bool[] colors = new bool[V];

            for (int i = 0; i < result.Length; i++)
                result[i] = -1;
            result[orderList[0]] = 0;

            Greedy(orderList, colors, result);
            Write(result,"Sldata.txt");
        }
        private int Search(List<List<int>> list, List<int> ignoreNodeList)
        {
            int minValue = int.MaxValue - 1;
            int nodeValue = -1;
            int i = 0;
            foreach (var element in list)
            {
                if (minValue > element.Count && !ignoreNodeList.Contains(i))
                {
                    minValue = element.Count;
                    nodeValue = i;
                }
                i++;
            }
            return nodeValue;
        }

        private void DeleteEdge(List<List<int>> list, int node)
        {
            list[node] = new List<int>();

            foreach (var nodes in list)
            {
                if(nodes.Contains(node))
                    nodes.Remove(node);
            }
        }

        private void Write(int[] result,string path)
        {
            int colorsCount = 0;
            int lastColor = 0;
            for (int u = 0; u < V; u++)
            {
                Console.WriteLine("Node " + (u + 1) + " --->  Color " + result[u]);
                if (result[u] != lastColor)
                    colorsCount++;
            }



            List<List<int>> colors = new List<List<int>>(colorsCount);
            for (int k = 0; k < colorsCount; k++)
            {
                colors.Add(new List<int>());
            }

            for (int j = 0; j < V; j++)
            {
                for (int k = 0; k < result.Length; k++)
                {
                    if (result[k] == j)
                    {
                        colors[j].Add(k + 1);
                        Console.WriteLine("Color " + j + " ---> Node " + (k + 1));
                    }
                }

            }

            StreamWriter writer = new StreamWriter(path);

            writer.WriteLine(colorsCount);
            int i = 0;
            foreach (var color in colors)
            {
                writer.Write("Color " + i + ": ");
                foreach (var node in color)
                    writer.Write(node + " ");
                writer.WriteLine();
                i++;
            }
            writer.Close();

        }

        private void Read(string path)
        {
            StreamReader reader = new StreamReader(path);

            string[] tab = reader.ReadLine().Split(" ");
            int nodes = int.Parse(tab[0]);
            int edges = int.Parse(tab[1]);

            V = nodes;
            IncidentList = new List<List<int>>(V);
            for (int i = 0; i < IncidentList.Capacity; i++)
                IncidentList.Add(new List<int>());

            for (int i = 0; i < edges; i++)
            {
                string[] tab1 = reader.ReadLine().Split(" ");
                int[] egzams = new int[tab1.Length];
                for (int k = 0; k < tab1.Length; k++)
                {
                    egzams[k] = int.Parse(tab1[k]);
                }

                for (int k = 0; k < egzams.Length; k++)
                {
                    for (int j = k + 1; j + k < egzams.Length; j++)
                    {
                        AddEdge(egzams[k], egzams[j]);
                    }
                }
            }
            reader.Close();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Graph g1 = new Graph("data.txt");
            g1.GreedyColoring();
            g1.SLColoring();
        }
    }
}

