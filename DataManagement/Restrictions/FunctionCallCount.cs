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
        public static void IncrementCounter(UserFunction function)
        {
            switch (function)
            {
                case UserFunction.GoForward:
                    GoForwardFuntionCallCount++;
                    break;
                case UserFunction.TurnLeft:
                    TurnLeftFuntionCallCount++;
                    break;
                case UserFunction.TurnRight:
                    TurnRightFuntionCallCount++;
                    break;
                case UserFunction.TakeChip:
                    TakeChipFuntionCallCount++;
                    break;
                case UserFunction.PlaceChip:
                    PlaceChipFuntionCallCount++;
                    break;
                case UserFunction.ChipThere:
                    ChipThereFuntionCallCount++;
                    break;
                case UserFunction.WallAhead:
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
        public static int GetCallCount(UserFunction function)
        {
            switch (function)
            {
                case UserFunction.GoForward:
                    return GoForwardFuntionCallCount;
                case UserFunction.TurnLeft:
                    return TurnLeftFuntionCallCount;
                case UserFunction.TurnRight:
                    return TurnRightFuntionCallCount;
                case UserFunction.TakeChip:
                    return TakeChipFuntionCallCount;
                case UserFunction.PlaceChip:
                    return PlaceChipFuntionCallCount;
                case UserFunction.ChipThere:
                    return ChipThereFuntionCallCount;
                case UserFunction.WallAhead:
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
