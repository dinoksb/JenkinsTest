echo %PATH%
 
echo %USERDOMAIN%\%USERNAME%
 
"C:\Program Files\Unity\Hub\Editor\2019.1.14f1\Editor\Unity.exe" -quit -batchmode -logFile BuildLog.txt -buildTarget Android -executeMethod JENKINS.AutoBuilder.JenkinsAutoBuildTest -appName JenkinsTest -buildFolder D:\UnityProject\TestProject\UnityJenkinsTest\Build