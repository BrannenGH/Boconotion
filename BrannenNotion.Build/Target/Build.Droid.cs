namespace BrannenNotion.Build.Target
{
    using Nuke.Common;
    using Nuke.Common.CI.GitHubActions;
    using Nuke.Common.IO;
    using Nuke.Common.ProjectModel;
    using Nuke.Common.Tools.DotNet;
    using Nuke.Common.Utilities.Collections;

    [GitHubActions(
        "droid-build",
        GitHubActionsImage.UbuntuLatest,
        AutoGenerate = true,
        OnPushBranches = new[] { "main" },
        InvokedTargets = new[] { nameof(CompileDroid) })]
    public partial class Build
    {
       
        protected Project TodoTaskManagerDroid => MacSolution
            .GetProject("BrannenNotion.TodoTaskManager.Droid");
        
        public Target RestoreDroid => _ => _
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetRestore(s => s.SetProjectFile(this.TodoTaskManagerDroid));
            });


        public Target CompileDroid => _ => _
            .DependsOn(RestoreDroid)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetBuild(s => s.SetProjectFile(this.TodoTaskManagerDroid));
            }); 
    }
}
