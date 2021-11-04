#!/bin/bash

dotnet build ./src/DataQuery.LanguageExt/DataQuery.LanguageExt.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt/DataQuery.LanguageExt.csproj -c Release -o ./publish /p:PackageVersion=0.0.4
