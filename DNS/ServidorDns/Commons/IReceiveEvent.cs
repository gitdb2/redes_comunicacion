using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;

namespace uy.edu.ort.obligatorio.Commons
{
    public interface IReceiveEvent
    {

        bool OnReceiveData(Connection connection);

        bool OnFatalError(Connection connection);

    }
}
