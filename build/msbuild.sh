#!/usr/bin/env bash
if [[ -z "$1" ]] ; then
    echo "Usage: msbuild.sh <path to .dotnet sdk>"
    exit 1
fi

SDK_DIR="$(cd $1 && pwd -P)"

echo $SDK_DIR

export MSBuildExtensionsPath=$SDK_DIR/
export CscToolExe=$SDK_DIR/Roslyn/RunCsc.sh
export MSBuildSDKsPath=$SDK_DIR/Sdks

#msbuild "$@"