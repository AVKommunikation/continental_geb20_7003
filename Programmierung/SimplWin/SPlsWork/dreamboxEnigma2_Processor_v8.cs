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

namespace UserModule_DREAMBOXENIGMA2_PROCESSOR_V8
{
    public class UserModuleClass_DREAMBOXENIGMA2_PROCESSOR_V8 : SplusObject
    {
        static CCriticalSection g_criticalSection = new CCriticalSection();
        
        
        
        
        
        
        
        
        
        
        
        Crestron.Logos.SplusObjects.DigitalInput GO_PARSE;
        Crestron.Logos.SplusObjects.DigitalInput POWER_ON;
        Crestron.Logos.SplusObjects.DigitalInput POWER_OFF;
        Crestron.Logos.SplusObjects.DigitalInput POWER;
        Crestron.Logos.SplusObjects.DigitalInput CUR_REFREH;
        InOutArray<Crestron.Logos.SplusObjects.DigitalInput> FAV;
        Crestron.Logos.SplusObjects.AnalogInput CHANNEL;
        Crestron.Logos.SplusObjects.AnalogInput TCP_STATUS;
        Crestron.Logos.SplusObjects.BufferInput RX;
        Crestron.Logos.SplusObjects.DigitalOutput ON_FB;
        Crestron.Logos.SplusObjects.DigitalOutput OFF_FB;
        Crestron.Logos.SplusObjects.DigitalOutput VOL_MUTED;
        Crestron.Logos.SplusObjects.DigitalOutput VOL_UNMUTED;
        Crestron.Logos.SplusObjects.DigitalOutput SAVING;
        Crestron.Logos.SplusObjects.DigitalOutput CHAN_CHANGE;
        Crestron.Logos.SplusObjects.AnalogOutput VOLUME_FB;
        Crestron.Logos.SplusObjects.StringOutput TX;
        Crestron.Logos.SplusObjects.StringOutput MESSAGE;
        Crestron.Logos.SplusObjects.StringOutput CCHANNEL;
        Crestron.Logos.SplusObjects.StringOutput CNAME;
        Crestron.Logos.SplusObjects.StringOutput CTITLE;
        Crestron.Logos.SplusObjects.StringOutput CDESCRIPTION;
        Crestron.Logos.SplusObjects.StringOutput CSTART;
        Crestron.Logos.SplusObjects.StringOutput CEND;
        Crestron.Logos.SplusObjects.StringOutput CDURATION;
        Crestron.Logos.SplusObjects.StringOutput CREMAINING;
        InOutArray<Crestron.Logos.SplusObjects.StringOutput> FAV_NAME;
        ushort LASTPRESS = 0;
        ushort TODO = 0;
        ushort SEND_OK = 0;
        ushort SEMAPHORE = 0;
        CrestronString FILENAME;
        CrestronString CHANNEL_MEM;
        private CrestronString PARSE (  SplusExecutionContext __context__, CrestronString DATA , CrestronString LEFT_NODE , CrestronString RIGHT_NODE , ushort START ) 
            { 
            CrestronString POM;
            POM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 256, this );
            
            ushort TOKEN_START = 0;
            ushort TOKEN_LEN = 0;
            
            
            __context__.SourceCodeLine = 67;
            TOKEN_START = (ushort) ( Functions.Find( LEFT_NODE , DATA , START ) ) ; 
            __context__.SourceCodeLine = 68;
            TOKEN_LEN = (ushort) ( Functions.Find( RIGHT_NODE , DATA , TOKEN_START ) ) ; 
            __context__.SourceCodeLine = 70;
            if ( Functions.TestForTrue  ( ( (Functions.BoolToInt (TOKEN_START == 0) | Functions.BoolToInt (TOKEN_LEN == 0)))  ) ) 
                {
                __context__.SourceCodeLine = 71;
                return ( "" ) ; 
                }
            
