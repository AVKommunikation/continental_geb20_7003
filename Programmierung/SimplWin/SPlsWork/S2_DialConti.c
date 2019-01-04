#include "TypeDefs.h"
#include "Globals.h"
#include "FnctList.h"
#include "Library.h"
#include "SimplSig.h"
#include "S2_DialConti.h"

FUNCTION_MAIN( S2_DialConti );
EVENT_HANDLER( S2_DialConti );
DEFINE_ENTRYPOINT( S2_DialConti );

DEFINE_INDEPENDENT_EVENTHANDLER( S2_DialConti, 00000 /*Dial*/ )

    {
    /* Begin local function variable declarations */
    
    SAVE_GLOBAL_POINTERS ;
    CheckStack( INSTANCE_PTR( S2_DialConti ) );
    
    
    /* End local function variable declarations */
    
    
    UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 146 );
    if ( (GetDigitalInput( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_EXTERN_DIG_INPUT ) == 1)) 
        { 
        UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 147 );
        if( ObtainStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ) == 0 ) {
        FormatString ( INSTANCE_PTR( S2_DialConti ) , GENERIC_STRING_OUTPUT( S2_DialConti )  ,10 , "10697603%s"  , GLOBAL_STRING_STRUCT( S2_DialConti, __NUMBER  )   )  ; 
        SetSerial( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_TX$_STRING_OUTPUT, GENERIC_STRING_OUTPUT( S2_DialConti ) );
        ReleaseStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ); }
        
        ; 
        } 
    
    else 
        {
        UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 149 );
        if ( (GetDigitalInput( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_INTERN_DIG_INPUT ) == 1)) 
            { 
            UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 150 );
            if( ObtainStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ) == 0 ) {
            FormatString ( INSTANCE_PTR( S2_DialConti ) , GENERIC_STRING_OUTPUT( S2_DialConti )  ,3 , "1%s"  , GLOBAL_STRING_STRUCT( S2_DialConti, __NUMBER  )   )  ; 
            SetSerial( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_TX$_STRING_OUTPUT, GENERIC_STRING_OUTPUT( S2_DialConti ) );
            ReleaseStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ); }
            
            ; 
            } 
        
        }
    
    
    S2_DialConti_Exit__Event_0:
    /* Begin Free local function variables */
/* End Free local function variables */
RESTORE_GLOBAL_POINTERS ;

}

DEFINE_INDEPENDENT_EVENTHANDLER( S2_DialConti, 00001 /*Dial_VC*/ )

    {
    /* Begin local function variable declarations */
    
    SAVE_GLOBAL_POINTERS ;
    CheckStack( INSTANCE_PTR( S2_DialConti ) );
    
    
    /* End local function variable declarations */
    
    
    UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 158 );
    if ( GetDigitalInput( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_ACTIVATE_SIP_DIG_INPUT )) 
        { 
        UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 160 );
        if( ObtainStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ) == 0 ) {
        FormatString ( INSTANCE_PTR( S2_DialConti ) , GENERIC_STRING_OUTPUT( S2_DialConti )  ,2 , "%s"  , GLOBAL_STRING_STRUCT( S2_DialConti, __NUMBERVC  )   )  ; 
        SetSerial( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_TX$_STRING_OUTPUT, GENERIC_STRING_OUTPUT( S2_DialConti ) );
        ReleaseStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ); }
        
        ; 
        } 
    
    else 
        {
        UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 162 );
        if ( GetDigitalInput( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_ACTIVATE_H323_DIG_INPUT )) 
            { 
            UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 164 );
            if( ObtainStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ) == 0 ) {
            FormatString ( INSTANCE_PTR( S2_DialConti ) , GENERIC_STRING_OUTPUT( S2_DialConti )  ,2 , "%s"  , GLOBAL_STRING_STRUCT( S2_DialConti, __NUMBERVC  )   )  ; 
            SetSerial( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_TX$_STRING_OUTPUT, GENERIC_STRING_OUTPUT( S2_DialConti ) );
            ReleaseStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ); }
            
            ; 
            } 
        
        }
    
    
    S2_DialConti_Exit__Event_1:
    /* Begin Free local function variables */
/* End Free local function variables */
RESTORE_GLOBAL_POINTERS ;

}

