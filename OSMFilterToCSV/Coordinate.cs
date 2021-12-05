using OsmSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSMFilterToCSV
{
    class Coordinate
    {
        private long X { get; set; }
        private int Radius = 6371;

        public long Y { get; set; }
        public long Z { get; set; }
        public Node Roadstub { get; }

        public Coordinate(long Latitude, long Longitude) {
            X = (long)(Radius * Math.Cos(Latitude) * Math.Cos(Longitude));
            Y = (long)(Radius * Math.Cos(Latitude) * Math.Sin(Longitude));
            Z = (long)(Radius * Math.Sin(Latitude));
        }

        public Coordinate(Node roadstub)
        {
            Roadstub = roadstub;
            X = (long)(Radius * Math.Cos(roadstub.Latitude.Value) * Math.Cos(roadstub.Longitude.Value));
            Y = (long)(Radius * Math.Cos(roadstub.Latitude.Value) * Math.Sin(roadstub.Longitude.Value));
            Z = (long)(Radius * Math.Sin(roadstub.Latitude.Value));
        }
        public String getSaveString() {
            return X + "," + Y + "," + Z;
        }
    }
}
