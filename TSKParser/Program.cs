using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSKParser.Generator;
using TSKParser.Parser;
using TSKParser.Test;

namespace TSKParser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testData\\005.-5_03_round_zebra");

                if (!File.Exists(inputFile))
                {
                    Console.WriteLine("File not found ! press any key to exit ...");
                    Console.Read();
                    return;
                }

                TskParser parser = new TskParser(inputFile, (mapFile) =>
                {
                    Console.WriteLine(mapFile.Header.LoadYear);
                    Console.WriteLine(mapFile.DieResults.Count);
                });

                FileStream fileStream = File.Open(inputFile + ".txt", FileMode.Create);
                StreamWriter streamWriter = new StreamWriter(fileStream);

                streamWriter.WriteLine(JsonConvert.SerializeObject(parser.MapFile.Header));
                List<DataPoint> dataPoints = new List<DataPoint>();

                foreach (var dieResult in parser.MapFile.DieResults)
                {
                    int x = dieResult.DieCoordinatorValueX;
                    int y = dieResult.DieCoordinatorValueY;
                    if (dieResult.CodeBitOfCorrdinatorValueY == 0)
                    {
                        y = -y;
                    }
                    if (dieResult.CodeBitOfCorrdinatorValueX == 0)
                    {
                        x = -x;
                    }

                    DataPoint dataPoint = new DataPoint()
                    {
                        X = x,
                        Y = y,
                        //HWBin = bin.HWBin,
                        //SWBin = bin.SWBin
                    };

                    if (dieResult.DummyData == 1)
                    {
                        dataPoint.HWBin = "4";
                        dataPoint.SWBin = "4";
                    }
                    else
                    {
                        dataPoint.HWBin = "1";
                        dataPoint.SWBin = "1";
                    }

                    //dataPoint.HWBin = dieResult.CategoryData.ToString();
                    //dataPoint.SWBin = dieResult.CategoryData.ToString();

                    dataPoints.Add(dataPoint);

                    //streamWriter.WriteLine($"{x},{y}" + JsonConvert.SerializeObject(dieResult));
                }

                streamWriter.Close();
                fileStream.Close();

                //Console.WriteLine("Program finished ...");
                //Console.ReadKey();

                string htmlFilePath = inputFile + ".html";
                string htmlTemplate = "./html/outputTemplate.html";
                using (var reader = new StreamReader(htmlTemplate))
                {
                    using (var writer = new StreamWriter(htmlFilePath))
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                            if (line.StartsWith("        //--tag"))
                            {
                                writer.WriteLine($"var dataPoints = {JsonConvert.SerializeObject(dataPoints)}");
                            }
                        }
                    }
                }

                Console.WriteLine("HTML文件已生成: " + htmlFilePath);
                Process.Start(htmlFilePath); // 打开HTML文件


                Console.WriteLine("Program finished ...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("Program finished ...");
                Console.ReadKey();
            }
        }


        class DataPoint
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string HWBin { get; set; }

            public string SWBin { get; set; }
        }

    }
}
