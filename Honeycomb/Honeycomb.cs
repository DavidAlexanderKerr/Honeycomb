using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeycomb
{
    public class Honeycomb<T>
    { 
        private readonly List<Cell<T>> cells;
        private readonly Dictionary<string, Cell<T>> cellIndex;

        public int Count { get
            {
                return cells.Count();
            } }

        public Cell<T> this[int column,int row] { get
            {
                string key = Cell<T>.CellKey(column, row);

                return this[key];
            }
        }

        public Cell<T> this[string key]
        {
            get
            {
                Cell<T> cell = null;
                try
                {
                    cell = cellIndex[key];
                }
                catch
                {
                    // ignore
                }

                return cell;
            }
        }

        public void ForEachCell(Action<Cell<T>> action)
        {
            foreach (Cell<T> cell in cells)
                action(cell);
        }

        public int Top { get
            {
                return cells.Max(c => c.Row);
            } }

        public int Bottom { get
            {
                return cells.Min(c => c.Row);
            } }

        public int Left { get
            {
                return cells.Min(c => c.Column);
            } }

        public int Right { get
            {
                return cells.Max(c => c.Column);
            } }

        private Honeycomb()
        {
            cells = new List<Cell<T>>();
            cellIndex = new Dictionary<string, Cell<T>>();
        }

        public static Cell<T> Create(T data)
        {
            var newHoneycomb = new Honeycomb<T>();
            var newCell = new Cell<T>(data, newHoneycomb,0,0);
            newHoneycomb.AddCell(newCell);

            return newCell;
        }

        public Cell<T> AddNorth(Cell<T> cell, T data)
        {
            if (cell.Honeycomb != this)
                throw new ArgumentException("cell not in this honeycomb");

            var newCell = new Cell<T>(data, this, cell.Column,cell.Row+2);

            AddCell(newCell);

            return newCell;
        }

        public Cell<T> AddSouth(Cell<T> cell, T data)
        {
            if (cell.Honeycomb != this)
                throw new ArgumentException("cell not in this honeycomb");

            var newCell = new Cell<T>(data, this, cell.Column, cell.Row - 2);

            AddCell(newCell);

            return newCell;
        }

        public Cell<T> AddNorthEast(Cell<T> cell, T data)
        {
            if (cell.Honeycomb != this)
                throw new ArgumentException("cell not in this honeycomb");

            var newCell = new Cell<T>(data, this, cell.Column+1, cell.Row + 1);

            AddCell(newCell);

            return newCell;
        }

        public Cell<T> AddNorthWest(Cell<T> cell, T data)
        {
            if (cell.Honeycomb != this)
                throw new ArgumentException("cell not in this honeycomb");

            var newCell = new Cell<T>(data, this, cell.Column-1, cell.Row +1);

            AddCell(newCell);

            return newCell;
        }

        public Cell<T> AddSouthEast(Cell<T> cell, T data)
        {
            if (cell.Honeycomb != this)
                throw new ArgumentException("cell not in this honeycomb");

            var newCell = new Cell<T>(data, this, cell.Column+1, cell.Row - 1);

            AddCell(newCell);

            return newCell;
        }

        public Cell<T> AddSouthWest(Cell<T> cell, T data)
        {
            if (cell.Honeycomb!=this)
                throw new ArgumentException("cell not in this honeycomb");

            var newCell = new Cell<T>(data, this, cell.Column-1, cell.Row -1);

            AddCell(newCell);

            return newCell;
        }

        public void Save(string filepath)
        {
            StringBuilder output = new StringBuilder();

            int maxWidth=cells.Max(c => c.Data.ToString().Length);

            for (int row = Top; row >= Bottom; row--)
            {
                for (int col = Left; col <= Right; col++)
                {
                    string dataOutput = "-";
                    Cell<T> cell = this[col, row];
                    if (cell != null)
                        dataOutput = cell.Data.ToString();
                    else
                        dataOutput = new string('-', maxWidth);

                    output.Append(dataOutput);
                    string padding = new string(' ', maxWidth - dataOutput.Length + 1);
                    output.Append(padding);
                }
                output.AppendLine();
            }

            using (var wrtr = new StreamWriter(filepath))
            {
                wrtr.Write(output.ToString());
            }
        }

        private void AddCell(Cell<T> cell)
        {
            cells.Add(cell);
            cellIndex[cell.Key] = cell;
        }
    }
}
