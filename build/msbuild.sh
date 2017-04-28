#!/usr/bin/env bash
if [[ -z "$DOTNETSDK" ]] ; then
    echo "DOTNETSDK variable needs to be set to location of .NET Core SDK (ie. /usr/share/dotnet/sdk/1.0.3)"
    exit 1
fi

echo $DOTNETSDK

export MSBuildExtensionsPath=$DOTNETSDK/
export CscToolExe=$DOTNETSDK/Roslyn/RunCsc.sh
export MSBuildSDKsPath=$DOTNETSDK/Sdks

msbuild "$@"