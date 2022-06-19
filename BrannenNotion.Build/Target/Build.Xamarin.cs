namespace BrannenNotion.Build.Target
{
    using Nuke.Common;
    using Nuke.Common.CI.GitHubActions;
    using Nuke.Common.IO;
    using Nuke.Common.Tools.DotNet;
    using Nuke.Common.Utilities.Collections;

    [GitHubActions(
        "xamarin-build",
        GitHubActionsImage.MacOsLatest,
        AutoGenerate = true,
        OnPushBranches = new[] { "main" },
        InvokedTargets = new[] { nameof(CompileXamarin) })]
    public partial class Build
    {
        public Target RestoreXamarin => _ => _
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetRestore(s => s.SetProjectFile(this.MacSolution));
            });


        public Target CompileXamarin => _ => _
            .DependsOn(RestoreXamarin)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetBuild(s => s.SetProjectFile(this.MacSolution));
            }); 
    }
}
