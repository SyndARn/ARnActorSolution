﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Actor.Base;
using Actor.Service;

namespace TestActor
{
    [TestClass()]
    public class PersistentActorTests
    {

        class EventSourceTest : EventSource<string>
        {
            public EventSourceTest(string aString) : base()
            {
                Data = aString;
            }
            public override string Apply(string aT)
            {
                // Data = aT;
                return Data;
            }

        }

        [TestMethod()]
        public void PersistentActorTest()
        {
            TestLauncherActor.Test(() =>
            {
                MemoizePersistentService<string> service = new MemoizePersistentService<string>();
                PersistentActor<string> persistent = new PersistentActor<string>(service, "TestActor");
                persistent.SendMessage(new EventSourceTest("A"));
                persistent.SendMessage(new EventSourceTest("B"));
                persistent.SendMessage(new EventSourceTest("C"));
                Assert.AreEqual("C", persistent.GetCurrent().Result());
                var persistent2 = new PersistentActor<string>(service, "TestActor");
                persistent2.Reload();
                Assert.AreEqual("C", persistent2.GetCurrent().Result());
            });
        }

    }
}