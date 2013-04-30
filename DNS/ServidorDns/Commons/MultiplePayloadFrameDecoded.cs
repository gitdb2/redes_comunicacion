using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunicacion;

namespace uy.edu.ort.obligatorio.Commons
{
    public class MultiplePayloadFrameDecoded
    {
        public int PartsTotal { get; private set; }
        public int PartsCurrent { get; private set; }
        public bool IsError { get; private set; }
        public string Destination { get; private set; }
        public string Payload { get; private set; }

        public bool IsLastpart(){return PartsCurrent == PartsTotal;}

        public const string DELIMITER = "|";

        private MultiplePayloadFrameDecoded() { }

        public static MultiplePayloadFrameDecoded Parse(string frame)
        {
            //01|01|DESTINATION|message....

            try
            {
                string[] payload = frame.Split(new string[] { DELIMITER }, 4, StringSplitOptions.None);

                MultiplePayloadFrameDecoded ret = new MultiplePayloadFrameDecoded();
                ret.PartsTotal = int.Parse(payload[0]);
                ret.PartsCurrent = int.Parse(payload[1]);
                ret.Destination = payload[2];
                ret.Payload = payload[3];
                ret.IsError = ret.Payload.Trim().ToUpper().StartsWith("ERROR");
                return ret;
            }
            catch (Exception e)
            {
                throw new System.FormatException("No se pudo parsear " + frame, e); ;
            }
        }

        public override string ToString()
        {
            return String.Format(

                @"PartsTotal: [{0}]
                PartsCurrent: [{1}]
                IsError: [{2}]
                Destination: [{3}]
                Payload: [{4}]", PartsTotal, PartsCurrent, IsError, Destination, Payload);


        }


    }
}
