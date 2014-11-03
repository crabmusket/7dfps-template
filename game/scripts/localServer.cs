new ScriptMsgListener(LocalServer) {
   chosenLevel = "";
};

GameEvents.subscribe(LocalServer, EvtStart);
function LocalServer::onEvtStart(%this) {
   GuiEvents.subscribe(LocalServer, EvtSelectLevel);
   GuiEvents.subscribe(LocalServer, EvtStartGame);
   LevelEvents.subscribe(LocalServer, EvtLevelRegistered);
   NetClientEvents.subscribe(LocalServer, EvtDisconnected);
}

GameEvents.subscribe(LocalServer, EvtExit);
function LocalServer::onEvtExit(%this) {
   NetClient.disconnect();
}

function LocalServer::onEvtSelectLevel(%this, %level) {
   %this.selectedLevel = %level;
}

function LocalServer::onEvtStartGame(%this) {
   Levels.loadLevel(%this.chosenLevel);
   NetClient.connectTo(self);
   GuiEvents.postEvent(EvtStartLoading);
}

function LocalServer::onEvtLevelRegistered(%this, %level) {
   SelectLevelGui.addLevel(%level);
}

function LocalServer::onEvtDisconnected(%this) {
   Levels.destroyLevel();
}
