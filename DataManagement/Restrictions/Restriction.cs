
using System.Text.RegularExpressions;

namespace Pyrux.DataManagement.Restrictions
{
    internal class Restriction
    {
        public RestrictionType Type;
        public int LimitValue;
        public UserFuntion LimitedFunction;

        /// <summary>
        /// Create a new Restriction object. Add to PyruxLevel.CompletionRestrictions to add a restriction to a level.
        /// </summary>
        /// <param name="type">Type of restriction</param>
        /// <param name="limitedFunction">The function the restriction should restrict</param>
        /// <param name="limitValue">Limit the value the restriction will enforce. How it enforces depends on the type of restriction.</param>
        [JsonConstructor]
        public Restriction(RestrictionType type, UserFuntion limitedFunction, int limitValue)
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
                    return isReferenceLimitSatisfied(level);
                case RestrictionType.ReferenceMin:
                    return isReferenceMinSatisfied(level);
                case RestrictionType.CallLimit:
                    return isCallLimitSatisfied();
                case RestrictionType.CallMin:
                    return isCallMinSatisfied();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        private bool isReferenceLimitSatisfied(PyruxLevel level)
        {
            int countOfReferences = Regex.Matches(level.Script, LimitedFunction.ToCallString()).Count;
            return countOfReferences <= LimitValue;
        }

        private bool isReferenceMinSatisfied(PyruxLevel level)
        {
            int countOfReferences = Regex.Matches(level.Script, LimitedFunction.ToCallString()).Count;
            return countOfReferences >= LimitValue;
        }

        private bool isCallLimitSatisfied()
        {
            return FunctionCallCount.GetCallCount(LimitedFunction) <= LimitValue;
        }

        private bool isCallMinSatisfied()
        {
            return FunctionCallCount.GetCallCount(LimitedFunction) >= LimitValue;
        }
    }
}
