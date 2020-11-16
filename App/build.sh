#!/bin/bash
dotnet publish -r win-x64 -c Release -p:PublishSingleFile=true -p:Debugtype=None
dotnet publish -r osx-x64 -c Release -p:PublishSingleFile=true -p:Debugtype=None
dotnet publish -r linux-x64 -c Release -p:PublishSingleFile=true -p:Debugtype=None

mv ./bin/Release/netcoreapp3.1/linux-x64/publish/encryption-rocks "../Binaries/Encryption Rocks Linux"
mv ./bin/Release/netcoreapp3.1/win-x64/publish/encryption-rocks.exe "../Binaries/Encryption Rocks Windows.exe"
mv ./bin/Release/netcoreapp3.1/osx-x64/publish/encryption-rocks "../Binaries/Encryption Rocks Mac"