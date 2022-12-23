# NDI Preset Selector
A simple Windows utility allowing users to control a PTZ camera via global hot keys

## Introduction
NewTek's NDI technology not only makes streaming and receiving video streams easy, but it also 
provides a standard way to control PTZ cameras.  Unfortunately, the free NDI tools provided by 
NewTek are too cumbersome to use and drain too many system resources to make them practical.  Many 
people, therefore, resort to paying for a physical camera controller device.  Such devices do solve 
the problem by being more convenient to use and by offloading the work to an external device, but 
they can be expensive and overly complicated.  

For simple setups, like small churches live-streaming their services, this utility provides a free 
and convenient alternative.  It does not provide full PTZ control.  Instead, it relies on presets.  
If you take the time to pre-configure the settings you want to use for your shots/angles, and save 
them as presets (which you can do with the free NDI monitor application), then you can use this 
tool to easily recall those presets, commanding the camera to pan, tilt, or zoom to those saved 
settings, all via simple kot keys.

For instance, if you configure preset #1 to be zoomed in on the pulpit, and you configure preset #2 
to be zoomed out to the whole stage, then you can use this program to recall those presets by 
simply pressing Ctrl+1 for the pulpit and Ctrl+2 for the whole stage.  As long as the utility is 
running, the hot keys will work everywhere, no matter what program you're currently using.

The utility is light weight, using very little bandwidth and other resources, so it doesn't get in 
your way or harm the performance of your video streaming software (e.g. OBS).  It runs as an icon 
in your Windows system tray and it's configuration free.  Simply pick the camera you want it to 
control via it's context menu, and then press the hot keys to control that camera.  It's that easy.

## Technical Stuff
It is a native Windows application.  It will not run on other platforms.  It is written in C# and 
uses the NDILib library for Windows which is provided with the NDI SDK (`Processing.NDI.Lib.x64.dll` 
or `Processing.NDI.Lib.x86.dll`).

## Installation
A convenient setup utility is provided for each 
[Release](https://github.com/StevenDoggart/NdiPresetSelector/releases) of this repository.

## Attributions
The icon for this application is taken from the Captiva Icons created by bokehlicia, as found on 
https://iconarchive.com