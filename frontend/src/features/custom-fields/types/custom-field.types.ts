export enum FieldType {
    Text = 1,
    Number = 2,
    Date = 3,
    Boolean = 4,
    SingleSelect = 5,
    MultiSelect = 6,
    Url = 7
}

export interface CustomFieldDefinitionDto {
    id: string;
    workspaceId: string;
    projectId: string | null;
    name: string;
    type: FieldType;
    optionsJson: string | null;
    isRequired: boolean;
}

export interface CreateFieldDefinitionCommand {
    workspaceId: string;
    projectId?: string | null;
    name: string;
    type: FieldType;
    optionsJson?: string | null;
    isRequired: boolean;
}

export interface UpdateFieldDefinitionCommand {
    id: string;
    name: string;
    optionsJson?: string | null;
    isRequired: boolean;
}

export interface SetTaskFieldValueCommand {
    taskId: string;
    fieldDefinitionId: string;
    value: string;
}