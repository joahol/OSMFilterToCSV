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
                        
                    var progress = source.ShowProgress();
              
                    var filteredNodes = from osmGeo in progress
                                        where osmGeo.Type == OsmSharp.OsmGeoType.Way && osmGeo.Tags!=null && osmGeo.Tags.ContainsKey("highway")
                                        select osmGeo;
                    var noTagNodes = from osmGeo in progress
                                     where osmGeo.Type == OsmGeoType.Node && osmGeo.Tags == null
                                     select osmGeo;


                 Console.WriteLine("filter");
                    var roadNodes = noTagNodes.ToFeatureSource();


             
                    FileStream f = File.Create("c:\\coords.csv");
                    StringBuilder sb = new StringBuilder();
                    Console.WriteLine("Let's look at the nodes");
                    foreach (var n in filteredNodes) {
                        List<Coordinate> coords = new List<Coordinate>();
                        Console.WriteLine(n.Id);
                        //for hver node knyttet til denne veien, hent alle dens referanse noder 
                        foreach (var veinode in ((Way)n).Nodes)
                        {
                            
                            var roadstub = (from i in source where i.Id == veinode select i).FirstOrDefault();
                            if (roadstub != null) {

                                Coordinate c = new Coordinate((Node)roadstub);
                                coords.Add(c);
                                Console.WriteLine("RoadStub:" +c.getSaveString());
                            }
                      
                        }
                        //hent ut veitypen definert i tag
                        foreach (var t in n.Tags) {
                            Console.WriteLine(t.Key + " " + t.Value);
                          
                        }
                      



                    }

                    Console.WriteLine("finished");
                }     
                    }

        } 
    }

    }

