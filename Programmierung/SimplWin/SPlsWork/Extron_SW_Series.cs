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

namespace UserModule_EXTRON_SW_SERIES
{
    public class UserModuleClass_EXTRON_SW_SERIES : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        Crestron.Logos.SplusObjects.AnalogInput OUTPUT_1;
        Crestron.Logos.SplusObjects.BufferInput FROM_DEVICE;
        InOutArray<Crestron.Logos.SplusObjects.DigitalOutput> HDMI_DETECT;
        Crestron.Logos.SplusObjects.StringOutput TO_DEVICE;
        ushort RXOK = 0;
        CrestronString TRASH__DOLLAR__;
        object OUTPUT_1_OnChange_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 184;
                TO_DEVICE  .UpdateValue ( Functions.ItoA (  (int) ( OUTPUT_1  .UshortValue ) ) + "!"  ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object FROM_DEVICE_OnChange_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            CrestronString TEMPOUT__DOLLAR__;
            CrestronString TEMPSIG__DOLLAR__;
            TEMPOUT__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 25, this );
            TEMPSIG__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 25, this );
            
            ushort I = 0;
            ushort IN = 0;
            ushort CHAR = 0;
            
            
            __context__.SourceCodeLine = 196;
            I = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 198;
            if ( Functions.TestForTrue  ( ( RXOK)  ) ) 
                { 
                __context__.SourceCodeLine = 200;
                RXOK = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 201;
                while ( Functions.TestForTrue  ( ( Functions.Find( "\u000a" , FROM_DEVICE ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 203;
                    if ( Functions.TestForTrue  ( ( Functions.Find( "Sig" , FROM_DEVICE ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 205;
                        TRASH__DOLLAR__  .UpdateValue ( Functions.Remove ( "Sig" , FROM_DEVICE )  ) ; 
                        __context__.SourceCodeLine = 206;
                        TEMPSIG__DOLLAR__  .UpdateValue ( Functions.Remove ( "*" , FROM_DEVICE )  ) ; 
                        __context__.SourceCodeLine = 207;
                        TEMPSIG__DOLLAR__  .UpdateValue ( TEMPSIG__DOLLAR__ + "\u0020"  ) ; 
                        __context__.SourceCodeLine = 208;
                        while ( Functions.TestForTrue  ( ( Functions.Find( "\u0020" , TEMPSIG__DOLLAR__ ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 210;
                            I = (ushort) ( (I + 1) ) ; 
                            __context__.SourceCodeLine = 211;
                            TRASH__DOLLAR__  .UpdateValue ( Functions.Remove ( "\u0020" , TEMPSIG__DOLLAR__ )  ) ; 
                            __context__.SourceCodeLine = 212;
                            CHAR = (ushort) ( Functions.Atoi( Functions.Chr( (int)( Functions.GetC( TRASH__DOLLAR__ ) ) ) ) ) ; 
                            __context__.SourceCodeLine = 213;
                            HDMI_DETECT [ I]  .Value = (ushort) ( CHAR ) ; 
                            __context__.SourceCodeLine = 208;
                            } 
                        
                        } 
                    
                    __context__.SourceCodeLine = 218;
                    Functions.ClearBuffer ( FROM_DEVICE ) ; 
                    __context__.SourceCodeLine = 201;
                    } 
                
                __context__.SourceCodeLine = 220;
                RXOK = (ushort) ( 1 ) ; 
                } 
            
            
            
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
        
        __context__.SourceCodeLine = 269;
        RXOK = (ushort) ( 1 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    SocketInfo __socketinfo__ = new SocketInfo( 1, this );
    InitialParametersClass.ResolveHostName = __socketinfo__.ResolveHostName;
    _SplusNVRAM = new SplusNVRAM( this );
    TRASH__DOLLAR__  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 255, this );
    
    HDMI_DETECT = new InOutArray<DigitalOutput>( 8, this );
    for( uint i = 0; i < 8; i++ )
    {
        HDMI_DETECT[i+1] = new Crestron.Logos.SplusObjects.DigitalOutput( HDMI_DETECT__DigitalOutput__ + i, this );
        m_DigitalOutputList.Add( HDMI_DETECT__DigitalOutput__ + i, HDMI_DETECT[i+1] );
    }
    
    OUTPUT_1 = new Crestron.Logos.SplusObjects.AnalogInput( OUTPUT_1__AnalogSerialInput__, this );
    m_AnalogInputList.Add( OUTPUT_1__AnalogSerialInput__, OUTPUT_1 );
    
    TO_DEVICE = new Crestron.Logos.SplusObjects.StringOutput( TO_DEVICE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( TO_DEVICE__AnalogSerialOutput__, TO_DEVICE );
    
    FROM_DEVICE = new Crestron.Logos.SplusObjects.BufferInput( FROM_DEVICE__AnalogSerialInput__, 56, this );
    m_StringInputList.Add( FROM_DEVICE__AnalogSerialInput__, FROM_DEVICE );
    
    
    OUTPUT_1.OnAnalogChange.Add( new InputChangeHandlerWrapper( OUTPUT_1_OnChange_0, false ) );
    FROM_DEVICE.OnSerialChange.Add( new InputChangeHandlerWrapper( FROM_DEVICE_OnChange_1, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_EXTRON_SW_SERIES ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint OUTPUT_1__AnalogSerialInput__ = 0;
const uint FROM_DEVICE__AnalogSerialInput__ = 1;
const uint HDMI_DETECT__DigitalOutput__ = 0;
const uint TO_DEVICE__AnalogSerialOutput__ = 0;

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
