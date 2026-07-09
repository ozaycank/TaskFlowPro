using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Infrastructure.Persistence.Seeder;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context, ILogger logger)
    {
        logger.LogInformation("Checking sample development data...");

        // 1. Önce Admin ve QA var mı diye kontrol et (Yoklarsa HİÇBİR ŞEY YAPMA, Geliştiricinin Register olmasını bekle)
        var admin = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@velyo.com");
        var qaUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "qa@velyo.com");

        if (admin == null || qaUser == null)
        {
            logger.LogWarning("Seed Paused: Please register 'admin@velyo.com' and 'qa@velyo.com' via Swagger/UI first!");
            return; // Seed'i durdur, çünkü ID'lere ihtiyacımız var.
        }

        // Eğer Workspace yoksa yarat (böylece seed çalışır)
        if (!await context.Workspaces.AnyAsync(w => w.Name == "Acme Corporation"))
        {
            logger.LogInformation("Seeding Enterprise Entities...");

            var workspace = Workspace.Create("Acme Corporation", "Enterprise tech company", admin.Id);
            context.Workspaces.Add(workspace);

            var adminMember = WorkspaceMember.Create(workspace.Id, admin.Id, WorkspaceRole.Admin);
            var qaMember = WorkspaceMember.Create(workspace.Id, qaUser.Id, WorkspaceRole.Member);
            context.WorkspaceMembers.AddRange(adminMember, qaMember);

            var project = Project.Create(workspace.Id, "Project Phoenix", "Next-gen platform rewrite");
            context.Projects.Add(project);

            var workflow = Workflow.Create(workspace.Id, "Standard Software Flow", true);
            var todoState = workflow.AddState("To Do", "#e2e8f0", StateCategory.Unstarted, 100);
            var inProgressState = workflow.AddState("In Progress", "#3b82f6", StateCategory.Started, 200);
            var doneState = workflow.AddState("Done", "#22c55e", StateCategory.Completed, 300);
            context.Workflows.Add(workflow);

            var sprint = Sprint.Create(project.Id, "Sprint 1", "Initial setup", DateTimeOffset.UtcNow.AddDays(-2), DateTimeOffset.UtcNow.AddDays(12));
            sprint.Start();
            context.Sprints.Add(sprint);

            var task1 = TaskItem.Create(workspace.Id, project.Id, "Setup Next.js architecture", "Configure App Router and Tailwind", PriorityLevel.High, todoState.Id, 1.0f, null);
            task1.AssignToSprint(sprint.Id);

            var task2 = TaskItem.Create(workspace.Id, project.Id, "Implement JWT Auth", "Security setup", PriorityLevel.High, inProgressState.Id, 2.0f, null);
            task2.AssignToSprint(sprint.Id);
            task2.AssignTo(qaUser.Id);

            context.TaskItems.AddRange(task1, task2);

            var doc = Document.Create(workspace.Id, project.Id, null, "Architecture Guidelines", "# Velyo Architecture \n\n Keep it clean.", "🏗️", 1);
            context.Documents.Add(doc);

            var rule = AutomationRule.Create(workspace.Id, project.Id, "Auto-assign QA", "When done, assign to QA", AutomationTriggerType.TaskStateChanged, null, AutomationActionType.AssignUser, $"{{\"assigneeId\": \"{qaUser.Id}\"}}");
            context.AutomationRules.Add(rule);

            var log1 = ActivityLog.Create(workspace.Id, project.Id, null, admin.Id, "Workspace", workspace.Id, "Created", "Seeded workspace");
            var log2 = ActivityLog.Create(workspace.Id, project.Id, task1.Id, admin.Id, "Task", task1.Id, "Created", "Created Task 1");
            context.ActivityLogs.AddRange(log1, log2);

            await context.SaveChangesAsync();
            logger.LogInformation("Database seeding completed successfully.");
        }
    }
}