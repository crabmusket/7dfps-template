// Client game state.
new ScriptMsgListener(ClientState) {
   class = StateMachine;
   state = null;

   transition[null, start] = splashscreens;
   transition[splashscreens, escape] = mainMenu;
   transition[splashscreens, splashscreensDone] = mainMenu;

   transition[mainMenu, newGame] = selectLevel;
   transition[mainMenu, joinGame] = selectServer;

   transition[selectLevel, escape] = mainMenu;
   transition[selectLevel, back] = mainMenu;

   transition[selectServer, escape] = mainMenu;
   transition[selectServer, back] = mainMenu;
};

// Load scripts.
exec("scripts/input/main.cs");
exec("gui/main.cs");

// Subscribe to events.
GameEvents.subscribe(ClientState, EvtStart);
GameEvents.subscribe(ClientState, EvtExit);
function ClientState::onEvtStart(%this) { %this.onEvent(start); }
function ClientState::onEvtExit(%this)  { %this.onEvent(exit); }

InputEvents.subscribe(ClientState, EvtAdvance);
InputEvents.subscribe(ClientState, EvtEscape);
function ClientState::onEvtAdvance(%this) { %this.onEvent(advance); }
function ClientState::onEvtEscape(%this)  { %this.onEvent(escape); }

GuiEvents.subscribe(ClientState, EvtNewGame);
GuiEvents.subscribe(ClientState, EvtJoinGame);
function ClientState::onEvtNewGame(%this)  { %this.onEvent(newGame); }
function ClientState::onEvtJoinGame(%this) { %this.onEvent(joinGame); }

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
   Canvas.setContent(ChooseLevelGui);
}

function ClientState::enterSelectServer(%this) {
   Canvas.setContent(ChooseServerGui);
}
