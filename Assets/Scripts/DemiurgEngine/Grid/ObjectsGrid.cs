using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DemiurgEngine
{
    public class ObjectsGrid
    {
        public Dictionary<Vector2Int, Cell> cells = new();

        public Cell this[Vector2Int pos]
        {
            get => cells[pos];
            set 
            {
                SetCell(value, pos);
            }
        
        }
        public void SetCell(Cell cell, Vector2Int pos)
        {
            if (cells.ContainsKey(pos))
            {
                cells[pos] = cell;
            }
            else
            {
                cells.Add(pos, cell);
            }
        }
    }
}