// Client game state
new ScriptMsgListener(ClientState) {
   class = StateMachine;
   state = null;
   transition[null, start] = splashes;
};

GameEvents.subscribe(ClientState, Start);
GameEvents.subscribe(ClientState, Exit);
function ClientState::onStart(%this) { %this.onEvent(Start); }
function ClientState::onExit(%this)  { %this.onEvent(Exit); }

function ClientState::enterSplashes(%this) {
   closeSplashWindow();
   Canvas.showWindow();
}
