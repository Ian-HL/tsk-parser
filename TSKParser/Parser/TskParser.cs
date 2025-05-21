using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSKParser.Parser
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class TskParser
    {
        public TskMapFile MapFile { get; private set; }

        public TskParser(string filePath, Action<TskMapFile> callback)
        {
            ReadFile(filePath, callback);
        }

        private void ReadFile(string filePath, Action<TskMapFile> callback)
        {
            byte[] data = File.ReadAllBytes(filePath);
            BinaryReader br = new BinaryReader(data);
            MapFile = new TskMapFile();
            MapFile.Header = LoadMapFileHeader(br);

            switch (MapFile.Header.MapVersion)
            {
                case 0:
                    // Handle case 0
                    break;
                case 1:
                    // Handle case 1
                    break;
                case 2:
                    br.Goto(MapFile.Header.TestDieInformationAddress);
                    int dieCount = MapFile.Header.MapDataAreaRowSize * MapFile.Header.MapDataAreaLineSize;
                    MapFile.DieResults = LoadTestResults(br, dieCount);
                    if (MapFile.Header.MapFileConfiguration.ExtensionHeaderInformation(0) != 0)
                    {
                        // Handle extension header
                    }
                    break;
                case 3:
                    // Handle case 3
                    break;
                case 4:
                    // Handle case 4
                    break;
            }
            callback(MapFile);
        }

        private MapFileHeader LoadMapFileHeader(BinaryReader br)
        {
            MapFileHeader header = new MapFileHeader();
            header.OperatorName = br.ReadAsString(20);
            header.DeviceName = br.ReadAsString(16);
            header.WaferSize = br.ReadAsInt(2);
            header.MachineNo = br.ReadAsInt(2);
            header.IndexSizeX = br.ReadAsInt(4);
            header.IndexSizeY = br.ReadAsInt(4);
            header.StandardOrientationFlatDirection = br.ReadAsInt(2);
            header.FinalEditingMachineType = br.ReadAsInt(1);
            header.MapVersion = br.ReadAsInt(1);
            header.MapDataAreaRowSize = br.ReadAsInt(2);
            header.MapDataAreaLineSize = br.ReadAsInt(2);
            header.MapDataForm = br.ReadAsInt(4);
            header.WaferId = br.ReadAsString(21);
            header.NumberOfProbing = br.ReadAsInt(1);
            header.LotNo = br.ReadAsString(18);
            header.CassetteNo = br.ReadAsInt(2);
            header.SlotNo = br.ReadAsInt(2);
            header.XCoordinatesIncreaseDirection = br.ReadAsInt(1);
            header.YCoordinatesIncreaseDirection = br.ReadAsInt(1);
            header.ReferenceDieSettingProcedures = br.ReadAsInt(1);
            header.Reserved = br.ReadAsInt(1);
            header.TargetDiePositionX = (uint)br.ReadAsInt(4);
            header.TargetDiePositionY = (uint)br.ReadAsInt(4);
            header.ReferenceDieCoordinatorX = br.ReadAsInt(2);
            header.ReferenceDieCoordinatorY = br.ReadAsInt(2);
            header.ProbingStartPosition = br.ReadAsInt(1);
            header.ProbingDirection = br.ReadAsInt(1);
            header.Reserved = br.ReadAsInt(2);
            header.DistanceXtoWaferCenterDieOrigin = (uint)br.ReadAsInt(4);
            header.DistanceYtoWaferCenterDieOrigin = (uint)br.ReadAsInt(4);
            header.CoordinatorXofWaferCenterDie = br.ReadAsInt(4);
            header.CoordinatorYofWaferCenterDie = br.ReadAsInt(4);
            header.FirstDieCoordinatorX = (uint)br.ReadAsInt(4);
            header.FirstDieCoordinatorY = (uint)br.ReadAsInt(4);
            header.StartYear = br.ReadAsString(2);
            header.StartMonth = br.ReadAsString(2);
            header.StartDay = br.ReadAsString(2);
            header.StartHour = br.ReadAsString(2);
            header.StartMinute = br.ReadAsString(2);
            header.Reserved1 = br.ReadAsBytes(2);
            header.EndYear = br.ReadAsString(2);
            header.EndMonth = br.ReadAsString(2);
            header.EndDay = br.ReadAsString(2);
            header.EndHour = br.ReadAsString(2);
            header.EndMinute = br.ReadAsString(2);
            header.Reserved2 = br.ReadAsBytes(2);
            header.LoadYear = br.ReadAsString(2);
            header.LoadMonth = br.ReadAsString(2);
            header.LoadDay = br.ReadAsString(2);
            header.LoadHour = br.ReadAsString(2);
            header.LoadMinute = br.ReadAsString(2);
            header.Reserved3 = br.ReadAsBytes(2);
            header.UnloadYear = br.ReadAsString(2);
            header.UnloadMonth = br.ReadAsString(2);
            header.UnloadDay = br.ReadAsString(2);
            header.UnloadHour = br.ReadAsString(2);
            header.UnloadMinute = br.ReadAsString(2);
            header.Reserved4 = br.ReadAsBytes(2);
            header.VegaMachineNo = br.ReadAsString(4);
            header.VegaMachineNo2 = br.ReadAsString(4);
            header.SpecialCharacters = br.ReadAsString(4);
            header.TestingEndInformation = br.ReadAsInt(1);
            header.Reserved5 = br.ReadAsBytes(1);
            header.TotalTestedDice = br.ReadAsInt(2);
            header.TotalPassDice = br.ReadAsInt(2);
            header.TotalFailDice = br.ReadAsInt(2);
            header.TestDieInformationAddress = br.ReadAsInt(4);
            header.NumberOfLineCategoryData = br.ReadAsInt(4);
            header.LineCategoryAddress = br.ReadAsInt(4);
            header.MapFileConfiguration = new MapFileConfiguration(br.ReadAsBit(2));
            header.MaxMultiSite = br.ReadAsInt(2);
            header.MaxCategories = br.ReadAsInt(2);
            header.Reserved6 = br.ReadAsBytes(2);
            return header;
        }

        private List<TestResultPerDie> LoadTestResults(BinaryReader br, int dieCount)
        {
            List<TestResultPerDie> results = new List<TestResultPerDie>();
            for (int i = 0; i < dieCount; i++)
            {
                TestResultPerDie die = new TestResultPerDie();
                int word = br.ReadAsInt(2);
                die.DieTestResult = word >> 14;
                die.Marking = (word >> 13) & 1;
                die.FailMarkInspection = (word >> 12) & 1;
                die.ReProbingResult = (word >> 10) & 3;
                die.NeedleMarkInspectionResult = (word >> 9) & 1;
                die.DieCoordinatorValueX = word & 511;
                word = br.ReadAsInt(2);
                die.DieProperty = word >> 14;
                die.NeedleMarkInspectionExecutionDieSelection = (word >> 13) & 1;
                die.SamplingDie = (word >> 12) & 1;
                die.CodeBitOfCorrdinatorValueX = (word >> 11) & 1;
                die.CodeBitOfCorrdinatorValueY = (word >> 10) & 1;
                die.DummyData = (word >> 9) & 1;
                die.DieCoordinatorValueY = word & 511;
                word = br.ReadAsInt(2);
                die.MeasurementFinish = (word >> 15) & 1;
                die.RejectChipFlag = (word >> 14) & 1;
                die.TestExecutionSite = (word >> 8) & 63;
                die.BlockAreaJudgmentFunction = (word >> 6) & 3;
                die.CategoryData = word & 63;
                results.Add(die);
            }
            return results;
        }
    }
}
