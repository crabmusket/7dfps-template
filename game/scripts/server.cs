// Load server scripts.
exec("lib/net/server.cs");
exec("levels/main.cs");

// Materials.
exec("art/materials.cs");
exec("art/structures/materials.cs");
exec("art/weapons/machinegun/materials.cs");

// Datablocks and associated scripts.
exec("scripts/server/connection.cs");
exec("scripts/server/weapons/main.cs");
exec("scripts/server/camera.cs");
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
