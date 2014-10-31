aisplaySplashWindow("gui/splash.bmp");

// Events for game start and exit.
new EventManager(GameEvents) {
   queue = GameEventQueue;
};

GameEvents.registerEvent(EvtStart);
GameEvents.registerEvent(EvtExit);

function onExit() {
   GameEvents.postEvent(EvtExit);
}

// Miscellaneous setup.
$Gui::fontCacheDirectory = "gui/fontCache";

// Open a window.
exec("lib/sys/main.cs");
Sys.init();

// Load libraries.
exec("lib/net/client.cs");
exec("lib/net/server.cs");
exec("lib/console/main.cs");
exec("lib/metrics/main.cs");
exec("lib/twillex/main.cs");
exec("lib/stateMachine/main.cs");

// Load game code.
exec("scripts/game.cs");

// Let the game begin!
GameEvents.postEvent(EvtStart);
