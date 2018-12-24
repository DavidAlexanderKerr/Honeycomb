using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Honeycomb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class WalkerTest
    {
        [TestMethod]
        public void TestWalk()
        {
            Honeycomb<long> honeycomb = CreateHex19();
            var walker = new Walker(honeycomb);
            int steps = 4;

            walker.Walk(steps);

            SaveResults(walker.Honeycomb);
        }

        private void SaveResults(Honeycomb<long> honeycomb)
        {
            var now = DateTime.Now;
            string filename = $"Hex19Results_{now:yyyyMMdd}_{now:HHmmss}.txt";
            string folder = @"C:\Users\lotop_000\Documents\Quizzes";
            string filepath = Path.Combine(folder, filename);
            honeycomb.Save(filepath);
        }

        private Honeycomb<long> CreateHex19()
        {
            Cell<long> seed = Honeycomb<long>.Create(0);
            Honeycomb<long> bhc = seed.Honeycomb;
            Cell<long> cell;

            // 15 columns - col[0] -> col[14]
            // seed is top of col11 - tops[11]

            var tops = new Cell<long>[15];
            var bottoms = new Cell<long>[15];

            // create tops 1->13 starting from seed - tops[11]
            // tops 12->13 - going east from seed
            tops[11] = seed;
            // top of col 12 is SE of top of col 11
            tops[12] = bhc.AddSouthEast(tops[11], 0);
            // top of col 13 is NE of top of col 12
            tops[13] = bhc.AddNorthEast(tops[12], 0);

            // fill columns 11->13 - going south each time
            // col 11 - 8 cells, need to create 7 more
            cell = bhc.AddSouth(tops[11], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[11] = cell;
            // col 12 - 5 cells, need to create 4 more
            cell = bhc.AddSouth(tops[12], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[12] = cell;
            // col 13 - 5 cells, need to create 4 more
            cell = bhc.AddSouth(tops[13], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[13] = cell;

            // col 14 is one cell to the NE of the bottom of col 13
            tops[14] = bhc.AddNorthEast(bottoms[13], 0);
            bottoms[14] = tops[14];

            // tops 10->1 going west from seed - tops[11]

            // top of col 10 is S then SW of top of col 11
            tops[10] = bhc.AddSouthWest(tops[11].South, 0);
            // col 10 - 10 cells, need to add 9 more
            cell = bhc.AddSouth(tops[10], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[10] = cell;

            // top of col 9 is S then SW of top of col 10
            tops[9] = bhc.AddSouthWest(tops[10].South, 0);
            // col 9 - 9 cells, need to add 8 more
            cell = bhc.AddSouth(tops[9], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[9] = cell;

            // top of col 8 is SW of top of col 9
            tops[8] = bhc.AddSouthWest(tops[9], 0);
            // col 8 - 8 cells, need to add 7 more
            cell = bhc.AddSouth(tops[8], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[8] = cell;

            // top of col 7 is SW of top of col 8
            tops[7] = bhc.AddSouthWest(tops[8], 0);
            // col 7 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[7], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[7] = cell;

            // top of col 6 is SW of top of col 7
            tops[6] = bhc.AddSouthWest(tops[7], 0);
            // col 6 - 3 cells, need to add 2 more
            cell = bhc.AddSouth(tops[6], 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[6] = cell;

            // top of col 5 is NW of top of col 6
            tops[5] = bhc.AddNorthWest(tops[6], 0);
            // col 5 - 3 cells, need to add 2 more
            cell = bhc.AddSouth(tops[5], 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[5] = cell;

            // top of col 4 is NW of top of col 5
            tops[4] = bhc.AddNorthWest(tops[5], 0);
            // col 4 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[4], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[4] = cell;

            // top of col 3 is NW of top of col 4
            tops[3] = bhc.AddNorthWest(tops[4], 0);
            // col 3 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[3], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[3] = cell;

            // top of col 2 is NW of top of col 3
            tops[2] = bhc.AddNorthWest(tops[3], 0);
            // col 2 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[2], 0);
            cell = bhc.AddSouth(cell, 0);
            cell = bhc.AddSouth(cell, 0);
            bottoms[2] = cell;

            // top of col 1 is NW of top of col 2
            tops[1] = bhc.AddNorthWest(tops[2], 0);
            // col 1 - 4 cells, need to add 3 more and mind the gap in the middle
            cell = bhc.AddSouth(tops[1], 0);
            // gap!
            // bottom of col 1 is SW of bottom of col 2
            bottoms[1] = bhc.AddSouthWest(bottoms[2], 0);
            cell = bhc.AddNorth(bottoms[1], 0);

            // col 0 is one cell SW of bottom of col 1
            tops[0] = bhc.AddSouthWest(bottoms[1], 0);
            bottoms[0] = tops[0];

            return bhc;
        }
    }
}
