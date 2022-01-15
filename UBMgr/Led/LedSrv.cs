using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  enum LedCode
  {
    LEDSRV_FIXED_ALL_OFF = 0,
    LEDSRV_FIXED_ALL_ON = 1,

    LEDSRV_ALTERNATE_TWO_ESTERNAL_INTERNAL = 2,
    LEDSRV_ALTERNATE_TWO_EVEN_ODD = 3,
    LEDSRV_ALTERNATE_TWO_LEFTS_RIGHTS = 4,

    LEDSRV_ROTATE_ONE_RIGHT = 5,
    LEDSRV_ROTATE_ONE_LEFT = 6,

    LEDSRV_ROTATE_TWO_RIGHT = 7,
    LEDSRV_ROTATE_TWO_LEFT = 8,

    LEDSRV_ROTATE_THREE_RIGHT = 9,
    LEDSRV_ROTATE_THREE_LEFT = 10,

    LEDSRV_RIGHT_LEFT_LEFT_RIGHT_ONE = 11,
    LEDSRV_RIGHT_LEFT_LEFT_RIGHT_TWO = 12,
  }

  enum LedState
  {
    LED_NON_IDENTIFICATO = LedCode.LEDSRV_ALTERNATE_TWO_LEFTS_RIGHTS,
    LED_NON_CONFIGURATO = LedCode.LEDSRV_ROTATE_TWO_LEFT,
    LED_IN_CONFIG = LedCode.LEDSRV_ROTATE_TWO_RIGHT,
    LED_DEGRADATO = LedCode.LEDSRV_ROTATE_ONE_LEFT,
    LED_NOMINALE = LedCode.LEDSRV_ROTATE_ONE_RIGHT,
    LED_BLOCCATO = LedCode.LEDSRV_ALTERNATE_TWO_EVEN_ODD,
    LED_CHIUSO = LedCode.LEDSRV_ALTERNATE_TWO_ESTERNAL_INTERNAL,
    LED_SPEGNIMENTO = LedCode.LEDSRV_ROTATE_THREE_RIGHT,
    LED_SHUTDOWN = LedCode.LEDSRV_FIXED_ALL_ON,
  }


  internal static class LedSrv
  {
    internal static void Set(LedState lED_NON_IDENTIFICATO)
    {
      // XXXXX DA FARE
      throw new NotImplementedException();
    }

    internal static void End()
    {
      throw new NotImplementedException();
    }
  }
}
