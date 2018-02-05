
using System;
using System.Runtime.InteropServices;

public class DgpsWrapper : IDisposable {
	const String DLL_LOCATION = "libdgps";

	[DllImport (DLL_LOCATION)]
	private static extern IntPtr CreateDgpsObject(string portName, int baudRate);

	[DllImport (DLL_LOCATION)]
	private static extern void DeleteDgpsObject(IntPtr pDgps);

	[DllImport (DLL_LOCATION)]
	private static extern void RunDgps(IntPtr pDgps);

	[DllImport (DLL_LOCATION)]
	private static extern void SendDgpsData(IntPtr pDgps);

	[DllImport (DLL_LOCATION)]
	private static extern void SetPosition(IntPtr pDgps, double latitude, double longitude, double altitude);
	
	[DllImport (DLL_LOCATION)]
	private static extern void SetVelocities(IntPtr pDgps, double latSpeed, double longSpeed, double altAzimuth);

	[DllImport (DLL_LOCATION)]
	private static extern void SetDgpsTimeStamp(IntPtr pDgps, int timeStamp);

	private IntPtr m_nativeObject;

	public DgpsWrapper(string portName, int baudRate) {
		this.m_nativeObject = CreateDgpsObject(portName, baudRate);
	}

	~DgpsWrapper() {Dispose(false);}
	
	public void Dispose() { Dispose(true);}

    protected virtual void Dispose(bool bDisposing) {
        if (this.m_nativeObject != IntPtr.Zero) {
            //DeleteDgpsObject(this.m_nativeObject);
             //this.m_nativeObject = IntPtr.Zero;
        }

        if (bDisposing) {
            GC.SuppressFinalize(this);
        }
    }

	public void Run() {
		RunDgps(this.m_nativeObject);
	}

	public void SetPosition(double latitude, double longitude, double altitude) {
		SetPosition(this.m_nativeObject, latitude, longitude, altitude);
	}

	public void SetVelocities(double latSpeed, double longSpeed, double altAzimuth) {
		SetVelocities(this.m_nativeObject, latSpeed, longSpeed, altAzimuth);
	}

	public void SetTimeStamp(int timeStamp) {
		SetDgpsTimeStamp(this.m_nativeObject, timeStamp);
	}

	public void SendData() {
		SendDgpsData(this.m_nativeObject);
	}
}