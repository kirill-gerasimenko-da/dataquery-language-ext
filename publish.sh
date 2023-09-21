#!/bin/bash

dotnet build ./src/DataQuery.LanguageExt.Sql/DataQuery.LanguageExt.Sql.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt.Sql/DataQuery.LanguageExt.Sql.csproj -c Release -o ./publish /p:PackageVersion=0.0.41

dotnet build ./src/DataQuery.LanguageExt/DataQuery.LanguageExt.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt/DataQuery.LanguageExt.csproj -c Release -o ./publish /p:PackageVersion=0.0.2

dotnet build ./src/DataQuery.LanguageExt.SourceGenerator/DataQuery.LanguageExt.SourceGenerator.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt.SourceGenerator/DataQuery.LanguageExt.SourceGenerator.csproj -c Release -o ./publish /p:PackageVersion=0.0.2
