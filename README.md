# ConvoyUnity
Project for Unity3d simulation of an autonomous convoy

Requirements:

    OS:      Linux, Ubuntu 16.04 (should work on other versions)

    Tested with: Unity 2017.2.0f3 - Download from: http://beta.unity3d.com/download/ee86734cf592/public_download.html
    
Installation:

Install the latest Blender from the PPA: (Required for Blender files import)
    
    sudo add-apt-repository ppa:thomas-schiex/blender
    sudo apt-get -y  update
    sudo apt-get -y  install blender
    
Install the tested Unity Editor from:
    
    http://beta.unity3d.com/download/ee86734cf592/public_download.html
Or go for the Latest
    
    https://forum.unity.com/threads/unity-on-linux-release-notes-and-known-issues.350256/page-2
    
Clone the repository and open the "ConvoyUnity" project in the Unity Editor.

To enable VScode integration, in the Editor, go to Edit->Preferences->VScode->Enable Integration.

For code completion in VScode install mono-develop:
    
    http://www.mono-project.com/download/#download-lin
    
Notes:
    
    -Blender should be installed before the Project is opened.
    -In the case you openned without the PPA blender installed and having missing models, install blender and do Right-Click ->     "reimport assets" on the models folder.
