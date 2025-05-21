using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace TSKParser.Parser
{
    public class MapFileConfiguration
    {
        private string value;
        private List<int> bitSet;

        public MapFileConfiguration(string value1)
        {
            this.value = value1;
            //this.bitSet = this.value.toString(2).split('').reverse();
            this.bitSet = new List<int>(value.ToCharArray().Select(c => (int)char.GetNumericValue(c)).Reverse());
        }

        public string GetValue()
        {
            return value;
        }

        public int GetRawData()
        {
            bitSet.Reverse();
            return Convert.ToInt32(string.Join("", bitSet), 2);
        }

        public int HeaderInformation(int? bitValue = null)
        {
            if (bitValue.HasValue)
            {
                bitSet[0] = bitValue.Value;
            }
            return bitSet[0];
        }

        public int TestResultInformationPerDie(int? bitValue = null)
        {
            if (bitValue.HasValue)
            {
                bitSet[1] = bitValue.Value;
            }
            return bitSet[1];
        }

        public int LineCategoryInformation(int? bitValue = null)
        {
            if (bitValue.HasValue)
            {
                bitSet[2] = bitValue.Value;
            }
            return bitSet[2];
        }

        public int ExtensionHeaderInformation(int? bitValue = null)
        {
            if (bitValue.HasValue)
            {
                bitSet[3] = bitValue.Value;
            }
            return bitSet[3];
        }

        public int TestResultInformationPerExtensionDie(int? bitValue = null)
        {
            if (bitValue.HasValue)
            {
                bitSet[4] = bitValue.Value;
            }
            return bitSet[4];
        }

        public int ExtensionLineCategoryInformation(int? bitValue = null)
        {
            if (bitValue.HasValue)
            {
                bitSet[5] = bitValue.Value;
            }
            return bitSet[5];
        }
    }

    public class MapFileHeader
    {
        public string OperatorName { get; set; }
        public string DeviceName { get; set; }
        /// <summary>
        /// 40, 45, 50, 60, 80 (Unit: 0.1 inch) 
        /// 100, 115, 125, 150, 200 (Unit: mm) 
        /// </summary>
        public int WaferSize { get; set; }
        public int MachineNo { get; set; }
        /// <summary>
        /// (Unit: 0.01um) 
        /// 示例：50000
        /// </summary>
        public int IndexSizeX { get; set; }
        /// <summary>
        /// (Unit: 0.01um) 
        /// 示例：50000
        /// </summary>
        public int IndexSizeY { get; set; }
        /// <summary>
        /// 0 to 359 (Unit: degree°) 
        /// 示例：180
        /// </summary>
        public int StandardOrientationFlatDirection { get; set; }
        public int FinalEditingMachineType { get; set; }
        /// <summary>
        /// 0: Normal
        /// 1: 250,000 Chips
        /// 2: 256 Multi-sites (目前只支持该项解析)
        /// 3: 256 Multi-sites(without extended header information)
        /// 4: 1024 category
        /// </summary>
        public int MapVersion { get; set; }
        /// <summary>
        /// 列die数量
        /// 示例：400
        /// </summary>
        public int MapDataAreaRowSize { get; set; }
        /// <summary>
        /// 行die数量
        /// 示例：400
        /// </summary>
        public int MapDataAreaLineSize { get; set; }
        public int MapDataForm { get; set; }
        public string WaferId { get; set; }
        public int NumberOfProbing { get; set; }
        public string LotNo { get; set; }
        public int CassetteNo { get; set; }
        public int SlotNo { get; set; }
        /// <summary>
        /// 1: leftward 2: rightward 
        /// </summary>
        public int XCoordinatesIncreaseDirection { get; set; }
        /// <summary>
        /// 1: forward 2: backward 
        /// </summary>
        public int YCoordinatesIncreaseDirection { get; set; }
        /// <summary>
        /// 1: Wafer center die 3: Target sense die 2: Teaching die
        /// </summary>
        public int ReferenceDieSettingProcedures { get; set; }
        public int Reserved { get; set; }
        /// <summary>
        /// Reference die position X from wafer center
        ///        (Unit: 0.01um)
        ///* Only when target sense die is selected for ”Standard die setting procedure”
        /// </summary>
        public uint TargetDiePositionX { get; set; }
        /// <summary>
        /// Reference die position Y from wafer center
        ///        (Unit: 0.01um)
        ///* Only when target sense die is selected for ”Standard die setting procedure”
        /// </summary>
        public uint TargetDiePositionY { get; set; }
        public int ReferenceDieCoordinatorX { get; set; }
        public int ReferenceDieCoordinatorY { get; set; }
        public int ProbingStartPosition { get; set; }
        public int ProbingDirection { get; set; }
        public uint DistanceXtoWaferCenterDieOrigin { get; set; }
        public uint DistanceYtoWaferCenterDieOrigin { get; set; }
        public int CoordinatorXofWaferCenterDie { get; set; }
        public int CoordinatorYofWaferCenterDie { get; set; }
        public uint FirstDieCoordinatorX { get; set; }
        public uint FirstDieCoordinatorY { get; set; }
        public string StartYear { get; set; }
        public string StartMonth { get; set; }
        public string StartDay { get; set; }
        public string StartHour { get; set; }
        public string StartMinute { get; set; }
        public byte[] Reserved1 { get; set; }
        public string EndYear { get; set; }
        public string EndMonth { get; set; }
        public string EndDay { get; set; }
        public string EndHour { get; set; }
        public string EndMinute { get; set; }
        public byte[] Reserved2 { get; set; }
        public string LoadYear { get; set; }
        public string LoadMonth { get; set; }
        public string LoadDay { get; set; }
        public string LoadHour { get; set; }
        public string LoadMinute { get; set; }
        public byte[] Reserved3 { get; set; }
        public string UnloadYear { get; set; }
        public string UnloadMonth { get; set; }
        public string UnloadDay { get; set; }
        public string UnloadHour { get; set; }
        public string UnloadMinute { get; set; }
        public byte[] Reserved4 { get; set; }
        public string VegaMachineNo { get; set; }
        public string VegaMachineNo2 { get; set; }
        public string SpecialCharacters { get; set; }
        public int TestingEndInformation { get; set; }
        public byte[] Reserved5 { get; set; }
        /// <summary>
        /// 总测试数量 = TotalPassDice + TotalFailDice
        /// </summary>
        public int TotalTestedDice { get; set; }
        /// <summary>
        /// 总通过数量
        /// </summary>
        public int TotalPassDice { get; set; }
        /// <summary>
        /// 总失败数量
        /// </summary>
        public int TotalFailDice { get; set; }
        public int TestDieInformationAddress { get; set; }
        public int NumberOfLineCategoryData { get; set; }
        public int LineCategoryAddress { get; set; }
        public MapFileConfiguration MapFileConfiguration { get; set; }
        public int MaxMultiSite { get; set; }
        public int MaxCategories { get; set; }
        public byte[] Reserved6 { get; set; }
    }

    public class TestResultPerDie
    {
        /// <summary>
        /// 0: Not Tested 
        /// 1: Pass Die 
        /// 2: Fail 1 Die  
        /// 3: Fail 2 Die
        /// </summary>
        public int DieTestResult { get; set; }
        public int Marking { get; set; }
        public int FailMarkInspection { get; set; }
        public int ReProbingResult { get; set; }
        public int NeedleMarkInspectionResult { get; set; }
        public int DieCoordinatorValueX { get; set; }
        /// <summary>
        /// 0: Skip Die 
        /// 1: Probing Die 
        /// 2: Compulsory Marking Die
        /// </summary>
        public int DieProperty { get; set; }
        public int NeedleMarkInspectionExecutionDieSelection { get; set; }
        public int SamplingDie { get; set; }
        /// <summary>
        /// 0:DieCoordinatorValueX  为负
        /// 1:DieCoordinatorValueX  为正
        /// </summary>
        public int CodeBitOfCorrdinatorValueX { get; set; }
        /// <summary>
        /// 0:DieCoordinatorValueY  为负
        /// 1:DieCoordinatorValueY  为正
        /// </summary>
        public int CodeBitOfCorrdinatorValueY { get; set; }
        /// <summary>
        /// except wafer
        /// 0-否 1-忽略掉数据
        /// </summary>
        public int DummyData { get; set; }
        public int DieCoordinatorValueY { get; set; }
        public int MeasurementFinish { get; set; }
        public int RejectChipFlag { get; set; }
        public int TestExecutionSite { get; set; }
        public int BlockAreaJudgmentFunction { get; set; }
        public int CategoryData { get; set; }
    }

    public class ExtensionHeaderInformation
    {
        public int TestedDice { get; set; }
        public int TestedPassDice { get; set; }
        public int TestedFailDice { get; set; }

        public ExtensionHeaderInformation(BinaryReader br)
        {
            br.Skip(52);
            TestedDice = br.ReadAsInt(4);
            TestedPassDice = br.ReadAsInt(4);
            TestedFailDice = br.ReadAsInt(4);
        }
    }

    public class TskMapFile
    {
        public MapFileHeader Header { get; set; }
        public List<TestResultPerDie> DieResults { get; set; }
    }
}
