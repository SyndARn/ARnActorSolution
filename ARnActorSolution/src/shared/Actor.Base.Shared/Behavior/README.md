#ARnActorModel

##Actor.Base

###Behavior

Behaviors are the brain of an actor.

Each behavior handles two functions :

  - a PATTERN function, it answers TRUE if a received message is to be handled by this behavior and FALSE in other cases
  - an APPLY function, if a message was accepted by the PATTERN function, then the APPLY function is called with the message as params/
  
During Actor creation (new BaseActor() ..), a Become statement is most always used to set the initial Behavior of this actor.

Typically, the following pattern is used :

    public class MyActor : BaseActor
    {
      public MyActor() : base()
      {
        Become(new MyBehavior()) ;
      }
    }

To continue this small example :

    public MyBehavior()
    {
      Pattern = t => true ; // accept all messages
      Apply = t => Console.WriteLine(t.ToString()) ; // print message received
    }

And of course :

    var myActor = new MyActor() ;
    myActor.SendMessage("Hello world !") ;

will print "Hello world !" to the console.
