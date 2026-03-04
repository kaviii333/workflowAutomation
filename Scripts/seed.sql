
CREATE TABLE [dbo].[ExecutionLogs] (
    [Id]               UNIQUEIDENTIFIER NOT NULL,
    [ExecutionId]      UNIQUEIDENTIFIER NOT NULL,
    [StepName]         NVARCHAR (MAX)   NOT NULL,
    [StepType]         NVARCHAR (MAX)   NOT NULL,
    [EvaluatedRules]   NVARCHAR (MAX)   NOT NULL,
    [SelectedNextStep] NVARCHAR (MAX)   NOT NULL,
    [Status]           NVARCHAR (MAX)   NOT NULL,
    [ErrorMessage]     NVARCHAR (MAX)   NOT NULL,
    [StartedAt]        DATETIME2 (7)    NOT NULL,
    [EndedAt]          DATETIME2 (7)    NULL,
    CONSTRAINT [PK_ExecutionLogs] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ExecutionLogs_Executions_ExecutionId] FOREIGN KEY ([ExecutionId]) REFERENCES [dbo].[Executions] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ExecutionLogs_ExecutionId]
    ON [dbo].[ExecutionLogs]([ExecutionId] ASC);

CREATE TABLE [dbo].[Executions] (
    [Id]                   UNIQUEIDENTIFIER NOT NULL,
    [WorkflowId]           UNIQUEIDENTIFIER NOT NULL,
    [WorkflowVersion]      INT              NOT NULL,
    [Status]               NVARCHAR (MAX)   NOT NULL,
    [Data]                 NVARCHAR (MAX)   NOT NULL,
    [CurrentStepId]        UNIQUEIDENTIFIER NOT NULL,
    [StartedAt]            DATETIME2 (7)    NOT NULL,
    [EndedAt]              DATETIME2 (7)    NULL,
    [TotalDurationSeconds] FLOAT (53)       NULL,
    CONSTRAINT [PK_Executions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Executions_Workflows_WorkflowId] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Executions_WorkflowId]
    ON [dbo].[Executions]([WorkflowId] ASC);

CREATE TABLE [dbo].[Rules] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [StepId]     UNIQUEIDENTIFIER NOT NULL,
    [Condition]  NVARCHAR (MAX)   NOT NULL,
    [NextStepId] UNIQUEIDENTIFIER NOT NULL,
    [Priority]   INT              NOT NULL,
    CONSTRAINT [PK_Rules] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Rules_Steps_StepId] FOREIGN KEY ([StepId]) REFERENCES [dbo].[Steps] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Rules_StepId]
    ON [dbo].[Rules]([StepId] ASC);

CREATE TABLE [dbo].[Steps] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [WorkflowId] UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (MAX)   NOT NULL,
    [StepType]   NVARCHAR (MAX)   NOT NULL,
    [Order]      INT              NOT NULL,
    [Metadata]   NVARCHAR (MAX)   NOT NULL,
    CONSTRAINT [PK_Steps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Steps_Workflows_WorkflowId] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Steps_WorkflowId]
    ON [dbo].[Steps]([WorkflowId] ASC);


CREATE TABLE [dbo].[Workflows] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Name]        NVARCHAR (MAX)   NOT NULL,
    [Version]     INT              NOT NULL,
    [IsActive]    BIT              NOT NULL,
    [InputSchema] NVARCHAR (MAX)   NOT NULL,
    [StartStepId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Workflows] PRIMARY KEY CLUSTERED ([Id] ASC)
);



-- ================================
-- Workflows
-- ================================
INSERT INTO Workflows (Id, Name, Version, IsActive, InputSchema, StartStepId)
VALUES (
    '11111111-1111-1111-1111-111111111111',
    'Loan Approval Workflow',
    1,
    1,
    '{"age": "int", "creditScore": "int"}',
    '33333333-3333-3333-3333-333333333333'
);

-- ================================
-- Steps
-- ================================
INSERT INTO Steps (Id, WorkflowId, Name, StepType, [Order], Metadata)
VALUES
('33333333-3333-3333-3333-333333333333', '11111111-1111-1111-1111-111111111111', 'Check Credit Score', 'Decision', 1, '{}'),
('44444444-4444-4444-4444-444444444444', '11111111-1111-1111-1111-111111111111', 'Approve Loan', 'Task', 2, '{}'),
('55555555-5555-5555-5555-555555555555', '11111111-1111-1111-1111-111111111111', 'Notify Applicant', 'Notification', 3, '{}');