DEFINE_INDEPENDENT_EVENTHANDLER( S2_DialConti, 00002 /*Frankfurt*/ )

    {
    /* Begin local function variable declarations */
    
    SAVE_GLOBAL_POINTERS ;
    CheckStack( INSTANCE_PTR( S2_DialConti ) );
    
    
    /* End local function variable declarations */
    
    
    UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 170 );
    if( ObtainStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ) == 0 ) {
    FormatString ( INSTANCE_PTR( S2_DialConti ) , GENERIC_STRING_OUTPUT( S2_DialConti )  ,13 , "1069201725184"  )  ; 
    SetSerial( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_TX$_STRING_OUTPUT, GENERIC_STRING_OUTPUT( S2_DialConti ) );
    ReleaseStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ); }
    
    ; 
    
    S2_DialConti_Exit__Event_2:
    /* Begin Free local function variables */
/* End Free local function variables */
RESTORE_GLOBAL_POINTERS ;

}

DEFINE_INDEPENDENT_EVENTHANDLER( S2_DialConti, 00003 /*munchen*/ )

    {
    /* Begin local function variable declarations */
    
    SAVE_GLOBAL_POINTERS ;
    CheckStack( INSTANCE_PTR( S2_DialConti ) );
    
    
    /* End local function variable declarations */
    
    
    UpdateSourceCodeLine( INSTANCE_PTR( S2_DialConti ), 175 );
    if( ObtainStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ) == 0 ) {
    FormatString ( INSTANCE_PTR( S2_DialConti ) , GENERIC_STRING_OUTPUT( S2_DialConti )  ,13 , "1089204038490"  )  ; 
    SetSerial( INSTANCE_PTR( S2_DialConti ), __S2_DialConti_TX$_STRING_OUTPUT, GENERIC_STRING_OUTPUT( S2_DialConti ) );
    ReleaseStringOutputSemaphore( INSTANCE_PTR( S2_DialConti ) ); }
    
    ; 
    
    S2_DialConti_Exit__Event_3:
    /* Begin Free local function variables */
/* End Free local function variables */
RESTORE_GLOBAL_POINTERS ;

}


/********************************************************************************
  Constructor
********************************************************************************/

/********************************************************************************
  Destructor
********************************************************************************/

/********************************************************************************
  static void ProcessDigitalEvent( struct Signal_s *Signal )
********************************************************************************/
static void ProcessDigitalEvent( struct Signal_s *Signal )
{
    switch ( Signal->SignalNumber )
    {
        case __S2_DialConti_FRANKFURT_DIG_INPUT :
            if ( Signal->Value /*Push*/ )
            {
                SAFESPAWN_EVENTHANDLER( S2_DialConti, 00002 /*Frankfurt*/, 0 );
                
            }
            else /*Release*/
            {
                SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
                
            }
            break;
            
        case __S2_DialConti_MUNCHEN_DIG_INPUT :
            if ( Signal->Value /*Push*/ )
            {
                SAFESPAWN_EVENTHANDLER( S2_DialConti, 00003 /*munchen*/, 0 );
                
            }
            else /*Release*/
            {
                SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
                
            }
            break;
            
        case __S2_DialConti_DIAL_DIG_INPUT :
            if ( Signal->Value /*Push*/ )
            {
                SAFESPAWN_EVENTHANDLER( S2_DialConti, 00000 /*Dial*/, 0 );
                
            }
            else /*Release*/
            {
                SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
                
            }
            break;
            
        case __S2_DialConti_DIAL_VC_DIG_INPUT :
            if ( Signal->Value /*Push*/ )
            {
                SAFESPAWN_EVENTHANDLER( S2_DialConti, 00001 /*Dial_VC*/, 0 );
                
            }
            else /*Release*/
            {
                SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
                
            }
            break;
            
        default :
            SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
            break ;
        
    }
}

/********************************************************************************
  static void ProcessAnalogEvent( struct Signal_s *Signal )
********************************************************************************/
static void ProcessAnalogEvent( struct Signal_s *Signal )
{
    switch ( Signal->SignalNumber )
    {
        default :
            SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
            break ;
        
    }
}

