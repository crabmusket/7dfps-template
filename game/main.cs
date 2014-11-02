setLogMode(2);

// Events for game start and exit.
new EventManager(GameEvents) {
   queue = GameEventQueue;
};

GameEvents.registerEvent(EvtStart);
GameEvents.registerEvent(EvtExit);
GameEvents.registerEvent(EvtParseArgs);

GameEvents.subscribe(GameEvents, EvtExit);
function GameEvents::onEvtExit(%this) {
   // Defer quit till after all event handlers have had a chance to run.
   schedule(1, 0, quit);
}
GlobalActionMap.bindCmd(keyboard, "alt f4", "GameEvents.postEvent(EvtExit);");

// Miscellaneous setup.
$Gui::fontCacheDirectory = "gui/fontCache";

// Load game code.
exec("scripts/common.cs");
exec("scripts/client.cs");
exec("scripts/server.cs");
exec("scripts/localServer.cs");

%parser = ArgParser();
GameEvents.postEvent(EvtParseArgs, %parser);
%args = %parser.parse();

// Let the game begin!
GameEvents.postEvent(EvtStart, %args);
