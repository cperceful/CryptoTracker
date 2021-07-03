using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CryptoTracker.Modules
{
    class Holdings
    {
        public List<Position> Positions { get; set; }

        public Holdings()
        {
            Positions = new List<Position>();
        }
    }
}
