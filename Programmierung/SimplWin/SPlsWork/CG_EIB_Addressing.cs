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

namespace UserModule_CG_EIB_ADDRESSING
{
    public class UserModuleClass_CG_EIB_ADDRESSING : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        Crestron.Logos.SplusObjects.StringInput ADDRESS__DOLLAR__;
        Crestron.Logos.SplusObjects.StringInput EIB_FB_IN__DOLLAR__;
        Crestron.Logos.SplusObjects.StringInput COMMAND_IN__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput EIB_FB_OUT__DOLLAR__;
        Crestron.Logos.SplusObjects.StringOutput COMMAND_OUT__DOLLAR__;
        ushort MARKER1 = 0;
        ushort MARKER2 = 0;
        CrestronString HIGHGROUP;
        CrestronString MIDDLEGROUP;
        CrestronString UNDERGROUP_HIGH;
        CrestronString UNDERGROUP_LOW;
        object ADDRESS__DOLLAR___OnChange_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 139;
                MARKER1 = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 140;
                MARKER2 = (ushort) ( Functions.Find( "/" , ADDRESS__DOLLAR__ ) ) ; 
                __context__.SourceCodeLine = 141;
                MakeString ( HIGHGROUP , "{0}", Functions.Chr (  (int) ( Functions.Atoi( Functions.Mid( ADDRESS__DOLLAR__ , (int)( MARKER1 ) , (int)( (MARKER2 - MARKER1) ) ) ) ) ) ) ; 
                __context__.SourceCodeLine = 142;
                MARKER1 = (ushort) ( (MARKER2 + 1) ) ; 
                __context__.SourceCodeLine = 143;
                MARKER2 = (ushort) ( Functions.Find( "/" , ADDRESS__DOLLAR__ , MARKER1 ) ) ; 
                __context__.SourceCodeLine = 144;
                if ( Functions.TestForTrue  ( ( MARKER2)  ) ) 
                    { 
                    __context__.SourceCodeLine = 146;
                    MakeString ( MIDDLEGROUP , "{0}", Functions.Chr (  (int) ( Functions.Atoi( Functions.Mid( ADDRESS__DOLLAR__ , (int)( MARKER1 ) , (int)( (MARKER2 - MARKER1) ) ) ) ) ) ) ; 
                    __context__.SourceCodeLine = 147;
                    MARKER1 = (ushort) ( (MARKER2 + 1) ) ; 
                    __context__.SourceCodeLine = 148;
                    MARKER2 = (ushort) ( (Functions.Length( ADDRESS__DOLLAR__ ) + 1) ) ; 
                    __context__.SourceCodeLine = 149;
                    MakeString ( UNDERGROUP_HIGH , "{0}", Functions.Chr (  (int) ( ((Functions.Atoi( Functions.Mid( ADDRESS__DOLLAR__ , (int)( MARKER1 ) , (int)( (MARKER2 - MARKER1) ) ) ) & 240) >> 4) ) ) ) ; 
                    __context__.SourceCodeLine = 150;
                    MakeString ( UNDERGROUP_LOW , "{0}", Functions.Chr (  (int) ( (Functions.Atoi( Functions.Mid( ADDRESS__DOLLAR__ , (int)( MARKER1 ) , (int)( (MARKER2 - MARKER1) ) ) ) & 15) ) ) ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 154;
                    MARKER2 = (ushort) ( (Functions.Length( ADDRESS__DOLLAR__ ) + 1) ) ; 
                    __context__.SourceCodeLine = 155;
                    MakeString ( MIDDLEGROUP , "{0}", Functions.Chr (  (int) ( ((Functions.Atoi( Functions.Mid( ADDRESS__DOLLAR__ , (int)( MARKER1 ) , (int)( (MARKER2 - MARKER1) ) ) ) & 3840) >> 8) ) ) ) ; 
                    __context__.SourceCodeLine = 156;
                    MakeString ( UNDERGROUP_HIGH , "{0}", Functions.Chr (  (int) ( ((Functions.Atoi( Functions.Mid( ADDRESS__DOLLAR__ , (int)( MARKER1 ) , (int)( (MARKER2 - MARKER1) ) ) ) & 240) >> 4) ) ) ) ; 
                    __context__.SourceCodeLine = 157;
                    MakeString ( UNDERGROUP_LOW , "{0}", Functions.Chr (  (int) ( (Functions.Atoi( Functions.Mid( ADDRESS__DOLLAR__ , (int)( MARKER1 ) , (int)( (MARKER2 - MARKER1) ) ) ) & 15) ) ) ) ; 
                    } 
                
                __context__.SourceCodeLine = 159;
                Print( "\r\n{0:X2}{1:X2}{2:X2}{3:X2}", Byte( HIGHGROUP , (int)( 1 ) ), Byte( MIDDLEGROUP , (int)( 1 ) ), Byte( UNDERGROUP_HIGH , (int)( 1 ) ), Byte( UNDERGROUP_LOW , (int)( 1 ) )) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object EIB_FB_IN__DOLLAR___OnChange_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 165;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (Functions.Mid( EIB_FB_IN__DOLLAR__ , (int)( 2 ) , (int)( 1 ) ) == HIGHGROUP) ) && Functions.TestForTrue ( Functions.BoolToInt (Functions.Mid( EIB_FB_IN__DOLLAR__ , (int)( 3 ) , (int)( 1 ) ) == MIDDLEGROUP) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt (Functions.Mid( EIB_FB_IN__DOLLAR__ , (int)( 4 ) , (int)( 1 ) ) == UNDERGROUP_HIGH) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt (Functions.Mid( EIB_FB_IN__DOLLAR__ , (int)( 5 ) , (int)( 1 ) ) == UNDERGROUP_LOW) )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 167;
                EIB_FB_OUT__DOLLAR__  .UpdateValue ( EIB_FB_IN__DOLLAR__  ) ; 
                } 
            
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object COMMAND_IN__DOLLAR___OnChange_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 175;
        MakeString ( COMMAND_OUT__DOLLAR__ , "{0}{1}{2}{3}{4}{5}", Functions.Left ( COMMAND_IN__DOLLAR__ ,  (int) ( 1 ) ) , HIGHGROUP , MIDDLEGROUP , UNDERGROUP_HIGH , UNDERGROUP_LOW , Functions.Right ( COMMAND_IN__DOLLAR__ ,  (int) ( (Functions.Length( COMMAND_IN__DOLLAR__ ) - 1) ) ) ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}


