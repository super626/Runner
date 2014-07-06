using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Control
{
    class LevelCellState
    {
        public bool selected = false;
    }
    class LevelControl
    {
        public GameLevel m_Level = new GameLevel();
        public GameLevel Level { get { return m_Level; } }

        public List<List<LevelCellState>> m_States = new List<List<LevelCellState>>();

        public void UpdateStates()
        {
            if (m_States.Count != m_Level.GetHeight())
            {
                if (m_States.Count < m_Level.GetHeight())
                {
                    for (int i = m_States.Count; i < m_Level.GetHeight(); i++)
                    {
                        List<LevelCellState> line = new List<LevelCellState>();
                        for (int j = 0; j < m_Level.GetWidth(); j++)
                        {
                            line.Add(new LevelCellState());
                        }
                        m_States.Add(line);
                    }
                }
                else
                {
                    m_States.RemoveRange(m_Level.GetHeight(), m_States.Count - m_Level.GetHeight());
                }

            }
        }

        public LevelCellState GetState(int x, int y)
        {
            UpdateStates();
            return m_States[y][x];
        }

        public void Load(string fileName)
        {
            m_Level.LoadFromFile(fileName);
        }

        public char GetCell(int x, int y)
        {
            return m_Level.GetCell(x + GameLevel.MAP_MINX, y);
        }

        public void SetCell(int x, int y, char cell)
        {
            m_Level.SetCell(x + GameLevel.MAP_MINX, y, cell);
        }
             
    }
}
