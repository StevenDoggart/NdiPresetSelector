# NDI Preset Selector
A simple Windows utility allowing users to control a PTZ camera via global hot keys

## Introduction
Not only does NewTek's NDI technology simplify A/V streaming, but it also provides a standard way 
to control PTZ cameras.  NewTek's [NDI Tools for Windows](https://www.newtek.com/ndi/applications/) 
even allow you to control PTZ cameras for free.  Unfortunately, those applications aren't practical 
for live PTZ control because they're too cumbersome to use and they drain system resources.  As a 
result, many people resort to camera controller devices.  Such devices do solve the problem, by 
offloading the work to a dedicated external device, but they can be expensive and overly 
complicated.

For simple setups, like small churches that live-stream their services, the NDI Preset Selector 
is a free and convenient alternative.  It does not provide full PTZ control.  Instead, it relies on 
camera presets.  If you take the time to pre-configure the settings for your shots/angles, and save 
them to the camera (which you can do with the free NDI Studio Monitor application), you can then 
use this tool to recall those presets via simple kot keys.

For instance, if you configure preset #1 to be zoomed in on the pulpit, and you configure preset #2 
to be zoomed out to the whole stage, then you can use this utility to switch between those two 
views by pressing Ctrl+1 for the pulpit and Ctrl+2 for the whole stage.  As long as this utility is 
running, the hot keys will work anywhere on the computer, no matter what program is currently 
active.

This utility is lightweight, using very little bandwidth and very few system resources, so it 
won't get in your way or harm the performance of your video streaming software (e.g. OBS).  It runs 
in the background as an icon in your Windows system tray.  It's configuration free.  Simply pick 
the camera that you want to control via the context menu, and then press the hot keys to control 
that camera.  It's that easy.

## Technical Stuff
This utility is a native Windows application.  It will not run on other platforms.  It is written 
in C# and uses the NDILib library for Windows which is provided with the NDI SDK 
(`Processing.NDI.Lib.x64.dll`).

## Installation
A convenient setup utility is provided for each release of this repository.  The setup utility for 
the latest version of the software is:

[NdiPresetSelectorSetup_1.1.zip](https://github.com/StevenDoggart/NdiPresetSelector/releases/download/v1.1/NdiPresetSelectorSetup_1.1.zip)

To install it, unzip the file and run `NdiPresetSelectorSetup.exe`.

## Attributions
The icon for this application is taken from the Captiva Icons created by bokehlicia, as found on 
https://iconarchive.com