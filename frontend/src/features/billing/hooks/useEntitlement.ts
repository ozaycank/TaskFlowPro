import { useQuery } from '@tanstack/react-query';

export enum EntitlementFeature {
    CustomFields = 1,
    AdvancedSearch = 2,
    PrioritySupport = 3,
    MaxWorkspaceMembers = 101,
    MaxStorageBytes = 102,
    MaxActiveProjects = 103
}

export const useEntitlement = (featureCode: EntitlementFeature): boolean => {
    const { data: entitlements } = useQuery<EntitlementFeature[]>({
        queryKey: ['entitlements'],
        queryFn: async () => {
            // TO-DO: Replace with actual API call
            return [EntitlementFeature.CustomFields];
        },
        staleTime: 1000 * 60 * 5, // 5 minutes cache
    });

    return entitlements?.includes(featureCode) ?? false;
};

/* USAGE EXAMPLE (IN A .tsx FILE):
  import { useEntitlement, EntitlementFeature } from './useEntitlement';
  
  const canUseCustomFields = useEntitlement(EntitlementFeature.CustomFields);
  if (!canUseCustomFields) return <div>Please upgrade to Pro</div>;
*/