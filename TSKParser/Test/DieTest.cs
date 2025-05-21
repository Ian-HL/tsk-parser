using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSKParser.Parser;

namespace TSKParser.Test
{
    internal class DieTest
    {
        /// <summary>
        /// 跳行进行测试
        /// </summary>
        /// <param name="tskMapFile"></param>
        /// <param name="overWrite">是否覆盖原本的设置</param>
        internal static void ZebraLine(TskMapFile tskMapFile, bool overWrite = false)
        {
            foreach (var die in tskMapFile.DieResults)
            {

                if (die.DieCoordinatorValueY % 2 == 0)
                {
                    if (overWrite)
                    {
                        if (die.DummyData == 1)
                        {
                            continue;
                        }
                    }
                    EnableDie(die, true);
                    continue;
                }

                EnableDie(die, false);
            }
        }

        /// <summary>
        /// 使用圆形作为有效区域
        /// </summary>
        /// <param name="tskMapFile"></param>
        /// <param name="centerX">中心点X</param>
        /// <param name="centerY">中心点Y</param>
        /// <param name="radius">半径，单位为die的Count</param>
        /// <param name="overWrite">区域内是否覆盖原本的设置。如果原本是有效，则不修改</param>
        internal static void Round(TskMapFile tskMapFile, int radius, bool overWrite = true)
        {
            int maxX = int.MinValue;
            int minX = int.MaxValue;
            int maxY = int.MinValue;
            int minY = int.MaxValue;
            foreach (var die in tskMapFile.DieResults)
            {
                int x = die.DieCoordinatorValueX;
                int y = die.DieCoordinatorValueY;
                if (die.CodeBitOfCorrdinatorValueX == 0)
                {
                    x = -x;
                }
                if (die.CodeBitOfCorrdinatorValueY == 0)
                {
                    y = -y;
                }

                if (x > maxX)
                {
                    maxX = x;
                }
                if (x < minX)
                {
                    minX = x;
                }
                if (y > maxY)
                {
                    maxY = y;
                }
                if (y < minY)
                {
                    minY = y;
                }
            }
            int centerX = (maxX + minX) / 2;
            int centerY = (maxY + minY) / 2;

            foreach (var die in tskMapFile.DieResults)
            {
                int x = die.DieCoordinatorValueX;
                int y = die.DieCoordinatorValueY;
                if (die.CodeBitOfCorrdinatorValueX == 0)
                {
                    x = -x;
                }
                if (die.CodeBitOfCorrdinatorValueY == 0)
                {
                    y = -y;
                }


                //判断 X,Y是否在圆形区域内，如果在区域内
                if (Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2) <= Math.Pow(radius, 2))
                {
                    if (overWrite)
                    {
                        if (die.DummyData == 1)
                        {
                            continue;
                        }
                    }

                    EnableDie(die, true);
                }
                else
                {
                    EnableDie(die, false);
                }
            }
        }

        /// <summary>
        /// 使用圆形作为有效区域
        /// 
        /// overWrite 是指有效区域进行叠加  dummyData与之前的数据同时为有效才行。否则disable掉
        /// </summary>
        /// <param name="tskMapFile"></param>
        /// <param name="centerX">中心点X</param>
        /// <param name="centerY">中心点Y</param>
        /// <param name="radius">半径，单位为die的Count</param>
        /// <param name="overWrite">区域内是否覆盖原本的设置。如果原本是有效，则不修改</param>
        internal static void Round(TskMapFile tskMapFile, int centerX, int centerY, int radius, bool overWrite = true)
        {
            foreach (var die in tskMapFile.DieResults)
            {
                int x = die.DieCoordinatorValueX;
                int y = die.DieCoordinatorValueY;
                if (die.CodeBitOfCorrdinatorValueX == 0)
                {
                    x = -x;
                }
                if (die.CodeBitOfCorrdinatorValueY == 0)
                {
                    y = -y;
                }


                //判断 X,Y是否在圆形区域内，如果在区域内
                if (Math.Pow(x - centerX, 2) + Math.Pow(y - centerY, 2) <= Math.Pow(radius, 2))
                {
                    if (overWrite)
                    {
                        if (die.DummyData == 1)
                        {
                            continue;
                        }
                    }

                    EnableDie(die, true);
                }
                else
                {
                    EnableDie(die, false);
                }
            }
        }

        private static void EnableDie(TestResultPerDie die, bool enable)
        {
            if (enable)
            {
                die.DummyData = 0;
                die.DieProperty = 1;
            }
            else
            {
                die.DummyData = 1;
                //这里尚未验证，设置为2 能否让晶圆显示为黄色的 M
                die.DieProperty = 2;
                die.DieProperty = 0;
            }
        }
    }
}
