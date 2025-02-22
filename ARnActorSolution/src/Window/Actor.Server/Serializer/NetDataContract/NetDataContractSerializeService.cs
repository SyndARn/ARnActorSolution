﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actor.Base;

namespace Actor.Server
{
    public class NetDataContractSerializeService : ISerializeService
    {
        public void Serialize(object data, ActorTag tag, Stream stream)
        {
            SerialObject so = new SerialObject(data, tag);
            NetDataActorSerializer.Serialize(so, stream);
        }

        public object Deserialize(Stream stream)
        {
            return NetDataActorSerializer.DeSerialize(stream);
        }
    }
}
