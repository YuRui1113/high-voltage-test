using SortingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SortingApp.Services
{
    class StepFileReader
    {
        static List<int> usedIds;

        public static void ParseSteps(ObservableCollection<Step> steps, string fileName, int max)
        {
            usedIds = new List<int>();

            FileStream fs = new FileStream(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/" + fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs);
            List<string> lines = new List<string>();
            while (!sr.EndOfStream)
            {
                lines.Add(sr.ReadLine());
            }

            if (lines != null && lines.Count > 1)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i].Trim();
                    if (line.Length > 0)
                    {
                        steps.Add(ParseStep(line, max));
                    }
                }
            }

            // Order by index
            var lstStep = steps.OrderBy(s => s.Index).ToList();
            steps.Clear();
            foreach (var step in lstStep)
            {
                steps.Add(step);
            }
        }

        private static Step ParseStep(string line, int max)
        {
            string[] stepItems = line.Split(";");

            return new Step(int.Parse(stepItems[0]), stepItems[1], GetRandomID(max));
        }

        private static int GetRandomID(int max)
        {
            var rand = new Random();
            int next = 0;
            do
            {
                next = rand.Next(1, max + 1);
            } while (usedIds.Contains(next));

            usedIds.Add(next);

            return next;
        }
    }
}
