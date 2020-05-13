using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Game
{
    public class ScoreDataForJson
    {
        public int HighScore { get; set; }
        public ScoreDataForJson(int data)
        {
            HighScore = data;
        }
    }
}
