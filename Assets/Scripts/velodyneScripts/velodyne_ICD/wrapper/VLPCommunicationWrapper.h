
#include "../include/VLPCommunication.h"

class VLPCommunicationWrapper {
private:
    VLPCommunication* m_vlp;
    VLPCommunication::VLPData m_data;
    VLPCommunication::t_channel_data m_currChannels;
public:
    VLPCommunicationWrapper(const std::string& ipAddress, const std::string& port, int resolution,
        int returnMode, int dataSource, int sensorFrequency, int velType);

    ~VLPCommunicationWrapper();

    void Run();

    void SetData();

    void SetAzimuth(double azimuth);

    void SetTimeStamp(int timeStamp);

    void SetChannel(double distance, short reflectivity);

};