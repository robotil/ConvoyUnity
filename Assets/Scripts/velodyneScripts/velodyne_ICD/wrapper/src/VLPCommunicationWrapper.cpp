
#include "VLPCommunicationWrapper.h"
#include "VLPCommunication16.h"
#include "VLPCommunication32.h"

VLPCommunicationWrapper::VLPCommunicationWrapper(const std::string& ipAddress, const std::string& port, int resolution,
        int returnMode, int dataSource, int sensorFrequency, int velType) {
    VLPCommunication::VLPConfig vlpConfig(ipAddress, port, VLPCommunication::Resolution(resolution), VLPCommunication::ReturnMode(returnMode),
        VLPCommunication::DataSource(dataSource), sensorFrequency);
    // create the required object
    if (velType == 16) {
        m_vlp = new VLPCommunication16(vlpConfig);
    }
    else {
        m_vlp = new VLPCommunication32(vlpConfig);
    }
}

VLPCommunicationWrapper::~VLPCommunicationWrapper(){
    delete m_vlp;
}

void VLPCommunicationWrapper::Run() {
    m_vlp->Run();
}

void VLPCommunicationWrapper::SetData() {
    // assign the data, create vector in size 1 to adapt the API and clear the current data
    m_data.m_channels = m_currChannels;
    std::vector<VLPCommunication::VLPData> data(1, m_data);
    m_vlp->SetData(data);
    ClearCurrentData();
}

void VLPCommunicationWrapper::SetAzimuth(double azimuth){
     m_data.m_azimuth = azimuth;
}

void VLPCommunicationWrapper::SetTimeStamp(int timeStamp) {
     m_data.m_durationAfterLastHour = boost::posix_time::microseconds(timeStamp);;
}

void VLPCommunicationWrapper::SetChannel(double distance, short reflectivity) {
    m_currChannels.push_back(std::pair<double, short>(distance, reflectivity));
}

void VLPCommunicationWrapper::ClearCurrentData() {
    m_currChannels.clear();
    m_data.m_channels.clear();
    m_data.m_azimuth = 0;
    m_data.m_durationAfterLastHour = boost::posix_time::microseconds(0);
}
