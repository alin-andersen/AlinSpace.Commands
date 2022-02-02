#!/bin/bash

dotnet build -c Release
dotnet nuget push AlinSpace.Command/bin/Release/AlinSpace.Command.*.nupkg --source github --skip-duplicate