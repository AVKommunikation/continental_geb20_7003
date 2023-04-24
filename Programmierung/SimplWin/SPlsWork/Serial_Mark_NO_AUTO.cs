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

namespace UserModule_SERIAL_MARK_NO_AUTO
{
    public class UserModuleClass_SERIAL_MARK_NO_AUTO : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        Crestron.Logos.SplusObjects.DigitalInput TRIG;
        Crestron.Logos.SplusObjects.DigitalInput CLEAR;
        Crestron.Logos.SplusObjects.StringInput IN1__DOLLAR__;
        Crestron.Logos.SplusObjects.StringInput IN2__DOLLAR__;
        Crestron.Logos.SplusObjects.StringInput IN3__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput OUT__DOLLAR__;
        object TRIG_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 43;
                OUT__DOLLAR__  .UpdateValue ( IN1__DOLLAR__ + IN2__DOLLAR__ + IN3__DOLLAR__  ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object CLEAR_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 48;
            OUT__DOLLAR__  .UpdateValue ( ""  ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
public override object FunctionMain (  object __obj__ ) 
    { 
    try
    {
        SplusExecutionContext __context__ = SplusFunctionMainStartCode();
        
        __context__.SourceCodeLine = 72;
        OUT__DOLLAR__  .UpdateValue ( ""  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    
    TRIG = new Crestron.Logos.SplusObjects.DigitalInput( TRIG__DigitalInput__, this );
    m_DigitalInputList.Add( TRIG__DigitalInput__, TRIG );
    
    CLEAR = new Crestron.Logos.SplusObjects.DigitalInput( CLEAR__DigitalInput__, this );
    m_DigitalInputList.Add( CLEAR__DigitalInput__, CLEAR );
    
    IN1__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( IN1__DOLLAR____AnalogSerialInput__, 255, this );
    m_StringInputList.Add( IN1__DOLLAR____AnalogSerialInput__, IN1__DOLLAR__ );
    
    IN2__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( IN2__DOLLAR____AnalogSerialInput__, 255, this );
    m_StringInputList.Add( IN2__DOLLAR____AnalogSerialInput__, IN2__DOLLAR__ );
    
    IN3__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( IN3__DOLLAR____AnalogSerialInput__, 255, this );
    m_StringInputList.Add( IN3__DOLLAR____AnalogSerialInput__, IN3__DOLLAR__ );
    
    OUT__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( OUT__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( OUT__DOLLAR____AnalogSerialOutput__, OUT__DOLLAR__ );
    
    
    TRIG.OnDigitalPush.Add( new InputChangeHandlerWrapper( TRIG_OnPush_0, false ) );
    CLEAR.OnDigitalPush.Add( new InputChangeHandlerWrapper( CLEAR_OnPush_1, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_SERIAL_MARK_NO_AUTO ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint TRIG__DigitalInput__ = 0;
const uint CLEAR__DigitalInput__ = 1;
const uint IN1__DOLLAR____AnalogSerialInput__ = 0;
const uint IN2__DOLLAR____AnalogSerialInput__ = 1;
const uint IN3__DOLLAR____AnalogSerialInput__ = 2;
const uint OUT__DOLLAR____AnalogSerialOutput__ = 0;

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
