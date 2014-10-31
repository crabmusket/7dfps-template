// Load scripts.
exec("scripts/input/main.cs");
exec("gui/main.cs");

// Client game state.
new ScriptMsgListener(ClientState) {
   class = StateMachine;
   state = null;
   transition[null, start] = logos;
   transition[logos, escape] = mainMenu;
   transition[logos, advance] = mainMenu;
   transition[logos, noMoreLogos] = mainMenu;
   transition[mainMenu, escape] = quit;
};

GameEvents.subscribe(ClientState, EvtStart);
GameEvents.subscribe(ClientState, EvtExit);
function ClientState::onEvtStart(%this) { %this.onEvent(start); }
function ClientState::onEvtExit(%this)  { %this.onEvent(exit); }

InputEvents.subscribe(ClientState, EvtAdvance);
InputEvents.subscribe(ClientState, EvtEscape);
function ClientState::onEvtAdvance(%this) { %this.onEvent(advance); }
function ClientState::onEvtEscape(%this)  { %this.onEvent(escape); }

function ClientState::leaveNull(%this) {
   closeSplashWindow();
   Canvas.showWindow();
}

function ClientState::enterLogos(%this) {
   Canvas.setContent(LogoGui);
   MenuActionMap.push();
}

function ClientState::enterMainMenu(%this) {
   Canvas.setContent(MainMenuGui);
   MenuActionMap.push();
}

function ClientState::enterQuit(%this) {
   GameEvents.postEvent(EvtExit);
}
