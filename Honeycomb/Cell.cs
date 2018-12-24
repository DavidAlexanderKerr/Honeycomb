using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb
{
    public class Cell<T>
    {
        public Honeycomb<T> Honeycomb { get; private set; }
        
        public T Data { get;  set; } 

        public int Column { get; set; }

        public int Row { get; set; }
        
        public Cell<T> North {
            get {
                int column = Column;
                int row = Row + 2;
                return Honeycomb[column,row]; }
        }
        
        public Cell<T> South
        {
            get {
                int column = Column;
                int row = Row - 2;
                return Honeycomb[column,row]; }
        }
        
        public Cell<T> NorthEast
        {
            get
            {
                int column = Column + 1;
                int row = Row + 1;
                return Honeycomb[column, row];
            }
        }
        
        public Cell<T> NorthWest
        {
            get
            {
                int column = Column - 1;
                int row = Row + 1;
                return Honeycomb[column, row];
            }
        }
        
        public Cell<T> SouthEast
        {
            get
            {
                int column = Column + 1;
                int row = Row - 1;
                return Honeycomb[column, row];
            }
        }
        
        public Cell<T> SouthWest
        {
            get
            {
                int column = Column - 1;
                int row = Row - 1;
                return Honeycomb[column, row];
            }
        }

        public List<Cell<T>> Adjacent
        {
            get
            {
                var cells = new List<Cell<T>>();

                Cell<T> cell = North;
                if (cell != null) cells.Add(cell);

                cell = South;
                if (cell != null) cells.Add(cell);

                cell = NorthWest;
                if (cell != null) cells.Add(cell);

                cell = NorthEast;
                if (cell != null) cells.Add(cell);

                cell = SouthWest;
                if (cell != null) cells.Add(cell);

                cell = SouthEast;
                if (cell != null) cells.Add(cell);

                return cells;
            }
        }

        public Cell(T data, Honeycomb<T> honeycomb, int column, int row)
        {
            Data = data;
            Honeycomb = honeycomb;
            Column = column;
            Row = row;
        }
    }
}
