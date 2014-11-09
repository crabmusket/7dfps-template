new ScriptMsgListener(LocalServer) {
   chosenLevel = "";
};

GameEvents.subscribe(LocalServer, EvtPreStart);
function LocalServer::onEvtPreStart(%this, %parser) {
   GuiEvents.subscribe(LocalServer, EvtSelectLevel);
   GuiEvents.subscribe(LocalServer, EvtStartGame);
   LevelEvents.subscribe(LocalServer, EvtLevelRegistered);
   NetClientEvents.subscribe(LocalServer, EvtDisconnected);
}

function LocalServer::onEvtSelectLevel(%this, %level) {
   %this.selectedLevel = %level;
}

function LocalServer::onEvtStartGame(%this) {
   // TODO: move this logic.
   if (Levels.loadLevel(%this.selectedLevel)) {
      NetClient.connectTo(self);
      GuiEvents.postEvent(EvtStartLoading);
   }
}

function LocalServer::onEvtLevelRegistered(%this, %level) {
   SelectLevelGui.addLevel(%level);
}

function LocalServer::onEvtDisconnected(%this) {
   Levels.destroyLevel();
}
