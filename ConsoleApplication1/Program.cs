﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string inPath = @"../../in.txt";
            string writePath = "../../out.txt";

            int[][] graph;
            List<int> result;
            int totalWeight;

            int start;
            int end;

            (graph, start, end) = ReadGraph(inPath);

            (result, totalWeight) = FordBellman(graph, start, end);

            WriteResult(writePath, result, totalWeight);
        }

        private static void WriteResult(string writePath, List<int> result, int totalWeight)
        {
            using (StreamWriter sw = new StreamWriter(writePath, false, Encoding.Default))
            {
                if (result == null)
                    sw.WriteLine("N");
                else
                {
                    sw.WriteLine("Y");
                    foreach (int node in result)
                    {
                        sw.Write(node + " ");
                    }
                    sw.WriteLine();
                    sw.WriteLine(totalWeight);
                }
            }
        }

        private static (int[][] graph, int start,int end) ReadGraph(string inPath)
        {
            int[][] graph;
            int start;
            int end;

            using (StreamReader sr = new StreamReader(inPath, Encoding.Default))
            {
                int numberOfNodes = Convert.ToInt32(sr.ReadLine());
                graph = new int[numberOfNodes + 1][];
                graph[0] = null;

                for (int j = 0; j < numberOfNodes + 1; j++)
                {
                    graph[j] = new int[numberOfNodes + 1];
                    for (int i = 0; i < numberOfNodes + 1; i++)
                        graph[j][i] = -1;
                }

                for (int i = 1; i < numberOfNodes + 1; i++)
                {
                    string[] words = sr.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    for (int j = 2; j < words.Length; j += 2)
                    {
                        int row = Convert.ToInt32(words[j - 2]);
                        int weight = Convert.ToInt32(words[j - 1]);
                        graph[i][row] = weight;
                    }
                }
                start = Convert.ToInt32(sr.ReadLine());
                end = Convert.ToInt32(sr.ReadLine());
            }

            return (graph, start, end);
        }

        private static (List<int> route, int weight) FordBellman(int[][] graph, int start, int end)
        {
            if (start == end)
                return (new List<int>(), 0);

            int[] D = new int[graph.GetLength(0)];
            int[] Next = new int[graph.GetLength(0)];

            D[start] = 0;
            Next[start] = 0;

            for (int i = 1; i < D.Length; i++)
            {
                if (i == start)
                    continue;
                D[i] = graph[start][i];
                Next[i] = start;
            }

            for (int i = 2; i < D.Length - 2; i++)
            {
                for (int j = 1; j < D.Length; j++)
                {
                    for (int k = 1; k < D.Length; k++)
                    {
                        if (j == start || k == start)
                            continue;
                        if (D[k] == -1 || graph[k][j] == -1)
                            continue;

                        if (D[k] * graph[k][j] > D[j])
                        {
                            D[j] = D[k] * graph[k][j];
                            Next[j] = k;
                        }
                    }
                }
            }

            return GetPath(start, end, D, Next);
        }

        private static (List<int> route, int weight) GetPath(int start, int end, int[] D, int[] Next)
        {
            List<int> result = new List<int>();
            if (D[end] == -1)
                return (null, 0);

            result.Add(end);
            int u = Next[end];
            while (Next[u] != 0)
            {
                result.Add(u);
                u = Next[u];
            }
            result.Add(start);

            result.Reverse();
            return (result, D[end]);
        }
    }
}