namespace BrannenNotion.Build.Target
{
    using Nuke.Common;
    using Nuke.Common.CI.GitHubActions;
    using Nuke.Common.IO;
    using Nuke.Common.ProjectModel;
    using Nuke.Common.Tools.DotNet;
    using Nuke.Common.Utilities.Collections;

    [GitHubActions(
        "ios-build",
        GitHubActionsImage.MacOsLatest,
        AutoGenerate = true,
        OnPushBranches = new[] { "main" },
        InvokedTargets = new[] { nameof(CompileIos) })]
    public partial class Build
    {

        protected Project TodoTaskManageriOS => MacSolution
            .GetProject("BrannenNotion.TodoTaskManager.iOS");

        public Target RestoreIos => _ => _
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetRestore(s => s.SetProjectFile(this.TodoTaskManageriOS));
            });


        public Target CompileIos => _ => _
            .DependsOn(RestoreIos)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetBuild(s => s.SetProjectFile(this.TodoTaskManageriOS));
            }); 
    }
}