            __context__.SourceCodeLine = 73;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( ((TOKEN_LEN - TOKEN_START) - Functions.Length( LEFT_NODE )) > 255 ))  ) ) 
                {
                __context__.SourceCodeLine = 74;
                return ( "" ) ; 
                }
            
            __context__.SourceCodeLine = 76;
            POM  .UpdateValue ( Functions.Mid ( DATA ,  (int) ( (TOKEN_START + Functions.Length( LEFT_NODE )) ) ,  (int) ( ((TOKEN_LEN - TOKEN_START) - Functions.Length( LEFT_NODE )) ) )  ) ; 
            __context__.SourceCodeLine = 77;
            return ( POM ) ; 
            
            }
            
        private CrestronString REPLACESTRM (  SplusExecutionContext __context__, CrestronString SOURCE , CrestronString ORIG , CrestronString NEW ) 
            { 
            ushort STRPOZ = 0;
            ushort POZ = 0;
            
            
            __context__.SourceCodeLine = 84;
            STRPOZ = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 85;
            POZ = (ushort) ( Functions.Find( ORIG , SOURCE , STRPOZ ) ) ; 
            __context__.SourceCodeLine = 87;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( POZ > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 89;
                SOURCE  .UpdateValue ( Functions.Mid ( SOURCE ,  (int) ( 1 ) ,  (int) ( (POZ - 1) ) ) + NEW + Functions.Mid ( SOURCE ,  (int) ( (POZ + Functions.Length( ORIG )) ) ,  (int) ( (((Functions.Length( SOURCE ) - POZ) - Functions.Length( ORIG )) + 1) ) )  ) ; 
                __context__.SourceCodeLine = 90;
                STRPOZ = (ushort) ( (POZ + Functions.Length( NEW )) ) ; 
                __context__.SourceCodeLine = 91;
                POZ = (ushort) ( Functions.Find( ORIG , SOURCE , STRPOZ ) ) ; 
                __context__.SourceCodeLine = 87;
                } 
            
            __context__.SourceCodeLine = 94;
            return ( SOURCE ) ; 
            
            }
            
        private void GET_POWER_FN (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 99;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 100;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 99;
                }
            
            __context__.SourceCodeLine = 102;
            TX  .UpdateValue ( "GET /web/powerstate HTTP/1.0\r\n\r\n"  ) ; 
            
            }
            
        private void GET_VOLUME_FN (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 107;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 108;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 107;
                }
            
            __context__.SourceCodeLine = 110;
            TX  .UpdateValue ( "GET /web/vol HTTP/1.0\r\n\r\n"  ) ; 
            
            }
            
        private void GET_CURRENT_FN (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 115;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 116;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 115;
                }
            
            __context__.SourceCodeLine = 118;
            TX  .UpdateValue ( "GET /web/subservices HTTP/1.0\r\n\r\n"  ) ; 
            
            }
            
        private void GET_CURRENT_FULL_FN (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 123;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 124;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 123;
                }
            
            __context__.SourceCodeLine = 126;
            TX  .UpdateValue ( "GET /web/getcurrent HTTP/1.0\r\n\r\n"  ) ; 
            
            }
            
        private void GET_MAC_FN (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 131;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 132;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 131;
                }
            
            __context__.SourceCodeLine = 134;
            TX  .UpdateValue ( "GET /web/deviceinfo HTTP/1.0\r\n\r\n"  ) ; 
            
            }
            
        private void SET_POWER_ON (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 139;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 140;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 139;
                }
            
            __context__.SourceCodeLine = 142;
            TX  .UpdateValue ( "GET /web/powerstate?newstate=4 HTTP/1.0\r\n\r\n"  ) ; 
            __context__.SourceCodeLine = 144;
            Functions.Delay (  (int) ( 100 ) ) ; 
            __context__.SourceCodeLine = 145;
            GET_POWER_FN (  __context__  ) ; 
            __context__.SourceCodeLine = 147;
            TODO = (ushort) ( 1 ) ; 
            
            }
            
        private void SET_POWER_OFF (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 152;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 153;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 152;
                }
            
            __context__.SourceCodeLine = 155;
            TX  .UpdateValue ( "GET /web/powerstate?newstate=5 HTTP/1.0\r\n\r\n"  ) ; 
            __context__.SourceCodeLine = 157;
            Functions.Delay (  (int) ( 100 ) ) ; 
            __context__.SourceCodeLine = 158;
            GET_POWER_FN (  __context__  ) ; 
            
            }
            
        private void SET_POWER (  SplusExecutionContext __context__ ) 
            { 
            
            __context__.SourceCodeLine = 163;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                {
                __context__.SourceCodeLine = 164;
                Functions.Delay (  (int) ( 200 ) ) ; 
                __context__.SourceCodeLine = 163;
                }
            
            __context__.SourceCodeLine = 166;
            TX  .UpdateValue ( "GET /web/powerstate?newstate=0 HTTP/1.0\r\n\r\n"  ) ; 
            __context__.SourceCodeLine = 168;
            Functions.Delay (  (int) ( 100 ) ) ; 
            __context__.SourceCodeLine = 169;
            GET_POWER_FN (  __context__  ) ; 
            
            }
            
        private void HANDLE_POWER (  SplusExecutionContext __context__ ) 
            { 
            CrestronString POM;
            POM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 255, this );
            
            
            __context__.SourceCodeLine = 176;
            POM  .UpdateValue ( PARSE (  __context__ , RX, "<e2instandby>", "</e2instandby>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 177;
            if ( Functions.TestForTrue  ( ( Functions.Find( "true" , POM ))  ) ) 
                { 
                __context__.SourceCodeLine = 179;
                ON_FB  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 180;
                OFF_FB  .Value = (ushort) ( 1 ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 182;
                if ( Functions.TestForTrue  ( ( Functions.Find( "false" , POM ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 184;
                    OFF_FB  .Value = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 185;
                    ON_FB  .Value = (ushort) ( 1 ) ; 
                    } 
                
                }
            
            __context__.SourceCodeLine = 188;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (TODO == 3))  ) ) 
                {
                __context__.SourceCodeLine = 189;
                TODO = (ushort) ( 2 ) ; 
                }
            
            
            }
            
        private void HANDLE_VOLUME (  SplusExecutionContext __context__ ) 
            { 
            CrestronString POM;
            POM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 10, this );
            
            
            __context__.SourceCodeLine = 197;
            if ( Functions.TestForTrue  ( ( Functions.Find( "<e2ismuted>True</e2ismuted>" , RX ))  ) ) 
                { 
                __context__.SourceCodeLine = 199;
                VOL_UNMUTED  .Value = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 200;
                VOL_MUTED  .Value = (ushort) ( 1 ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 202;
                if ( Functions.TestForTrue  ( ( Functions.Find( "<e2ismuted>False</e2ismuted>" , RX ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 204;
                    VOL_MUTED  .Value = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 205;
                    VOL_UNMUTED  .Value = (ushort) ( 1 ) ; 
                    } 
                
                }
            
            __context__.SourceCodeLine = 208;
            POM  .UpdateValue ( PARSE (  __context__ , RX, "<e2current>", "</e2current>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 209;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Mod( (65535 * Functions.Atoi( POM )) , 100 ) >= 50 ))  ) ) 
                {
                __context__.SourceCodeLine = 210;
                VOLUME_FB  .Value = (ushort) ( (((65535 * Functions.Atoi( POM )) / 100) + 1) ) ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 212;
                VOLUME_FB  .Value = (ushort) ( ((65535 * Functions.Atoi( POM )) / 100) ) ; 
                }
            
            
            }
            
        private void HANDLE_CURRENT (  SplusExecutionContext __context__ ) 
            { 
            CrestronString CHANNELNAME;
            CHANNELNAME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 100, this );
            
            CrestronString TMPNAME;
            TMPNAME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 100, this );
            
            CrestronString TMPREF;
            TMPREF  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 100, this );
            
            CrestronString CHANNELREF;
            CHANNELREF  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 100, this );
            
            CrestronString LINE;
            CrestronString READBUF;
            CrestronString WRITEBUF;
            LINE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 100, this );
            READBUF  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 4096, this );
            WRITEBUF  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 4096, this );
            
            short NFILEHANDLE = 0;
            short I = 0;
            short POZ1 = 0;
            short POZ2 = 0;
            
            
            __context__.SourceCodeLine = 224;
            if ( Functions.TestForTrue  ( ( LASTPRESS)  ) ) 
                { 
                __context__.SourceCodeLine = 226;
                CHANNELREF  .UpdateValue ( PARSE (  __context__ , RX, "<e2servicereference>", "</e2servicereference>", (ushort)( 1 ))  ) ; 
                __context__.SourceCodeLine = 227;
                CHANNELNAME  .UpdateValue ( PARSE (  __context__ , RX, "<e2servicename>", "</e2servicename>", (ushort)( 1 ))  ) ; 
                __context__.SourceCodeLine = 229;
                StartFileOperations ( ) ; 
                __context__.SourceCodeLine = 230;
                NFILEHANDLE = (short) ( FileOpen( FILENAME ,(ushort) (0 | 32768) ) ) ; 
                __context__.SourceCodeLine = 232;
                ReadString (  (short) ( NFILEHANDLE ) ,  ref READBUF ) ; 
                __context__.SourceCodeLine = 234;
                FileClose (  (short) ( NFILEHANDLE ) ) ; 
                __context__.SourceCodeLine = 236;
                NFILEHANDLE = (short) ( FileOpen( FILENAME ,(ushort) ((1 | 256) | 32768) ) ) ; 
                __context__.SourceCodeLine = 237;
                POZ1 = (short) ( 1 ) ; 
                __context__.SourceCodeLine = 238;
                WRITEBUF  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 240;
                short __FN_FORSTART_VAL__1 = (short) ( 1 ) ;
                short __FN_FOREND_VAL__1 = (short)20; 
                int __FN_FORSTEP_VAL__1 = (int)1; 
                for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (short)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 242;
                    POZ2 = (short) ( Functions.Find( "\r\n" , READBUF , (POZ1 + 1) ) ) ; 
                    __context__.SourceCodeLine = 243;
                    if ( Functions.TestForTrue  ( ( Functions.BoolToInt (I == LASTPRESS))  ) ) 
                        { 
                        __context__.SourceCodeLine = 245;
                        MakeString ( LINE , "<fav{0:d}><name>{1}</name><ref>{2}</ref></fav{3:d}>", (short)I, CHANNELNAME , CHANNELREF , (short)I) ; 
                        __context__.SourceCodeLine = 246;
                        FAV_NAME [ I]  .UpdateValue ( CHANNELNAME  ) ; 
                        } 
                    
                    else 
                        { 
                        __context__.SourceCodeLine = 250;
                        LINE  .UpdateValue ( Functions.Mid ( READBUF ,  (int) ( POZ1 ) ,  (int) ( (POZ2 - POZ1) ) )  ) ; 
                        __context__.SourceCodeLine = 251;
                        TMPNAME  .UpdateValue ( PARSE (  __context__ , LINE, "<name>", "</name>", (ushort)( 1 ))  ) ; 
                        __context__.SourceCodeLine = 252;
                        TMPREF  .UpdateValue ( PARSE (  __context__ , LINE, "<ref>", "</ref>", (ushort)( 1 ))  ) ; 
                        __context__.SourceCodeLine = 253;
                        MakeString ( LINE , "<fav{0:d}><name>{1}</name><ref>{2}</ref></fav{3:d}>", (short)I, TMPNAME , TMPREF , (short)I) ; 
                        __context__.SourceCodeLine = 254;
                        FAV_NAME [ I]  .UpdateValue ( TMPNAME  ) ; 
                        } 
                    
                    __context__.SourceCodeLine = 256;
                    
                    __context__.SourceCodeLine = 259;
                    POZ1 = (short) ( (POZ2 + 2) ) ; 
                    __context__.SourceCodeLine = 260;
                    MakeString ( WRITEBUF , "{0}{1}\r\n", WRITEBUF , LINE ) ; 
                    __context__.SourceCodeLine = 240;
                    } 
                
                __context__.SourceCodeLine = 264;
                I = (short) ( WriteString( (short)( NFILEHANDLE ) , WRITEBUF ) ) ; 
                __context__.SourceCodeLine = 266;
                FileClose (  (short) ( NFILEHANDLE ) ) ; 
                __context__.SourceCodeLine = 267;
                EndFileOperations ( ) ; 
                __context__.SourceCodeLine = 269;
                LASTPRESS = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 270;
                MESSAGE  .UpdateValue ( ""  ) ; 
                __context__.SourceCodeLine = 271;
                SAVING  .Value = (ushort) ( 0 ) ; 
                } 
            
            
            }
            
        private CrestronString TIME2HHMMSS (  SplusExecutionContext __context__, CrestronString STR_T ) 
            { 
            CrestronString POM;
            POM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 25, this );
            
            ushort T = 0;
            
            
            __context__.SourceCodeLine = 281;
            T = (ushort) ( Functions.Atoi( STR_T ) ) ; 
            __context__.SourceCodeLine = 285;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( T < 60 ))  ) ) 
                {
                __context__.SourceCodeLine = 286;
                MakeString ( POM , "0:00") ; 
                }
            
            else 
                {
                __context__.SourceCodeLine = 287;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( T < 3600 ))  ) ) 
                    {
                    __context__.SourceCodeLine = 288;
                    MakeString ( POM , "0:{0:d2}", (short)(T / 60)) ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 290;
                    MakeString ( POM , "{0:d}:{1:d2}", (short)(T / 3600), (short)((T - ((T / 3600) * 3600)) / 60)) ; 
                    }
                
                }
            
            __context__.SourceCodeLine = 292;
            return ( POM ) ; 
            
            }
            
        private CrestronString UNIX2HHMM (  SplusExecutionContext __context__, uint T ) 
            { 
            CrestronString POM;
            POM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 25, this );
            
            uint DAYS = 0;
            uint SECS = 0;
            
            ushort REMDAYS = 0;
            ushort REMSECS = 0;
            ushort REMYEARS = 0;
            
            ushort QC_CYCLES = 0;
            ushort C_CYCLES = 0;
            ushort Q_CYCLES = 0;
            
            ushort YEARS = 0;
            ushort MONTHS = 0;
            
            ushort WDAY = 0;
            ushort YDAY = 0;
            ushort LEAP = 0;
            
            ushort [] DAYS_IN_MONTH;
            DAYS_IN_MONTH  = new ushort[ 13 ];
            
            
            __context__.SourceCodeLine = 305;
            DAYS_IN_MONTH [ 1] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 306;
            DAYS_IN_MONTH [ 2] = (ushort) ( 30 ) ; 
            __context__.SourceCodeLine = 307;
            DAYS_IN_MONTH [ 3] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 308;
            DAYS_IN_MONTH [ 4] = (ushort) ( 30 ) ; 
            __context__.SourceCodeLine = 309;
            DAYS_IN_MONTH [ 5] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 310;
            DAYS_IN_MONTH [ 6] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 311;
            DAYS_IN_MONTH [ 7] = (ushort) ( 30 ) ; 
            __context__.SourceCodeLine = 312;
            DAYS_IN_MONTH [ 8] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 313;
            DAYS_IN_MONTH [ 9] = (ushort) ( 30 ) ; 
            __context__.SourceCodeLine = 314;
            DAYS_IN_MONTH [ 10] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 315;
            DAYS_IN_MONTH [ 11] = (ushort) ( 31 ) ; 
            __context__.SourceCodeLine = 316;
            DAYS_IN_MONTH [ 12] = (ushort) ( 29 ) ; 
            __context__.SourceCodeLine = 318;
            SECS = (uint) ( (T - 951868800) ) ; 
            __context__.SourceCodeLine = 319;
            DAYS = (uint) ( (SECS / 86400) ) ; 
            __context__.SourceCodeLine = 320;
            REMSECS = (ushort) ( Mod( SECS , 86400 ) ) ; 
            __context__.SourceCodeLine = 321;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( REMSECS < 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 322;
                REMSECS = (ushort) ( (REMSECS + 86400) ) ; 
                __context__.SourceCodeLine = 323;
                DAYS = (uint) ( (DAYS - 1) ) ; 
                } 
            
            __context__.SourceCodeLine = 326;
            WDAY = (ushort) ( Mod( (3 + DAYS) , 7 ) ) ; 
            __context__.SourceCodeLine = 327;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( WDAY < 0 ))  ) ) 
                {
                __context__.SourceCodeLine = 327;
                WDAY = (ushort) ( (WDAY + 7) ) ; 
                }
            
            __context__.SourceCodeLine = 329;
            QC_CYCLES = (ushort) ( (DAYS / 146097) ) ; 
            __context__.SourceCodeLine = 330;
            REMDAYS = (ushort) ( Mod( DAYS , 146097 ) ) ; 
            __context__.SourceCodeLine = 331;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( REMDAYS < 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 332;
                REMDAYS = (ushort) ( (REMDAYS + 146097) ) ; 
                __context__.SourceCodeLine = 333;
                QC_CYCLES = (ushort) ( (QC_CYCLES - 1) ) ; 
                } 
            
            __context__.SourceCodeLine = 336;
            C_CYCLES = (ushort) ( (REMDAYS / 36524) ) ; 
            __context__.SourceCodeLine = 337;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (C_CYCLES == 4))  ) ) 
                {
                __context__.SourceCodeLine = 337;
                C_CYCLES = (ushort) ( (C_CYCLES - 1) ) ; 
                }
            
            __context__.SourceCodeLine = 338;
            REMDAYS = (ushort) ( (REMDAYS - (C_CYCLES * 36524)) ) ; 
            __context__.SourceCodeLine = 340;
            Q_CYCLES = (ushort) ( (REMDAYS / 1461) ) ; 
            __context__.SourceCodeLine = 341;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (Q_CYCLES == 25))  ) ) 
                {
                __context__.SourceCodeLine = 341;
                Q_CYCLES = (ushort) ( (Q_CYCLES - 1) ) ; 
                }
            
            __context__.SourceCodeLine = 342;
            REMDAYS = (ushort) ( (REMDAYS - (Q_CYCLES * 1461)) ) ; 
            __context__.SourceCodeLine = 344;
            REMYEARS = (ushort) ( (REMDAYS / 365) ) ; 
            __context__.SourceCodeLine = 345;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (REMYEARS == 4))  ) ) 
                {
                __context__.SourceCodeLine = 345;
                REMYEARS = (ushort) ( (REMYEARS - 1) ) ; 
                }
            
            __context__.SourceCodeLine = 346;
            REMDAYS = (ushort) ( (REMDAYS - (REMYEARS * 365)) ) ; 
            __context__.SourceCodeLine = 348;
            LEAP = (ushort) ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.Not( REMYEARS ) ) && Functions.TestForTrue ( Functions.BoolToInt ( (Functions.TestForTrue ( Q_CYCLES ) || Functions.TestForTrue ( Functions.Not( C_CYCLES ) )) ) )) ) ) ; 
            __context__.SourceCodeLine = 349;
            YDAY = (ushort) ( (((REMDAYS + 31) + 28) + LEAP) ) ; 
            __context__.SourceCodeLine = 350;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( YDAY >= (365 + LEAP) ))  ) ) 
                {
                __context__.SourceCodeLine = 350;
                YDAY = (ushort) ( (YDAY - (365 + LEAP)) ) ; 
                }
            
            __context__.SourceCodeLine = 352;
            YEARS = (ushort) ( (((REMYEARS + (4 * Q_CYCLES)) + (100 * C_CYCLES)) + (400 * QC_CYCLES)) ) ; 
            __context__.SourceCodeLine = 354;
            MONTHS = (ushort) ( 1 ) ; 
            __context__.SourceCodeLine = 355;
            while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( DAYS_IN_MONTH[ MONTHS ] <= REMDAYS ))  ) ) 
                { 
                __context__.SourceCodeLine = 357;
                REMDAYS = (ushort) ( (REMDAYS - DAYS_IN_MONTH[ MONTHS ]) ) ; 
                __context__.SourceCodeLine = 358;
                MONTHS = (ushort) ( (MONTHS + 1) ) ; 
                __context__.SourceCodeLine = 355;
                } 
            
            __context__.SourceCodeLine = 361;
            MakeString ( POM , "{0:d}:{1:d}", (short)((REMSECS / 3600) + Functions.GetGmtOffset()), (short)Mod( (REMSECS / 60) , 60 )) ; 
            __context__.SourceCodeLine = 362;
            return ( POM ) ; 
            
            }
            
        private void HANDLE_CURRENT_FULL (  SplusExecutionContext __context__ ) 
            { 
            CrestronString SSTART;
            CrestronString SDURATION;
            SSTART  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 25, this );
            SDURATION  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 25, this );
            
            
            __context__.SourceCodeLine = 369;
            CCHANNEL  .UpdateValue ( PARSE (  __context__ , RX, "<e2servicename>", "</e2servicename>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 370;
            CNAME  .UpdateValue ( PARSE (  __context__ , RX, "<e2eventname>", "</e2eventname>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 371;
            CTITLE  .UpdateValue ( PARSE (  __context__ , RX, "<e2eventdescription>", "</e2eventdescription>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 372;
            CDESCRIPTION  .UpdateValue ( PARSE (  __context__ , RX, "<e2eventdescriptionextended>", "</e2eventdescriptionextended>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 373;
            SSTART  .UpdateValue ( PARSE (  __context__ , RX, "<e2eventstart>", "</e2eventstart>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 374;
            SDURATION  .UpdateValue ( PARSE (  __context__ , RX, "<e2eventduration>", "</e2eventduration>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 375;
            CSTART  .UpdateValue ( UNIX2HHMM (  __context__ , (uint)( Functions.Atol( SSTART ) ))  ) ; 
            __context__.SourceCodeLine = 376;
            CEND  .UpdateValue ( UNIX2HHMM (  __context__ , (uint)( (Functions.Atol( SSTART ) + Functions.Atol( SDURATION )) ))  ) ; 
            __context__.SourceCodeLine = 377;
            CDURATION  .UpdateValue ( TIME2HHMMSS (  __context__ , SDURATION)  ) ; 
            __context__.SourceCodeLine = 378;
            CREMAINING  .UpdateValue ( TIME2HHMMSS (  __context__ , PARSE( __context__ , RX , "<e2eventremaining>" , "</e2eventremaining>" , (ushort)( 1 ) ))  ) ; 
            
            }
            
        private void HANDLE_MAC (  SplusExecutionContext __context__ ) 
            { 
            short NFILEHANDLE = 0;
            short I = 0;
            short POZ1 = 0;
            short POZ2 = 0;
            
            CrestronString SBUF;
            CrestronString READBUF;
            SBUF  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 4096, this );
            READBUF  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 4096, this );
            
            CrestronString LINE;
            LINE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 100, this );
            
            
            __context__.SourceCodeLine = 387;
            SBUF  .UpdateValue ( PARSE (  __context__ , RX, "<e2mac>", "</e2mac>", (ushort)( 1 ))  ) ; 
            __context__.SourceCodeLine = 388;
            FILENAME  .UpdateValue ( "\\NVRAM\\DB_" + REPLACESTRM (  __context__ , SBUF, ":", "-") + ".db"  ) ; 
            __context__.SourceCodeLine = 389;
            SBUF  .UpdateValue ( ""  ) ; 
            __context__.SourceCodeLine = 391;
            StartFileOperations ( ) ; 
            __context__.SourceCodeLine = 392;
            NFILEHANDLE = (short) ( FileOpen( FILENAME ,(ushort) (0 | 32768) ) ) ; 
            __context__.SourceCodeLine = 393;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NFILEHANDLE == Functions.ToSignedLongInteger( -( 3024 ) )))  ) ) 
                { 
                __context__.SourceCodeLine = 395;
                NFILEHANDLE = (short) ( FileOpen( FILENAME ,(ushort) (((1 | 256) | 8) | 32768) ) ) ; 
                __context__.SourceCodeLine = 397;
                short __FN_FORSTART_VAL__1 = (short) ( 1 ) ;
                short __FN_FOREND_VAL__1 = (short)20; 
                int __FN_FORSTEP_VAL__1 = (int)1; 
                for ( I  = __FN_FORSTART_VAL__1; (__FN_FORSTEP_VAL__1 > 0)  ? ( (I  >= __FN_FORSTART_VAL__1) && (I  <= __FN_FOREND_VAL__1) ) : ( (I  <= __FN_FORSTART_VAL__1) && (I  >= __FN_FOREND_VAL__1) ) ; I  += (short)__FN_FORSTEP_VAL__1) 
                    { 
                    __context__.SourceCodeLine = 399;
                    MakeString ( SBUF , "<fav{0:d}><name></name><ref></ref></fav{1:d}>\r\n", (short)I, (short)I) ; 
                    __context__.SourceCodeLine = 400;
                    POZ1 = (short) ( WriteString( (short)( NFILEHANDLE ) , SBUF ) ) ; 
                    __context__.SourceCodeLine = 397;
                    } 
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 403;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NFILEHANDLE >= 0 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 405;
                    ReadString (  (short) ( NFILEHANDLE ) ,  ref READBUF ) ; 
                    __context__.SourceCodeLine = 406;
                    POZ1 = (short) ( 1 ) ; 
                    __context__.SourceCodeLine = 408;
                    
                    __context__.SourceCodeLine = 412;
                    short __FN_FORSTART_VAL__2 = (short) ( 1 ) ;
                    short __FN_FOREND_VAL__2 = (short)20; 
                    int __FN_FORSTEP_VAL__2 = (int)1; 
                    for ( I  = __FN_FORSTART_VAL__2; (__FN_FORSTEP_VAL__2 > 0)  ? ( (I  >= __FN_FORSTART_VAL__2) && (I  <= __FN_FOREND_VAL__2) ) : ( (I  <= __FN_FORSTART_VAL__2) && (I  >= __FN_FOREND_VAL__2) ) ; I  += (short)__FN_FORSTEP_VAL__2) 
                        { 
                        __context__.SourceCodeLine = 414;
                        POZ2 = (short) ( Functions.Find( "\r\n" , READBUF , (POZ1 + 1) ) ) ; 
                        __context__.SourceCodeLine = 415;
                        LINE  .UpdateValue ( Functions.Mid ( READBUF ,  (int) ( POZ1 ) ,  (int) ( (POZ2 - POZ1) ) )  ) ; 
                        __context__.SourceCodeLine = 416;
                        
                        __context__.SourceCodeLine = 419;
                        FAV_NAME [ I]  .UpdateValue ( PARSE (  __context__ , LINE, "<name>", "</name>", (ushort)( 1 ))  ) ; 
                        __context__.SourceCodeLine = 421;
                        POZ1 = (short) ( (POZ2 + 2) ) ; 
                        __context__.SourceCodeLine = 412;
                        } 
                    
                    } 
                
                else 
                    {
                    __context__.SourceCodeLine = 425;
                    MakeString ( MESSAGE , "unable to open file [code:{0:d}]", (short)NFILEHANDLE) ; 
                    }
                
                }
            
            __context__.SourceCodeLine = 427;
            FileClose (  (short) ( NFILEHANDLE ) ) ; 
            __context__.SourceCodeLine = 428;
            EndFileOperations ( ) ; 
            
            }
            
        private CrestronString READLINE (  SplusExecutionContext __context__ ) 
            { 
            CrestronString LINE;
            CrestronString POM;
            LINE  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 100, this );
            POM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 4096, this );
            
            short NFILEHANDLE = 0;
            short I = 0;
            short POZ1 = 0;
            short POZ2 = 0;
            
            
            __context__.SourceCodeLine = 436;
            if ( Functions.TestForTrue  ( ( LASTPRESS)  ) ) 
                { 
                __context__.SourceCodeLine = 438;
                StartFileOperations ( ) ; 
                __context__.SourceCodeLine = 439;
                NFILEHANDLE = (short) ( FileOpen( FILENAME ,(ushort) (2 | 32768) ) ) ; 
                __context__.SourceCodeLine = 440;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( NFILEHANDLE < 0 ))  ) ) 
                    { 
                    __context__.SourceCodeLine = 442;
                    MakeString ( MESSAGE , "unable to open database file [code:{0:d}]", (short)NFILEHANDLE) ; 
                    __context__.SourceCodeLine = 443;
                    LASTPRESS = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 444;
                    return ( "" ) ; 
                    } 
                
                else 
                    { 
                    __context__.SourceCodeLine = 449;
                    ReadString (  (short) ( NFILEHANDLE ) ,  ref POM ) ; 
                    __context__.SourceCodeLine = 450;
                    FileClose (  (short) ( NFILEHANDLE ) ) ; 
                    __context__.SourceCodeLine = 451;
                    EndFileOperations ( ) ; 
                    __context__.SourceCodeLine = 453;
                    I = (short) ( 0 ) ; 
                    __context__.SourceCodeLine = 454;
                    POZ1 = (short) ( 1 ) ; 
                    __context__.SourceCodeLine = 455;
                    do 
                        { 
                        __context__.SourceCodeLine = 457;
                        POZ2 = (short) ( Functions.Find( "\r\n" , POM , (POZ1 + 1) ) ) ; 
                        __context__.SourceCodeLine = 458;
                        LINE  .UpdateValue ( Functions.Mid ( POM ,  (int) ( POZ1 ) ,  (int) ( (POZ2 - POZ1) ) )  ) ; 
                        __context__.SourceCodeLine = 459;
                        POZ1 = (short) ( (POZ2 + 2) ) ; 
                        __context__.SourceCodeLine = 460;
                        I = (short) ( (I + 1) ) ; 
                        } 
                    while (false == ( Functions.TestForTrue  ( Functions.BoolToInt (I == LASTPRESS)) )); 
                    } 
                
                __context__.SourceCodeLine = 465;
                LASTPRESS = (ushort) ( 0 ) ; 
                __context__.SourceCodeLine = 466;
                return ( PARSE (  __context__ , LINE, "<ref>", "</ref>", (ushort)( 1 )) ) ; 
                } 
            
            
            return ""; // default return value (none specified in module)
            }
            
        private void LOAD_FAV (  SplusExecutionContext __context__ ) 
            { 
            CrestronString POM;
            POM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 50, this );
            
            
            __context__.SourceCodeLine = 474;
            POM  .UpdateValue ( READLINE (  __context__  )  ) ; 
            __context__.SourceCodeLine = 475;
            if ( Functions.TestForTrue  ( ( Functions.Length( POM ))  ) ) 
                { 
                __context__.SourceCodeLine = 477;
                while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                    {
                    __context__.SourceCodeLine = 478;
                    Functions.Delay (  (int) ( 200 ) ) ; 
                    __context__.SourceCodeLine = 477;
                    }
                
                __context__.SourceCodeLine = 479;
                MakeString ( TX , "GET /web/zap?sRef={0} HTTP/1.0\r\n\r\n", POM ) ; 
                } 
            
            else 
                {
                __context__.SourceCodeLine = 482;
                MESSAGE  .UpdateValue ( "empty position"  ) ; 
                }
            
            
            }
            
        private void SET_CHANNEL (  SplusExecutionContext __context__ ) 
            { 
            ushort NR = 0;
            
            
            __context__.SourceCodeLine = 489;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( Functions.Length( CHANNEL_MEM ) > 0 ))  ) ) 
                { 
                __context__.SourceCodeLine = 491;
                while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                    {
                    __context__.SourceCodeLine = 492;
                    Functions.Delay (  (int) ( 200 ) ) ; 
                    __context__.SourceCodeLine = 491;
                    }
                
                __context__.SourceCodeLine = 494;
                NR = (ushort) ( Functions.Atoi( Functions.Left( CHANNEL_MEM , (int)( 1 ) ) ) ) ; 
                __context__.SourceCodeLine = 495;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (NR == 0))  ) ) 
                    {
                    __context__.SourceCodeLine = 496;
                    MakeString ( TX , "GET /web/remotecontrol?command=11 HTTP/1.0\r\n\r\n") ; 
                    }
                
                else 
                    {
                    __context__.SourceCodeLine = 498;
                    MakeString ( TX , "GET /web/remotecontrol?command={0:d} HTTP/1.0\r\n\r\n", (short)(NR + 1)) ; 
                    }
                
                __context__.SourceCodeLine = 501;
                CHANNEL_MEM  .UpdateValue ( Functions.Mid ( CHANNEL_MEM ,  (int) ( 2 ) ,  (int) ( 10 ) )  ) ; 
                __context__.SourceCodeLine = 503;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (Functions.Length( CHANNEL_MEM ) == 0))  ) ) 
                    {
                    __context__.SourceCodeLine = 504;
                    SEND_OK = (ushort) ( 1 ) ; 
                    }
                
                } 
            
            else 
                {
                __context__.SourceCodeLine = 506;
                if ( Functions.TestForTrue  ( ( SEND_OK)  ) ) 
                    { 
                    __context__.SourceCodeLine = 508;
                    while ( Functions.TestForTrue  ( ( Functions.BoolToInt ( (Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 1) ) || Functions.TestForTrue ( Functions.BoolToInt (TCP_STATUS  .UshortValue == 2) )) ))  ) ) 
                        {
                        __context__.SourceCodeLine = 509;
                        Functions.Delay (  (int) ( 200 ) ) ; 
                        __context__.SourceCodeLine = 508;
                        }
                    
                    __context__.SourceCodeLine = 510;
                    MakeString ( TX , "GET /web/remotecontrol?command=352 HTTP/1.0\r\n\r\n") ; 
                    __context__.SourceCodeLine = 511;
                    SEND_OK = (ushort) ( 0 ) ; 
                    } 
                
                }
            
            __context__.SourceCodeLine = 514;
            Functions.Pulse ( 10, CHAN_CHANGE ) ; 
            
            }
            
        object GO_PARSE_OnPush_0 ( Object __EventInfo__ )
        
            { 
            Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
            try
            {
                SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
                
                __context__.SourceCodeLine = 522;
                if ( Functions.TestForTrue  ( ( Functions.BoolToInt (SEMAPHORE == 1))  ) ) 
                    { 
                    __context__.SourceCodeLine = 524;
                    SEMAPHORE = (ushort) ( 0 ) ; 
                    __context__.SourceCodeLine = 525;
                    if ( Functions.TestForTrue  ( ( Functions.Find( "</e2instandby>" , RX ))  ) ) 
                        {
                        __context__.SourceCodeLine = 526;
                        HANDLE_POWER (  __context__  ) ; 
                        }
                    
                    else 
                        {
                        __context__.SourceCodeLine = 527;
                        if ( Functions.TestForTrue  ( ( Functions.Find( "<e2deviceinfo>" , RX ))  ) ) 
                            {
                            __context__.SourceCodeLine = 528;
                            HANDLE_MAC (  __context__  ) ; 
                            }
                        
                        else 
                            {
                            __context__.SourceCodeLine = 529;
                            if ( Functions.TestForTrue  ( ( Functions.Find( "<e2servicelist>" , RX ))  ) ) 
                                {
                                __context__.SourceCodeLine = 530;
                                HANDLE_CURRENT (  __context__  ) ; 
                                }
                            
                            else 
                                {
                                __context__.SourceCodeLine = 531;
                                if ( Functions.TestForTrue  ( ( Functions.Find( "<e2currentserviceinformation>" , RX ))  ) ) 
                                    {
                                    __context__.SourceCodeLine = 532;
                                    HANDLE_CURRENT_FULL (  __context__  ) ; 
                                    }
                                
                                else 
                                    {
                                    __context__.SourceCodeLine = 533;
                                    if ( Functions.TestForTrue  ( ( Functions.Find( "<e2volume>" , RX ))  ) ) 
                                        {
                                        __context__.SourceCodeLine = 534;
                                        HANDLE_VOLUME (  __context__  ) ; 
                                        }
                                    
                                    else 
                                        {
                                        __context__.SourceCodeLine = 535;
                                        if ( Functions.TestForTrue  ( ( Functions.Find( "<e2remotecontrol>" , RX ))  ) ) 
                                            {
                                            __context__.SourceCodeLine = 536;
                                            SET_CHANNEL (  __context__  ) ; 
                                            }
                                        
                                        else 
                                            {
                                            __context__.SourceCodeLine = 538;
                                            Functions.ClearBuffer ( RX ) ; 
                                            }
                                        
                                        }
                                    
                                    }
                                
                                }
                            
                            }
                        
                        }
                    
                    __context__.SourceCodeLine = 543;
                    switch ((int)TODO)
                    
                        { 
                        case 3 : 
                        
                            { 
                            __context__.SourceCodeLine = 547;
                            TODO = (ushort) ( 2 ) ; 
                            __context__.SourceCodeLine = 548;
                            GET_POWER_FN (  __context__  ) ; 
                            __context__.SourceCodeLine = 549;
                            break ; 
                            } 
                        
                        goto case 2 ;
                        case 2 : 
                        
                            { 
                            __context__.SourceCodeLine = 553;
                            TODO = (ushort) ( 1 ) ; 
                            __context__.SourceCodeLine = 554;
                            GET_VOLUME_FN (  __context__  ) ; 
                            __context__.SourceCodeLine = 555;
                            break ; 
                            } 
                        
                        goto case 1 ;
                        case 1 : 
                        
                            { 
                            __context__.SourceCodeLine = 559;
                            TODO = (ushort) ( 0 ) ; 
                            __context__.SourceCodeLine = 560;
                            GET_MAC_FN (  __context__  ) ; 
                            __context__.SourceCodeLine = 561;
                            break ; 
                            } 
                        
                        break;
                        } 
                        
                    
                    __context__.SourceCodeLine = 565;
                    Functions.ClearBuffer ( RX ) ; 
                    __context__.SourceCodeLine = 567;
                    SEMAPHORE = (ushort) ( 1 ) ; 
                    } 
                
                
                
            }
            catch(Exception e) { ObjectCatchHandler(e); }
            finally { ObjectFinallyHandler( __SignalEventArg__ ); }
            return this;
            
        }
        
    object POWER_ON_OnPush_1 ( Object __EventInfo__ )
    
        { 
        Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
        try
        {
            SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
            
            __context__.SourceCodeLine = 571;
            SET_POWER_ON (  __context__  ) ; 
            
            
        }
        catch(Exception e) { ObjectCatchHandler(e); }
        finally { ObjectFinallyHandler( __SignalEventArg__ ); }
        return this;
        
    }
    
