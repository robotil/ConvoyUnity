
#include "VLPCommunication.h"

/**
 * VLPCommunicationWrapper holds a pointer to VLPCommunication, and temporary data.
 * Once the object is created - the pointer is assigned (new).
 * m_data holds the temporary data of the user, and m_currChannles holds the temporary channels that the user saved.
 * When the user calls "SetData" - we take the data that we aggregated until now and send it (set it) to VLP pointer.
 * We clear the temporary data right after the send
 * */
class VLPCommunicationWrapper {
private:
    /**
     *  Pointer to the native object
     */
    VLPCommunication* m_vlp;
    /**
     * temporary data to aggregate
     */ 
    VLPCommunication::VLPData m_data;
    /**
     * temporary channels to aggregate
     */ 
    VLPCommunication::t_channel_data m_currChannels;

    /**
     * Clear current data of the object
     */ 
    void ClearCurrentData();

public:
    VLPCommunicationWrapper(const std::string& ipAddress, const std::string& port, int resolution,
        int returnMode, int dataSource, int sensorFrequency, int velType);

    ~VLPCommunicationWrapper();

    /**
     * Calls vlp->run
     */ 
    void Run();

    /**
     * Take the temporary data and set it to vlp
     */ 
    void SetData();

    /**
     * Set azimuth to the temporary data
     */ 
    void SetAzimuth(double azimuth);

    /**
     * Set time stamp to the temporary data
     */ 
    void SetTimeStamp(int timeStamp);

    /**
     * Set channel to temporary channels
     */ 
    void SetChannel(double distance, short reflectivity);

};