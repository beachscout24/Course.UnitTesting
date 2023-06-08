# PURPOSE: Automates the running of Unit Tests and Code Coverage
# REF: https://stackoverflow.com/a/70321555/495455

# If running outside the test folder
#cd E:\Dev\XYZ\src\XYZTestProject

# This only needs to be installed once (globally), if installed it fails silently: 
dotnet tool install -g dotnet-reportgenerator-globaltool

# Save currect directory into a variable
$dir = pwd

# Delete previous test run results (there's a bunch of subfolders named with guids)
Remove-Item -Recurse -Force $dir/TestResults/

# Run the Coverlet.Collector - REPLACING YOUR SOLUTION NAME!!!
$output = [string] (& dotnet test ./Users.Api.Tests.Unit.csproj --collect:"XPlat Code Coverage" 2>&1)
Write-Host "Last Exit Code: $lastexitcode"
Write-Host $output

# Delete previous test run reports - note if you're getting wrong results do a Solution Clean and Rebuild to remove stale DLLs in the bin folder
Remove-Item -Recurse -Force $dir/Coverage_Report/

# To keep a history of the Code Coverage we need to use the argument: -historydir:SOME_DIRECTORY 
if (!(Test-Path -path $dir/Coverage_History)) {  
 New-Item -ItemType directory -Path $dir/Coverage_History
}

# Generate the Code Coverage HTML Report
reportgenerator -reports:"$dir/**/coverage.cobertura.xml" -targetdir:"$dir/Coverage_Report" -title:Users.Api.Tests.Unit -classfilters:-UserRepository.cs,-Users.Api.Data.* -reporttypes:Html -historydir:$dir/Coverage_History 

# Open the Code Coverage HTML Report (if running on a WorkStation)
$osInfo = Get-CimInstance -ClassName Win32_OperatingSystem
if ($osInfo.ProductType -eq 1) {
(& "$dir/Coverage_Report/index.html")
}

