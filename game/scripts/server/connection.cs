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
   %this.observeThroughCam(0);
   commandToClient(%this.connection, 'enterObserving');
}

function ClientConnectionState::observeThroughCam(%this, %idx) {
   if (!isObject(ObserverCamGroup)) {
      return;
   }

   %markers = ObserverCamGroup.getCount();
   if (%idx < 0) {
      %idx = %markers - 1;
   } else if (%idx >= %markers) {
      %idx = 0;
   }
   %marker = ObserverCamGroup.getObject(%idx);
   if (isObject(%marker)) {
      %transform = %marker.getTransform();
      %this.connection.camera.setTransform(%transform);
      %this.observerCamIndex = %idx;
   }
}

function ClientConnectionState::enterPlaying(%this) {
   if (!isObject(%this.connection.character)) {
      %this.connection.character = new Player() {
         datablock = PlayerSoldier;
         client = %this.connection;
      };
      MissionCleanup.add(%this.connection.character);
      LevelEvents.postEvent(EvtSpawn, %this.connection);
   }
   %this.connection.setControlObject(%this.connection.character);
   commandToClient(%this.connection, 'enterPlaying');
}

function serverCmdShiftObserverCam(%this, %dir) {
   if (%this.state.state $= observing) {
      %this.state.observeThroughCam(%this.state.observerCamIndex + %dir);
   }
}

function GameConnection::lookingAtItem(%this, %item) {
   if (%item !$= %this.currentItem) {
      %this.currentItem = %item;
      if (isObject(%item)) {
         commandToClient(%this, 'interaction',
            %this.character.getDataBlock().getInteraction(%this.character, %item));
      } else {
         commandToClient(%this, 'noInteraction');
      }
   }
}
