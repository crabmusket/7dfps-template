new ScriptMsgListener(LocalServer) {
   chosenLevel = "";
};

GuiEvents.subscribe(LocalServer, EvtSelectLevel);
function LocalServer::onEvtSelectLevel(%this, %level) {
   %this.selectedLevel = %level;
}

GuiEvents.subscribe(LocalServer, EvtStartGame);
function LocalServer::onEvtStartGame(%this) {
   Levels.loadLevel(%this.chosenLevel);
   NetClient.connectTo(self);
   GuiEvents.postEvent(EvtStartLoading);
}

LevelEvents.subscribe(LocalServer, EvtLevelRegistered);
function LocalServer::onEvtLevelRegistered(%this, %level) {
   SelectLevelGui.addLevel(%level);
}

GameEvents.subscribe(LocalServer, EvtExit);
function LocalServer::onEvtExit(%this) {
   NetClient.disconnect();
}