public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    HIGHGROUP  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 2, this );
    MIDDLEGROUP  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 2, this );
    UNDERGROUP_HIGH  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 2, this );
    UNDERGROUP_LOW  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 2, this );
    
    ADDRESS__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( ADDRESS__DOLLAR____AnalogSerialInput__, 11, this );
    m_StringInputList.Add( ADDRESS__DOLLAR____AnalogSerialInput__, ADDRESS__DOLLAR__ );
    
    EIB_FB_IN__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( EIB_FB_IN__DOLLAR____AnalogSerialInput__, 60, this );
    m_StringInputList.Add( EIB_FB_IN__DOLLAR____AnalogSerialInput__, EIB_FB_IN__DOLLAR__ );
    
    COMMAND_IN__DOLLAR__ = new Crestron.Logos.SplusObjects.StringInput( COMMAND_IN__DOLLAR____AnalogSerialInput__, 36, this );
    m_StringInputList.Add( COMMAND_IN__DOLLAR____AnalogSerialInput__, COMMAND_IN__DOLLAR__ );
    
    EIB_FB_OUT__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( EIB_FB_OUT__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( EIB_FB_OUT__DOLLAR____AnalogSerialOutput__, EIB_FB_OUT__DOLLAR__ );
    
    COMMAND_OUT__DOLLAR__ = new Crestron.Logos.SplusObjects.StringOutput( COMMAND_OUT__DOLLAR____AnalogSerialOutput__, this );
    m_StringOutputList.Add( COMMAND_OUT__DOLLAR____AnalogSerialOutput__, COMMAND_OUT__DOLLAR__ );
    
    
    ADDRESS__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( ADDRESS__DOLLAR___OnChange_0, false ) );
    EIB_FB_IN__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( EIB_FB_IN__DOLLAR___OnChange_1, false ) );
    COMMAND_IN__DOLLAR__.OnSerialChange.Add( new InputChangeHandlerWrapper( COMMAND_IN__DOLLAR___OnChange_2, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_CG_EIB_ADDRESSING ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}




const uint ADDRESS__DOLLAR____AnalogSerialInput__ = 0;
const uint EIB_FB_IN__DOLLAR____AnalogSerialInput__ = 1;
const uint COMMAND_IN__DOLLAR____AnalogSerialInput__ = 2;
const uint EIB_FB_OUT__DOLLAR____AnalogSerialOutput__ = 0;
const uint COMMAND_OUT__DOLLAR____AnalogSerialOutput__ = 1;

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
