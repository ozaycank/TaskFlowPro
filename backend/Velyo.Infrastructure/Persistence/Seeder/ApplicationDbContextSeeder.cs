using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velyo.Domain.Entities;
using Velyo.Domain.Enums;

namespace Velyo.Infrastructure.Persistence.Seeder;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedSampleDataAsync(ApplicationDbContext context, ILogger logger)
    {
        logger.LogInformation("Checking enterprise development data...");

        // 1. GERÇEKÇİ KULLANICILARI OLUŞTUR VEYA BUL
        var defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123!");

        var ceo = await EnsureUserAsync(context, "sarah.connor@techflow.com", "Sarah", "Connor", defaultPasswordHash);
        var pm = await EnsureUserAsync(context, "michael.scott@techflow.com", "Michael", "Scott", defaultPasswordHash);
        var dev1 = await EnsureUserAsync(context, "alex.chen@techflow.com", "Alex", "Chen", defaultPasswordHash);
        var dev2 = await EnsureUserAsync(context, "emily.wang@techflow.com", "Emily", "Wang", defaultPasswordHash);

        // 2. WORKSPACE KONTROLÜ
        if (await context.Workspaces.AnyAsync(w => w.Name == "TechFlow Solutions"))
        {
            logger.LogInformation("Enterprise data already seeded.");
            return;
        }

        logger.LogInformation("Seeding Enterprise Workspace: TechFlow Solutions...");

        // WORKSPACE
        var workspace = Workspace.Create("TechFlow Solutions", "Enterprise software development & consulting", ceo.Id);
        context.Workspaces.Add(workspace);
        await context.SaveChangesAsync();

        // MEMBERS
        context.WorkspaceMembers.AddRange(
            WorkspaceMember.Create(workspace.Id, ceo.Id, WorkspaceRole.Owner),
            WorkspaceMember.Create(workspace.Id, pm.Id, WorkspaceRole.Admin),
            WorkspaceMember.Create(workspace.Id, dev1.Id, WorkspaceRole.Member),
            WorkspaceMember.Create(workspace.Id, dev2.Id, WorkspaceRole.Member)
        );

        // WORKFLOWS
        var devWorkflow = Workflow.Create(workspace.Id, "Software Development", isDefault: true);
        var stateTodo = devWorkflow.AddState("Backlog", "#e2e8f0", StateCategory.Unstarted, 0);
        var stateDev = devWorkflow.AddState("In Development", "#3b82f6", StateCategory.Started, 1);
        var stateReview = devWorkflow.AddState("Code Review", "#a855f7", StateCategory.Started, 2);
        var stateDone = devWorkflow.AddState("Done", "#22c55e", StateCategory.Completed, 3);
        context.Workflows.Add(devWorkflow);

        var designWorkflow = Workflow.Create(workspace.Id, "Design Process", isDefault: false);
        designWorkflow.AddState("Ideation", "#fef08a", StateCategory.Unstarted, 0);
        designWorkflow.AddState("Prototyping", "#fbd38d", StateCategory.Started, 1);
        designWorkflow.AddState("Approved", "#4ade80", StateCategory.Completed, 2);
        context.Workflows.Add(designWorkflow);

        // PROJECTS
        var projectEcom = Project.Create(workspace.Id, "E-Commerce Replatforming", "Migrating legacy monolith to microservices.");
        var projectMobile = Project.Create(workspace.Id, "iOS App Refactor", "Updating app to Swift UI and new design system.");
        context.Projects.AddRange(projectEcom, projectMobile);
        await context.SaveChangesAsync();

        // SPRINTS
        var sprintActive = Sprint.Create(projectEcom.Id, "Sprint 42: Checkout Flow", "Finalize Stripe integration and cart UI", DateTimeOffset.UtcNow.AddDays(-3), DateTimeOffset.UtcNow.AddDays(11));
        sprintActive.Start();

        var sprintNext = Sprint.Create(projectEcom.Id, "Sprint 43: User Profiles", "OAuth integration and profile dashboard", DateTimeOffset.UtcNow.AddDays(12), DateTimeOffset.UtcNow.AddDays(26));

        context.Sprints.AddRange(sprintActive, sprintNext);
        await context.SaveChangesAsync();

        // TASKS (E-Commerce Project)
        var tasks = new List<TaskItem>
        {
            // Done
            CreateTask(workspace.Id, projectEcom.Id, "Setup Next.js & Tailwind base", "Initialize the monorepo", PriorityLevel.Medium, stateDone.Id, 2.0f, dev1.Id, sprintActive.Id),
            CreateTask(workspace.Id, projectEcom.Id, "Database schema for Orders", "PostgreSQL schema design", PriorityLevel.High, stateDone.Id, 3.0f, dev2.Id, sprintActive.Id),
            
            // Code Review
            CreateTask(workspace.Id, projectEcom.Id, "Stripe Payment Webhooks", "Handle successful payments and failed charges", PriorityLevel.Critical, stateReview.Id, 5.0f, dev2.Id, sprintActive.Id),
            
            // In Development
            CreateTask(workspace.Id, projectEcom.Id, "Cart Context & State Management", "Use Zustand or Context API for local cart state", PriorityLevel.High, stateDev.Id, 3.0f, dev1.Id, sprintActive.Id),
            CreateTask(workspace.Id, projectEcom.Id, "OAuth2 Google Login", "Implement NextAuth for Google providers", PriorityLevel.Medium, stateDev.Id, 2.0f, dev1.Id, sprintActive.Id),
            
            // Backlog (Active Sprint)
            CreateTask(workspace.Id, projectEcom.Id, "Responsive Checkout UI", "Figma to React conversion for checkout page", PriorityLevel.Medium, stateTodo.Id, 4.0f, null, sprintActive.Id),
            
            // Backlog (Next Sprint)
            CreateTask(workspace.Id, projectEcom.Id, "User Profile Settings Page", "Allow users to change email and address", PriorityLevel.Low, stateTodo.Id, 2.0f, null, sprintNext.Id),
            
            // Backlog (No Sprint)
            CreateTask(workspace.Id, projectEcom.Id, "Email Notification Templates", "Design transactional emails", PriorityLevel.Low, stateTodo.Id, 3.0f, pm.Id, null)
        };
        context.TaskItems.AddRange(tasks);

        // DOCUMENTS
        var doc1 = Document.Create(workspace.Id, projectEcom.Id, null, "Technical Architecture", "# Tech Stack\n- Frontend: Next.js 14\n- Backend: .NET 8 API\n- DB: PostgreSQL", "🏗️", 1);
        var doc2 = Document.Create(workspace.Id, projectEcom.Id, null, "API Guidelines", "# REST API Rules\nAlways use standard HTTP verbs and return correct status codes.", "📚", 2);
        context.Documents.AddRange(doc1, doc2);

        // ACTIVITY LOGS
        context.ActivityLogs.Add(ActivityLog.Create(workspace.Id, projectEcom.Id, null, pm.Id, "Sprint", sprintActive.Id, "Started", "Sprint 42 has been started."));
        context.ActivityLogs.Add(ActivityLog.Create(workspace.Id, projectEcom.Id, null, dev1.Id, "Task", Guid.Empty, "Completed", "Completed base setup."));

        await context.SaveChangesAsync();
        logger.LogInformation("Enterprise Database seeding completed successfully.");
    }

    // YARDIMCI METOTLAR
    private static async Task<User> EnsureUserAsync(ApplicationDbContext context, string email, string firstName, string lastName, string passHash)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            user = User.Create(email, firstName, lastName, passHash);
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }
        return user;
    }

    private static TaskItem CreateTask(Guid workspaceId, Guid projectId, string title, string desc, PriorityLevel priority, Guid stateId, float points, Guid? assigneeId, Guid? sprintId = null)
    {
        var task = TaskItem.Create(workspaceId, projectId, title, desc, priority, stateId, points, null);
        if (sprintId.HasValue) task.AssignToSprint(sprintId.Value);
        if (assigneeId.HasValue) task.AssignTo(assigneeId.Value);
        return task;
    }
}