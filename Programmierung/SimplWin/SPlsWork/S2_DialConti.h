#ifndef __S2_DIALCONTI_H__
#define __S2_DIALCONTI_H__




/*
* Constructor and Destructor
*/

/*
* DIGITAL_INPUT
*/
#define __S2_DialConti_INTERN_DIG_INPUT 0
#define __S2_DialConti_EXTERN_DIG_INPUT 1
#define __S2_DialConti_FRANKFURT_DIG_INPUT 2
#define __S2_DialConti_MUNCHEN_DIG_INPUT 3
#define __S2_DialConti_DIAL_DIG_INPUT 4


/*
* ANALOG_INPUT
*/

#define __S2_DialConti_NUMBER_STRING_INPUT 0
#define __S2_DialConti_NUMBER_STRING_MAX_LEN 50
CREATE_STRING_STRUCT( S2_DialConti, __NUMBER, __S2_DialConti_NUMBER_STRING_MAX_LEN );



/*
* DIGITAL_OUTPUT
*/


/*
* ANALOG_OUTPUT
*/

#define __S2_DialConti_TX$_STRING_OUTPUT 0


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

/*
* STRUCTURE
*/

START_GLOBAL_VAR_STRUCT( S2_DialConti )
{
   void* InstancePtr;
   struct GenericOutputString_s sGenericOutStr;
   unsigned short LastModifiedArrayIndex;

   DECLARE_STRING_STRUCT( S2_DialConti, __NUMBER );
};

START_NVRAM_VAR_STRUCT( S2_DialConti )
{
};



#endif //__S2_DIALCONTI_H__

