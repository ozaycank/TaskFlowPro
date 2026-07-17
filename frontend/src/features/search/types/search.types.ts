export interface SearchResultDto {
    entityType: string; // e.g., 'Task', 'Document', 'Project'
    title: string;
    snippet: string;
    url: string;
}