object POWER_OFF_OnPush_2 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 572;
        SET_POWER_OFF (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object POWER_OnPush_3 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 573;
        SET_POWER (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CUR_REFREH_OnPush_4 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 574;
        GET_CURRENT_FULL_FN (  __context__  ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object FAV_OnPush_5 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 578;
        LASTPRESS = (ushort) ( Functions.GetLastModifiedArrayIndex( __SignalEventArg__ ) ) ; 
        __context__.SourceCodeLine = 579;
        MESSAGE  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 581;
        if ( Functions.TestForTrue  ( ( Functions.BoolToInt ( LASTPRESS > 20 ))  ) ) 
            {
            __context__.SourceCodeLine = 582;
            Functions.TerminateEvent (); 
            }
        
        __context__.SourceCodeLine = 584;
        CreateWait ( "FAVORITETIMER" , 100 , FAVORITETIMER_Callback ) ;
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

public void FAVORITETIMER_CallbackFn( object stateInfo )
{

    try
    {
        Wait __LocalWait__ = (Wait)stateInfo;
        SplusExecutionContext __context__ = SplusThreadStartCode(__LocalWait__);
        __LocalWait__.RemoveFromList();
        
            
            __context__.SourceCodeLine = 586;
            if ( Functions.TestForTrue  ( ( Functions.BoolToInt (FAV[ LASTPRESS ] .Value == 1))  ) ) 
                { 
                __context__.SourceCodeLine = 588;
                MESSAGE  .UpdateValue ( "saving.. please wait"  ) ; 
                __context__.SourceCodeLine = 589;
                SAVING  .Value = (ushort) ( 1 ) ; 
                __context__.SourceCodeLine = 590;
                GET_CURRENT_FN (  __context__  ) ; 
                } 
            
            else 
                { 
                __context__.SourceCodeLine = 594;
                LOAD_FAV (  __context__  ) ; 
                __context__.SourceCodeLine = 595;
                Functions.Pulse ( 10, CHAN_CHANGE ) ; 
                } 
            
            
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler(); }
    
}

object FAV_OnRelease_6 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 600;
        RetimeWait ( (int)0, "FAVORITETIMER" ) ; 
        
        
    }
    catch(Exception e) { ObjectCatchHandler(e); }
    finally { ObjectFinallyHandler( __SignalEventArg__ ); }
    return this;
    
}

object CHANNEL_OnChange_7 ( Object __EventInfo__ )

    { 
    Crestron.Logos.SplusObjects.SignalEventArgs __SignalEventArg__ = (Crestron.Logos.SplusObjects.SignalEventArgs)__EventInfo__;
    try
    {
        SplusExecutionContext __context__ = SplusThreadStartCode(__SignalEventArg__);
        
        __context__.SourceCodeLine = 602;
        CHANNEL_MEM  .UpdateValue ( Functions.ItoA (  (int) ( CHANNEL  .UshortValue ) )  ) ; 
        __context__.SourceCodeLine = 602;
        SET_CHANNEL (  __context__  ) ; 
        
        
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
        
        __context__.SourceCodeLine = 609;
        FILENAME  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 610;
        MESSAGE  .UpdateValue ( ""  ) ; 
        __context__.SourceCodeLine = 611;
        TODO = (ushort) ( 3 ) ; 
        __context__.SourceCodeLine = 612;
        SEND_OK = (ushort) ( 0 ) ; 
        __context__.SourceCodeLine = 613;
        WaitForInitializationComplete ( ) ; 
        __context__.SourceCodeLine = 615;
        GET_POWER_FN (  __context__  ) ; 
        __context__.SourceCodeLine = 616;
        SEMAPHORE = (ushort) ( 1 ) ; 
        
        
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
    FILENAME  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 50, this );
    CHANNEL_MEM  = new CrestronString( Crestron.Logos.SplusObjects.CrestronStringEncoding.eEncodingUTF16, 10, this );
    
    GO_PARSE = new Crestron.Logos.SplusObjects.DigitalInput( GO_PARSE__DigitalInput__, this );
    m_DigitalInputList.Add( GO_PARSE__DigitalInput__, GO_PARSE );
    
    POWER_ON = new Crestron.Logos.SplusObjects.DigitalInput( POWER_ON__DigitalInput__, this );
    m_DigitalInputList.Add( POWER_ON__DigitalInput__, POWER_ON );
    
    POWER_OFF = new Crestron.Logos.SplusObjects.DigitalInput( POWER_OFF__DigitalInput__, this );
    m_DigitalInputList.Add( POWER_OFF__DigitalInput__, POWER_OFF );
    
    POWER = new Crestron.Logos.SplusObjects.DigitalInput( POWER__DigitalInput__, this );
    m_DigitalInputList.Add( POWER__DigitalInput__, POWER );
    
    CUR_REFREH = new Crestron.Logos.SplusObjects.DigitalInput( CUR_REFREH__DigitalInput__, this );
    m_DigitalInputList.Add( CUR_REFREH__DigitalInput__, CUR_REFREH );
    
    FAV = new InOutArray<DigitalInput>( 20, this );
    for( uint i = 0; i < 20; i++ )
    {
        FAV[i+1] = new Crestron.Logos.SplusObjects.DigitalInput( FAV__DigitalInput__ + i, FAV__DigitalInput__, this );
        m_DigitalInputList.Add( FAV__DigitalInput__ + i, FAV[i+1] );
    }
    
    ON_FB = new Crestron.Logos.SplusObjects.DigitalOutput( ON_FB__DigitalOutput__, this );
    m_DigitalOutputList.Add( ON_FB__DigitalOutput__, ON_FB );
    
    OFF_FB = new Crestron.Logos.SplusObjects.DigitalOutput( OFF_FB__DigitalOutput__, this );
    m_DigitalOutputList.Add( OFF_FB__DigitalOutput__, OFF_FB );
    
    VOL_MUTED = new Crestron.Logos.SplusObjects.DigitalOutput( VOL_MUTED__DigitalOutput__, this );
    m_DigitalOutputList.Add( VOL_MUTED__DigitalOutput__, VOL_MUTED );
    
    VOL_UNMUTED = new Crestron.Logos.SplusObjects.DigitalOutput( VOL_UNMUTED__DigitalOutput__, this );
    m_DigitalOutputList.Add( VOL_UNMUTED__DigitalOutput__, VOL_UNMUTED );
    
    SAVING = new Crestron.Logos.SplusObjects.DigitalOutput( SAVING__DigitalOutput__, this );
    m_DigitalOutputList.Add( SAVING__DigitalOutput__, SAVING );
    
    CHAN_CHANGE = new Crestron.Logos.SplusObjects.DigitalOutput( CHAN_CHANGE__DigitalOutput__, this );
    m_DigitalOutputList.Add( CHAN_CHANGE__DigitalOutput__, CHAN_CHANGE );
    
    CHANNEL = new Crestron.Logos.SplusObjects.AnalogInput( CHANNEL__AnalogSerialInput__, this );
    m_AnalogInputList.Add( CHANNEL__AnalogSerialInput__, CHANNEL );
    
    TCP_STATUS = new Crestron.Logos.SplusObjects.AnalogInput( TCP_STATUS__AnalogSerialInput__, this );
    m_AnalogInputList.Add( TCP_STATUS__AnalogSerialInput__, TCP_STATUS );
    
    VOLUME_FB = new Crestron.Logos.SplusObjects.AnalogOutput( VOLUME_FB__AnalogSerialOutput__, this );
    m_AnalogOutputList.Add( VOLUME_FB__AnalogSerialOutput__, VOLUME_FB );
    
    TX = new Crestron.Logos.SplusObjects.StringOutput( TX__AnalogSerialOutput__, this );
    m_StringOutputList.Add( TX__AnalogSerialOutput__, TX );
    
    MESSAGE = new Crestron.Logos.SplusObjects.StringOutput( MESSAGE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( MESSAGE__AnalogSerialOutput__, MESSAGE );
    
    CCHANNEL = new Crestron.Logos.SplusObjects.StringOutput( CCHANNEL__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CCHANNEL__AnalogSerialOutput__, CCHANNEL );
    
    CNAME = new Crestron.Logos.SplusObjects.StringOutput( CNAME__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CNAME__AnalogSerialOutput__, CNAME );
    
    CTITLE = new Crestron.Logos.SplusObjects.StringOutput( CTITLE__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CTITLE__AnalogSerialOutput__, CTITLE );
    
    CDESCRIPTION = new Crestron.Logos.SplusObjects.StringOutput( CDESCRIPTION__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CDESCRIPTION__AnalogSerialOutput__, CDESCRIPTION );
    
    CSTART = new Crestron.Logos.SplusObjects.StringOutput( CSTART__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CSTART__AnalogSerialOutput__, CSTART );
    
    CEND = new Crestron.Logos.SplusObjects.StringOutput( CEND__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CEND__AnalogSerialOutput__, CEND );
    
    CDURATION = new Crestron.Logos.SplusObjects.StringOutput( CDURATION__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CDURATION__AnalogSerialOutput__, CDURATION );
    
    CREMAINING = new Crestron.Logos.SplusObjects.StringOutput( CREMAINING__AnalogSerialOutput__, this );
    m_StringOutputList.Add( CREMAINING__AnalogSerialOutput__, CREMAINING );
    
    FAV_NAME = new InOutArray<StringOutput>( 20, this );
    for( uint i = 0; i < 20; i++ )
    {
        FAV_NAME[i+1] = new Crestron.Logos.SplusObjects.StringOutput( FAV_NAME__AnalogSerialOutput__ + i, this );
        m_StringOutputList.Add( FAV_NAME__AnalogSerialOutput__ + i, FAV_NAME[i+1] );
    }
    
    RX = new Crestron.Logos.SplusObjects.BufferInput( RX__AnalogSerialInput__, 65534, this );
    m_StringInputList.Add( RX__AnalogSerialInput__, RX );
    
    FAVORITETIMER_Callback = new WaitFunction( FAVORITETIMER_CallbackFn );
    
    GO_PARSE.OnDigitalPush.Add( new InputChangeHandlerWrapper( GO_PARSE_OnPush_0, false ) );
    POWER_ON.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWER_ON_OnPush_1, false ) );
    POWER_OFF.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWER_OFF_OnPush_2, false ) );
    POWER.OnDigitalPush.Add( new InputChangeHandlerWrapper( POWER_OnPush_3, false ) );
    CUR_REFREH.OnDigitalPush.Add( new InputChangeHandlerWrapper( CUR_REFREH_OnPush_4, false ) );
    for( uint i = 0; i < 20; i++ )
        FAV[i+1].OnDigitalPush.Add( new InputChangeHandlerWrapper( FAV_OnPush_5, false ) );
        
    for( uint i = 0; i < 20; i++ )
        FAV[i+1].OnDigitalRelease.Add( new InputChangeHandlerWrapper( FAV_OnRelease_6, false ) );
        
    CHANNEL.OnAnalogChange.Add( new InputChangeHandlerWrapper( CHANNEL_OnChange_7, false ) );
    
    _SplusNVRAM.PopulateCustomAttributeList( true );
    
    NVRAM = _SplusNVRAM;
    
}

