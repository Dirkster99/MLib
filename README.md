[![Build status](https://ci.appveyor.com/api/projects/status/mhg80nk8ywbk9dat?svg=true)](https://ci.appveyor.com/project/Dirkster99/mlib)
[![Release](https://img.shields.io/github/release/Dirkster99/MLib.svg)](https://github.com/Dirkster99/MLib/releases/latest)
# MLib
MLib is a set of WPF theming libraries based on <a href="https://github.com/MahApps/MahApps.Metro">MahApps.Metro</a>,
<a href="https://github.com/firstfloorsoftware/mui/">MUI</a>,
and <a href="https://github.com/Infragistics/InfragisticsThemesForMicrosoftControls">Infragistics Themes For Microsoft Controls</a>.

This set of theming libraries is used to power several Windows dektop app projects:
- ![Edi](https://github.com/Dirkster99/Edi)
- ![File System Controls](https://github.com/Dirkster99/fsc/wiki/FSC-Themeable-Explorer-(Clone)) (TestExplorerMLib and ThemedExplorer projects)

# Features

All styles are available with a Light and Dark theme.

The framework supports a dialog service that supports ContentDialogs and Modal Dialogs using one seemless API:
- <a href="https://www.codeproject.com/Articles/1170500/A-ContentDialog-in-a-WPF-Desktop-Application">A ContentDialog in a WPF Application</a>

Review this article and give feedback on designing WPF controls for <b>more than one</b> specific theming library:
- <a href="File System Controls in WPF (Version III)">File System Controls in WPF (Version III)</a>


# Nuget Packages

There is more than one NuGet package available bacause not all theming features are impemented in one solid big library but within a set of libraries. So, what you actually downlod and install depends on what you need. But most people are going to want to style an applaiction window and its containing controls, which can be done by downloading and installing **MLib** with **MWindowLib**. Install **MLib** with **MWindowDialogLib** if you are planning to implement <a href="https://www.codeproject.com/Articles/1170500/A-ContentDialog-in-a-WPF-Desktop-Application">ContentDialogs</a> in your application.

The above setup with **MWindowDialogLib** includes the libraries for **MWindowLib**, so either one of the 2 deployment scenarios mentioned above, should work for most situations.

- [![NuGet](https://img.shields.io/nuget/dt/Dirkster.MLib.svg)](http://nuget.org/packages/Dirkster.MLib) MLib
- [![NuGet](https://img.shields.io/nuget/dt/Dirkster.MWindowLib.svg)](http://nuget.org/packages/Dirkster.MWindowLib) MWindowLib
- [![NuGet](https://img.shields.io/nuget/dt/Dirkster.MWindowDialogLib.svg)](http://nuget.org/packages/Dirkster.MWindowDialogLib) MWindowDialogLib

# Demo/Template Projects
- [![Build status](https://ci.appveyor.com/api/projects/status/4bjtkyk7eqlor0su?svg=true)](https://ci.appveyor.com/project/Dirkster99/mlib-hq656) source/00_DemoTemplates\ThemedDemo.sln

# Supported OS

This framework is designed with Windows 10 UI guidelines in mind but it should also work for Windows 7 or 8.
