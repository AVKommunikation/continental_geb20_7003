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

namespace UserModule_BIAMP_AUDIAFLEX_COMMAND_PROCESSOR_IP_V7_4
{
    public class UserModuleClass_BIAMP_AUDIAFLEX_COMMAND_PROCESSOR_IP_V7_4 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput SEND_NEXT;
        Crestron.Logos.SplusObjects.DigitalInput INITIALIZE;
        Crestron.Logos.SplusObjects.DigitalInput GET_NEXT_NAME;
        Crestron.Logos.SplusObjects.DigitalInput CONNECTED_F;
        Crestron.Logos.SplusObjects.BufferInput FROM_DEVICE;
        Crestron.Logos.SplusObjects.BufferInput FROM_MODULES;
        Crestron.Logos.SplusObjects.DigitalOutput TIMED_OUT;
        Crestron.Logos.SplusObjects.DigitalOutput INITIALIZE_BUSY;
        Crestron.Logos.SplusObjects.DigitalOutput NAME_TIMED_OUT;
        Crestron.Logos.SplusObjects.StringOutput TO_DEVICE;
        InOutArray<Crestron.Logos.SplusObjects.StringOutput> TO_MODULES;
        ushort INEXTCOMMANDSTORE = 0;
        ushort INEXTCOMMANDSEND = 0;
        ushort INEXTGETSTORE = 0;
        ushort INEXTGETSEND = 0;
        ushort ITEMPID = 0;
        ushort BFLAG1 = 0;
        ushort BFLAG2 = 0;
        ushort BOKTOSEND = 0;
        ushort ITEMP = 0;
        ushort A = 0;
        ushort B = 0;
        ushort ISENDNAME = 0;
        CrestronString [] SCOMMAND;
        CrestronString [] SGET;
        CrestronString STEMPMODULES;
        CrestronString STEMPDEVICE;
        CrestronString [] SMODULEINSTANCEID;
        private void FTIMEOUT (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 64;
            CreateWait ( "WTIMEOUT" , 200 , WTIMEOUT_Callback ) ;
            
            }
            
        public void WTIMEOUT_CallbackFn( object stateInfo )
        {
        
            try
            {
                Wait __LocalWait__ = (Wait)stateInfo;
                SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
                __LocalWait__.RemoveFromList();
                
            
            __context__.SourceCodeLine = 66;
            TIMED_OUT  .Value = (ushort) ( 1 ) ; 
            
        
        
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler(); }
            
        }
        
    private void FNAMETIMEOUT (  SplusExecutionContext __context__ ) 
        { 
        
        __context__.SourceCodeLine = 72;
        CreateWait ( "WNAMETIMEOUT" , 25 , WNAMETIMEOUT_Callback ) ;
        
        }
        
