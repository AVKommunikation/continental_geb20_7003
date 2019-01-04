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

namespace UserModule_CG_EIB_IO_V4_1
{
    public class UserModuleClass_CG_EIB_IO_V4_1 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput TRIGGER;
        Crestron.Logos.SplusObjects.BufferInput RX;
        Crestron.Logos.SplusObjects.BufferInput EIB_CMD;
        Crestron.Logos.SplusObjects.StringOutput TX;
        Crestron.Logos.SplusObjects.StringOutput EIB_FB;
        ushort WAITING = 0;
        ushort ACTION = 0;
        ushort PROC_CMD = 0;
        ushort PROC_RX = 0;
        ushort NAK_COUNT = 0;
        CrestronString MSG;
        ushort [] SEND_ADR;
        ushort SEND_IN = 0;
        ushort SEND_OUT = 0;
        CrestronString [] SEND_PARM;
        ushort [] POLL_ADR;
        ushort POLL_IN = 0;
        ushort POLL_OUT = 0;
        public override object FunctionMain (  object __obj__ ) 
            { 
            try
            {
                SplusExecutionContext __context__ = SplusFunctionMainStartCode();
                
                __context__.SourceCodeLine = 78;
                WAITING = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 79;
                ACTION = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 80;
                PROC_CMD = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 81;
                PROC_RX = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 82;
                NAK_COUNT = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 84;
                SEND_IN = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 85;
                SEND_OUT = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 86;
                POLL_IN = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 87;
                POLL_OUT = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 89;
                RX  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 90;
                EIB_CMD  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 92;
                TX  .UpdateValue ( "\u0002089C\u000D"  ) ; 
                __context__.SourceCodeLine = 93;
                WAITING = (ushort) ( 10 ) ; 
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler(); }
            return __obj__;
            }
            
