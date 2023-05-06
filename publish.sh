#!/bin/bash

dotnet build ./src/DataQuery.LanguageExt.Sql/DataQuery.LanguageExt.Sql.csproj -c Release
dotnet pack ./src/DataQuery.LanguageExt.Sql/DataQuery.LanguageExt.Sql.csproj -c Release -o ./publish /p:PackageVersion=0.0.38