    public void WNAMETIMEOUT_CallbackFn( object stateInfo )
    {
    
        try
        {
            Wait __LocalWait__ = (Wait)stateInfo;
            SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
            __LocalWait__.RemoveFromList();
            
            
            __context__.SourceCodeLine = 74;
            NAME_TIMED_OUT  .Value = (ushort) ( 1 ) ; 
            
        
        
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler(); }
        
    }
    
object INITIALIZE_OnPush_0 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 83;
        INITIALIZE_BUSY  .Value = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 84;
        BOKTOSEND = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 85;
        ISENDNAME = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 86;
        MakeString ( TO_MODULES [ ISENDNAME] , "{0:d} {1}", (short)ISENDNAME, "Send Name\u000D\u000A" ) ; 
        __context__.SourceCodeLine = 87;
        FNAMETIMEOUT (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SEND_NEXT_OnPush_1 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 92;
        TIMED_OUT  .Value = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object SEND_NEXT_OnRelease_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 97;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (CONNECTED_F  .Value == 1))  ) ) 
            { 
            __context__.SourceCodeLine = 99;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( SCOMMAND[ INEXTCOMMANDSEND ] ) > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 101;
                TO_DEVICE  .UpdateValue ( SCOMMAND [ INEXTCOMMANDSEND ]  ) ; 
                __context__.SourceCodeLine = 102;
                SCOMMAND [ INEXTCOMMANDSEND ]  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 103;
                FTIMEOUT (  __context__  ) ; 
                __context__.SourceCodeLine = 104;
                BOKTOSEND = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 105;
                INEXTCOMMANDSEND = (ushort) ( (INEXTCOMMANDSEND + 1) ) ; 
                __context__.SourceCodeLine = 106;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( INEXTCOMMANDSEND > 500 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 108;
                    INEXTCOMMANDSEND = (ushort) ( 1 ) ; 
                    } 
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 111;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( SGET[ INEXTGETSEND ] ) > 0 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 113;
                    TO_DEVICE  .UpdateValue ( SGET [ INEXTGETSEND ]  ) ; 
                    __context__.SourceCodeLine = 114;
                    SGET [ INEXTGETSEND ]  .UpdateValue ( ""  ) ; 
                    __context__.SourceCodeLine = 115;
                    FTIMEOUT (  __context__  ) ; 
                    __context__.SourceCodeLine = 116;
                    BOKTOSEND = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 117;
                    INEXTGETSEND = (ushort) ( (INEXTGETSEND + 1) ) ; 
                    __context__.SourceCodeLine = 118;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( INEXTGETSEND > 500 ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 120;
                        INEXTGETSEND = (ushort) ( 1 ) ; 
                        } 
                    
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 125;
                    BOKTOSEND = (ushort) ( 1 ) ; 
                    } 
                
                }
            
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object GET_NEXT_NAME_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 132;
        Functions.Delay (  (int) ( 1 ) ) ; 
        __context__.SourceCodeLine = 133;
        NAME_TIMED_OUT  .Value = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 134;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( ISENDNAME < 500 ))  ) ) 
            { 
            __context__.SourceCodeLine = 136;
            SMODULEINSTANCEID [ ISENDNAME ]  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 137;
            ISENDNAME = (ushort) ( (ISENDNAME + 1) ) ; 
            __context__.SourceCodeLine = 138;
            MakeString ( TO_MODULES [ ISENDNAME] , "{0:d} {1}", (short)ISENDNAME, "Send Name\u000D\u000A" ) ; 
            __context__.SourceCodeLine = 139;
            FNAMETIMEOUT (  __context__  ) ; 
            } 
        
        else 
            {
            __context__.SourceCodeLine = 141;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (ISENDNAME == 500))  ) ) 
                { 
                __context__.SourceCodeLine = 143;
                INITIALIZE_BUSY  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 144;
                BOKTOSEND = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 145;
                TIMED_OUT  .Value = (ushort) ( 1 ) ; 
                } 
            
            }
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CONNECTED_F_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 151;
        Functions.Delay (  (int) ( 100 ) ) ; 
        __context__.SourceCodeLine = 152;
        TO_DEVICE  .UpdateValue ( "\u00FF\u00FE\u0001"  ) ; 
        __context__.SourceCodeLine = 153;
        FTIMEOUT (  __context__  ) ; 
        __context__.SourceCodeLine = 154;
        BOKTOSEND = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object FROM_MODULES_OnChange_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 159;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BFLAG1 == 0))  ) ) 
            { 
            __context__.SourceCodeLine = 161;
            BFLAG1 = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 162;
            while ( Functions.TestForTrue  ( ( 1)  ) ) 
                { 
                __context__.SourceCodeLine = 164;
                STEMPMODULES  .UpdateValue ( Functions.Gather ( "\u000A" , FROM_MODULES )  ) ; 
                __context__.SourceCodeLine = 165;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "Send Name" , STEMPMODULES ) > 0 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 167;
                    CancelWait ( "WNAMETIMEOUT" ) ; 
                    __context__.SourceCodeLine = 168;
                    ITEMPID = (ushort) ( Functions.Atoi( STEMPMODULES ) ) ; 
                    __context__.SourceCodeLine = 169;
                    STEMPMODULES  .UpdateValue ( Functions.Left ( STEMPMODULES ,  (int) ( (Functions.Length( STEMPMODULES ) - 2) ) )  ) ; 
                    __context__.SourceCodeLine = 170;
                    SMODULEINSTANCEID [ ITEMPID ]  .UpdateValue ( Functions.Mid ( STEMPMODULES ,  (int) ( (Functions.Find( "=" , STEMPMODULES ) + 1) ) ,  (int) ( (Functions.Length( STEMPMODULES ) - Functions.Find( "=" , STEMPMODULES )) ) ) + " "  ) ; 
                    __context__.SourceCodeLine = 171;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( ISENDNAME < 500 ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 173;
                        ISENDNAME = (ushort) ( (ISENDNAME + 1) ) ; 
                        __context__.SourceCodeLine = 174;
                        MakeString ( TO_MODULES [ ISENDNAME] , "{0:d} {1}", (short)ISENDNAME, "Send Name\u000D\u000A" ) ; 
                        __context__.SourceCodeLine = 175;
                        FNAMETIMEOUT (  __context__  ) ; 
                        } 
                    
                    else 
                        { 
                        __context__.SourceCodeLine = 179;
                        INITIALIZE_BUSY  .Value = (ushort) ( 0 ) ; 
                        __context__.SourceCodeLine = 180;
                        BOKTOSEND = (ushort) ( 1 ) ; 
                        __context__.SourceCodeLine = 181;
                        TIMED_OUT  .Value = (ushort) ( 1 ) ; 
                        } 
                    
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 184;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (CONNECTED_F  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (BOKTOSEND == 1) )) ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 186;
                        TO_DEVICE  .UpdateValue ( STEMPMODULES  ) ; 
                        __context__.SourceCodeLine = 187;
                        FTIMEOUT (  __context__  ) ; 
                        __context__.SourceCodeLine = 188;
                        BOKTOSEND = (ushort) ( 0 ) ; 
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 190;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "GET" , STEMPMODULES ) > 0 ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 192;
                            SGET [ INEXTGETSTORE ]  .UpdateValue ( STEMPMODULES  ) ; 
                            __context__.SourceCodeLine = 193;
                            INEXTGETSTORE = (ushort) ( (INEXTGETSTORE + 1) ) ; 
                            __context__.SourceCodeLine = 194;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( INEXTGETSTORE > 500 ))  ) ) 
                                { 
                                __context__.SourceCodeLine = 196;
                                INEXTGETSTORE = (ushort) ( 1 ) ; 
                                } 
                            
                            } 
                        
                        else 
                            { 
                            __context__.SourceCodeLine = 201;
                            SCOMMAND [ INEXTCOMMANDSTORE ]  .UpdateValue ( STEMPMODULES  ) ; 
                            __context__.SourceCodeLine = 202;
                            INEXTCOMMANDSTORE = (ushort) ( (INEXTCOMMANDSTORE + 1) ) ; 
                            __context__.SourceCodeLine = 203;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( INEXTCOMMANDSTORE > 500 ))  ) ) 
                                { 
                                __context__.SourceCodeLine = 205;
                                INEXTCOMMANDSTORE = (ushort) ( 1 ) ; 
                                } 
                            
                            } 
                        
                        }
                    
                    }
                
                __context__.SourceCodeLine = 208;
                STEMPMODULES  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 162;
                } 
            
            __context__.SourceCodeLine = 210;
            BFLAG1 = (ushort) ( 0 ) ; 
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object FROM_DEVICE_OnChange_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 216;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt (BFLAG2 == 0))  ) ) 
            { 
            __context__.SourceCodeLine = 218;
            BFLAG2 = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 219;
            while ( Functions.TestForTrue  ( ( 1)  ) ) 
                { 
                __context__.SourceCodeLine = 221;
                STEMPDEVICE  .UpdateValue ( Functions.Gather ( "\u000D\u000A" , FROM_DEVICE )  ) ; 
                __context__.SourceCodeLine = 222;
                CancelWait ( "WTIMEOUT" ) ; 
                __context__.SourceCodeLine = 223;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Find( "\u00FF\u00FC\u0001" , STEMPDEVICE ) > 0 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 225;
                    STEMPDEVICE  .UpdateValue ( ""  ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 227;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (Functions.Find( "RECALL" , Functions.Upper( STEMPDEVICE ) ) == 0) ) && Functions.TestForTrue ( Functions.BoolToInt (Functions.Find( "PRESET" , Functions.Upper( STEMPDEVICE ) ) == 0) )) ) ) && Functions.TestForTrue ( Functions.BoolToInt (Functions.Find( "-ERR" , Functions.Upper( STEMPDEVICE ) ) == 0) )) ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 229;
                        ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                        ushort __FN_FOREND_VAL__1 = (ushort)500; 
                        int __FN_FORSTEP_VAL__1 = (int)1; 
                        for ( B  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (B  >= __FN_FORSTART_VAL__1) && (B  <= __FN_FOREND_VAL__1) ) : ( (B  <= __FN_FORSTART_VAL__1) && (B  >= __FN_FOREND_VAL__1) ) ; B  += (ushort)__FN_FORSTEP_VAL__1) 
                            { 
                            __context__.SourceCodeLine = 231;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( Functions.Length( SMODULEINSTANCEID[ B ] ) > 0 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( Functions.Find( SMODULEINSTANCEID[ B ] , STEMPDEVICE ) > 0 ) )) ))  ) ) 
                                { 
                                __context__.SourceCodeLine = 233;
                                TO_MODULES [ B]  .UpdateValue ( STEMPDEVICE  ) ; 
                                __context__.SourceCodeLine = 234;
                                break ; 
                                } 
                            
                            __context__.SourceCodeLine = 229;
                            } 
                        
                        } 
                    
                    }
                
                __context__.SourceCodeLine = 238;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (CONNECTED_F  .Value == 1) ) && Functions.TestForTrue ( Functions.BoolToInt (INITIALIZE_BUSY  .Value == 0) )) ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 240;
                    Functions.Delay (  (int) ( 5 ) ) ; 
                    __context__.SourceCodeLine = 241;
                    BOKTOSEND = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 242;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( SCOMMAND[ INEXTCOMMANDSEND ] ) > 0 ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 244;
                        TO_DEVICE  .UpdateValue ( SCOMMAND [ INEXTCOMMANDSEND ]  ) ; 
                        __context__.SourceCodeLine = 245;
                        SCOMMAND [ INEXTCOMMANDSEND ]  .UpdateValue ( ""  ) ; 
                        __context__.SourceCodeLine = 246;
                        FTIMEOUT (  __context__  ) ; 
                        __context__.SourceCodeLine = 247;
                        BOKTOSEND = (ushort) ( 0 ) ; 
                        __context__.SourceCodeLine = 248;
                        INEXTCOMMANDSEND = (ushort) ( (INEXTCOMMANDSEND + 1) ) ; 
                        __context__.SourceCodeLine = 249;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( INEXTCOMMANDSEND > 500 ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 251;
                            INEXTCOMMANDSEND = (ushort) ( 1 ) ; 
                            } 
                        
                        } 
                    
                    else 
                        {
                        __context__.SourceCodeLine = 254;
                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( SGET[ INEXTGETSEND ] ) > 0 ))  ) ) 
                            { 
                            __context__.SourceCodeLine = 256;
                            TO_DEVICE  .UpdateValue ( SGET [ INEXTGETSEND ]  ) ; 
                            __context__.SourceCodeLine = 257;
                            SGET [ INEXTGETSEND ]  .UpdateValue ( ""  ) ; 
                            __context__.SourceCodeLine = 258;
                            FTIMEOUT (  __context__  ) ; 
                            __context__.SourceCodeLine = 259;
                            BOKTOSEND = (ushort) ( 0 ) ; 
                            __context__.SourceCodeLine = 260;
                            INEXTGETSEND = (ushort) ( (INEXTGETSEND + 1) ) ; 
                            __context__.SourceCodeLine = 261;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( INEXTGETSEND > 500 ))  ) ) 
                                { 
                                __context__.SourceCodeLine = 263;
                                INEXTGETSEND = (ushort) ( 1 ) ; 
                                } 
                            
                            } 
                        
                        }
                    
                    } 
                
                __context__.SourceCodeLine = 267;
                STEMPDEVICE  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 219;
                } 
            
            __context__.SourceCodeLine = 269;
            BFLAG2 = (ushort) ( 0 ) ; 
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
        
        __context__.SourceCodeLine = 279;
        STEMPMODULES  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 280;
        STEMPDEVICE  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 281;
        Functions.SetArray ( SCOMMAND , "" ) ; 
        __context__.SourceCodeLine = 282;
        Functions.SetArray ( SGET , "" ) ; 
        __context__.SourceCodeLine = 283;
        BOKTOSEND = (ushort) ( 1 ) ; 
        __context__.SourceCodeLine = 284;
        BFLAG1 = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 285;
        BFLAG2 = (ushort) ( 0 ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    return __obj__;
    }
    

