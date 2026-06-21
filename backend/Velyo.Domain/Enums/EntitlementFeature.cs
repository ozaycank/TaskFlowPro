namespace Velyo.Domain.Enums;

public enum EntitlementFeature
{
    // Boolean Features
    CustomFields = 1,
    AdvancedSearch = 2,
    PrioritySupport = 3,

    // Quota/Usage Based Features
    MaxWorkspaceMembers = 101, // e.g. Free: 5, Pro: Unlimited
    MaxStorageBytes = 102,     // e.g. Free: 5GB, Pro: 100GB
    MaxActiveProjects = 103    // e.g. Free: 3, Pro: Unlimited
}