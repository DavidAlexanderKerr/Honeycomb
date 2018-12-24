using Honeycomb;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace Tests
{
    [TestClass]
    public class BinaryHoneycombTest
    {
        [TestMethod]
        public void TestNorth()
        {
            Cell<bool> seed = Honeycomb<bool>.Create(true);
            Honeycomb<bool> bhc = seed.Honeycomb;
            Cell<bool> cell = bhc.AddNorth(seed, false);

            Assert.AreEqual(2, bhc.Count);
            Assert.AreEqual(cell, seed.North);
            Assert.AreEqual(seed, cell.South);
        }

        [TestMethod]
        public void TestHex19()
        {
            Cell<bool>[] tops;
            Cell<bool>[] bottoms;
            Honeycomb<bool> hex19 = CreateHex19(out tops,out bottoms);

            int count = 73;
            int top = 0;
            int bottom = -22;
            int left = -11;
            int right = 3;

            Assert.AreEqual(count, hex19.Count);
            Assert.AreEqual(top, hex19.Top);
            Assert.AreEqual(bottom, hex19.Bottom);
            Assert.AreEqual(left, hex19.Left);
            Assert.AreEqual(right, hex19.Right);

            Cell<bool> cell = hex19[-11, -13];
            Assert.AreEqual(cell, bottoms[0]);
            Assert.AreEqual(cell, tops[0]);

            cell = hex19[-10, -12];
            Assert.AreEqual(cell, bottoms[1]);
            cell = hex19[-10, -4];
            Assert.AreEqual(cell, tops[1]);

            cell = hex19[-9, -11];
            Assert.AreEqual(cell, bottoms[2]);
            cell = hex19[-9, -5];
            Assert.AreEqual(cell, tops[2]);

            cell = hex19[-8, -12];
            Assert.AreEqual(cell, bottoms[3]);
            cell = hex19[-8, -6];
            Assert.AreEqual(cell, tops[3]);

            cell = hex19[-7, -13];
            Assert.AreEqual(cell, bottoms[4]);
            cell = hex19[-7, -7];
            Assert.AreEqual(cell, tops[4]);

            cell = hex19[-6, -12];
            Assert.AreEqual(cell, bottoms[5]);
            cell = hex19[-6, -8];
            Assert.AreEqual(cell, tops[5]);

            cell = hex19[-5, -13];
            Assert.AreEqual(cell, bottoms[6]);
            cell = hex19[-5, -9];
            Assert.AreEqual(cell, tops[6]);

            cell = hex19[-4, -14];
            Assert.AreEqual(cell, bottoms[7]);
            cell = hex19[-4, -8];
            Assert.AreEqual(cell, tops[7]);

            cell = hex19[-3, -21];
            Assert.AreEqual(cell, bottoms[8]);
            cell = hex19[-3, -7];
            Assert.AreEqual(cell, tops[8]);

            cell = hex19[-2, -22];
            Assert.AreEqual(cell, bottoms[9]);
            cell = hex19[-2, -6];
            Assert.AreEqual(cell, tops[9]);

            cell = hex19[-1, -21];
            Assert.AreEqual(cell, bottoms[10]);
            cell = hex19[-1, -3];
            Assert.AreEqual(cell, tops[10]);

            cell = hex19[0, -14];
            Assert.AreEqual(cell, bottoms[11]);
            cell = hex19[0, 0];
            Assert.AreEqual(cell, tops[11]);

            cell = hex19[1, -9];
            Assert.AreEqual(cell, bottoms[12]);
            cell = hex19[1, -1];
            Assert.AreEqual(cell, tops[12]);

            cell = hex19[2, -8];
            Assert.AreEqual(cell, bottoms[13]);
            cell = hex19[2, 0];
            Assert.AreEqual(cell, tops[13]);

            cell = hex19[3, -7];
            Assert.AreEqual(cell, bottoms[14]);
            Assert.AreEqual(cell, tops[14]);
        }
        
        private Honeycomb<bool> CreateHex19(out Cell<bool>[] tops, out Cell<bool>[] bottoms)
        {
            Cell<bool> seed = Honeycomb<bool>.Create(true);
            Honeycomb<bool> bhc = seed.Honeycomb;
            Cell<bool> cell;

            // 15 columns - col[0] -> col[14]
            // seed is top of col11 - tops[11]

            tops = new Cell<bool>[15];
            bottoms = new Cell<bool>[15];

            // create tops 1->13 starting from seed - tops[11]
            // tops 12->13 - going east from seed
            tops[11] = seed;
            // top of col 12 is SE of top of col 11
            tops[12] = bhc.AddSouthEast(tops[11], false);
            // top of col 13 is NE of top of col 12
            tops[13] = bhc.AddNorthEast(tops[12], false);

            // fill columns 11->13 - going south each time
            // col 11 - 8 cells, need to create 7 more
            cell = bhc.AddSouth(tops[11], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[11] = cell;
            // col 12 - 5 cells, need to create 4 more
            cell = bhc.AddSouth(tops[12], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[12] = cell;
            // col 13 - 5 cells, need to create 4 more
            cell = bhc.AddSouth(tops[13], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[13] = cell;

            // col 14 is one cell to the NE of the bottom of col 13
            tops[14] = bhc.AddNorthEast(bottoms[13], false);
            bottoms[14] = tops[14];

            // tops 10->1 going west from seed - tops[11]

            // top of col 10 is S then SW of top of col 11
            tops[10] = bhc.AddSouthWest(tops[11].South, false);
            // col 10 - 10 cells, need to add 9 more
            cell = bhc.AddSouth(tops[10], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[10] = cell;

            // top of col 9 is S then SW of top of col 10
            tops[9] = bhc.AddSouthWest(tops[10].South, false);
            // col 9 - 9 cells, need to add 8 more
            cell = bhc.AddSouth(tops[9], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[9] = cell;

            // top of col 8 is SW of top of col 9
            tops[8] = bhc.AddSouthWest(tops[9], false);
            // col 8 - 8 cells, need to add 7 more
            cell = bhc.AddSouth(tops[8], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[8] = cell;

            // top of col 7 is SW of top of col 8
            tops[7] = bhc.AddSouthWest(tops[8], false);
            // col 7 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[7], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[7] = cell;

            // top of col 6 is SW of top of col 7
            tops[6] = bhc.AddSouthWest(tops[7], false);
            // col 6 - 3 cells, need to add 2 more
            cell = bhc.AddSouth(tops[6], false);
            cell = bhc.AddSouth(cell, false);
            bottoms[6] = cell;

            // top of col 5 is NW of top of col 6
            tops[5] = bhc.AddNorthWest(tops[6], false);
            // col 5 - 3 cells, need to add 2 more
            cell = bhc.AddSouth(tops[5], false);
            cell = bhc.AddSouth(cell, false);
            bottoms[5] = cell;

            // top of col 4 is NW of top of col 5
            tops[4] = bhc.AddNorthWest(tops[5], false);
            // col 4 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[4], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[4] = cell;

            // top of col 3 is NW of top of col 4
            tops[3] = bhc.AddNorthWest(tops[4], false);
            // col 3 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[3], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[3] = cell;

            // top of col 2 is NW of top of col 3
            tops[2] = bhc.AddNorthWest(tops[3], false);
            // col 2 - 4 cells, need to add 3 more
            cell = bhc.AddSouth(tops[2], false);
            cell = bhc.AddSouth(cell, false);
            cell = bhc.AddSouth(cell, false);
            bottoms[2] = cell;

            // top of col 1 is NW of top of col 2
            tops[1] = bhc.AddNorthWest(tops[2], false);
            // col 1 - 4 cells, need to add 3 more and mind the gap in the middle
            cell = bhc.AddSouth(tops[1], false);
            // gap!
            // bottom of col 1 is SW of bottom of col 2
            bottoms[1] = bhc.AddSouthWest(bottoms[2], false);
            cell = bhc.AddNorth(bottoms[1], false);

            // col 0 is one cell SW of bottom of col 1
            tops[0] = bhc.AddSouthWest(bottoms[1], false);
            bottoms[0] = tops[0];

            return bhc;
        }

    }
}
