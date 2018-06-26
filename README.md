# ConvoyUnity
Project for Unity3d simulation of an autonomous convoy

## Requirements ##
1. OS: Linux, Ubuntu 16.04 (should work on other versions)
1. Unity: Unity 2017.2.0f3 - Download from: http://beta.unity3d.com/download/ee86734cf592/public_download.html
(or Latest: https://forum.unity.com/threads/unity-on-linux-release-notes-and-known-issues.350256/page-2)
1. Blender:
'''
sudo add-apt-repository ppa:thomas-schiex/blender
sudo apt-get -y  update
sudo apt-get -y  install blender
'''
    
## Installation ##
1. Clone the repository

1. Clone the repository of the simulated ICD: https://github.com/iosp/simulationICD.git and follow the instructions:
In brief:
    1. After compilation, copy the wrappers to the libwrappers
    1. Copy the config files to ~/simConfigs/
    1. Copy the shared object libraries (*.so) to /usr/lib and run sudo ldconfig
    
1. Open the "ConvoyUnity" project in the Unity Editor.

1. To enable VScode integration, in the Editor, go to Edit->Preferences->VScode->Enable Integration.

1. For code completion in VScode install mono-develop:
    
    http://www.mono-project.com/download/#download-lin
    
Notes:
    
    -Blender should be installed before the Project is opened.
    -In the case you openned without the PPA blender installed and having missing models, install blender and do Right-Click ->     "reimport assets" on the models folder.
