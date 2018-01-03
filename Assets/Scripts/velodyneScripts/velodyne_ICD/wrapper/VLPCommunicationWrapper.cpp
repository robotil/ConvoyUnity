
#include "VLPCommunicationWrapper.h"
#include "VLPCommunication16.h"
#include "VLPCommunication32.h"

VLPCommunicationWrapper::VLPCommunicationWrapper(const std::string& ipAddress, const std::string& port, int resolution,
        int returnMode, int dataSource, int sensorFrequency, int velType) {
    VLPCommunication::VLPConfig vlpConfig(ipAddress, port, VLPCommunication::Resolution(resolution), VLPCommunication::ReturnMode(returnMode),
        VLPCommunication::DataSource(dataSource), sensorFrequency);
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
    m_data.m_channels = m_currChannels;
    std::vector<VLPCommunication::VLPData> data(1, m_data);
    m_vlp->SetData(data);
    m_currChannels.clear();
}

void VLPCommunicationWrapper::SetAzimuth(double azimuth){
     m_data.m_azimuth = azimuth;
}

void VLPCommunicationWrapper::SetTimeStamp(int timeStamp) {
     boost::posix_time::time_duration td = boost::posix_time::microseconds(timeStamp);
     m_data.m_durationAfterLastHour = td;
}

void VLPCommunicationWrapper::SetChannel(double distance, short reflectivity) {
    m_currChannels.push_back(std::pair<double, short>(distance, reflectivity));
}



extern "C" {
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
}