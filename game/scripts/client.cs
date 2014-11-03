displaySplashWindow("gui/splash.bmp");

// Miscellaneous setup.
$Gui::fontCacheDirectory = "gui/fontCache";

// Load libraries.
exec("lib/sys/main.cs");
Sys.init();
exec("lib/console/main.cs");
exec("lib/metrics/main.cs");
exec("lib/net/client.cs");

// Client game state.
new ScriptMsgListener(ClientState) {
   class = StateMachine;
   state = null;

   // When the game starts, the UI first displays splash screens with logos and
   // so on. You can escape from that directly to the menu, but the SplashscreenGui
   // will also send you to the menu with an event when it's finished.
   transition[null, start] = splashscreens;
   transition[splashscreens, escape] = mainMenu;
   transition[splashscreens, splashscreensDone] = mainMenu;

   // The main menu has transitions to other screens. Its 'exit' button directly
   // posts an EvtExit, so that transition is not part of this state machine.
   transition[mainMenu, newGame] = selectLevel;
   transition[mainMenu, joinGame] = selectServer;

   // The select level menu has a few internal transitions like EvtSelectLevel,
   // but the only one we're concerned with is starting a game, which sends you
   // to the loading screen.
   transition[selectLevel, escape] = mainMenu;
   transition[selectLevel, back] = mainMenu;
   transition[selectLevel, startGame] = loading;

   // TODO
   transition[selectServer, escape] = mainMenu;
   transition[selectServer, back] = mainMenu;

   // There should also be error handlers here.
   transition[loading, enterGame] = inGame;

   transition[inGame, escape] = inGameMenu;

   // The in-game menu lets you leave the current game, but otherwise will just
   // send you back to being ingame. leaveGame is a special state we wait in while
   // we disconnect from the game.
   transition[inGameMenu, leaveGame] = leaveGame;
   transition[inGameMenu, escape] = inGame;
   transition[inGameMenu, back] = inGame;

   // This state is not expected to survive long - in fact for local games, it
   // should happen instantly.
   transition[leaveGame, disconnected] = mainMenu;
};

// Load client scripts.
exec("scripts/input/main.cs");
exec("gui/main.cs");

// Subscribe to events.
GameEvents.subscribe(ClientState, EvtExit);
function ClientState::onEvtExit(%this) { %this.onEvent(exit); }

GameEvents.subscribe(ClientState, EvtStart);
function ClientState::onEvtStart(%this) {
   InputEvents.subscribe(ClientState, EvtEscape);
   GuiEvents.subscribe(ClientState, EvtSplashscreensDone);
   GuiEvents.subscribe(ClientState, EvtNewGame);
   GuiEvents.subscribe(ClientState, EvtJoinGame);
   GuiEvents.subscribe(ClientState, EvtStartGame);
   GuiEvents.subscribe(ClientState, EvtLeaveGame);
   NetClientEvents.subscribe(ClientState, EvtInitialControlSet);
   NetClientEvents.subscribe(ClientState, EvtDisconnected);

   %this.onEvent(start);
}

function ClientState::onEvtEscape(%this) { %this.onEvent(escape); }
function ClientState::onEvtSplashscreensDone(%this) { %this.onEvent(splashscreensDone); }
function ClientState::onEvtNewGame(%this) { %this.onEvent(newGame); }
function ClientState::onEvtJoinGame(%this) { %this.onEvent(joinGame); }
function ClientState::onEvtStartGame(%this) { %this.onEvent(startGame); }
function ClientState::onEvtLeaveGame(%this) { %this.onEvent(leaveGame); }
function ClientState::onEvtInitialControlSet(%this) { %this.onEvent(enterGame); }
function ClientState::onEvtDisconnected(%this) { %this.onEvent(disconnected); }

// Callbacks on state changes. This is where most of the logic happens.
function ClientState::enterSplashscreens(%this) {
   Canvas.setContent(SplashscreenGui);
   Canvas.showWindow();
   closeSplashWindow();
   MenuActionMap.push();
}

function ClientState::enterMainMenu(%this) {
   Canvas.setContent(MainMenuGui);
}

function ClientState::enterQuit(%this) {
   GameEvents.postEvent(EvtExit);
}

function ClientState::enterSelectLevel(%this) {
   Canvas.setContent(SelectLevelGui);
}

function ClientState::enterSelectServer(%this) {
   Canvas.setContent(SelectServerGui);
}

function ClientState::enterLoading(%this) {
   Canvas.setContent(LoadingGui);
}

function ClientState::enterInGame(%this) {
   MenuActionMap.pop();
   InGameMap.push();
   Canvas.setContent(GameViewGui);
}

function ClientState::enterLeaveGame(%this) {
   Canvas.setContent(LoadingGui);
   NetClient.disconnect();
   InGameMap.pop();
   MenuActionMap.push();
}

function ClientState::enterInGameMenu(%this) {
   Canvas.pushDialog(InGameMenuGui);
   InGameMap.pop();
   MenuActionMap.push();
}

function ClientState::leaveInGameMenu(%this) {
   Canvas.popDialog(InGameMenuGui);
   MenuActionMap.pop();
   InGameMap.push();
}
