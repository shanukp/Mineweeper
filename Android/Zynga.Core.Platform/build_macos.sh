#!/bin/bash

android update project -p .
ant clean release
cp ./bin/classes.jar ../../Assets/Zdk/Zynga.CSharp.Core.Platform.Unity.Source/Plugins/Android/zynga.core.platform.jar
rm -rf bin

