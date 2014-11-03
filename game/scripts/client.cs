displaySplashWindow("gui/splash.bmp");

// Miscellaneous setup.
$Gui::fontCacheDirectory = "gui/fontCache";

// Load libraries.
exec("lib/sys/main.cs");
Sys.init();
exec("lib/console/main.cs");
exec("lib/metrics/main.cs");
exec("lib/net/client.cs");

// Load client scripts.
exec("scripts/input/main.cs");
exec("gui/main.cs");

// Client game state.
new ScriptMsgListener(ClientState) {
   class = StateMachine;
   state = null;

   // The null state doesn't do anything - it's just the state before the game
   // engine has started working. It's useful because when we respond to the
   // 'start' event, we will get a nice 'enterSplashscreens' callback, which we
   // wouldn't get if we'd started in the splashscreens state.
   transition[null, start] = splashscreens;

   // When the game starts, the UI first displays splash screens with logos and
   // so on. You can escape from that directly to the menu, but the SplashscreenGui
   // will also send you to the menu with an event when it's finished.
   transition[splashscreens, escape] = mainMenu;
   transition[splashscreens, splashscreensDone] = mainMenu;

   // The main menu has transitions to other screens. Its 'exit' button directly
   // calls quit(), so its not represented in this state machine.
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

   // The inGame state represents, well, being in-game, playing. TODO: what to do
   // about game GUI screens like inventories. To include in this state machine
   // or not? The answer is probably yes. But those screens may have their own
   // internal state.
   transition[inGame, escape] = inGameMenu;

   // The in-game menu lets you leave the current game, but otherwise will just
   // send you back to being ingame. leaveGame is a special state we wait in while
   // we disconnect from the game.
   transition[inGameMenu, leaveGame] = leaveGame;
   transition[inGameMenu, escape] = inGame;
   transition[inGameMenu, back] = inGame;

   // This state is only expected to provide a callback so we can disconnect
   // from the server and fiddle with the UI and input maps appropriately. It
   // will receive the disconnected event almost immediately.
   transition[leaveGame, disconnected] = mainMenu;
};

GameEvents.subscribe(ClientState, EvtExit);
function ClientState::onEvtExit(%this) {
   // When the game exits, disconnect from any server we're connected to just
   // in case we're in-game.
   NetClient.disconnect();
   // Notify the state machine that we're exiting the application.
   %this.onEvent(exit);
}

GameEvents.subscribe(ClientState, EvtPreStart);
function ClientState::onEvtPreStart(%this, %parser) {
   // In EvtPreStart, we set up event listeners for all the events we're
   // interested in.
   InputEvents.subscribe(ClientState, EvtEscape);
   GuiEvents.subscribe(ClientState, EvtSplashscreensDone);
   GuiEvents.subscribe(ClientState, EvtNewGame);
   GuiEvents.subscribe(ClientState, EvtJoinGame);
   GuiEvents.subscribe(ClientState, EvtStartGame);
   GuiEvents.subscribe(ClientState, EvtLeaveGame);
   NetClientEvents.subscribe(ClientState, EvtInitialControlSet);
   NetClientEvents.subscribe(ClientState, EvtDisconnected);
}

GameEvents.subscribe(ClientState, EvtStart);
function ClientState::onEvtStart(%this, %args) {
   // When the game starts, notify our state machine of the occurrence. The state
   // enter/leave callbacks will handle the actual logic of making the UI and
   // input work.
   %this.onEvent(start);
}

// All these callbacks are called by the events we subscribed to in EvtPreStart.
// We pass their logic on to our internal state machine by calling onEvent.
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

function ClientState::enterLeaveGame(%this) {
   Canvas.setContent(LoadingGui);
   NetClient.disconnect();
   InGameMap.pop();
   MenuActionMap.push();
}
