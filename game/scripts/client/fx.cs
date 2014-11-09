exec("./postFx/toonEdges.cs");
exec("./postFx/glow.cs");

new ScriptMsgListener(PostFX) {
};

GameEvents.subscribe(PostFX, EvtPreStart);
function PostFX::onEvtPreStart(%this, %parser) {
   NetClientEvents.subscribe(%this, EvtInitialControlSet);
   NetClientEvents.subscribe(%this, EvtDisconnected);
}

function PostFX::onEvtInitialControlSet(%this) {
   ToonEdgesFx.enable();
   GlowFx.enable();
}

function PostFX::onEvtDisconnected(%this) {
   ToonEdgesFx.disable();
   GlowFx.disable();
}
