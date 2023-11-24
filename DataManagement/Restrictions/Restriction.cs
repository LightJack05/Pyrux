
using System.Text.RegularExpressions;

namespace Pyrux.DataManagement.Restrictions
{
    internal class Restriction
    { 

        public RestrictionType Type;
        public int LimitValue;
        public UserFunction LimitedFunction;

        public static Dictionary<string, RestrictionType> UiStringToRestrictionTypeDictionary = new()
        {
            {"Maximum calls", RestrictionType.CallLimit},
            {"Mimimum calls", RestrictionType.CallMin },
            {"Maximum references", RestrictionType.ReferenceLimit },
            {"Minimum references", RestrictionType.ReferenceMin }
        };
        public static Dictionary<RestrictionType, string> RestrictionTypeToUiStringDictionary = new()
        {
            {RestrictionType.CallLimit, "Maximum calls" },
            {RestrictionType.CallMin, "Mimimum calls" },
            {RestrictionType.ReferenceLimit , "Maximum references"},
            {RestrictionType.ReferenceMin , "Minimum references"}
        };
        public static List<string> RestrictionTypeUiStrings { get => UiStringToRestrictionTypeDictionary.Keys.ToList(); }
        public static List<RestrictionType> RestrictionTypes { get => UiStringToRestrictionTypeDictionary.Values.ToList(); }

        public static Dictionary<string, UserFunction> FunctionUiTypeDictionary = new()
        {
            {"GoForward", UserFunction.GoForward},
            {"TurnLeft", UserFunction.TurnLeft},
            {"TurnRight", UserFunction.TurnRight},
            {"TakeChip", UserFunction.TakeChip },
            {"PlaceChip",UserFunction.PlaceChip },
            {"ChipThere", UserFunction.ChipThere},
            {"WallAhead", UserFunction.WallAhead},
        };
        public static List<string> FunctionTypeUiStrings { get => FunctionUiTypeDictionary.Keys.ToList(); }
        public static List<UserFunction> FunctionTypes { get => FunctionUiTypeDictionary.Values.ToList(); }
        public static List<int> AvailableNumbersForRestrictions = new()
        {
            0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32
        };


        /// <summary>
        /// Create a new Restriction object. Add to PyruxLevel.CompletionRestrictions to add a restriction to a level.
        /// </summary>
        /// <param name="type">Type of restriction</param>
        /// <param name="limitedFunction">The function the restriction should restrict</param>
        /// <param name="limitValue">Limit the value the restriction will enforce. How it enforces depends on the type of restriction.</param>
        [JsonConstructor]
        public Restriction(RestrictionType type, UserFunction limitedFunction, int limitValue)
        {
            Type = type;
            LimitValue = limitValue;
            LimitedFunction = limitedFunction;
        }
        /// <summary>
        /// Check if the Restriction is satisfied for the current level in it's current state.
        /// NOTE: Will count for the current execution state!
        /// </summary>
        /// <param name="level">Level to check.</param>
        /// <returns>True if satisfied, otherwise False</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public bool IsSatisfied(PyruxLevel level)
        {
            switch(Type)
            {
                case RestrictionType.ReferenceLimit:
                    return IsReferenceLimitSatisfied(level);
                case RestrictionType.ReferenceMin:
                    return IsReferenceMinSatisfied(level);
                case RestrictionType.CallLimit:
                    return IsCallLimitSatisfied();
                case RestrictionType.CallMin:
                    return IsCallMinSatisfied();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private bool IsReferenceLimitSatisfied(PyruxLevel level)
        {
            int countOfReferences = Regex.Matches(level.Script, LimitedFunction.ToCallString()).Count;
            return countOfReferences <= LimitValue;
        }

        private bool IsReferenceMinSatisfied(PyruxLevel level)
        {
            int countOfReferences = Regex.Matches(level.Script, LimitedFunction.ToCallString()).Count;
            return countOfReferences >= LimitValue;
        }

        private bool IsCallLimitSatisfied()
        {
            return FunctionCallCount.GetCallCount(LimitedFunction) <= LimitValue;
        }

        private bool IsCallMinSatisfied()
        {
            return FunctionCallCount.GetCallCount(LimitedFunction) >= LimitValue;
        }

        public bool IsRuntimeDependant()
        {
            List<RestrictionType> runtimeDependantRestrictionTypes = new()
            {
                RestrictionType.CallMin, 
                RestrictionType.CallLimit
            };

            return runtimeDependantRestrictionTypes.Contains(this.Type);

        }

        public override string ToString()
        {
            return $"{RestrictionTypeToUiStringDictionary[this.Type]} to function {this.LimitedFunction.ToString()} are {this.LimitValue}";
        }
    }
}
