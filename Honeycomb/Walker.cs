using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb
{
    public class Walker
    {
        public Honeycomb<long> Honeycomb { get; private set; }

        public Walker(Honeycomb<long> honeycomb)
        {
            Honeycomb = honeycomb;
        }

        public void Walk(int steps)
        {
            Cell<long> startPoint = Honeycomb[0, 0];
            Cell<long> position = startPoint;
            Step(position, steps);
        }

        private void Step(Cell<long> position, int steps)
        {
            if (steps==0)
            {
                // we have arrived at our destination
                position.Data= position.Data+1;
            }
            else
            {
                List<Cell<long>> adjacentCells = position.Adjacent;
                foreach(Cell<long> nextPosition in adjacentCells)
                {
                    Step(nextPosition, steps - 1);
                }
            }
        }
    }
}
