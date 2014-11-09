function TrainingLevel::onLoad(%this) {
   LevelEvents.subscribe(%this, EvtLevelLoaded);
   LevelEvents.subscribe(%this, EvtSpawn);
   NetServerEvents.subscribe(%this, EvtClientEnterGame);
}

function TrainingLevel::onDestroy(%this) {
   LevelEvents.removeAll(%this);
   NetServerEvents.removeAll(%this);
}

function TrainingLevel::onEvtLevelLoaded(%this) {
   %this.state = new ScriptMsgListener() {
      class = StateMachine;
      state = null;
      transition[null, load] = playing;
   };

   %this.state.onEvent(load);
}

function TrainingLevel::onEvtClientEnterGame(%this, %client) {
   // Spawn immediately.
   %client.state.onEvent(spawn);
}

function TrainingLevel::onEvtSpawn(%this, %client) {
   // Fixed spawn point!
   %client.character.setTransform(MissionGroup-->TheSpawnPoint.getTransform());
}
