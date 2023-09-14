cls
echo off
c:


cd windows
cd microsoft.net
cd framework
cd v3.5

%msbuild% "E:\Git projects\Rose\roseBot\SourceClrBootstrap.sln" /t:rebuild /p:Configuration=Debug /p:Platform="any cpu" /clp:Nosummary 