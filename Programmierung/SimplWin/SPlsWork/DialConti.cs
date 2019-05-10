using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Crestron;
using Crestron.Logos.SplusLibrary;
using Crestron.Logos.SplusObjects;
using Crestron.SimplSharp;

namespace UserModule_DIALCONTI
{
    public class UserModuleClass_DIALCONTI : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.DigitalInput INTERN;
        Crestron.Logos.SplusObjects.DigitalInput EXTERN;
        Crestron.Logos.SplusObjects.DigitalInput FRANKFURT;
        Crestron.Logos.SplusObjects.DigitalInput MUNCHEN;
        Crestron.Logos.SplusObjects.DigitalInput DIAL;
        Crestron.Logos.SplusObjects.DigitalInput DIAL_VC;
        Crestron.Logos.SplusObjects.DigitalInput ACTIVATE_SIP;
        Crestron.Logos.SplusObjects.DigitalInput ACTIVATE_H323;
        Crestron.Logos.SplusObjects.StringInput NUMBER;
        Crestron.Logos.SplusObjects.StringInput NUMBERVC;
        Crestron.Logos.SplusObjects.StringOutput TX__DOLLAR__;
        object DIAL_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 146;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (EXTERN  .Value == 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 147;
                    MakeString ( TX__DOLLAR__ , "10697603{0}", NUMBER ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 149;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (INTERN  .Value == 1))  ) ) 
                        { 
                        __context__.SourceCodeLine = 150;
                        MakeString ( TX__DOLLAR__ , "1{0}", NUMBER ) ; 
                        } 
                    
                    }
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object DIAL_VC_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 158;
            if ( Functions.TestForTrue  ( ( ACTIVATE_SIP  .Value)  ) ) 
                { 
                __context__.SourceCodeLine = 160;
                MakeString ( TX__DOLLAR__ , "{0}", NUMBERVC ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 162;
                if ( Functions.TestForTrue  ( ( ACTIVATE_H323  .Value)  ) ) 
                    { 
                    __context__.SourceCodeLine = 164;
                    MakeString ( TX__DOLLAR__ , "{0}", NUMBERVC ) ; 
                    } 
                
                }
            
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object FRANKFURT_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 170;
        MakeString ( TX__DOLLAR__ , "1069201725184") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object MUNCHEN_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 175;
        MakeString ( TX__DOLLAR__ , "1089204038490") ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}


public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    INTERN = new Crestron.Logos.SplusObjects.DigitalInput( INTERN__DigitalInput__, this );
    m_DigitalInputList.Add( INTERN__DigitalInput__, INTERN );
    
    EXTERN = new Crestron.Logos.SplusObjects.DigitalInput( EXTERN__DigitalInput__, this );
    m_DigitalInputList.Add( EXTERN__DigitalInput__, EXTERN );
    
    FRANKFURT = new Crestron.Logos.SplusObjects.DigitalInput( FRANKFURT__DigitalInput__, this );
    m_DigitalInputList.Add( FRANKFURT__DigitalInput__, FRANKFURT );
    
    MUNCHEN = new Crestron.Logos.SplusObjects.DigitalInput( MUNCHEN__DigitalInput__, this );
    m_DigitalInputList.Add( MUNCHEN__DigitalInput__, MUNCHEN );
    
    DIAL = new Crestron.Logos.SplusObjects.DigitalInput( DIAL__DigitalInput__, this );
    m_DigitalInputList.Add( DIAL__DigitalInput__, DIAL );
    
    DIAL_VC = new Crestron.Logos.SplusObjects.DigitalInput( DIAL_VC__DigitalInput__, this );
    m_DigitalInputList.Add( DIAL_VC__DigitalInput__, DIAL_VC );
    
    ACTIVATE_SIP = new Crestron.Logos.SplusObjects.DigitalInput( ACTIVATE_SIP__DigitalInput__, this );
    m_DigitalInputList.Add( ACTIVATE_SIP__DigitalInput__, ACTIVATE_SIP );
    
    ACTIVATE_H323 = new Crestron.Logos.SplusObjects.DigitalInput( ACTIVATE_H323__DigitalInput__, this );
    m_DigitalInputList.Add( ACTIVATE_H323__DigitalInput__, ACTIVATE_H323 );
    
    NUMBER = new Crestron.Logos.SplusObjects.StringInput( NUMBER__AnalogSerialInput__, 50, this );
    m_StringInputList.Add( NUMBER__AnalogSerialInput__, NUMBER );
    
    NUMBERVC = new Crestron.Logos.SplusObjects.StringInput( NUMBERVC__AnalogSerialInput__, 50, this );
    m_StringInputList.Add( NUMBERVC__AnalogSerialInput__, NUMBERVC );
    
    TX__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( TX__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( TX__DOLLAR____AnalogSerialOutput__, TX__DOLLAR__ );
    
    
    DIAL.OnDigitalPush.Add( new InputChangeHandlerWrapper( DIAL_OnPush_0, false ) );
    DIAL_VC.OnDigitalPush.Add( new InputChangeHandlerWrapper( DIAL_VC_OnPush_1, false ) );
    FRANKFURT.OnDigitalPush.Add( new InputChangeHandlerWrapper( FRANKFURT_OnPush_2, false ) );
    MUNCHEN.OnDigitalPush.Add( new InputChangeHandlerWrapper( MUNCHEN_OnPush_3, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_DIALCONTI ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint INTERN__DigitalInput__ = 0;
const uint EXTERN__DigitalInput__ = 1;
const uint FRANKFURT__DigitalInput__ = 2;
const uint MUNCHEN__DigitalInput__ = 3;
const uint DIAL__DigitalInput__ = 4;
const uint DIAL_VC__DigitalInput__ = 5;
const uint ACTIVATE_SIP__DigitalInput__ = 6;
const uint ACTIVATE_H323__DigitalInput__ = 7;
const uint NUMBER__AnalogSerialInput__ = 0;
const uint NUMBERVC__AnalogSerialInput__ = 1;
const uint TX__DOLLAR____AnalogSerialOutput__ = 0;

[SplusStructAttribute(-1, true, false)]
public class SplusNVRAM : SplusStructureBase
{

    public SplusNVRAM( SplusObject __caller__ ) : base( __caller__ ) {}
    
    
}

SplusNVRAM _SplusNVRAM = null;

public class __CEvent__ : CEvent
{
    public __CEvent__() {}
    public void Close() { base.Close(); }
    public int Reset() { return base.Reset() ? 1 : 0; }
    public int Set() { return base.Set() ? 1 : 0; }
    public int Wait( int timeOutInMs ) { return base.Wait( timeOutInMs ) ? 1 : 0; }
}
public class __CMutex__ : CMutex
{
    public __CMutex__() {}
    public void Close() { base.Close(); }
    public void ReleaseMutex() { base.ReleaseMutex(); }
    public int WaitForMutex() { return base.WaitForMutex() ? 1 : 0; }
}
 public int IsNull( object obj ){ return (obj == null) ? 1 : 0; }
}


}
