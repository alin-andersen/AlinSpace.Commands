#!/bin/bash

dotnet build -c Release
dotnet nuget push AlinSpace.Commands/bin/Release/AlinSpace.Commands.*.nupkg --source github --skip-duplicate