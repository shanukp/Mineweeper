# Building the Zynga.Core.UnityUtil Plugin

## Environment Setup

Ensure you have **android** and **ant** commands available to your path. Check with:
```
which android
which ant
```

### Android SDK

The **android** command line tool should be in the **tools** directory of your Android SDK installation. Get the [Android SDK](https://developer.android.com/studio/index.html#downloads) if you don't already have it. You may use the command-line-tool-only version. The IDE is unnecessary.

Once you locate the **tools** directory of your SDK installation, add it to your $PATH:
```
[android-sdk-root-directory]/tools
```

### ant

You can get ant using [Homebrew](http://brew.sh/).
```
brew install ant
```

## Generating Android Project Files

Create the files needed to complete the Android project by running this command in the root folder of the project:
```
android update project -p .
```

## Building the Project

Ensure that the **project.properties** file has:
```
android.library=true
```
If not, add it. This will make the build generate a JAR library.

To build, run:
```
ant clean release
```

This will build a **classes.jar** file in the **bin** folder if successful. This JAR can be renamed and included in your project (For example project, under Android/Plugins). If the JAR doesn't appear after a successful build, ensure that **project.properties** has the *android.library=true* line. 