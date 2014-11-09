function GameConnection::onConnectRequest(%this, %addr) {
   // Accept all connections.
   return "";
}

function GameConnection::onEnterGame(%this) {
   %this.state = new ScriptMsgListener() {
      connection = %this;
      class = ClientConnectionState;
      superclass = StateMachine;
      state = null;

      transition[null, enterGame] = observing;
      transition[observing, spawn] = playing;
      transition[playing, died] = observing;
   };

   %camera = new Camera() {
      datablock = ObserverCam;
      position = "0 0 2";
   };
   %camera.scopeToClient(%this);
   %this.camera = %camera;
   %this.add(%camera);

   %this.state.onEvent(enterGame);
}

function ClientConnectionState::enterObserving(%this) {
   %this.connection.setControlObject(%this.connection.camera);
}

function ClientConnectionState::enterPlaying(%this) {
}
