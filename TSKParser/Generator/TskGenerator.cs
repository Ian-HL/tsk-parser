using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSKParser.Parser;

namespace TSKParser.Generator
{
    internal class TskGenerator
    {

        TskBinaryWriter _bw;
        internal void Convert(TskMapFile tskMapFile, string filePath)
        {
            FileStream fs = File.Open(filePath, FileMode.Create);
            _bw = new TskBinaryWriter(fs);

            WriteMapFileHeader(tskMapFile.Header);
            ConvertTestResultsToBinary(tskMapFile.DieResults);

            //添加 00 的点位符，满足后续数据填充要求
            foreach (var dieContent in tskMapFile.DieResults)
            {
                _bw.WriteAsBytes(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }

            _bw.Close();
            fs.Close();
        }

        private void WriteMapFileHeader(MapFileHeader header)
        {
            _bw.WriteAsString(header.OperatorName, 20);
            _bw.WriteAsString(header.DeviceName, 16);
            _bw.WriteAsInt(header.WaferSize, 2);
            _bw.WriteAsInt(header.MachineNo, 2);
            _bw.WriteAsInt(header.IndexSizeX, 4);
            _bw.WriteAsInt(header.IndexSizeY, 4);
            _bw.WriteAsInt(header.StandardOrientationFlatDirection, 2);
            _bw.WriteAsInt(header.FinalEditingMachineType, 1);
            _bw.WriteAsInt(header.MapVersion, 1);
            _bw.WriteAsInt(header.MapDataAreaRowSize, 2);
            _bw.WriteAsInt(header.MapDataAreaLineSize, 2);
            _bw.WriteAsInt(header.MapDataForm, 4);
            _bw.WriteAsString(header.WaferId, 21);
            _bw.WriteAsInt(header.NumberOfProbing, 1);
            _bw.WriteAsString(header.LotNo, 18);
            _bw.WriteAsInt(header.CassetteNo, 2);
            _bw.WriteAsInt(header.SlotNo, 2);
            _bw.WriteAsInt(header.XCoordinatesIncreaseDirection, 1);
            _bw.WriteAsInt(header.YCoordinatesIncreaseDirection, 1);
            _bw.WriteAsInt(header.ReferenceDieSettingProcedures, 1);
            _bw.WriteAsInt(header.Reserved, 1);
            _bw.WriteAsInt((int)header.TargetDiePositionX, 4);
            _bw.WriteAsInt((int)header.TargetDiePositionY, 4);
            _bw.WriteAsInt(header.ReferenceDieCoordinatorX, 2);
            _bw.WriteAsInt(header.ReferenceDieCoordinatorY, 2);
            _bw.WriteAsInt(header.ProbingStartPosition, 1);
            _bw.WriteAsInt(header.ProbingDirection, 1);
            _bw.WriteAsInt(header.Reserved, 2);
            _bw.WriteAsInt((int)header.DistanceXtoWaferCenterDieOrigin, 4);
            _bw.WriteAsInt((int)header.DistanceYtoWaferCenterDieOrigin, 4);
            _bw.WriteAsInt(header.CoordinatorXofWaferCenterDie, 4);
            _bw.WriteAsInt(header.CoordinatorYofWaferCenterDie, 4);
            _bw.WriteAsInt((int)header.FirstDieCoordinatorX, 4);
            _bw.WriteAsInt((int)header.FirstDieCoordinatorY, 4);
            _bw.WriteAsString(header.StartYear, 2);
            _bw.WriteAsString(header.StartMonth, 2);
            _bw.WriteAsString(header.StartDay, 2);
            _bw.WriteAsString(header.StartHour, 2);
            _bw.WriteAsString(header.StartMinute, 2);
            _bw.WriteAsBytes(header.Reserved1);
            _bw.WriteAsString(header.EndYear, 2);
            _bw.WriteAsString(header.EndMonth, 2);
            _bw.WriteAsString(header.EndDay, 2);
            _bw.WriteAsString(header.EndHour, 2);
            _bw.WriteAsString(header.EndMinute, 2);
            _bw.WriteAsBytes(header.Reserved2);
            _bw.WriteAsString(header.LoadYear, 2);
            _bw.WriteAsString(header.LoadMonth, 2);
            _bw.WriteAsString(header.LoadDay, 2);
            _bw.WriteAsString(header.LoadHour, 2);
            _bw.WriteAsString(header.LoadMinute, 2);
            _bw.WriteAsBytes(header.Reserved3);
            _bw.WriteAsString(header.UnloadYear, 2);
            _bw.WriteAsString(header.UnloadMonth, 2);
            _bw.WriteAsString(header.UnloadDay, 2);
            _bw.WriteAsString(header.UnloadHour, 2);
            _bw.WriteAsString(header.UnloadMinute, 2);
            _bw.WriteAsBytes(header.Reserved4);
            _bw.WriteAsString(header.VegaMachineNo, 4);
            _bw.WriteAsString(header.VegaMachineNo2, 4);
            _bw.WriteAsString(header.SpecialCharacters, 4);
            _bw.WriteAsInt(header.TestingEndInformation, 1);
            _bw.WriteAsBytes(header.Reserved5);
            _bw.WriteAsInt(header.TotalTestedDice, 2);
            _bw.WriteAsInt(header.TotalPassDice, 2);
            _bw.WriteAsInt(header.TotalFailDice, 2);
            _bw.WriteAsInt(header.TestDieInformationAddress, 4);

            _bw.OpenLog = true;
            _bw.WriteAsInt(header.NumberOfLineCategoryData, 4);
            _bw.WriteAsInt(header.LineCategoryAddress, 4);

            // Write MapFileConfiguration if necessary
            // To do..
            // 这里按解析的方式，应该使用int，但是嫌麻烦。所以直接用String了
            _bw.WriteAsBytes(TskBinaryWriter.BinaryStringToBytes(header.MapFileConfiguration.GetValue()));

            _bw.WriteAsInt(header.MaxMultiSite, 2);
            _bw.WriteAsInt(header.MaxCategories, 2);
            _bw.WriteAsBytes(header.Reserved6);

            _bw.OpenLog = false;
        }

        // ... existing code ...
        private void ConvertTestResultsToBinary(List<TestResultPerDie> results)
        {
            foreach (var die in results)
            {
                int word = (die.DieTestResult << 14) |
                           (die.Marking << 13) |
                           (die.FailMarkInspection << 12) |
                           (die.ReProbingResult << 10) |
                           (die.NeedleMarkInspectionResult << 9) |
                           die.DieCoordinatorValueX;
                _bw.WriteAsInt((short)word, 2);

                word = (die.DieProperty << 14) |
                       (die.NeedleMarkInspectionExecutionDieSelection << 13) |
                       (die.SamplingDie << 12) |
                       (die.CodeBitOfCorrdinatorValueX << 11) |
                       (die.CodeBitOfCorrdinatorValueY << 10) |
                       (die.DummyData << 9) |
                       die.DieCoordinatorValueY;
                _bw.WriteAsInt((short)word, 2);

                word = (die.MeasurementFinish << 15) |
                       (die.RejectChipFlag << 14) |
                       (die.TestExecutionSite << 8) |
                       (die.BlockAreaJudgmentFunction << 6) |
                       die.CategoryData;
                _bw.WriteAsInt((short)word, 2);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tskMapFile">map头文件信息</param>
        /// <param name="outPutFilePath">保存的文件</param>
        /// <param name="dieMapFilePath">die map 的文件</param>
        /// <param name="lineCount">从第几行开始保存的。下标从1开始（自然阅读）</param>
        internal void ConvertFromDieMapTxt(TskMapFile tskMapFile, string outPutFilePath, string dieMapFilePath)
        {
            FileStream fs = File.Open(outPutFilePath, FileMode.Create);
            _bw = new TskBinaryWriter(fs);

            WriteMapFileHeader(tskMapFile.Header);

            int totalDieCount = 400;
            bool isCompareDieCount = false;
            //有些die文件，可能不需要匹配数量
            if (isCompareDieCount && tskMapFile.DieResults.Count != totalDieCount)
            {
                throw new Exception("die map文件的die数量与cp 机生成的 map文件不匹配");
            }

            // ... existing code ...
            List<TestResultPerDie> dieResults = new List<TestResultPerDie>();

            // 读取 dieMapFilePath 文件并处理内容
            string[] lines = File.ReadAllLines(dieMapFilePath);

            //是否找到目标的数据
            bool startLineInited = false;

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];

                if (!startLineInited && !line.StartsWith(".."))
                {
                    continue;
                }
                if (!startLineInited)
                {
                    startLineInited = true;
                }

                for (int x = 0; x < line.Length; x++)
                {
                    // 创建 TestResultPerDie 实例并设置坐标
                    TestResultPerDie result = new TestResultPerDie
                    {
                        //坐标都是为正数的
                        CodeBitOfCorrdinatorValueX = 1,
                        CodeBitOfCorrdinatorValueY = 1,
                        DieCoordinatorValueX = x, // X坐标
                        DieCoordinatorValueY = y, // Y坐标
                                                  // 其他属性可以根据需要设置
                        DieProperty = 2     //默认为去边不测的设置
                    };
                    char currentChar = line[x];
                    if (currentChar == '.')
                    {
                        result.DummyData = 1;
                        dieResults.Add(result);
                        continue;
                    }

                    if (currentChar == '1')
                    {
                        result.DieProperty = 1;
                    }
                    //
                    //else if (currentChar == 'X')
                    //{
                    //  result.DieProperty = 0;
                    //}

                    dieResults.Add(result);
                }
            }

            tskMapFile.DieResults = dieResults;
            ConvertTestResultsToBinary(tskMapFile.DieResults);

            // 添加 00 的点位符，满足后续数据填充要求。
            //（应该也可以不填充，但是还没有测试过。因为当时拿到原始数据是有填充的）
            foreach (var dieContent in tskMapFile.DieResults)
            {
                _bw.WriteAsBytes(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
            }

            _bw.Close();
            fs.Close();
        }
    }
}
