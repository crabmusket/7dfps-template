new ScriptMsgListener(ServerState) {
};

// Load server scripts.
exec("lib/net/server.cs");
exec("levels/main.cs");
exec("scripts/server/camera.cs");

function GameConnection::onConnectRequest(%this, %addr) {
   return "";
}

function GameConnection::onEnterGame(%this) {
   %camera = new Camera() {
      datablock = ObserverCam;
   };
   %camera.setTransform("0 0 2 1 0 0 0");
   %camera.scopeToClient(%this);
   %this.setControlObject(%camera);
   %this.add(%camera);
}

singleton Material(BlankWhite) {
   diffuseColor[0] = "White";
};
