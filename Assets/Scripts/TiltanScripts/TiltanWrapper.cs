
using System;
using System.Runtime.InteropServices;

public class TiltanWrapper : IDisposable {
	const String DLL_LOCATION = "libtiltan";

	[DllImport (DLL_LOCATION)]
	private static extern IntPtr TiltanCreateObject(string confFilePath);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanDeleteObject(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSendStatusMsgData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSendInternalGPSData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSendNavigationData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSendErrorEstimationData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetTimeStamps(IntPtr pObj, float simTime, float utcTime);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetPose(IntPtr pObj, float latitude, float longitude, float altitude);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetOrientation(IntPtr pObj, float azimuth, float pitch, float roll);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetAzimuthRate(IntPtr pObj, float azimuthRate);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetVelocity(IntPtr pObj, float northVelocity, float eastVelocity, float downVelocity);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetDistances(IntPtr pObj, float distanceTraveled, float odometerDistance);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetMotionDetected(IntPtr pObj, bool motionDetected);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetInternalGpsFields(IntPtr pObj, short gpsFom, short numOfSatelites);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetDirectionErrors(IntPtr pObj, float horizontalError, float verticalError, float northingError,
                             float eastingError, float altitudeError);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetVelocityErrors(IntPtr pObj, float northVelocityError, float eastVelocityError, float downVelocityError);

	[DllImport (DLL_LOCATION)]
	private static extern void TiltanSetOrientationErrors(IntPtr pObj, float azimuthErrorEstimation, float pitchErrorEstimation, float rollErrorEstimation);

	private IntPtr m_nativeObject;

	public TiltanWrapper(string confFilePath) {
		this.m_nativeObject = TiltanCreateObject(confFilePath);
	}

	~TiltanWrapper() {Dispose(false);}
	
	public void Dispose() { Dispose(true);}

    protected virtual void Dispose(bool bDisposing) {
        if (this.m_nativeObject != IntPtr.Zero) {
            TiltanDeleteObject(this.m_nativeObject);
            this.m_nativeObject = IntPtr.Zero;
        }

        if (bDisposing) {
            GC.SuppressFinalize(this);
        }
    }

	public void SendStatusMsgData() {
		TiltanSendStatusMsgData(this.m_nativeObject);
	}

	public void SendInternalGPSData() {
		TiltanSendInternalGPSData(this.m_nativeObject);
	}

	public void SendNavigationData() {
		TiltanSendNavigationData(this.m_nativeObject);
	}

	public void SendErrorEstimationData() {
		TiltanSendErrorEstimationData(this.m_nativeObject);
	}

	public void SetTimeStamps(float simTime, float utcTime) {
		TiltanSetTimeStamps(this.m_nativeObject, simTime, utcTime);
	}

    // Tiltan navigation data message
    public void SetPose(float latitude, float longitude, float altitude) {
		TiltanSetPose(this.m_nativeObject, latitude, longitude, altitude);
	}

    public void SetOrientation(float azimuth, float pitch, float roll) {
		TiltanSetOrientation(this.m_nativeObject, azimuth, pitch, roll);
	}

    public void SetAzimuthRate(float azimuthRate) {
		TiltanSetAzimuthRate(this.m_nativeObject, azimuthRate);
	}

    public void SetVelocity(float northVelocity, float eastVelocity, float downVelocity) {
		TiltanSetVelocity(this.m_nativeObject, northVelocity, eastVelocity, downVelocity);
	}

    public void SetDistances(float distanceTraveled, float odometerDistance) {
		TiltanSetDistances(this.m_nativeObject, distanceTraveled, odometerDistance);
	}

    public void SetMotionDetected(bool motionDetected) {
		TiltanSetMotionDetected(this.m_nativeObject, motionDetected);
	}

    // Tiltan internal GPS
    public void SetInternalGpsFields(short gpsFom, short numOfSatelites) {
		TiltanSetInternalGpsFields(this.m_nativeObject, gpsFom, numOfSatelites);
	}

    // Tiltan Errors estimation message
    public void SetDirectionErrors(float horizontalError, float verticalError, float northingError,
                             float eastingError, float altitudeError) {
		TiltanSetDirectionErrors(this.m_nativeObject, horizontalError, verticalError, northingError, eastingError, altitudeError);
	}

    public void SetVelocityErrors(float northVelocityError, float eastVelocityError, float downVelocityError) {
		TiltanSetVelocityErrors(this.m_nativeObject, northVelocityError, eastVelocityError, downVelocityError);
	}

    public void SetOrientationErrors(float azimuthErrorEstimation, float pitchErrorEstimation, float rollErrorEstimation) {
		TiltanSetOrientationErrors(this.m_nativeObject, azimuthErrorEstimation, pitchErrorEstimation, rollErrorEstimation);
	}
}