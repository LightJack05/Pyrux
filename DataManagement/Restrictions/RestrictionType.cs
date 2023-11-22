namespace Pyrux.DataManagement.Restrictions;

/// <summary>
/// Restriction type. Call will enforce limits on execution, reference on raw string references.
/// </summary>
public enum RestrictionType
{
    CallLimit,
    ReferenceLimit,
    CallMin,
    ReferenceMin
}
