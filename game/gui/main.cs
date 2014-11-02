new EventManager(GuiEvents) {
   queue = GuiEventQueue;
};

GuiEvents.registerEvent(EvtNewGame);
GuiEvents.registerEvent(EvtJoinGame);

exec("./profiles.cs");
exec("./menus/splashscreens.cs");
exec("./menus/mainMenu.cs");
exec("./menus/selectLevel.cs");
exec("./menus/selectServer.cs");
exec("./menus/loading.cs");
exec("./ingame/gameView.cs");
