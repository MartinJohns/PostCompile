param(
	$InstallPath,
	$ToolsPath,
	$Package,
	$Project
)

$PostCompileFile = 'PostCompile.exe'
$PostCompilePath = $ToolsPath | Join-Path -ChildPath $PostCompileFile

Add-Type -AssemblyName 'Microsoft.Build, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'

$MSBProject = [Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($Project.FullName) | Select-Object -First 1

$ProjectUri = New-Object -TypeName Uri -ArgumentList "file://$($Project.FullName)"
$TargetUri = New-Object -TypeName Uri -ArgumentList "file://$PostCompilePath"

$RelativePath = $ProjectUri.MakeRelativeUri($TargetUri) -replace '/','\'

$ExistingTargets = $MSBProject.Xml.Targets |
	Where-Object { ($_.Name -eq "RunPostCompile") -or ($_.Name -eq "CleanPostCompile") }
if ($ExistingTargets) {
	$ExistingTargets |
		ForEach-Object {
			$MSBProject.Xml.RemoveChild($_)
		}
}

$buildTarget = $MSBProject.Xml.AddTarget("RunPostCompile")
$buildTarget.BeforeTargets = "AfterBuild"
$buildTask = $buildTarget.AddTask("Exec")
$buildTask.SetParameter("Command", $RelativePath + ' build "$(ProjectDir)$(OutputPath)$(TargetFileName)"')
$buildTask.ContinueOnError = $false

$cleanTarget = $MSBProject.Xml.AddTarget("CleanPostCompile")
$cleanTarget.BeforeTargets = "BeforeClean"
$cleanTask = $cleanTarget.AddTask("Exec")
$cleanTask.SetParameter("Command", $RelativePath + ' clean "$(ProjectDir)$(OutputPath)$(TargetFileName)"')
$cleanTask.ContinueOnError = $false

$Project.Save()
