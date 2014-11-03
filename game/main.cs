// Enable logging to console.log.
setLogMode(2);

// Events for the entire application.
new EventManager(GameEvents) {
   queue = GameEventQueue;
};
// This event is posted once after all scripts have been loaded. It lets modules
// add their own command-line arguments. It will be passed with an ArgParser
// object. See lib/argParser for details.
GameEvents.registerEvent(EvtParseArgs);
// This event is posted once after arguments have been parsed. It signals the
// start of the game itself, and should be used to display the Canvas, push a
// default UI, etc., or start hosting a dedicated server.
GameEvents.registerEvent(EvtStart);
// The EvtExit is posted once when something triggers an application exit. Modules
// should clean themselves up immediately (i.e. delete any objects they have
// created, close any files that are open).
GameEvents.registerEvent(EvtExit);

function onExit(%this) {
   GameEvents.postEvent(EvtExit);
}
// And for convenience we provide a global keybind to quit the engine immediately.
GlobalActionMap.bindCmd(keyboard, "alt f4", "quit();");

// Load game code.
exec("scripts/common.cs");
exec("scripts/client.cs");
exec("scripts/localServer.cs");
exec("scripts/server.cs");

%parser = ArgParser();
GameEvents.postEvent(EvtParseArgs, %parser);
%args = %parser.parse();

// Let the game begin!
GameEvents.postEvent(EvtStart, %args);