-- ================================
-- Rules
-- ================================
INSERT INTO Rules (Id, StepId, Condition, NextStepId, Priority)
VALUES
('7d395907-d9ee-4353-92f5-abd20bd01f38', '33333333-3333-3333-3333-333333333333', 'creditScore >= 700', '44444444-4444-4444-4444-444444444444', 1),
('8e395907-d9ee-4353-92f5-abd20bd01f39', '33333333-3333-3333-3333-333333333333', 'creditScore < 700', '55555555-5555-5555-5555-555555555555', 2);

-- ================================
-- Executions (sample runs)
-- ================================
INSERT INTO Executions (Id, WorkflowId, Status, StartedAt, EndedAt, TotalDurationSeconds)
VALUES
('bf968e63-e8ba-4817-9804-ed56d5d8b93a', '11111111-1111-1111-1111-111111111111', 'Completed', GETUTCDATE(), GETUTCDATE(), 0.5),
('4b66a439-565c-458d-834c-5f3644a4b8c3', '11111111-1111-1111-1111-111111111111', 'Completed', GETUTCDATE(), GETUTCDATE(), 0.7),
('5e500d0a-9c03-42ed-912a-afc5e07e8df0', '11111111-1111-1111-1111-111111111111', 'Completed', GETUTCDATE(), GETUTCDATE(), 0.6);

-- ================================
-- ExecutionLogs (linked to executions above)
-- ================================

-- bf968e63...
INSERT INTO ExecutionLogs (Id, ExecutionId, StepName, StepType, EvaluatedRules, SelectedNextStep, Status, ErrorMessage, StartedAt, EndedAt)
VALUES (NEWID(), 'bf968e63-e8ba-4817-9804-ed56d5d8b93a', 'Check Age', 'Decision',
        '[{"Condition":"amount > 1000","Passed":true},{"Condition":"age >= 18","Passed":true}]',
        'Approval Notification', 'Completed', '', GETUTCDATE(), GETUTCDATE());

INSERT INTO ExecutionLogs (Id, ExecutionId, StepName, StepType, EvaluatedRules, SelectedNextStep, Status, ErrorMessage, StartedAt, EndedAt)
VALUES (NEWID(), 'bf968e63-e8ba-4817-9804-ed56d5d8b93a', 'Approval Notification', 'Notification',
        '[]', 'End of workflow', 'Completed', '', GETUTCDATE(), GETUTCDATE());

-- 4b66a439...
INSERT INTO ExecutionLogs (Id, ExecutionId, StepName, StepType, EvaluatedRules, SelectedNextStep, Status, ErrorMessage, StartedAt, EndedAt)
VALUES (NEWID(), '4b66a439-565c-458d-834c-5f3644a4b8c3', 'Check Age', 'Decision',
        '[{"Condition":"amount > 1000","Passed":true},{"Condition":"age >= 18","Passed":true}]',
        'Approval Notification', 'Completed', '', GETUTCDATE(), GETUTCDATE());

INSERT INTO ExecutionLogs (Id, ExecutionId, StepName, StepType, EvaluatedRules, SelectedNextStep, Status, ErrorMessage, StartedAt, EndedAt)
VALUES (NEWID(), '4b66a439-565c-458d-834c-5f3644a4b8c3', 'Approval Notification', 'Notification',
        '[]', 'End of workflow', 'Completed', '', GETUTCDATE(), GETUTCDATE());

-- 5e500d0a...
INSERT INTO ExecutionLogs (Id, ExecutionId, StepName, StepType, EvaluatedRules, SelectedNextStep, Status, ErrorMessage, StartedAt, EndedAt)
VALUES (NEWID(), '5e500d0a-9c03-42ed-912a-afc5e07e8df0', 'Check Age', 'Decision',
        '[{"Condition":"amount > 1000","Passed":true},{"Condition":"age >= 18","Passed":true}]',
        'Approval Notification', 'Completed', '', GETUTCDATE(), GETUTCDATE());

INSERT INTO ExecutionLogs (Id, ExecutionId, StepName, StepType, EvaluatedRules, SelectedNextStep, Status, ErrorMessage, StartedAt, EndedAt)
VALUES (NEWID(), '5e500d0a-9c03-42ed-912a-afc5e07e8df0', 'Approval Notification', 'Notification',
        '[]', 'End of workflow', 'Completed', '', GETUTCDATE(), GETUTCDATE());
