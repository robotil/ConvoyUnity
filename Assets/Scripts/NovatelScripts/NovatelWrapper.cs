
using System;
using System.Runtime.InteropServices;

public class NovatelWrapper : IDisposable {
	const String DLL_LOCATION = "libnovatel";

	[DllImport (DLL_LOCATION)]
	private static extern IntPtr NovatelCreateObject(string confFilePath);

	[DllImport (DLL_LOCATION)]
	private static extern void NovatelDeleteObject(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void NovatelSendBestPosData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void NovatelSendBestVelData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void NovatelSetPosition(IntPtr pObj, double latitude, double longitude, double altitude);
	
	[DllImport (DLL_LOCATION)]
	private static extern void NovatelSetVelocities(IntPtr pObj, double latSpeed, double longSpeed, double altAzimuth);

	[DllImport (DLL_LOCATION)]
	private static extern void NovatelSetTimeStamp(IntPtr pObj, float timeStamp);

	private IntPtr m_nativeObject;

	public NovatelWrapper(string confFilePath) {
		this.m_nativeObject = NovatelCreateObject(confFilePath);
	}

	~NovatelWrapper() {Dispose(false);}
	
	public void Dispose() { Dispose(true);}

    protected virtual void Dispose(bool bDisposing) {
        if (this.m_nativeObject != IntPtr.Zero) {
            NovatelDeleteObject(this.m_nativeObject);
            this.m_nativeObject = IntPtr.Zero;
        }

        if (bDisposing) {
            GC.SuppressFinalize(this);
        }
    }

	public void SetPosition(double latitude, double longitude, double altitude) {
		NovatelSetPosition(this.m_nativeObject, latitude, longitude, altitude);
	}

	public void SetVelocities(double latSpeed, double longSpeed, double altAzimuth) {
		NovatelSetVelocities(this.m_nativeObject, latSpeed, longSpeed, altAzimuth);
	}

	public void SetTimeStamp(float timeStamp) {
		NovatelSetTimeStamp(this.m_nativeObject, timeStamp);
	}

	public void SendBestPosData() {
		NovatelSendBestPosData(this.m_nativeObject);
	}

	public void SendBestVelData() {
		NovatelSendBestVelData(this.m_nativeObject);
	}
}