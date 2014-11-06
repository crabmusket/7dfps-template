exec("./postFx/toonEdges.cs");

new ScriptMsgListener(PostFX) {
};

GameEvents.subscribe(PostFX, EvtPreStart);
function PostFX::onEvtPreStart(%this, %parser) {
   NetClientEvents.subscribe(%this, EvtInitialControlSet);
   NetClientEvents.subscribe(%this, EvtDisconnected);
}

function PostFX::onEvtInitialControlSet(%this) {
   ToonEdgesFx.enable();
}

function PostFX::onEvtDisconnected(%this) {
   ToonEdgesFx.disable();
}
