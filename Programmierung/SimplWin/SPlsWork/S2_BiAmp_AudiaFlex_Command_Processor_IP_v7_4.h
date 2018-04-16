#ifndef __S2_BIAMP_AUDIAFLEX_COMMAND_PROCESSOR_IP_V7_4_H__
#define __S2_BIAMP_AUDIAFLEX_COMMAND_PROCESSOR_IP_V7_4_H__




/*
* Constructor and Destructor
*/

/*
* DIGITAL_INPUT
*/
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SEND_NEXT_DIG_INPUT 0
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_INITIALIZE_DIG_INPUT 1
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_GET_NEXT_NAME_DIG_INPUT 2
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_CONNECTED_F_DIG_INPUT 3


/*
* ANALOG_INPUT
*/


#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_FROM_DEVICE_BUFFER_INPUT 0
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_FROM_DEVICE_BUFFER_MAX_LEN 5000
CREATE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __FROM_DEVICE, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_FROM_DEVICE_BUFFER_MAX_LEN );
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_FROM_MODULES_BUFFER_INPUT 1
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_FROM_MODULES_BUFFER_MAX_LEN 5000
CREATE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __FROM_MODULES, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_FROM_MODULES_BUFFER_MAX_LEN );


/*
* DIGITAL_OUTPUT
*/
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_TIMED_OUT_DIG_OUTPUT 0
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_INITIALIZE_BUSY_DIG_OUTPUT 1
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_NAME_TIMED_OUT_DIG_OUTPUT 2


/*
* ANALOG_OUTPUT
*/

#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_TO_DEVICE_STRING_OUTPUT 0

#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_TO_MODULES_STRING_OUTPUT 1
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_TO_MODULES_ARRAY_LENGTH 500

/*
* Direct Socket Variables
*/




/*
* INTEGER_PARAMETER
*/
/*
* SIGNED_INTEGER_PARAMETER
*/
/*
* LONG_INTEGER_PARAMETER
*/
/*
* SIGNED_LONG_INTEGER_PARAMETER
*/
/*
* INTEGER_PARAMETER
*/
/*
* SIGNED_INTEGER_PARAMETER
*/
/*
* LONG_INTEGER_PARAMETER
*/
/*
* SIGNED_LONG_INTEGER_PARAMETER
*/
/*
* STRING_PARAMETER
*/


/*
* INTEGER
*/


/*
* LONG_INTEGER
*/


/*
* SIGNED_INTEGER
*/


/*
* SIGNED_LONG_INTEGER
*/


/*
* STRING
*/
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_STEMPMODULES_STRING_MAX_LEN 100
CREATE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __STEMPMODULES, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_STEMPMODULES_STRING_MAX_LEN );
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_STEMPDEVICE_STRING_MAX_LEN 100
CREATE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __STEMPDEVICE, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_STEMPDEVICE_STRING_MAX_LEN );
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SCOMMAND_ARRAY_NUM_ELEMS 500
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SCOMMAND_ARRAY_NUM_CHARS 100
CREATE_STRING_ARRAY( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __SCOMMAND, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SCOMMAND_ARRAY_NUM_ELEMS, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SCOMMAND_ARRAY_NUM_CHARS );
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SGET_ARRAY_NUM_ELEMS 500
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SGET_ARRAY_NUM_CHARS 100
CREATE_STRING_ARRAY( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __SGET, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SGET_ARRAY_NUM_ELEMS, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SGET_ARRAY_NUM_CHARS );
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SMODULEINSTANCEID_ARRAY_NUM_ELEMS 500
#define __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SMODULEINSTANCEID_ARRAY_NUM_CHARS 100
CREATE_STRING_ARRAY( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __SMODULEINSTANCEID, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SMODULEINSTANCEID_ARRAY_NUM_ELEMS, __S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4_SMODULEINSTANCEID_ARRAY_NUM_CHARS );

/*
* STRUCTURE
*/

START_GLOBAL_VAR_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4 )
{
   void* InstancePtr;
   struct GenericOutputString_s sGenericOutStr;
   unsigned short LastModifiedArrayIndex;

   DECLARE_IO_ARRAY( __TO_MODULES );
   unsigned short __INEXTCOMMANDSTORE;
   unsigned short __INEXTCOMMANDSEND;
   unsigned short __INEXTGETSTORE;
   unsigned short __INEXTGETSEND;
   unsigned short __ITEMPID;
   unsigned short __BFLAG1;
   unsigned short __BFLAG2;
   unsigned short __BOKTOSEND;
   unsigned short __ITEMP;
   unsigned short __A;
   unsigned short __B;
   unsigned short __ISENDNAME;
   DECLARE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __STEMPMODULES );
   DECLARE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __STEMPDEVICE );
   DECLARE_STRING_ARRAY( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __SCOMMAND );
   DECLARE_STRING_ARRAY( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __SGET );
   DECLARE_STRING_ARRAY( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __SMODULEINSTANCEID );
   DECLARE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __FROM_DEVICE );
   DECLARE_STRING_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, __FROM_MODULES );
};

START_NVRAM_VAR_STRUCT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4 )
{
};

DEFINE_WAITEVENT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, WTIMEOUT );
DEFINE_WAITEVENT( S2_BiAmp_AudiaFlex_Command_Processor_IP_v7_4, WNAMETIMEOUT );


#endif //__S2_BIAMP_AUDIAFLEX_COMMAND_PROCESSOR_IP_V7_4_H__

