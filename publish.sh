#!/bin/bash

dotnet build ./src/DataQuery.LanguageExt.Sql/DataQuery.LanguageExt.Sql.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt.Sql/DataQuery.LanguageExt.Sql.csproj -c Release -o ./publish /p:PackageVersion=0.0.41

VER=0.0.8

dotnet build ./src/DataQuery.LanguageExt/DataQuery.LanguageExt.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt/DataQuery.LanguageExt.csproj -c Release -o ./publish /p:PackageVersion=$VER

dotnet build ./src/DataQuery.LanguageExt.SourceGenerator.Common/DataQuery.LanguageExt.SourceGenerator.Common.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt.SourceGenerator.Common/DataQuery.LanguageExt.SourceGenerator.Common.csproj -c Release -o ./publish /p:PackageVersion=$VER

dotnet build ./src/DataQuery.LanguageExt.SourceGenerator.NormNet/DataQuery.LanguageExt.SourceGenerator.NormNet.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt.SourceGenerator.NormNet/DataQuery.LanguageExt.SourceGenerator.NormNet.csproj -c Release -o ./publish /p:PackageVersion=$VER

dotnet build ./src/DataQuery.LanguageExt.SourceGenerator.SystemData/DataQuery.LanguageExt.SourceGenerator.SystemData.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt.SourceGenerator.SystemData/DataQuery.LanguageExt.SourceGenerator.SystemData.csproj -c Release -o ./publish /p:PackageVersion=$VER
