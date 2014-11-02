new ScriptMsgListener(LocalServer) {
};

GuiEvents.subscribe(LocalServer, EvtStartGame);
function LocalServer::onEvtStartGame(%this, %level) {
   %level.load();
   NetClient.connectTo(self);
   GuiEvents.postEvent(EvtStartLoading);
}
