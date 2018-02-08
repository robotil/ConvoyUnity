
using System;
using System.Runtime.InteropServices;

public class VelodyneWrapper : IDisposable {
	const String DLL_LOCATION = "libvlp";

	[DllImport (DLL_LOCATION)]
	private static extern IntPtr CreateVLPObject(string confFilePath);

	[DllImport (DLL_LOCATION)]
	private static extern void DeleteVLPObject(IntPtr pVlp);

	[DllImport (DLL_LOCATION)]
	private static extern void RunVLP(IntPtr pVlp);

	[DllImport (DLL_LOCATION)]
	private static extern void SetAzimuth(IntPtr pVlp, double azimuth);

	[DllImport (DLL_LOCATION)]
	private static extern void SetVLPTimeStamp(IntPtr pVlp, int timeStamp);

	[DllImport (DLL_LOCATION)]
	private static extern void SetChannel(IntPtr pVlp, double distance, short reflectivity);

	[DllImport (DLL_LOCATION)]
	private static extern void SendVLPData(IntPtr pVlp);

	private IntPtr m_nativeObject;

	public VelodyneWrapper(string confFilePath) {
			this.m_nativeObject = CreateVLPObject(confFilePath);
	}

	~VelodyneWrapper() {Dispose(false);}
	
	public void Dispose() { Dispose(true);}

    protected virtual void Dispose(bool bDisposing) {
        if (this.m_nativeObject != IntPtr.Zero) {
            //DeleteVLPObject(this.m_nativeObject);
             //this.m_nativeObject = IntPtr.Zero;
        }

        if (bDisposing) {
            GC.SuppressFinalize(this);
        }
    }

	public void Run() {
		RunVLP(this.m_nativeObject);
	}

	public void SetAzimuth(double azimuth) {
		SetAzimuth(this.m_nativeObject, azimuth);
	}

	public void SetTimeStamp(int timeStamp) {
		SetVLPTimeStamp(this.m_nativeObject, timeStamp);
	}

	public void SetChannel(double distance, short reflectivity) {
		SetChannel(this.m_nativeObject, distance, reflectivity);
	}

	public void SendData() {
		SendVLPData(this.m_nativeObject);
	}
}