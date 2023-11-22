using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Text;

namespace Pyrux.DataManagement.Restrictions
{
    internal static class FunctionCallCount
    {
        public static int GoForwardFuntionCallCount { get; private set; }
        public static int TurnLeftFuntionCallCount { get; private set; }
        public static int TurnRightFuntionCallCount { get; private set; }
        public static int TakeChipFuntionCallCount { get; private set; }
        public static int PlaceChipFuntionCallCount { get; private set; }
        public static int ChipThereFuntionCallCount { get; private set; }
        public static int WallAheadFunctionCallCount { get; private set; }

        /// <summary>
        /// Increment the counter for the given function
        /// </summary>
        /// <param name="function">Function to increment counter for</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when invalid function is given</exception>
        public static void IncrementCounter(UserFuntion function)
        {
            switch (function)
            {
                case UserFuntion.GoForward:
                    GoForwardFuntionCallCount++;
                    break;
                case UserFuntion.TurnLeft:
                    TurnLeftFuntionCallCount++;
                    break;
                case UserFuntion.TurnRight:
                    TurnRightFuntionCallCount++;
                    break;
                case UserFuntion.TakeChip:
                    TakeChipFuntionCallCount++;
                    break;
                case UserFuntion.PlaceChip:
                    PlaceChipFuntionCallCount++;
                    break;
                case UserFuntion.ChipThere:
                    ChipThereFuntionCallCount++;
                    break;
                case UserFuntion.WallAhead:
                    WallAheadFunctionCallCount++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid argument for positional argument of IncrementCounter (function): {function}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function">Function to get counter for</param>
        /// <returns>Value of the counter, representing the number of times the function has been called</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when invalid function is given</exception>
        public static int GetCallCount(UserFuntion function)
        {
            switch (function)
            {
                case UserFuntion.GoForward:
                    return GoForwardFuntionCallCount;
                case UserFuntion.TurnLeft:
                    return TurnLeftFuntionCallCount;
                case UserFuntion.TurnRight:
                    return TurnRightFuntionCallCount;
                case UserFuntion.TakeChip:
                    return TakeChipFuntionCallCount;
                case UserFuntion.PlaceChip:
                    return PlaceChipFuntionCallCount;
                case UserFuntion.ChipThere:
                    return ChipThereFuntionCallCount;
                case UserFuntion.WallAhead:
                    return WallAheadFunctionCallCount;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid argument for positional argument of IncrementCounter (function): {function}");
            }
        }

        /// <summary>
        /// Reset all call counters to 0.
        /// </summary>
        public static void ResetCallCount()
        {
            GoForwardFuntionCallCount = 0;
            TurnLeftFuntionCallCount = 0;
            TurnRightFuntionCallCount = 0;
            TakeChipFuntionCallCount = 0;
            PlaceChipFuntionCallCount = 0;
            ChipThereFuntionCallCount = 0;
            WallAheadFunctionCallCount = 0;
        }
    }
}
