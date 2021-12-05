using OsmSharp;
using OsmSharp.Streams;
using OsmSharp.Geo;
using System;
using System.IO;
using System.Linq;
using NetTopologySuite.Features;
using System.Collections.Generic;
using System.Text;

namespace OSMFilterToCSV
{
    class Program
    {
        static void Main(string[] args)
        {
         
            if (args[0] == null)
            {
                Console.WriteLine("Command arguments: path filter");
            }
            else {
                using (var fileStream = File.OpenRead(args[0])) {

                   
                    Console.WriteLine("fileStream size(bytes):"+fileStream.Length);
                    Console.WriteLine("Reading File:" + args[0] + " filtering using:" + args[1]);
                    var source = new XmlOsmStreamSource(fileStream);
                    var prog = source.ShowProgress();
              
                    var filteredNodes = (from osm in prog
                                        where osm.Type == OsmSharp.OsmGeoType.Way && osm.Tags!=null && osm.Tags.ContainsKey("highway")
                                        select osm).Distinct();
                    var noTagNodes = from osm in prog
                                     where osm.Type == OsmGeoType.Node && osm.Tags == null
                                     select osm;

                    Console.WriteLine("filtering:highway");
                    var roadNodes = noTagNodes.ToFeatureSource();

                    using StreamWriter sw = new StreamWriter("coords.csv");
                    Console.WriteLine("Traversing nodes");
                    int counter = 0;
                    HashSet<long> investigated = new HashSet<long>();
                    foreach (var n in filteredNodes)
                    {
                        if (!investigated.Contains((long)n.Id))
                        {
                            investigated.Add((long)n.Id);

                            counter++;
                            List<Coordinate> coords = new List<Coordinate>();
                            Console.WriteLine("Traversing node:" + counter + " of " + filteredNodes.Count() + " Node id:" + n.Id);
                            //for hver node knyttet til denne veien, hent alle dens referanse noder 
                            int nodeCount = 0;
                            foreach (var veinode in ((Way)n).Nodes)
                            {
                                nodeCount++;

                                var roadstub = (from i in noTagNodes where i.Id == veinode select i).FirstOrDefault();
                                if (roadstub != null)
                                {

                                    Coordinate c = new Coordinate((Node)roadstub);
                                    coords.Add(c);
                                    Console.WriteLine("Stub(x,y,z):" + nodeCount + " of " + ((Way)n).Nodes.Length);//+c.getSaveString());
                                }

                            }
                            Road r = new Road();
                            r.roadSegments = coords;
                            //hent ut veitypen definert i tag
                            foreach (var t in n.Tags)
                            {
                                Console.WriteLine(t.Key + " " + t.Value);
                                if (t.Key.Equals("highway"))
                                {
                                    r.RoadType = t.Value;
                                }
                            }
                            Console.WriteLine("writeline");
                            sw.WriteLine(r.getSaveString());



                        }
                        else {
                            Console.WriteLine("Node already exist, skipping");
                            counter++;
                        }
                        
                    }
                    Console.WriteLine("finished");
                }     
                    }

        } 
    }

    }