        private CrestronString HEX (  SplusExecutionContext __context__, ushort I ) 
            { 
            CrestronString OUT;
            OUT  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 2, this );
            
            
            __context__.SourceCodeLine = 98;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (I & 240) >= 160 ))  ) ) 
                {
                __context__.SourceCodeLine = 99;
                OUT  .UpdateValue ( Functions.Chr (  (int) ( (((I & 240) >> 4) + 55) ) )  ) ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 101;
                OUT  .UpdateValue ( Functions.Chr (  (int) ( (((I & 240) >> 4) | 48) ) )  ) ; 
                }
            
            __context__.SourceCodeLine = 102;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (I & 15) >= 10 ))  ) ) 
                {
                __context__.SourceCodeLine = 103;
                OUT  .UpdateValue ( OUT + Functions.Chr (  (int) ( ((I & 15) + 55) ) )  ) ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 105;
                OUT  .UpdateValue ( OUT + Functions.Chr (  (int) ( ((I & 15) | 48) ) )  ) ; 
                }
            
            __context__.SourceCodeLine = 106;
            return ( OUT ) ; 
            
            }
            
        private void SEND (  SplusExecutionContext __context__ ) 
            { 
            ushort ADR = 0;
            ushort PARBYTE = 0;
            ushort CHK = 0;
            ushort CT = 0;
            
            CrestronString PARMS;
            CrestronString PAR;
            PARMS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 56, this );
            PAR  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 28, this );
            
            
            __context__.SourceCodeLine = 113;
            if ( Functions.TestForTrue  ( ( Functions.OnesComplement( WAITING ))  ) ) 
                { 
                __context__.SourceCodeLine = 115;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SEND_IN != SEND_OUT))  ) ) 
                    { 
                    __context__.SourceCodeLine = 117;
                    ADR = (ushort) ( SEND_ADR[ SEND_OUT ] ) ; 
                    __context__.SourceCodeLine = 118;
                    PAR  .UpdateValue ( SEND_PARM [ SEND_OUT ]  ) ; 
                    __context__.SourceCodeLine = 119;
                    PARMS  .UpdateValue ( ""  ) ; 
                    __context__.SourceCodeLine = 120;
                    CHK = (ushort) ( ((Functions.High( (ushort) ADR ) + Functions.Low( (ushort) ADR )) + 23) ) ; 
                    __context__.SourceCodeLine = 121;
                    ushort __FN_FORSTART_VAL__1 = (ushort) ( 1 ) ;
                    ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( PAR ); 
                    int __FN_FORSTEP_VAL__1 = (int)1; 
                    for ( CT  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (CT  >= __FN_FORSTART_VAL__1) && (CT  <= __FN_FOREND_VAL__1) ) : ( (CT  <= __FN_FORSTART_VAL__1) && (CT  >= __FN_FOREND_VAL__1) ) ; CT  += (ushort)__FN_FORSTEP_VAL__1) 
                        { 
                        __context__.SourceCodeLine = 123;
                        PARBYTE = (ushort) ( Byte( PAR , (int)( CT ) ) ) ; 
                        __context__.SourceCodeLine = 124;
                        PARMS  .UpdateValue ( PARMS + HEX (  __context__ , (ushort)( PARBYTE ))  ) ; 
                        __context__.SourceCodeLine = 125;
                        CHK = (ushort) ( (CHK + PARBYTE) ) ; 
                        __context__.SourceCodeLine = 121;
                        } 
                    
                    __context__.SourceCodeLine = 127;
                    TX  .UpdateValue ( "\u00020B" + HEX (  __context__ , (ushort)( Functions.High( (ushort) ADR ) )) + HEX (  __context__ , (ushort)( Functions.Low( (ushort) ADR ) )) + "0C" + PARMS + HEX (  __context__ , (ushort)( (Functions.OnesComplement( CHK ) + 165) )) + "\u000D"  ) ; 
                    __context__.SourceCodeLine = 132;
                    ACTION = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 133;
                    WAITING = (ushort) ( 10 ) ; 
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 135;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (POLL_IN != POLL_OUT))  ) ) 
                        { 
                        __context__.SourceCodeLine = 137;
                        ADR = (ushort) ( POLL_ADR[ POLL_OUT ] ) ; 
                        __context__.SourceCodeLine = 138;
                        TX  .UpdateValue ( "\u00020C" + HEX (  __context__ , (ushort)( Functions.High( (ushort) ADR ) )) + HEX (  __context__ , (ushort)( Functions.Low( (ushort) ADR ) )) + HEX (  __context__ , (ushort)( (Functions.OnesComplement( ((Functions.High( (ushort) ADR ) + Functions.Low( (ushort) ADR )) + 12) ) + 165) )) + "\u000D"  ) ; 
                        __context__.SourceCodeLine = 142;
                        ACTION = (ushort) ( 2 ) ; 
                        __context__.SourceCodeLine = 143;
                        WAITING = (ushort) ( 10 ) ; 
                        } 
                    
                    }
                
                } 
            
            
            }
            
        ushort [] IN;
        private void REPLY (  SplusExecutionContext __context__ ) 
            { 
            ushort BAD = 0;
            ushort CT = 0;
            ushort CHAR = 0;
            ushort IDX = 0;
            
            CrestronString VALUE;
            VALUE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 56, this );
            
            
            __context__.SourceCodeLine = 156;
            BAD = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 157;
            IDX = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 158;
            ushort __FN_FORSTART_VAL__1 = (ushort) ( 2 ) ;
            ushort __FN_FOREND_VAL__1 = (ushort)(Functions.Length( MSG ) - 1); 
            int __FN_FORSTEP_VAL__1 = (int)2; 
            for ( CT  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (CT  >= __FN_FORSTART_VAL__1) && (CT  <= __FN_FOREND_VAL__1) ) : ( (CT  <= __FN_FORSTART_VAL__1) && (CT  >= __FN_FOREND_VAL__1) ) ; CT  += (ushort)__FN_FORSTEP_VAL__1) 
                { 
                __context__.SourceCodeLine = 159;
                CHAR = (ushort) ( Byte( MSG , (int)( CT ) ) ) ; 
                __context__.SourceCodeLine = 160;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( CHAR >= 48 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( CHAR <= 57 ) )) ))  ) ) 
                    {
                    __context__.SourceCodeLine = 161;
                    IN [ IDX] = (ushort) ( ((CHAR & 15) << 4) ) ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 162;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( CHAR >= 65 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( CHAR <= 70 ) )) ))  ) ) 
                        {
                        __context__.SourceCodeLine = 163;
                        IN [ IDX] = (ushort) ( (((CHAR & 79) - 55) << 4) ) ; 
                        }
                    
                    else 
                        {
                        __context__.SourceCodeLine = 164;
                        BAD = (ushort) ( 1 ) ; 
                        }
                    
                    }
                
                __context__.SourceCodeLine = 165;
                CHAR = (ushort) ( Byte( MSG , (int)( (CT + 1) ) ) ) ; 
                __context__.SourceCodeLine = 166;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( CHAR >= 48 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( CHAR <= 57 ) )) ))  ) ) 
                    {
                    __context__.SourceCodeLine = 167;
                    IN [ IDX] = (ushort) ( (IN[ IDX ] | (CHAR & 15)) ) ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 168;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt ( CHAR >= 65 ) ) && Functions.TestForTrue ( Functions.BoolToInt ( CHAR <= 70 ) )) ))  ) ) 
                        {
                        __context__.SourceCodeLine = 169;
                        IN [ IDX] = (ushort) ( (IN[ IDX ] | ((CHAR & 79) - 55)) ) ; 
                        }
                    
                    else 
                        {
                        __context__.SourceCodeLine = 170;
                        BAD = (ushort) ( 1 ) ; 
                        }
                    
                    }
                
                __context__.SourceCodeLine = 171;
                IDX = (ushort) ( (IDX + 1) ) ; 
                __context__.SourceCodeLine = 158;
                } 
            
            __context__.SourceCodeLine = 173;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( BAD ) || Functions.TestForTrue ( Functions.BoolToInt ( IDX <= 2 ) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 174;
                Print( "EIB: Bad receive packet len/char\r\n") ; 
                }
            
            else 
                { 
                __context__.SourceCodeLine = 176;
                IDX = (ushort) ( (IDX - 2) ) ; 
                __context__.SourceCodeLine = 177;
                CHAR = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 178;
                ushort __FN_FORSTART_VAL__2 = (ushort) ( 1 ) ;
                ushort __FN_FOREND_VAL__2 = (ushort)IDX; 
                int __FN_FORSTEP_VAL__2 = (int)1; 
                for ( CT  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (CT  >= __FN_FORSTART_VAL__2) && (CT  <= __FN_FOREND_VAL__2) ) : ( (CT  <= __FN_FORSTART_VAL__2) && (CT  >= __FN_FOREND_VAL__2) ) ; CT  += (ushort)__FN_FORSTEP_VAL__2) 
                    { 
                    __context__.SourceCodeLine = 179;
                    CHAR = (ushort) ( (CHAR + IN[ CT ]) ) ; 
                    __context__.SourceCodeLine = 178;
                    } 
                
                __context__.SourceCodeLine = 180;
                if ( Functions.TestForTrue  ( ( (((Functions.OnesComplement( CHAR ) + 165) ^ IN[ (IDX + 1) ]) & 255))  ) ) 
                    { 
                    __context__.SourceCodeLine = 181;
                    BAD = (ushort) ( 1 ) ; 
                    __context__.SourceCodeLine = 182;
                    Print( "EIB: Bad receive checksum\r\n") ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 185;
                    
                        {
                        int __SPLS_TMPVAR__SWTCH_1__ = ((int)IN[ 1 ]);
                        
                            { 
                            if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 252) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 187;
                                VALUE  .UpdateValue ( ""  ) ; 
                                __context__.SourceCodeLine = 188;
                                ushort __FN_FORSTART_VAL__3 = (ushort) ( 3 ) ;
                                ushort __FN_FOREND_VAL__3 = (ushort)IDX; 
                                int __FN_FORSTEP_VAL__3 = (int)1; 
                                for ( CT  = __FN_FORSTART_VAL__3; (__FN_FORSTEP_VAL__3 > 0)  ? ( (CT  >= __FN_FORSTART_VAL__3) && (CT  <= __FN_FOREND_VAL__3) ) : ( (CT  <= __FN_FORSTART_VAL__3) && (CT  >= __FN_FOREND_VAL__3) ) ; CT  += (ushort)__FN_FORSTEP_VAL__3) 
                                    {
                                    __context__.SourceCodeLine = 189;
                                    VALUE  .UpdateValue ( VALUE + Functions.Chr (  (int) ( (IN[ CT ] >> 4) ) ) + Functions.Chr (  (int) ( (IN[ CT ] & 15) ) )  ) ; 
                                    __context__.SourceCodeLine = 188;
                                    }
                                
                                __context__.SourceCodeLine = 190;
                                EIB_FB  .UpdateValue ( "\u00FE" + Functions.Chr (  (int) ( (IN[ 2 ] >> 3) ) ) + Functions.Chr (  (int) ( (IN[ 2 ] & 7) ) ) + VALUE + "\u00FF"  ) ; 
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_1__ == ( 254) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 194;
                                if ( Functions.TestForTrue  ( ( (IN[ 2 ] & 32))  ) ) 
                                    {
                                    __context__.SourceCodeLine = 194;
                                    Print( "CGEIB: BA layer\r\n") ; 
                                    }
                                
                                __context__.SourceCodeLine = 195;
                                if ( Functions.TestForTrue  ( ( (IN[ 2 ] & 8))  ) ) 
                                    {
                                    __context__.SourceCodeLine = 195;
                                    Print( "CGEIB: EEPROM CRC\r\n") ; 
                                    }
                                
                                __context__.SourceCodeLine = 196;
                                if ( Functions.TestForTrue  ( ( (IN[ 3 ] & 128))  ) ) 
                                    {
                                    __context__.SourceCodeLine = 196;
                                    Print( "CGEIB: Tx overflow\r\n") ; 
                                    }
                                
                                __context__.SourceCodeLine = 197;
                                if ( Functions.TestForTrue  ( ( (IN[ 3 ] & 16))  ) ) 
                                    {
                                    __context__.SourceCodeLine = 197;
                                    Print( "CGEIB: BA error\r\n") ; 
                                    }
                                
                                __context__.SourceCodeLine = 198;
                                if ( Functions.TestForTrue  ( ( (IN[ 3 ] & 8))  ) ) 
                                    {
                                    __context__.SourceCodeLine = 198;
                                    Print( "CGEIB: BA busy (EIB not connected?)\r\n") ; 
                                    }
                                
                                __context__.SourceCodeLine = 199;
                                if ( Functions.TestForTrue  ( ( (IN[ 3 ] & 1))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 200;
                                    
                                        {
                                        int __SPLS_TMPVAR__SWTCH_2__ = ((int)ACTION);
                                        
                                            { 
                                            if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 1) ) ) ) 
                                                {
                                                __context__.SourceCodeLine = 201;
                                                CT = (ushort) ( SEND_ADR[ SEND_OUT ] ) ; 
                                                }
                                            
                                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_2__ == ( 2) ) ) ) 
                                                {
                                                __context__.SourceCodeLine = 202;
                                                CT = (ushort) ( POLL_ADR[ POLL_OUT ] ) ; 
                                                }
                                            
                                            else 
                                                {
                                                __context__.SourceCodeLine = 203;
                                                CT = (ushort) ( 65535 ) ; 
                                                }
                                            
                                            } 
                                            
                                        }
                                        
                                    
                                    __context__.SourceCodeLine = 205;
                                    Print( "CGEIB: Not acknowledged on Adr {0:d}/{1:d}/{2:d}\r\n", (short)((CT & 30720) >> 11), (short)((CT & 1792) >> 8), (short)(CT & 255)) ; 
                                    } 
                                
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 210;
                                Print( "EIB: Unhandled response {0:X}h\r\n", IN[ 1 ]) ; 
                                }
                            
                            } 
                            
                        }
                        
                    
                    } 
                
                } 
            
            __context__.SourceCodeLine = 215;
            if ( Functions.TestForTrue  ( ( BAD)  ) ) 
                {
                __context__.SourceCodeLine = 216;
                TX  .UpdateValue ( "\u0015"  ) ; 
                }
            
            else 
                { 
                __context__.SourceCodeLine = 218;
                TX  .UpdateValue ( "\u0006"  ) ; 
                __context__.SourceCodeLine = 219;
                WAITING = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 220;
                NAK_COUNT = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 221;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (Functions.Length( RX ) == 0))  ) ) 
                    { 
                    __context__.SourceCodeLine = 223;
                    ACTION = (ushort) ( 0 ) ; 
                    } 
                
                } 
            
            
            }
            
        object TRIGGER_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 233;
                if ( Functions.TestForTrue  ( ( WAITING)  ) ) 
                    { 
                    __context__.SourceCodeLine = 235;
                    WAITING = (ushort) ( (WAITING - 1) ) ; 
                    __context__.SourceCodeLine = 236;
                    if ( Functions.TestForTrue  ( ( Functions.Not( WAITING ))  ) ) 
                        { 
                        __context__.SourceCodeLine = 238;
                        Print( "EIB: Timeout!\r\n") ; 
                        __context__.SourceCodeLine = 239;
                        SEND (  __context__  ) ; 
                        } 
                    
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 244;
                    SEND (  __context__  ) ; 
                    } 
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object EIB_CMD_OnChange_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            CrestronString CMD;
            CrestronString PARMS;
            CMD  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 20, this );
            PARMS  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 28, this );
            
            ushort ADR = 0;
            ushort CT = 0;
            ushort SEARCHING = 0;
            ushort CHAR = 0;
            
            
            __context__.SourceCodeLine = 251;
            if ( Functions.TestForTrue  ( ( Functions.Not( PROC_CMD ))  ) ) 
                { 
                __context__.SourceCodeLine = 252;
                PROC_CMD = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 253;
                CreateWait ( "WAIT_CMD" , 50 , WAIT_CMD_Callback ) ;
                __context__.SourceCodeLine = 257;
                CMD  .UpdateValue ( Functions.Remove ( "\u00FF" , EIB_CMD )  ) ; 
                __context__.SourceCodeLine = 258;
                while ( Functions.TestForTrue  ( ( Functions.Length( CMD ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 259;
                    
                        {
                        int __SPLS_TMPVAR__SWTCH_3__ = ((int)Byte( CMD , (int)( 1 ) ));
                        
                            { 
                            if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 254) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 261;
                                ADR = (ushort) ( ((((Byte( CMD , (int)( 2 ) ) << 11) + (Byte( CMD , (int)( 3 ) ) << 8)) + (Byte( CMD , (int)( 4 ) ) << 4)) + Byte( CMD , (int)( 5 ) )) ) ; 
                                __context__.SourceCodeLine = 263;
                                PARMS  .UpdateValue ( ""  ) ; 
                                __context__.SourceCodeLine = 264;
                                CMD  .UpdateValue ( Functions.Mid ( CMD ,  (int) ( 6 ) ,  (int) ( (Functions.Length( CMD ) - 6) ) )  ) ; 
                                __context__.SourceCodeLine = 265;
                                if ( Functions.TestForTrue  ( ( (Functions.Length( CMD ) & 1))  ) ) 
                                    {
                                    __context__.SourceCodeLine = 266;
                                    CMD  .UpdateValue ( "\u0000" + CMD  ) ; 
                                    }
                                
                                __context__.SourceCodeLine = 267;
                                ushort __FN_FORSTART_VAL__1 = (ushort) ( 2 ) ;
                                ushort __FN_FOREND_VAL__1 = (ushort)Functions.Length( CMD ); 
                                int __FN_FORSTEP_VAL__1 = (int)2; 
                                for ( CT  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (CT  >= __FN_FORSTART_VAL__1) && (CT  <= __FN_FOREND_VAL__1) ) : ( (CT  <= __FN_FORSTART_VAL__1) && (CT  >= __FN_FOREND_VAL__1) ) ; CT  += (ushort)__FN_FORSTEP_VAL__1) 
                                    { 
                                    __context__.SourceCodeLine = 268;
                                    CHAR = (ushort) ( ((Byte( CMD , (int)( (CT - 1) ) ) << 4) + Byte( CMD , (int)( CT ) )) ) ; 
                                    __context__.SourceCodeLine = 269;
                                    PARMS  .UpdateValue ( PARMS + Functions.Chr (  (int) ( CHAR ) )  ) ; 
                                    __context__.SourceCodeLine = 267;
                                    } 
                                
                                __context__.SourceCodeLine = 271;
                                CT = (ushort) ( SEND_OUT ) ; 
                                __context__.SourceCodeLine = 272;
                                SEARCHING = (ushort) ( 1 ) ; 
                                __context__.SourceCodeLine = 273;
                                while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (CT != SEND_IN) ) && Functions.TestForTrue ( SEARCHING )) ))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 274;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SEND_ADR[ CT ] == ADR))  ) ) 
                                        {
                                        __context__.SourceCodeLine = 275;
                                        SEARCHING = (ushort) ( 0 ) ; 
                                        }
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 277;
                                        CT = (ushort) ( (CT + 1) ) ; 
                                        __context__.SourceCodeLine = 278;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( CT > 100 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 278;
                                            CT = (ushort) ( 1 ) ; 
                                            }
                                        
                                        } 
                                    
                                    __context__.SourceCodeLine = 273;
                                    } 
                                
                                __context__.SourceCodeLine = 281;
                                if ( Functions.TestForTrue  ( ( SEARCHING)  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 282;
                                    SEND_ADR [ SEND_IN] = (ushort) ( ADR ) ; 
                                    __context__.SourceCodeLine = 283;
                                    SEND_PARM [ SEND_IN ]  .UpdateValue ( PARMS  ) ; 
                                    __context__.SourceCodeLine = 284;
                                    SEND_IN = (ushort) ( (SEND_IN + 1) ) ; 
                                    __context__.SourceCodeLine = 285;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( SEND_IN > 100 ))  ) ) 
                                        {
                                        __context__.SourceCodeLine = 285;
                                        SEND_IN = (ushort) ( 1 ) ; 
                                        }
                                    
                                    __context__.SourceCodeLine = 286;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SEND_IN == SEND_OUT))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 287;
                                        Print( "EIB: Send queue overflow!\r\n") ; 
                                        __context__.SourceCodeLine = 288;
                                        SEND_OUT = (ushort) ( (SEND_OUT + 1) ) ; 
                                        __context__.SourceCodeLine = 289;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( SEND_OUT > 100 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 289;
                                            SEND_OUT = (ushort) ( 1 ) ; 
                                            }
                                        
                                        } 
                                    
                                    } 
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 293;
                                    SEND_PARM [ CT ]  .UpdateValue ( PARMS  ) ; 
                                    }
                                
                                } 
                            
                            else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_3__ == ( 253) ) ) ) 
                                { 
                                __context__.SourceCodeLine = 296;
                                ADR = (ushort) ( ((((Byte( CMD , (int)( 2 ) ) << 11) + (Byte( CMD , (int)( 3 ) ) << 8)) + (Byte( CMD , (int)( 4 ) ) << 4)) + Byte( CMD , (int)( 5 ) )) ) ; 
                                __context__.SourceCodeLine = 298;
                                CT = (ushort) ( POLL_OUT ) ; 
                                __context__.SourceCodeLine = 299;
                                SEARCHING = (ushort) ( 1 ) ; 
                                __context__.SourceCodeLine = 300;
                                while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (CT != POLL_IN) ) && Functions.TestForTrue ( SEARCHING )) ))  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 301;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (POLL_ADR[ CT ] == ADR))  ) ) 
                                        {
                                        __context__.SourceCodeLine = 302;
                                        SEARCHING = (ushort) ( 0 ) ; 
                                        }
                                    
                                    else 
                                        { 
                                        __context__.SourceCodeLine = 304;
                                        CT = (ushort) ( (CT + 1) ) ; 
                                        __context__.SourceCodeLine = 305;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( CT > 50 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 305;
                                            CT = (ushort) ( 1 ) ; 
                                            }
                                        
                                        } 
                                    
                                    __context__.SourceCodeLine = 300;
                                    } 
                                
                                __context__.SourceCodeLine = 308;
                                if ( Functions.TestForTrue  ( ( SEARCHING)  ) ) 
                                    { 
                                    __context__.SourceCodeLine = 309;
                                    POLL_ADR [ POLL_IN] = (ushort) ( ADR ) ; 
                                    __context__.SourceCodeLine = 310;
                                    POLL_IN = (ushort) ( (POLL_IN + 1) ) ; 
                                    __context__.SourceCodeLine = 311;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( POLL_IN > 50 ))  ) ) 
                                        {
                                        __context__.SourceCodeLine = 311;
                                        POLL_IN = (ushort) ( 1 ) ; 
                                        }
                                    
                                    __context__.SourceCodeLine = 312;
                                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (POLL_IN == POLL_OUT))  ) ) 
                                        { 
                                        __context__.SourceCodeLine = 313;
                                        Print( "EIB: Poll queue overflow!\r\n") ; 
                                        __context__.SourceCodeLine = 314;
                                        POLL_OUT = (ushort) ( (POLL_OUT + 1) ) ; 
                                        __context__.SourceCodeLine = 315;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( POLL_OUT > 50 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 315;
                                            POLL_OUT = (ushort) ( 1 ) ; 
                                            }
                                        
                                        } 
                                    
                                    } 
                                
                                } 
                            
                            else 
                                {
                                __context__.SourceCodeLine = 320;
                                Print( "EIB: Bad command {0:X}h\r\n", Byte( CMD , (int)( 1 ) )) ; 
                                }
                            
                            } 
                            
                        }
                        
                    
                    __context__.SourceCodeLine = 322;
                    CMD  .UpdateValue ( Functions.Remove ( "\u00FF" , EIB_CMD )  ) ; 
                    __context__.SourceCodeLine = 258;
                    } 
                
                __context__.SourceCodeLine = 324;
                PROC_CMD = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 325;
                CancelWait ( "WAIT_CMD" ) ; 
                } 
            
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
public void WAIT_CMD_CallbackFn( object stateInfo )
{

    try
    {
        Wait __LocalWait__ = (Wait)stateInfo;
        SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
        __LocalWait__.RemoveFromList();
        
            
            __context__.SourceCodeLine = 254;
            Print( "EIB: Proc Cmd crash!\r\n") ; 
            __context__.SourceCodeLine = 255;
            PROC_CMD = (ushort) ( 0 ) ; 
            
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    
}

object RX_OnChange_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        ushort CHAR = 0;
        ushort COMPLETE = 0;
        ushort POLLNEXT = 0;
        
        
        __context__.SourceCodeLine = 331;
        if ( Functions.TestForTrue  ( ( Functions.Not( PROC_RX ))  ) ) 
            { 
            __context__.SourceCodeLine = 332;
            PROC_RX = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 333;
            CreateWait ( "WAIT_RX" , 100 , WAIT_RX_Callback ) ;
            __context__.SourceCodeLine = 338;
            COMPLETE = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 339;
            POLLNEXT = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 340;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.Length( RX ) ) && Functions.TestForTrue ( COMPLETE )) ))  ) ) 
                { 
                __context__.SourceCodeLine = 341;
                
                    {
                    int __SPLS_TMPVAR__SWTCH_4__ = ((int)Byte( RX , (int)( 1 ) ));
                    
                        { 
                        if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_4__ == ( 6) ) ) ) 
                            { 
                            __context__.SourceCodeLine = 343;
                            
                                {
                                int __SPLS_TMPVAR__SWTCH_5__ = ((int)ACTION);
                                
                                    { 
                                    if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_5__ == ( 1) ) ) ) 
                                        { 
                                        __context__.SourceCodeLine = 345;
                                        SEND_OUT = (ushort) ( (SEND_OUT + 1) ) ; 
                                        __context__.SourceCodeLine = 346;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( SEND_OUT > 100 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 346;
                                            SEND_OUT = (ushort) ( 1 ) ; 
                                            }
                                        
                                        } 
                                    
                                    else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_5__ == ( 2) ) ) ) 
                                        { 
                                        __context__.SourceCodeLine = 349;
                                        POLL_OUT = (ushort) ( (POLL_OUT + 1) ) ; 
                                        __context__.SourceCodeLine = 350;
                                        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( POLL_OUT > 50 ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 350;
                                            POLL_OUT = (ushort) ( 1 ) ; 
                                            }
                                        
                                        } 
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 352;
                                        POLLNEXT = (ushort) ( 0 ) ; 
                                        }
                                    
                                    } 
                                    
                                }
                                
                            
                            __context__.SourceCodeLine = 354;
                            CHAR = (ushort) ( Functions.GetC( RX ) ) ; 
                            __context__.SourceCodeLine = 355;
                            ACTION = (ushort) ( 0 ) ; 
                            __context__.SourceCodeLine = 356;
                            NAK_COUNT = (ushort) ( 0 ) ; 
                            __context__.SourceCodeLine = 357;
                            WAITING = (ushort) ( 0 ) ; 
                            } 
                        
                        else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_4__ == ( 21) ) ) ) 
                            { 
                            __context__.SourceCodeLine = 360;
                            NAK_COUNT = (ushort) ( (NAK_COUNT + 1) ) ; 
                            __context__.SourceCodeLine = 361;
                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NAK_COUNT > 2 ))  ) ) 
                                { 
                                __context__.SourceCodeLine = 362;
                                
                                    {
                                    int __SPLS_TMPVAR__SWTCH_6__ = ((int)ACTION);
                                    
                                        { 
                                        if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_6__ == ( 1) ) ) ) 
                                            { 
                                            __context__.SourceCodeLine = 364;
                                            CHAR = (ushort) ( SEND_ADR[ SEND_OUT ] ) ; 
                                            __context__.SourceCodeLine = 365;
                                            Print( "EIB: NAK on setting {0:d}/{1:d}/{2:d}\r\n", (short)((CHAR & 30720) >> 11), (short)((CHAR & 1792) >> 8), (short)(CHAR & 255)) ; 
                                            __context__.SourceCodeLine = 367;
                                            SEND_OUT = (ushort) ( (SEND_OUT + 1) ) ; 
                                            __context__.SourceCodeLine = 368;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( SEND_OUT > 100 ))  ) ) 
                                                {
                                                __context__.SourceCodeLine = 368;
                                                SEND_OUT = (ushort) ( 1 ) ; 
                                                }
                                            
                                            } 
                                        
                                        else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_6__ == ( 2) ) ) ) 
                                            { 
                                            __context__.SourceCodeLine = 371;
                                            CHAR = (ushort) ( POLL_ADR[ POLL_OUT ] ) ; 
                                            __context__.SourceCodeLine = 372;
                                            Print( "EIB: NAK on polling {0:d}/{1:d}/{2:d}\r\n", (short)((CHAR & 30720) >> 11), (short)((CHAR & 1792) >> 8), (short)(CHAR & 255)) ; 
                                            __context__.SourceCodeLine = 374;
                                            POLL_OUT = (ushort) ( (POLL_OUT + 1) ) ; 
                                            __context__.SourceCodeLine = 375;
                                            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( POLL_OUT > 50 ))  ) ) 
                                                {
                                                __context__.SourceCodeLine = 375;
                                                POLL_OUT = (ushort) ( 1 ) ; 
                                                }
                                            
                                            } 
                                        
                                        else 
                                            { 
                                            __context__.SourceCodeLine = 378;
                                            Print( "EIB: NAK\r\n") ; 
                                            } 
                                        
                                        } 
                                        
                                    }
                                    
                                
                                __context__.SourceCodeLine = 381;
                                NAK_COUNT = (ushort) ( 0 ) ; 
                                __context__.SourceCodeLine = 382;
                                ACTION = (ushort) ( 0 ) ; 
                                } 
                            
                            __context__.SourceCodeLine = 384;
                            CHAR = (ushort) ( Functions.GetC( RX ) ) ; 
                            __context__.SourceCodeLine = 385;
                            WAITING = (ushort) ( 0 ) ; 
                            } 
                        
                        else if  ( Functions.TestForTrue  (  ( __SPLS_TMPVAR__SWTCH_4__ == ( 2) ) ) ) 
                            { 
                            __context__.SourceCodeLine = 388;
                            MSG  .UpdateValue ( Functions.Remove ( "\u000D" , RX )  ) ; 
                            __context__.SourceCodeLine = 389;
                            if ( Functions.TestForTrue  ( ( Functions.Length( MSG ))  ) ) 
                                {
                                __context__.SourceCodeLine = 390;
                                REPLY (  __context__  ) ; 
                                }
                            
                            else 
                                {
                                __context__.SourceCodeLine = 392;
                                COMPLETE = (ushort) ( 0 ) ; 
                                }
                            
                            } 
                        
                        else 
                            {
                            __context__.SourceCodeLine = 394;
                            CHAR = (ushort) ( Functions.GetC( RX ) ) ; 
                            }
                        
                        } 
                        
                    }
                    
                
                __context__.SourceCodeLine = 340;
                } 
            
            __context__.SourceCodeLine = 397;
            PROC_RX = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 398;
            CancelWait ( "WAIT_RX" ) ; 
            __context__.SourceCodeLine = 399;
            if ( Functions.TestForTrue  ( ( POLLNEXT)  ) ) 
                {
                __context__.SourceCodeLine = 400;
                SEND (  __context__  ) ; 
                }
            
            } 
        
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void WAIT_RX_CallbackFn( object stateInfo )
{

    try
    {
        Wait __LocalWait__ = (Wait)stateInfo;
        SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
        __LocalWait__.RemoveFromList();
        
            
            __context__.SourceCodeLine = 334;
            Print( "EIB: Proc Rx crash!\r\n") ; 
            __context__.SourceCodeLine = 335;
            PROC_RX = (ushort) ( 0 ) ; 
            __context__.SourceCodeLine = 336;
            Functions.ClearBuffer ( RX ) ; 
            
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    
}


public override void LogosSplusInitialize()
{
    _SplusNVRAM = new SplusNVRAM( this );
    SEND_ADR  = new ushort[ 101 ];
    POLL_ADR  = new ushort[ 51 ];
    IN  = new ushort[ 131 ];
    MSG  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 255, this );
    SEND_PARM  = new CrestronString[ 101 ];
    for( uint i = 0; i < 101; i++ )
        SEND_PARM [i] = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingASCII, 28, this );
    
    TRIGGER = new Crestron.Logos.SplusObjects.DigitalInput( TRIGGER__DigitalInput__, this );
    m_DigitalInputList.Add( TRIGGER__DigitalInput__, TRIGGER );
    
    TX = new Crestron.Logos.SplusObjects.StringOutput( TX__AnalogSerialOutput__, this );
    m_StringOutputList.Add( TX__AnalogSerialOutput__, TX );
    
    EIB_FB = new Crestron.Logos.SplusObjects.StringOutput( EIB_FB__AnalogSerialOutput__, this );
    m_StringOutputList.Add( EIB_FB__AnalogSerialOutput__, EIB_FB );
    
    RX = new Crestron.Logos.SplusObjects.BufferInput( RX__AnalogSerialInput__, 255, this );
    m_StringInputList.Add( RX__AnalogSerialInput__, RX );
    
    EIB_CMD = new Crestron.Logos.SplusObjects.BufferInput( EIB_CMD__AnalogSerialInput__, 255, this );
    m_StringInputList.Add( EIB_CMD__AnalogSerialInput__, EIB_CMD );
    
    WAIT_CMD_Callback = new WaitFunction( WAIT_CMD_CallbackFn );
    WAIT_RX_Callback = new WaitFunction( WAIT_RX_CallbackFn );
    
    TRIGGER.OnDigitalPush.Add( new InputChangeHandlerWrapper( TRIGGER_OnPush_0, false ) );
    EIB_CMD.OnSerialChange.Add( new InputChangeHandlerWrapper( EIB_CMD_OnChange_1, false ) );
    RX.OnSerialChange.Add( new InputChangeHandlerWrapper( RX_OnChange_2, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_CG_EIB_IO_V4_1 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}


private WaitFunction WAIT_CMD_Callback;
private WaitFunction WAIT_RX_Callback;


const uint TRIGGER__DigitalInput__ = 0;
const uint RX__AnalogSerialInput__ = 0;
const uint EIB_CMD__AnalogSerialInput__ = 1;
const uint TX__AnalogSerialOutput__ = 0;
const uint EIB_FB__AnalogSerialOutput__ = 1;

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
