namespace BrannenNotion.Build.Target
{
    using Nuke.Common;
    using Nuke.Common.CI.GitHubActions;
    using Nuke.Common.IO;
    using Nuke.Common.ProjectModel;
    using Nuke.Common.Tools.DotNet;
    using Nuke.Common.Utilities.Collections;

    [GitHubActions(
        "macos-build",
        GitHubActionsImage.MacOsLatest,
        AutoGenerate = true,
        OnPushBranches = new[] { "main" },
        InvokedTargets = new[] { nameof(CompileMacOs) })]
    public partial class Build
    {

        protected Project TodoTaskManagerMacOs => MacSolution
            .GetProject("BrannenNotion.TodoTaskManager.macOS");

        public Target RestoreMacOs => _ => _
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetRestore(s => s.SetProjectFile(this.TodoTaskManagerMacOs));
            });


        public Target CompileMacOs => _ => _
            .DependsOn(RestoreMacOs)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetBuild(s => s.SetProjectFile(this.TodoTaskManagerMacOs));
            });
    }
}
