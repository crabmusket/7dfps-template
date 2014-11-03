// Enable logging to console.log.
setLogMode(2);

// Events for the entire application.
new EventManager(GameEvents) {
   queue = GameEventQueue;
};

// This event is posted once after all scripts have been loaded. It lets modules
// add their own command-line arguments. It will be passed with an ArgParser
// object. See lib/argParser for details. This event is also gives objects a
// chance to subscribe to events they're interested in, since all event queues
// have been created, but no game logic has run yet.
GameEvents.registerEvent(EvtPreStart);

// This event is posted once after arguments have been parsed. It signals the
// start of the game itself, and should be used to display the Canvas, push a
// default UI, etc., or start hosting a dedicated server. With it is passed the
// parsed args object containing command-line options.
GameEvents.registerEvent(EvtStart);

// The EvtExit is posted once when something triggers an application exit. Modules
// should clean themselves up immediately (i.e. delete any objects they have
// created, close any files that are open).
GameEvents.registerEvent(EvtExit);

// This global onExit function is called when the engine is shut down, either
// by calling quit() or by being killed by the OS (i.e., hitting the close button
// on the window). We use it to post the EvtExit event so all interested code
// can shut itself down.
function onExit() {
   GameEvents.postEvent(EvtExit);
}

// And for consistency we provide a global keybind to quit the engine immediately.
// TODO: other platforms?
GlobalActionMap.bindCmd(keyboard, "alt f4", "quit();");

// Load game code. Remember that the order in which these modules are executed
// will determine the order in which they get to respond to GameEvents, assuming
// they all subscribe immediately. This important to avoid crashes when disconnecting
// from a local server! The client module must be run first - otherwise the
// server module will try to delete the level while the client is still connected
// to it.
exec("scripts/common.cs");
exec("scripts/client.cs");
exec("scripts/server.cs");
exec("scripts/localServer.cs");

// Create an argument parser to read command-line args.
%parser = ArgParser();
GameEvents.postEvent(EvtPreStart, %parser);
%args = %parser.parse();

// And let the game begin!
GameEvents.postEvent(EvtStart, %args);
