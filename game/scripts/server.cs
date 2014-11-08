// Load server scripts.
exec("lib/net/server.cs");
exec("levels/main.cs");
exec("art/materials.cs");
exec("scripts/server/camera.cs");
exec("scripts/server/connection.cs");
exec("scripts/server/characters/players.cs");
exec("scripts/server/characters/enemies.cs");

new ScriptMsgListener(ServerState) {
   superclass = StateMachine;
   state = null;
};

GameEvents.subscribe(ServerState, EvtExit);
function ServerState::onEvtExit(%this) {
   deleteDatablocks();
}
