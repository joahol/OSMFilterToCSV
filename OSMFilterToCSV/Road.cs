using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMFilterToCSV
{
    class Road
    {
        public String RoadType{get;set;}
        public List<Coordinate> roadSegments { get; set; }

        public Road() { 
        
        }

        public String getSaveString() {
            String saveString = RoadType;
            foreach (Coordinate c in roadSegments) {
                saveString += ","+c.getSaveString();
            }
            return saveString;
        }

    }
}