public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    STEMPMODULES  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
    STEMPDEVICE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
    SCOMMAND  = new CrestronString[ 501 ];
    for( uint i = 0; i < 501; i++ )
        SCOMMAND [i] = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
    SGET  = new CrestronString[ 501 ];
    for( uint i = 0; i < 501; i++ )
        SGET [i] = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
    SMODULEINSTANCEID  = new CrestronString[ 501 ];
    for( uint i = 0; i < 501; i++ )
        SMODULEINSTANCEID [i] = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 100, this );
    
    SEND_NEXT = new Crestron.Logos.SplusObjects.DigitalInput( SEND_NEXT__DigitalInput__, this );
    m_DigitalInputList.Add( SEND_NEXT__DigitalInput__, SEND_NEXT );
    
    INITIALIZE = new Crestron.Logos.SplusObjects.DigitalInput( INITIALIZE__DigitalInput__, this );
    m_DigitalInputList.Add( INITIALIZE__DigitalInput__, INITIALIZE );
    
    GET_NEXT_NAME = new Crestron.Logos.SplusObjects.DigitalInput( GET_NEXT_NAME__DigitalInput__, this );
    m_DigitalInputList.Add( GET_NEXT_NAME__DigitalInput__, GET_NEXT_NAME );
    
    CONNECTED_F = new Crestron.Logos.SplusObjects.DigitalInput( CONNECTED_F__DigitalInput__, this );
    m_DigitalInputList.Add( CONNECTED_F__DigitalInput__, CONNECTED_F );
    
    TIMED_OUT = new Crestron.Logos.SplusObjects.DigitalOutput( TIMED_OUT__DigitalOutput__, this );
    m_DigitalOutputList.Add( TIMED_OUT__DigitalOutput__, TIMED_OUT );
    
    INITIALIZE_BUSY = new Crestron.Logos.SplusObjects.DigitalOutput( INITIALIZE_BUSY__DigitalOutput__, this );
    m_DigitalOutputList.Add( INITIALIZE_BUSY__DigitalOutput__, INITIALIZE_BUSY );
    
    NAME_TIMED_OUT = new Crestron.Logos.SplusObjects.DigitalOutput( NAME_TIMED_OUT__DigitalOutput__, this );
    m_DigitalOutputList.Add( NAME_TIMED_OUT__DigitalOutput__, NAME_TIMED_OUT );
    
    TO_DEVICE = new Crestron.Logos.SplusObjects.StringOutput( TO_DEVICE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( TO_DEVICE__AnalogSerialOutput__, TO_DEVICE );
    
    TO_MODULES = new InOutArray<StringOutput>( 500, this );
    for( uint i = 0; i < 500; i++ )
    {
        TO_MODULES[i+1] = new Crestron.Logos.SplusObjects.StringOutput( TO_MODULES__AnalogSerialOutput__ + i, this );
        m_StringOutputList.Add( TO_MODULES__AnalogSerialOutput__ + i, TO_MODULES[i+1] );
    }
    
    FROM_DEVICE = new Crestron.Logos.SplusObjects.BufferInput( FROM_DEVICE__AnalogSerialInput__, 5000, this );
    m_StringInputList.Add( FROM_DEVICE__AnalogSerialInput__, FROM_DEVICE );
    
    FROM_MODULES = new Crestron.Logos.SplusObjects.BufferInput( FROM_MODULES__AnalogSerialInput__, 5000, this );
    m_StringInputList.Add( FROM_MODULES__AnalogSerialInput__, FROM_MODULES );
    
    WTIMEOUT_Callback = new WaitFunction( WTIMEOUT_CallbackFn );
    WNAMETIMEOUT_Callback = new WaitFunction( WNAMETIMEOUT_CallbackFn );
    
    INITIALIZE.OnDigitalPush.Add( new InputChangeHandlerWrapper( INITIALIZE_OnPush_0, false ) );
    SEND_NEXT.OnDigitalPush.Add( new InputChangeHandlerWrapper( SEND_NEXT_OnPush_1, false ) );
    SEND_NEXT.OnDigitalRelease.Add( new InputChangeHandlerWrapper( SEND_NEXT_OnRelease_2, false ) );
    GET_NEXT_NAME.OnDigitalPush.Add( new InputChangeHandlerWrapper( GET_NEXT_NAME_OnPush_3, false ) );
    CONNECTED_F.OnDigitalPush.Add( new InputChangeHandlerWrapper( CONNECTED_F_OnPush_4, false ) );
    FROM_MODULES.OnSerialChange.Add( new InputChangeHandlerWrapper( FROM_MODULES_OnChange_5, false ) );
    FROM_DEVICE.OnSerialChange.Add( new InputChangeHandlerWrapper( FROM_DEVICE_OnChange_6, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_BIAMP_AUDIAFLEX_COMMAND_PROCESSOR_IP_V7_4 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}


private WaitFunction WTIMEOUT_Callback;
private WaitFunction WNAMETIMEOUT_Callback;


const uint SEND_NEXT__DigitalInput__ = 0;
const uint INITIALIZE__DigitalInput__ = 1;
const uint GET_NEXT_NAME__DigitalInput__ = 2;
const uint CONNECTED_F__DigitalInput__ = 3;
const uint FROM_DEVICE__AnalogSerialInput__ = 0;
const uint FROM_MODULES__AnalogSerialInput__ = 1;
const uint TIMED_OUT__DigitalOutput__ = 0;
const uint INITIALIZE_BUSY__DigitalOutput__ = 1;
const uint NAME_TIMED_OUT__DigitalOutput__ = 2;
const uint TO_DEVICE__AnalogSerialOutput__ = 0;
const uint TO_MODULES__AnalogSerialOutput__ = 1;

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
