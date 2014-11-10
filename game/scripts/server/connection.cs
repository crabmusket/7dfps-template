function GameConnection::onConnectRequest(%this, %addr) {
   // Accept all connections.
   return "";
}

// When a new client enters the game, we'll set up all the objects used to handle
// their time with us. The GameConnection is the actual network connection, and
// is created automatically by the engine. In addition, we'll create a state machine
// that will help us manage the different stages a connected player goes through,
// and we'll also make them a camera.
// We make the camera now because it's easy to do, and is probably always going
// to be useful. In addition, unlike the Player object that this player will
// control, it doesn't have a die/respawn cycle.
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

function serverCmdUse(%this) {
   if (%this.currentItem && %this.character) {
      %this.character.getDataBlock().use(%this.character, %this.currentItem);
   }
}

function serverCmdSwapWeapon(%this) {
   %this.character.getDataBlock().swapWeapon(%this.character);
}

function serverCmdThrowWeapon(%this) {
   %this.character.getDataBlock().throwWeapon(%this.character);
}
