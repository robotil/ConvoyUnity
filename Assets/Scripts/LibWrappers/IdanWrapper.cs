
using System;
using System.Runtime.InteropServices;

public class IdanWrapper: IDisposable {
	const String DLL_LOCATION = "libidan";
	
	[DllImport (DLL_LOCATION)]
	private static extern IntPtr IdanCreateObject(string confFilePath);

	[DllImport (DLL_LOCATION)]
	private static extern void IdanDeleteObject(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void SendIdanPrimaryData(IntPtr pObj);

    [DllImport (DLL_LOCATION)]
	private static extern void SendIdanSecondaryReportData(IntPtr pObj);

    [DllImport (DLL_LOCATION)]
	private static extern void SendIdanSecondarySensorData(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern void IdanReceiveData(IntPtr pObj);

/***************************************************** HLC Primary *********************************************** */
	[DllImport (DLL_LOCATION)]
	private static extern bool HasHLCPShutDownCmd(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool HasHLCPEmergencyCmd(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern float GetHLCPSteerCmd(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern float GetHLCPGasCmd(IntPtr pObj);

/********************************* HLC Secondary ***********************************************************/
	[DllImport (DLL_LOCATION)]
	private static extern bool HasHLCSShutDownCmd(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSRoadLightsApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSHighBeamApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSLightsCutoffApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSParkingLightApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSKeySwitchApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSMotorStarterApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSHornApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSLeftTurnSignalApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSRightTurnSignalApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSHazardsApplied(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern string GetHLCSGear(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool IsHLCSParkingBrakeReleased(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool HasHLCSEmergencyCmd(IntPtr pObj);

	[DllImport (DLL_LOCATION)]
	private static extern bool HasHLCSSacsOnCmd(IntPtr pObj);

   /************************************************* IDAN Primary ********************************************/
    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanPrimSteerPos(IntPtr pObj, float steerPose);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanPrimGasPos(IntPtr pObj, float gasPose);

    /************************************************* IDAN Secondary Report ********************************************/
	
    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepRoadLights(IntPtr pObj, bool roadLights); 

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepHighBeam(IntPtr pObj, bool highBeam);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepLightsCutoff(IntPtr pObj, bool lightsCutoff) ;

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepKeySwitch(IntPtr pObj, bool keySwitch);

	[DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepMotorStarter(IntPtr pObj, bool motorStarter);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepHorn(IntPtr pObj, bool horn);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepLeftTurnSignal(IntPtr pObj, bool leftTurnSignal);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepRightTurnSignal(IntPtr pObj, bool rightTurnSignal);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepHazards(IntPtr pObj, bool hazards);

	// gear map: "R"= reverse, "N" = neutral, D1, D2, D3, D4, D5, "PROG"= in progress
    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepRequestedGear(IntPtr pObj, string requestedGear);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepActualGear(IntPtr pObj, string actualGear);

	// parking map: "R"= released, "E" = engaged, "P" = in progress
    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepParkingBrake(IntPtr pObj, string parkingBrake);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepRpm(IntPtr pObj, float rpm);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecRepVelocity(IntPtr pObj, float velocity);

    /************************************************* IDAN Secondary Sensor ********************************************/
    
    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenEngineTemp(IntPtr pObj, float engineTemp);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenOilPress(IntPtr pObj, float oilPress);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenFuelLevel(IntPtr pObj, float fuelLevel);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenAlternatorVoltage(IntPtr pObj, float alternatorVoltage);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenBackupBattVoltage(IntPtr pObj, float backupBattVoltage);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenBatterySumUp(IntPtr pObj, int batterySumUp);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenAirPressFront(IntPtr pObj, float airPressFront);

    [DllImport (DLL_LOCATION)]
	private static extern void SetIdanSecSenAirPressRear(IntPtr pObj, float airPressRear);

	private IntPtr m_nativeObject;

	public IdanWrapper(string confFilePath) {
		this.m_nativeObject = IdanCreateObject(confFilePath);
	}

	~IdanWrapper() {Dispose(false);}
	
	public void Dispose() { Dispose(true);}

    protected virtual void Dispose(bool bDisposing) {
        if (this.m_nativeObject != IntPtr.Zero) {
            IdanDeleteObject(this.m_nativeObject);
            this.m_nativeObject = IntPtr.Zero;
        }

        if (bDisposing) {
            GC.SuppressFinalize(this);
        }
    }

	public void SendIdanPrimaryData() {
		SendIdanPrimaryData(this.m_nativeObject);
	}

	public void SendIdanSecondaryReportData() {
		SendIdanSecondaryReportData(this.m_nativeObject);
	}

	public void SendIdanSecondarySensorData() {
		SendIdanSecondarySensorData(this.m_nativeObject);
	}

	public void ReceiveData() {
		IdanReceiveData(this.m_nativeObject);
	}

/***************************************************** HLC Primary *********************************************** */
	public bool HasHLCPShutDownCmd() {
		return HasHLCPShutDownCmd(this.m_nativeObject);
	}

	public bool HasHLCPEmergencyCmd() {
		return HasHLCPEmergencyCmd(this.m_nativeObject);
	}

	public float GetHLCPSteerCmd() {
		return GetHLCPSteerCmd(this.m_nativeObject);
	}

	public float GetHLCPGasCmd() {
		return GetHLCPGasCmd(this.m_nativeObject);
	}

/********************************* HLC Secondary ***********************************************************/
	public bool HasHLCSShutDownCmd() {
		return HasHLCSShutDownCmd(this.m_nativeObject);
	}

	public bool IsHLCSRoadLightsApplied() {
		return IsHLCSRoadLightsApplied(this.m_nativeObject);
	}

	public bool IsHLCSHighBeamApplied() {
		return IsHLCSHighBeamApplied(this.m_nativeObject);
	}

	public bool IsHLCSLightsCutoffApplied() {
		return IsHLCSLightsCutoffApplied(this.m_nativeObject);
	}

	public bool IsHLCSParkingLightApplied() {
		return IsHLCSParkingLightApplied(this.m_nativeObject);
	}

	public bool IsHLCSKeySwitchApplied() {
		return IsHLCSKeySwitchApplied(this.m_nativeObject);
	}

	public bool IsHLCSMotorStarterApplied() {
		return IsHLCSMotorStarterApplied(this.m_nativeObject);
	}

	public bool IsHLCSHornApplied() {
		return IsHLCSHornApplied(this.m_nativeObject);
	}

	public bool IsHLCSLeftTurnSignalApplied() {
		return IsHLCSLeftTurnSignalApplied(this.m_nativeObject);
	}

	public bool IsHLCSRightTurnSignalApplied() {
		return IsHLCSRightTurnSignalApplied(this.m_nativeObject);
	}

	public bool IsHLCSHazardsApplied() {
		return IsHLCSHazardsApplied(this.m_nativeObject);
	}

	public string GetHLCSGear() {
		return GetHLCSGear(this.m_nativeObject);
	}

	public bool IsHLCSParkingBrakeReleased() {
		return IsHLCSParkingBrakeReleased(this.m_nativeObject);
	}

	public bool HasHLCSEmergencyCmd() {
		return HasHLCSEmergencyCmd(this.m_nativeObject);
	}

	public bool HasHLCSSacsOnCmd() {
		return HasHLCSSacsOnCmd(this.m_nativeObject);
	}

   /************************************************* IDAN Primary ********************************************/
    public void SetIdanPrimSteerPos(float steerPose) {
		SetIdanPrimSteerPos(this.m_nativeObject, steerPose);
	}

	public void SetIdanPrimGasPos(float gasPose) {
		SetIdanPrimGasPos(this.m_nativeObject, gasPose);
	}

    /************************************************* IDAN Secondary Report ********************************************/
	public void SetIdanSecRepRoadLights(bool roadLights) {
		SetIdanSecRepRoadLights(this.m_nativeObject, roadLights);
	}

	public void SetIdanSecRepHighBeam(bool highBeam) {
		SetIdanSecRepHighBeam(this.m_nativeObject, highBeam);
	}

	public void SetIdanSecRepLightsCutoff(bool lightsCutoff) {
		SetIdanSecRepLightsCutoff(this.m_nativeObject, lightsCutoff);
	}

	public void SetIdanSecRepKeySwitch(bool keySwitch) {
		SetIdanSecRepKeySwitch(this.m_nativeObject, keySwitch);
	}

	public void SetIdanSecRepMotorStarter(bool motorStarter) {
		SetIdanSecRepMotorStarter(this.m_nativeObject, motorStarter);
	}

	public void SetIdanSecRepHorn(bool horn) {
		SetIdanSecRepHorn(this.m_nativeObject, horn);
	}

	public void SetIdanSecRepLeftTurnSignal(bool leftTurnSignal) {
		SetIdanSecRepLeftTurnSignal(this.m_nativeObject, leftTurnSignal);
	}

	public void SetIdanSecRepRightTurnSignal(bool rightTurnSignal) {
		SetIdanSecRepRightTurnSignal(this.m_nativeObject, rightTurnSignal);
	}

	public void SetIdanSecRepHazards(bool hazards) {
		SetIdanSecRepHazards(this.m_nativeObject, hazards);
	}

	public void SetIdanSecRepRequestedGear(string requestedGear) {
		SetIdanSecRepRequestedGear(this.m_nativeObject, requestedGear);
	}

	public void SetIdanSecRepActualGear(string actualGear) {
		SetIdanSecRepActualGear(this.m_nativeObject, actualGear);
	}

	public void SetIdanSecRepParkingBrake(string parkingBrake) {
		SetIdanSecRepParkingBrake(this.m_nativeObject, parkingBrake);
	}

	public void SetIdanSecRepRpm(float rpm) {
		SetIdanSecRepRpm(this.m_nativeObject, rpm);
	}

	public void SetIdanSecRepVelocity(float velocity) {
		SetIdanSecRepVelocity(this.m_nativeObject, velocity);
	}

    /************************************************* IDAN Secondary Sensor ********************************************/
    
    public void SetIdanSecSenEngineTemp(float engineTemp) {
		SetIdanSecSenEngineTemp(this.m_nativeObject, engineTemp);
	}

    public void SetIdanSecSenOilPress(float oilPress) {
		SetIdanSecSenOilPress(this.m_nativeObject, oilPress);
	}

    public void SetIdanSecSenFuelLevel(float fuelLevel) {
		SetIdanSecSenFuelLevel(this.m_nativeObject, fuelLevel);
	}

    public void SetIdanSecSenAlternatorVoltage(float alternatorVoltage) {
		SetIdanSecSenAlternatorVoltage(this.m_nativeObject, alternatorVoltage);
	}

    public void SetIdanSecSenBackupBattVoltage(float backupBattVoltage) {
		SetIdanSecSenBackupBattVoltage(this.m_nativeObject, backupBattVoltage);
	}

    public void SetIdanSecSenBatterySumUp(int batterySumUp) {
		SetIdanSecSenBatterySumUp(this.m_nativeObject, batterySumUp);
	}

    public void SetIdanSecSenAirPressFront(float airPressFront) {
		SetIdanSecSenAirPressFront(this.m_nativeObject, airPressFront);
	}

    public void SetIdanSecSenAirPressRear(float airPressRear) {
		SetIdanSecSenAirPressRear(this.m_nativeObject, airPressRear);
	}
}