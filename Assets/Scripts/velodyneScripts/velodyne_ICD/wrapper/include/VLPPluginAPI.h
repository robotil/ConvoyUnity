
#include "VLPCommunicationWrapper.h"

// Functions that can be used by the plugin
extern "C" {
    VLPCommunicationWrapper* CreateVLPObject(short ip1, short ip2, short ip3, short ip4, int port, int resolution,
        int returnMode, int dataSource, int sensorFrequency, int velType);

    void DeleteVLPObject(VLPCommunicationWrapper* pVlp);

    void Run(VLPCommunicationWrapper* pVlp);

    void SetAzimuth(VLPCommunicationWrapper* pVlp, double azimuth);

    void SetTimeStamp(VLPCommunicationWrapper* pVlp, int timeStamp);

    void SetChannel(VLPCommunicationWrapper* pVlp, double distance, short reflectivity);

    void SendData(VLPCommunicationWrapper* pVlp);
}