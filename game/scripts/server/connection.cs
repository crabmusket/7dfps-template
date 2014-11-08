function GameConnection::onConnectRequest(%this, %addr) {
   // Accept all connections.
   return "";
}

function GameConnection::onEnterGame(%this) {
   %this.state = new ScriptMsgListener() {
      class = ClientConnectionState;
      superclass = StateMachine;
      state = null;

      transition[null, enterGame] = observing;
      transition[observing, spawn] = playing;
      transition[playing, died] = observing;
   };

   %camera = new Camera() {
      datablock = ObserverCam;
   };
   %camera.setTransform("0 0 2 1 0 0 0");
   %camera.scopeToClient(%this);
   %this.setControlObject(%camera);
   %this.camera = %camera;
   %this.add(%camera);

   %this.state.onEvent(enterGame);
}

function ClientConnectionState::enterObserving(%this) {
}

function ClientConnectionState::enterPlaying(%this) {
}
