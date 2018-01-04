
#include "VLPPluginAPI.h"

VLPCommunicationWrapper* CreateVLPObject(short ip1, short ip2, short ip3, short ip4, int port, int resolution,
    int returnMode, int dataSource, int sensorFrequency, int velType) {
    std::string ipAddress = std::to_string(ip1) + "." + std::to_string(ip2) + "." + std::to_string(ip3) + "." + std::to_string(ip4);
    return new VLPCommunicationWrapper(ipAddress, std::to_string(port), resolution, returnMode, dataSource, sensorFrequency, velType);
}

void DeleteVLPObject(VLPCommunicationWrapper* pVlp) {delete pVlp;}

void Run(VLPCommunicationWrapper* pVlp) { pVlp->Run();}

void SetAzimuth(VLPCommunicationWrapper* pVlp, double azimuth){ pVlp->SetAzimuth(azimuth); }

void SetTimeStamp(VLPCommunicationWrapper* pVlp, int timeStamp) { pVlp->SetTimeStamp(timeStamp); }

void SetChannel(VLPCommunicationWrapper* pVlp, double distance, short reflectivity) { pVlp->SetChannel(distance, reflectivity); }

void SendData(VLPCommunicationWrapper* pVlp) { pVlp->SetData(); }