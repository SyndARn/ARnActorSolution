﻿using System;
using System.Collections.Generic;
using System.Linq;
using Actor.Base;

namespace Actor.Server
{
    public class ShardDirectoryActor : BaseActor
    {
        private readonly Dictionary<string, string> _shardList;

        public static ShardDirectoryActor AttachShardDirectoryActor(ActorServer actorServer)
        {
            CheckArg.Actor(actorServer);
            IActor shardFound = DirectoryActor.GetDirectory().GetActorByName("KnownShards");
            if (shardFound != null)
            {
                return (ShardDirectoryActor)shardFound;
            }
            return new ShardDirectoryActor(actorServer);
        }

        public ShardDirectoryActor(ActorServer actorServer)
            : base()
        {
            CheckArg.Actor(actorServer);
            _shardList = new Dictionary<string, string>
            {
                { "LocalHost",actorServer.FullHost }
            };
            DirectoryActor.GetDirectory().Register(this, "KnownShards");
            HostDirectoryActor.Register(this);
            Become(new Behavior<ShardRequest>(
                t => t is ShardRequest,
                DoProcessShardRequest));
        }

        private void DoProcessShardRequest(ShardRequest msg)
        {
            switch (msg.RequestType)
            {
                case "Ask":
                    {
                        ShardRequest ans = msg.CastAnswer(_shardList.Values.AsEnumerable<string>());
                        msg.Target.SendMessage(ans);
                        break;
                    }
                case "Answer":
                    {
                        foreach (var item in msg.Data)
                        {
                            _shardList.Add(item, item);
                        }

                        break;
                    }
                default: break;
            }
        }
    }
}
