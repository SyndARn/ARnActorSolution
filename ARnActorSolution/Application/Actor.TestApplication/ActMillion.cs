﻿using System;
using Actor.Base;
using Actor.Util;
using System.Globalization;

namespace Actor.TestApplication
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "act")]
    public class actMillion : BaseActor
    {
        private readonly QueueActor<IActor> fQueue;
        const int KSize = 10000 ;

        public actMillion() : base()
        {
            fQueue = new QueueActor<IActor>();
            Become(new Behavior<string>(DoStart));
            SendMessage("DoStart");
        }

        public void Send()
        {
            Become(new Behavior<string>(DoSend));
            SendMessage("DoSend");
        }

        private void DoStart(string msg)
        {
            for (int i = 0; i < KSize; i++)
            {
                fQueue.Queue(new BaseActor());
            }

            Console.WriteLine("end start million");
        }

        private void DoSend(string msg)
        {
            int i = 0;
            IMsgQueue<IActor> item = fQueue.TryDequeueAsync().Result;
            while(item.Result && (i<KSize))
            {
                item.Data.SendMessage("Bop");
                item = fQueue.TryDequeueAsync().Result;
                Console.WriteLine("receive " + i.ToString(CultureInfo.InvariantCulture));
                i++;
            }

            Console.WriteLine("end "+i.ToString(CultureInfo.InvariantCulture));
        }
    }
}
