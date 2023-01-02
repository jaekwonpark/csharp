#!/usr/bin/env bash

package_name=${1}
configuration=${2:-Release}

dotnet nuget push ./bin/${configuration}/${package_name} --api-key ${NUGET_API_KEY_ARTIFACTORY} --source http://artifactory.dyn.nutanix.com/artifactory/api/nuget/powershell-cmdlets-nuget/