#ifndef __S2_EXTRON_SW_SERIES_H__
#define __S2_EXTRON_SW_SERIES_H__




/*
* Constructor and Destructor
*/

/*
* DIGITAL_INPUT
*/


/*
* ANALOG_INPUT
*/
#define __S2_Extron_SW_Series_OUTPUT_1_ANALOG_INPUT 0


#define __S2_Extron_SW_Series_FROM_DEVICE_BUFFER_INPUT 1
#define __S2_Extron_SW_Series_FROM_DEVICE_BUFFER_MAX_LEN 56
CREATE_STRING_STRUCT( S2_Extron_SW_Series, __FROM_DEVICE, __S2_Extron_SW_Series_FROM_DEVICE_BUFFER_MAX_LEN );


/*
* DIGITAL_OUTPUT
*/

#define __S2_Extron_SW_Series_HDMI_DETECT_DIG_OUTPUT 0
#define __S2_Extron_SW_Series_HDMI_DETECT_ARRAY_LENGTH 8

/*
* ANALOG_OUTPUT
*/

#define __S2_Extron_SW_Series_TO_DEVICE_STRING_OUTPUT 0


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
#define __S2_Extron_SW_Series_TRASH$_STRING_MAX_LEN 255
CREATE_STRING_STRUCT( S2_Extron_SW_Series, __TRASH$, __S2_Extron_SW_Series_TRASH$_STRING_MAX_LEN );

/*
* STRUCTURE
*/

START_GLOBAL_VAR_STRUCT( S2_Extron_SW_Series )
{
   void* InstancePtr;
   struct GenericOutputString_s sGenericOutStr;
   unsigned short LastModifiedArrayIndex;

   DECLARE_IO_ARRAY( __HDMI_DETECT );
   unsigned short __RXOK;
   DECLARE_STRING_STRUCT( S2_Extron_SW_Series, __TRASH$ );
   DECLARE_STRING_STRUCT( S2_Extron_SW_Series, __FROM_DEVICE );
};

START_NVRAM_VAR_STRUCT( S2_Extron_SW_Series )
{
};



#endif //__S2_EXTRON_SW_SERIES_H__

