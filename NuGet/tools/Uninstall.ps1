param(
	$InstallPath,
	$ToolsPath,
	$Package,
	$Project
)

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($Project.FullName) | Select-Object -First 1

$ExistingTargets = $MSBProject.Xml.Targets |
	Where-Object { ($_.Name -eq "RunPostCompile") -or ($_.Name -eq "CleanPostCompile") }
if ($ExistingTargets) {
	$ExistingTargets |
		ForEach-Object {
			$MSBProject.Xml.RemoveChild($_)
		}
}

$Project.Save()