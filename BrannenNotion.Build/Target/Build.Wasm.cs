using Nuke.Common.CI.GitHubActions;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.DotNet;

namespace BrannenNotion.Build.Target
{
    [GitHubActions(
        "wasm-build",
        GitHubActionsImage.UbuntuLatest,
        AutoGenerate = true,
        OnPushBranches = new[] { "main" },
        InvokedTargets = new[] { nameof(CompileWasm) })]
    public partial class Build
    {
        protected Project TodoTaskManagerWasm => MacSolution
            .GetProject("BrannenNotion.TodoTaskManager.Wasm");

        public Nuke.Common.Target RestoreWasm => _ => _
            .DependsOn(Clean)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetRestore(s =>
                    {
                        return s.SetProjectFile(this.TodoTaskManagerWasm);
                    });
            });


        public Nuke.Common.Target CompileWasm => _ => _
            .DependsOn(RestoreWasm)
            .DependsOn(CompileStandard)
            .Executes(() =>
            {
                DotNetTasks
                    .DotNetBuild(s => s
                        .SetProjectFile(this.TodoTaskManagerWasm));
            }); 
    }
}
