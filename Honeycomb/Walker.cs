using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb
{
    public class Walker
    {
        /// <summary>
        /// For each cell keep a list of destinations.
        /// nStepsFrom[fromCell][steps][toCell] = no of occurrences.
        /// e.g. nSteps["0,0"][4]["-1,-2"] = no of times we end at "-1,-2" starting from "0,0" taking 4 steps.
        /// </summary>
        private Dictionary<string, List<Dictionary<string, long>>> nStepsFrom;

        public Honeycomb<long> Honeycomb { get; private set; }

        public int Steps { get; private set; }

        public class CacheingEventArgs
        {
            public string Message { get; set; }
        }

        public delegate void CacheingEventHandler(object sender, CacheingEventArgs cea);

        public event CacheingEventHandler CacheingData;

        public Walker(Honeycomb<long> honeycomb, int steps)
        {
            Honeycomb = honeycomb;
            Steps = steps;

            InitNStepsFrom();
        }

        public Cell<long> Walk()
        {
            Cell<long> startPoint = Honeycomb[0, 0];
            Cell<long> position = startPoint;
            Dictionary<string,long> endPoints = Step(position, Steps);
            foreach (string cellKey in endPoints.Keys)
                Honeycomb[cellKey].Data = endPoints[cellKey];

            long maxEndings = long.MinValue;
            Cell<long> mostLikely=null;
            Honeycomb.ForEachCell((Cell<long> cell) =>
            {
                if (cell.Data>maxEndings)
                {
                    maxEndings = cell.Data;
                    mostLikely = cell;
                }
            });

            return mostLikely;
        }

        private Dictionary<string, long> Step(Cell<long> position, int steps)
        {
            Dictionary<string, long> endPoints = nStepsFrom[position.Key][steps];

            if (steps > 0)
            {
                List<Cell<long>> adjacentCells = position.Adjacent;

                if (steps == 1)
                {
                    // we are adjacent to our destination
                    if (endPoints == null)
                    {
                        endPoints = CreateOccurrenceDictionary(adjacentCells);
                        // save this info
                        nStepsFrom[position.Key][steps] = endPoints;
                        CacheingData?.Invoke(this, new CacheingEventArgs { Message = $"Saving end points: from {position.Key} taking 1 step" });
                    }
                }
                else
                {
                    // check if we have already calculated the endpoints
                    if (endPoints==null)
                    {
                        endPoints = CreateOccurrenceDictionary();
                        foreach(Cell<long> cell in adjacentCells)
                        {
                            Dictionary<string, long> partialEndPoints = nStepsFrom[cell.Key][steps - 1];
                            if (partialEndPoints==null)
                            {
                                partialEndPoints = Step(cell, steps - 1);
                            }

                            endPoints = AddOccurrences(endPoints, partialEndPoints);
                        }
                        // save this info
                        nStepsFrom[position.Key][steps] = endPoints;
                        CacheingData?.Invoke(this, new CacheingEventArgs { Message = $"Saving end points: from {position.Key} taking {steps} steps" });
                    }
                }
            }

            return endPoints;
        }

        private void InitNStepsFrom()
        {
            nStepsFrom = new Dictionary<string, List<Dictionary<string, long>>>();
            // for each cell create a list for each number of steps
            Honeycomb.ForEachCell((Cell<long> cell) => nStepsFrom[cell.Key] = CreateDestinationsByStep());
        }

        private List<Dictionary<string,long>> CreateDestinationsByStep()
        {
            var newDBS = new List<Dictionary<string, long>>();
            // initialise each occurrence dictionary as null - meaning we haven't calculated this yet
            for (int i = 0; i <= Steps; i++)
                newDBS.Add(null);

            return newDBS;
        }

        private Dictionary<string,long> CreateOccurrenceDictionary()
        {
            var newOD = new Dictionary<string, long>();
            Honeycomb.ForEachCell((Cell<long> cell) => newOD[cell.Key] = 0);
            return newOD;
        }

        private Dictionary<string, long> CreateOccurrenceDictionary(List<Cell<long>> toCells)
        {
            var newOD = CreateOccurrenceDictionary();
            foreach(Cell<long> cell in toCells)
                newOD[cell.Key] = 1;

            return newOD;
        }

        private Dictionary<string, long> AddOccurrences(Dictionary<string,long> lhs,Dictionary<string,long> rhs)
        {
            var newOD = CreateOccurrenceDictionary();

            foreach (string key in lhs.Keys)
                newOD[key] = lhs[key] + rhs[key];

            return newOD;
        }
    }
}
