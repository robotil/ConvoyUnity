using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class IdanVehicleControl : MonoBehaviour  
{

	IdanWrapper IdanInterface;
	public string ICD_ConfigFile = "/home/robil/simConfigs/idan.conf";


    Vehicle Oshkosh;

    public bool IsEngenOn = false,  KeySwitchApplied = false, MotorStarterApplied = false;   // Vehicle State Machine 
    public string GearStateRquesrt; public int GearState;
    public bool ParkingBrakeRwquest, ParkingBrakeState;

	public bool ShutDownCmd = false, EmergencyCmd = false;	
	public float SteerCmd = 0, SteerState = 0;
    public float GasCmd = 0, GasState =0;
    public float EngineRPM = 0, EngineTemp=30;


    float IdanPrimFreq = 100, IdanSecoFreq = 10, IdanSensFreq = 1;

  	float throttleCommand, BreakCommand, steeringCommand;
    int GearCommand;


    public Text displayText;

// ********************************************************** //

    // Use this for initialization
    void Start()
    {  
        Oshkosh = GetComponent<Vehicle>();

  		IdanInterface = new IdanWrapper(ICD_ConfigFile);
        //Invokes the method methodName in time seconds, then repeatedly every repeatRate seconds.
        InvokeRepeating("UpdateIdanPrimary", 3.0f, 1/IdanPrimFreq);
		InvokeRepeating("UpdateIdanSecondary", 3.0f, 1/IdanSecoFreq);
        InvokeRepeating("UpdateIdanSecondarySensors", 3.0f, 1/IdanSensFreq);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ApplyEngineAndGearLogic();

        steeringCommand = SteerCmd;

        if (GasCmd > 0) 
        {
    		throttleCommand = GasCmd; 
            BreakCommand = 0.0f;
        }  
        else
        {
            throttleCommand = 0.0f; 
            BreakCommand = GasCmd;
        }


        Oshkosh.throttleCommand = throttleCommand;
        Oshkosh.BreakCommand = BreakCommand;
        Oshkosh.steeringCommand = steeringCommand;
        Oshkosh.Gear = GearCommand;
    }


void ApplyEngineAndGearLogic() {


        if ((GasCmd > 0) && (EngineRPM > 0)) {
            EngineRPM = 3000 * GasCmd + 800;
        }
        else if (IsEngenOn && (GasCmd <= 0)) {
            EngineRPM = 800;
        }


        if (KeySwitchApplied && MotorStarterApplied) 
        {
            EngineRPM = 850;
            IsEngenOn = true;
        }
        else if (!KeySwitchApplied) {
            EngineRPM = 0;
            IsEngenOn = false;
        }



         GasState = GasCmd;
         SteerState = SteerCmd;


        switch (GearStateRquesrt)
        {
            case "R":   GearCommand = -1; break;
            case "N":   GearCommand = 0; break;
            case "DR":  GearCommand = 1; break;
            case "D1":  GearCommand = 1; break;
            case "D2":  GearCommand = 2; break;
            case "D3":  GearCommand = 3; break;
            case "D4":  GearCommand = 4; break;
            case "D5":  GearCommand = 5; break;
        }



        
        ParkingBrakeState = ParkingBrakeRwquest; 


        if (IsEngenOn) 
            { EngineTemp = 95.0f; }
        else 
            { EngineTemp = 30.0f; }

}


/************************************************* IDAN Primary ********************************************/
    void UpdateIdanPrimary() {

        IdanInterface.ReceiveData();

        ShutDownCmd  = IdanInterface.HasHLCPShutDownCmd();
        EmergencyCmd = IdanInterface.HasHLCPEmergencyCmd(); 
        SteerCmd = -IdanInterface.GetHLCPSteerCmd();  // range [-1, 1]
        GasCmd = IdanInterface.GetHLCPGasCmd();       // range [-1, 1]

           
        IdanInterface.SetIdanPrimSteerPos(SteerState);
	    IdanInterface.SetIdanPrimGasPos(GasState);

        IdanInterface.SendIdanPrimaryData();
    }



/********************************* HLC Secondary ***********************************************************/
    void UpdateIdanSecondary() {

        IdanInterface.ReceiveData();

        IdanInterface.SetIdanSecRepRoadLights(IdanInterface.IsHLCSRoadLightsApplied());
        IdanInterface.SetIdanSecRepHighBeam(IdanInterface.IsHLCSHighBeamApplied());
        IdanInterface.SetIdanSecRepLightsCutoff(IdanInterface.IsHLCSLightsCutoffApplied());
        IdanInterface.SetIdanSecRepHorn(IdanInterface.IsHLCSHornApplied());
        IdanInterface.SetIdanSecRepLeftTurnSignal(IdanInterface.IsHLCSLeftTurnSignalApplied());
        IdanInterface.SetIdanSecRepRightTurnSignal(IdanInterface.IsHLCSRightTurnSignalApplied());
        IdanInterface.SetIdanSecRepHazards(IdanInterface.IsHLCSHazardsApplied());
        

        KeySwitchApplied = IdanInterface.IsHLCSKeySwitchApplied();
        MotorStarterApplied = IdanInterface.IsHLCSMotorStarterApplied(); 

 
        IdanInterface.SetIdanSecRepKeySwitch(KeySwitchApplied);
        IdanInterface.SetIdanSecRepMotorStarter(MotorStarterApplied);
        IdanInterface.SetIdanSecRepRpm(EngineRPM);


        GearStateRquesrt = IdanInterface.GetHLCSGear();
        displayText.text = "gasCMD= " + GasCmd + "     steerCMD= " + SteerCmd + "   GearRequest: " + GearStateRquesrt;

        string gear= GearStateRquesrt;
        if (GearStateRquesrt == "DR") {
            gear = "D1";
        }

        IdanInterface.SetIdanSecRepRequestedGear(gear);
        IdanInterface.SetIdanSecRepActualGear(gear);

        ParkingBrakeRwquest = IdanInterface.IsHLCSParkingBrakeReleased();
        IdanInterface.SetIdanSecRepParkingBrake(ParkingBrakeState ? "R" : "E");


        IdanInterface.SetIdanSecRepVelocity(0);

        IdanInterface.SendIdanSecondaryReportData();
    }

// report car data
    void UpdateIdanSecondarySensors() {

        IdanInterface.SetIdanSecSenEngineTemp(EngineTemp);
        IdanInterface.SetIdanSecSenOilPress(50.0f);
        IdanInterface.SetIdanSecSenFuelLevel(100.0f);
        IdanInterface.SetIdanSecSenAlternatorVoltage(25.2f);
        IdanInterface.SetIdanSecSenBackupBattVoltage(25.2f);
        IdanInterface.SetIdanSecSenBatterySumUp(0);
        IdanInterface.SetIdanSecSenAirPressFront(100.0f);
        IdanInterface.SetIdanSecSenAirPressRear(100.0f);

        IdanInterface.SendIdanSecondarySensorData();

    }

}