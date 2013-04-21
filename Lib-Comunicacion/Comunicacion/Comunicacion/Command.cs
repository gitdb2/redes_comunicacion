using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comunicacion
{
    public enum Command
    {
        REQ = 0, RES
    }
    public enum OpCode
    {
        LOGIN = 0,
        LOGOUT,
        HELLO

    }
}
