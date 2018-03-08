
using System;
using System.Runtime.InteropServices;

public class InsWrapper : IDisposable {
	const String DLL_LOCATION = "libins";

	[DllImport (DLL_LOCATION)]
	private static extern IntPtr CreateInsObject(string confFilePath);

	[DllImport (DLL_LOCATION)]
	private static extern void DeleteInsObject(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void RunIns(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void SendInsData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsTimeStamps(IntPtr pObj, float simTime, float utcTime);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsPose(IntPtr pObj, float latitude, float longitude, float altitude);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsOrientation(IntPtr pObj, float azimuth, float pitch, float roll);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsAzimuthRate(IntPtr pObj, float azimuthRate);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsVelocity(IntPtr pObj, float northVelocity, float eastVelocity, float downVelocity);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsDistances(IntPtr pObj, float distanceTraveled, float odometerDistance);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsMotionDetected(IntPtr pObj, bool motionDetected);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsInternalGpsFields(IntPtr pObj, short gpsFom, short numOfSatelites);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsDirectionErrors(IntPtr pObj, float horizontalError, float verticalError, float northingError,
                             float eastingError, float altitudeError);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsVelocityErrors(IntPtr pObj, float northVelocityError, float eastVelocityError, float downVelocityError);

	[DllImport (DLL_LOCATION)]
	private static extern void SetInsOrientationErrors(IntPtr pObj, float azimuthErrorEstimation, float pitchErrorEstimation, float rollErrorEstimation);

	private IntPtr m_nativeObject;

	public InsWrapper(string confFilePath) {
		this.m_nativeObject = CreateInsObject(confFilePath);
	}

	~InsWrapper() {Dispose(false);}
	
	public void Dispose() { Dispose(true);}

    protected virtual void Dispose(bool bDisposing) {
        if (this.m_nativeObject != IntPtr.Zero) {
            //DeleteInsObject(this.m_nativeObject);
             //this.m_nativeObject = IntPtr.Zero;
        }

        if (bDisposing) {
            GC.SuppressFinalize(this);
        }
    }

	public void Run() {
		RunIns(this.m_nativeObject);
	}

	public void SendData() {
		SendInsData(this.m_nativeObject);
	}

	public void SetTimeStamps(float simTime, float utcTime) {
		SetInsTimeStamps(this.m_nativeObject, simTime, utcTime);
	}

    // INS navigation data message
    public void SetPose(float latitude, float longitude, float altitude) {
		SetInsPose(this.m_nativeObject, latitude, longitude, altitude);
	}

    public void SetOrientation(float azimuth, float pitch, float roll) {
		SetInsOrientation(this.m_nativeObject, azimuth, pitch, roll);
	}

    public void SetAzimuthRate(float azimuthRate) {
		SetInsAzimuthRate(this.m_nativeObject, azimuthRate);
	}

    public void SetVelocity(float northVelocity, float eastVelocity, float downVelocity) {
		SetInsVelocity(this.m_nativeObject, northVelocity, eastVelocity, downVelocity);
	}

    public void SetDistances(float distanceTraveled, float odometerDistance) {
		SetInsDistances(this.m_nativeObject, distanceTraveled, odometerDistance);
	}

    public void SetMotionDetected(bool motionDetected) {
		SetInsMotionDetected(this.m_nativeObject, motionDetected);
	}

    // INS internal GPS
    public void SetInternalGpsFields(short gpsFom, short numOfSatelites) {
		SetInsInternalGpsFields(this.m_nativeObject, gpsFom, numOfSatelites);
	}

    // INS Errors estimation message
    public void SetDirectionErrors(float horizontalError, float verticalError, float northingError,
                             float eastingError, float altitudeError) {
		SetInsDirectionErrors(this.m_nativeObject, horizontalError, verticalError, northingError, eastingError, altitudeError);
	}

    public void SetVelocityErrors(float northVelocityError, float eastVelocityError, float downVelocityError) {
		SetInsVelocityErrors(this.m_nativeObject, northVelocityError, eastVelocityError, downVelocityError);
	}

    public void SetOrientationErrors(float azimuthErrorEstimation, float pitchErrorEstimation, float rollErrorEstimation) {
		SetInsOrientationErrors(this.m_nativeObject, azimuthErrorEstimation, pitchErrorEstimation, rollErrorEstimation);
	}
}