public override void LogosSimplSharpInitialize()
{
    
    
}

public UserModuleClass_DREAMBOXENIGMA2_PROCESSOR_V8 ( string InstanceName, string ReferenceID, Crestron.Logos.SplusObjects.CrestronStringEncoding nEncodingType ) : base( InstanceName, ReferenceID, nEncodingType ) {}


private WaitFunction FAVORITETIMER_Callback;


const uint GO_PARSE__DigitalInput__ = 0;
const uint POWER_ON__DigitalInput__ = 1;
const uint POWER_OFF__DigitalInput__ = 2;
const uint POWER__DigitalInput__ = 3;
const uint CUR_REFREH__DigitalInput__ = 4;
const uint FAV__DigitalInput__ = 5;
const uint CHANNEL__AnalogSerialInput__ = 0;
const uint TCP_STATUS__AnalogSerialInput__ = 1;
const uint RX__AnalogSerialInput__ = 2;
const uint ON_FB__DigitalOutput__ = 0;
const uint OFF_FB__DigitalOutput__ = 1;
const uint VOL_MUTED__DigitalOutput__ = 2;
const uint VOL_UNMUTED__DigitalOutput__ = 3;
const uint SAVING__DigitalOutput__ = 4;
const uint CHAN_CHANGE__DigitalOutput__ = 5;
const uint VOLUME_FB__AnalogSerialOutput__ = 0;
const uint TX__AnalogSerialOutput__ = 1;
const uint MESSAGE__AnalogSerialOutput__ = 2;
const uint CCHANNEL__AnalogSerialOutput__ = 3;
const uint CNAME__AnalogSerialOutput__ = 4;
const uint CTITLE__AnalogSerialOutput__ = 5;
const uint CDESCRIPTION__AnalogSerialOutput__ = 6;
const uint CSTART__AnalogSerialOutput__ = 7;
const uint CEND__AnalogSerialOutput__ = 8;
const uint CDURATION__AnalogSerialOutput__ = 9;
const uint CREMAINING__AnalogSerialOutput__ = 10;
const uint FAV_NAME__AnalogSerialOutput__ = 11;

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