/********************************************************************************
  static void ProcessStringEvent( struct Signal_s *Signal )
********************************************************************************/
static void ProcessStringEvent( struct Signal_s *Signal )
{
    if ( UPDATE_INPUT_STRING( S2_DialConti ) == 1 ) return ;
    
    switch ( Signal->SignalNumber )
    {
        default :
            SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
            break ;
        
    }
}

/********************************************************************************
  static void ProcessSocketConnectEvent( struct Signal_s *Signal )
********************************************************************************/
static void ProcessSocketConnectEvent( struct Signal_s *Signal )
{
    switch ( Signal->SignalNumber )
    {
        default :
            SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
            break ;
        
    }
}

/********************************************************************************
  static void ProcessSocketDisconnectEvent( struct Signal_s *Signal )
********************************************************************************/
static void ProcessSocketDisconnectEvent( struct Signal_s *Signal )
{
    switch ( Signal->SignalNumber )
    {
        default :
            SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
            break ;
        
    }
}

/********************************************************************************
  static void ProcessSocketReceiveEvent( struct Signal_s *Signal )
********************************************************************************/
static void ProcessSocketReceiveEvent( struct Signal_s *Signal )
{
    if ( UPDATE_INPUT_STRING( S2_DialConti ) == 1 ) return ;
    
    switch ( Signal->SignalNumber )
    {
        default :
            SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
            break ;
        
    }
}

/********************************************************************************
  static void ProcessSocketStatusEvent( struct Signal_s *Signal )
********************************************************************************/
static void ProcessSocketStatusEvent( struct Signal_s *Signal )
{
    switch ( Signal->SignalNumber )
    {
        default :
            SetSymbolEventStart ( INSTANCE_PTR( S2_DialConti ) ); 
            break ;
        
    }
}

/********************************************************************************
  EVENT_HANDLER( S2_DialConti )
********************************************************************************/
EVENT_HANDLER( S2_DialConti )
    {
    SAVE_GLOBAL_POINTERS ;
    switch ( Signal->Type )
        {
        case e_SIGNAL_TYPE_DIGITAL :
            ProcessDigitalEvent( Signal );
            break ;
        case e_SIGNAL_TYPE_ANALOG :
            ProcessAnalogEvent( Signal );
            break ;
        case e_SIGNAL_TYPE_STRING :
            ProcessStringEvent( Signal );
            break ;
        case e_SIGNAL_TYPE_CONNECT :
            ProcessSocketConnectEvent( Signal );
            break ;
        case e_SIGNAL_TYPE_DISCONNECT :
            ProcessSocketDisconnectEvent( Signal );
            break ;
        case e_SIGNAL_TYPE_RECEIVE :
            ProcessSocketReceiveEvent( Signal );
            break ;
        case e_SIGNAL_TYPE_STATUS :
            ProcessSocketStatusEvent( Signal );
            break ;
        }
        
    RESTORE_GLOBAL_POINTERS ;
    
    }
    
/********************************************************************************
  FUNCTION_MAIN( S2_DialConti )
********************************************************************************/
FUNCTION_MAIN( S2_DialConti )
{
    SAVE_GLOBAL_POINTERS ;
    
    SET_INSTANCE_POINTER ( S2_DialConti );
    
    INITIALIZE_GLOBAL_STRING_STRUCT ( S2_DialConti, __NUMBER, e_INPUT_TYPE_STRING, __S2_DialConti_NUMBER_STRING_MAX_LEN );
    REGISTER_GLOBAL_INPUT_STRING ( S2_DialConti, __NUMBER, __S2_DialConti_NUMBER_STRING_INPUT );
    INITIALIZE_GLOBAL_STRING_STRUCT ( S2_DialConti, __NUMBERVC, e_INPUT_TYPE_STRING, __S2_DialConti_NUMBERVC_STRING_MAX_LEN );
    REGISTER_GLOBAL_INPUT_STRING ( S2_DialConti, __NUMBERVC, __S2_DialConti_NUMBERVC_STRING_INPUT );
    
    INITIALIZE_GLOBAL_STRING_STRUCT ( S2_DialConti, sGenericOutStr, e_INPUT_TYPE_NONE, GENERIC_STRING_OUTPUT_LEN );
    
    
    
    S2_DialConti_Exit__MAIN:
    RESTORE_GLOBAL_POINTERS ;
    return 0 ;
    }
