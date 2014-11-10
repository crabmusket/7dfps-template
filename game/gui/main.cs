new EventManager(GuiEvents) {
   queue = GuiEventQueue;
};

GuiEvents.registerEvent(EvtSplashscreensDone);

GuiEvents.registerEvent(EvtNewGame);
GuiEvents.registerEvent(EvtJoinGame);

// EvtStartGame is posted when we 
GuiEvents.registerEvent(EvtLeaveGame);
GuiEvents.registerEvent(EvtStartGame);
GuiEvents.registerEvent(EvtSelectLevel);

exec("./profiles.cs");
exec("./menus/splashscreens.cs");
exec("./menus/mainMenu.cs");
exec("./menus/selectLevel.cs");
exec("./menus/selectServer.cs");
exec("./menus/loading.cs");
exec("./ingame/gameView.cs");
exec("./ingame/inGameMenu.cs");
exec("./ingame/hud.cs");
