function TrainingLevel::onLoad(%this) {
   LevelEvents.subscribe(%this, EvtSpawn);
   NetServerEvents.subscribe(%this, EvtClientEnterGame);

   %this.state = new ScriptMsgListener() {
      class = StateMachine;
      state = null;
      transition[null, load] = playing;
   };

   %this.state.onEvent(load);

   // Spawn villains
   EnemySpawns.callOnChildren(spawnObject);
}

function TrainingLevel::onDestroy(%this) {
   LevelEvents.removeAll(%this);
   NetServerEvents.removeAll(%this);
}

function TrainingLevel::onEvtClientEnterGame(%this, %client) {
   // Spawn immediately.
   %client.state.onEvent(spawn);
}

function TrainingLevel::onEvtSpawn(%this, %client) {
   // Fixed spawn point!
   %client.character.setTransform(MissionGroup-->TheSpawnPoint.getTransform());
}
