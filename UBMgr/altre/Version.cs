using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sbme
{
  class Version
  {
    internal const int VERSIONE_MAJOR	= 0;
    internal const int VERSIONE_MINOR	= 65;
    internal const int VERSIONE_FILEATTIVITA	= 2;
    internal const int NUMERO_COMPILAZIONE	= 1;

    internal static int GetVersionLong()
    {
      return (VERSIONE_MAJOR * 100 + VERSIONE_MINOR);
    }


  }
}
