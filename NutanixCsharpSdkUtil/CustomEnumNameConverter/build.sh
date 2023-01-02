#!/usr/bin/env bash

version_suffix=${1:-1}
configuration=${2:-Release}

frameworkVersion=net6.0

echo "[INFO] Target framework: ${frameworkVersion}"

if ! type dotnet &>/dev/null; then
    echo "[ERROR] Download dotnet from https://dotnet.microsoft.com/en-us/download and install."
    exit 1
fi

dotnet build --configuration ${configuration} && dotnet pack --configuration ${configuration} --version-suffix ${version_suffix}
